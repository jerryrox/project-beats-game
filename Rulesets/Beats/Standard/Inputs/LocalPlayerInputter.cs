using System;
using System.IO;
using System.Collections.Generic;
using PBGame.UI;
using PBGame.UI.Models;
using PBGame.IO;
using PBGame.Stores;
using PBGame.Rulesets.Beats.Standard.UI;
using PBGame.Rulesets.Beats.Standard.UI.Components;
using PBGame.Rulesets.Judgements;
using PBGame.Graphics;
using PBFramework.Inputs;
using PBFramework.Threading;
using PBFramework.Allocation.Recyclers;
using PBFramework.Dependencies;
using UnityEngine.EventSystems;

namespace PBGame.Rulesets.Beats.Standard.Inputs
{
    public class LocalPlayerInputter : IGameInputter, IInputReceiver
    {
        public event Action<BeatsCursor> OnCursorPress;
        public event Action<BeatsCursor> OnCursorRelease;
        public event Action<BeatsKey> OnKeyPress;
        public event Action<BeatsKey> OnKeyRelease;

        private HitBarDisplay hitBar;
        private HitObjectHolder hitObjectHolder;

        private BeatsCursor hitBarCursor;
        private ManagedRecycler<BeatsKey> keyRecycler;
        private PointerEventData pointerEvent;
        private List<RaycastResult> raycastResults = new List<RaycastResult>();

        private DataStreamSaver<ReplayableInput> replayInputSaver;
        private FileInfo replayFile;
        private StreamWriter replayWriteStream;

        public BeatsCursor HitBarCursor => hitBarCursor;
        public List<BeatsKey> KeyInputs => keyRecycler.ActiveObjects;

        public int InputLayer => InputLayers.GameScreenComponents;

        public FileInfo ReplayFile => replayFile;

        [ReceivesDependency]
        private GameModel Model { get; set; }

        [ReceivesDependency]
        private IGameSession GameSession { get; set; }

        [ReceivesDependency]
        private IInputManager InputManager { get; set; }

        [ReceivesDependency]
        private IRoot3D Root3D { get; set; }

        [ReceivesDependency]
        private ITemporaryStore TemporaryStore { get; set; }

        [ReceivesDependency]
        private IRecordStore RecordStore { get; set; }


        public LocalPlayerInputter(HitBarDisplay hitBar, HitObjectHolder hitObjectHolder)
        {
            this.hitBar = hitBar;
            this.hitObjectHolder = hitObjectHolder;

            hitBarCursor = CreateCursor();
            hitBar.LinkCursor(hitBarCursor);

            keyRecycler = new ManagedRecycler<BeatsKey>(CreateKey);
            keyRecycler.Precook(3);
        }

        [InitWithDependency]
        private void Init()
        {
            GameSession.OnSoftInit += OnSoftInit;
            GameSession.OnSoftDispose += OnSoftDispose;
            GameSession.OnHardDispose += OnHardDispose;

            pointerEvent = new PointerEventData(Root3D.EventSystem);

            // Initialize replay input saver via another task due to potential spike in fps.
            Model.AddAsLoader(new ManualTask(InitReplayInputSaver));
        }

        public bool ProcessInput()
        {
            float curTime = hitObjectHolder.CurrentTime;
                
            // Process beats cursor aiming.
            if (hitBarCursor.IsActive)
            {
                // Aiming on hit bar?
                if (IsOnHitBar(hitBarCursor.Input, out float pos))
                {
                    hitBarCursor.HitBarPos = pos;
                    hitBarCursor.IsOnHitBar.Value = true;
                }
                else
                {
                    hitBarCursor.IsOnHitBar.Value = false;
                }

                // Check all key strokes whether the cursor is within the hit object boundary.
                foreach (var key in keyRecycler.ActiveObjects)
                {
                    if (key.IsActive && key.DraggerView != null)
                    {
                        key.DraggerView.StartCircle.SetHold(key.DraggerView.IsCursorInRange(pos), curTime);
                    }
                }
            }

            // Process new input presses only if not paused.
            if (!GameSession.IsPaused)
            {
                var cursors = InputManager.GetCursors();
                foreach (var cursor in cursors)
                {
                    if (cursor.State.Value == InputState.Press)
                    {
                        // If hit on the hit bar, register this as a new BeatsCursor.
                        if (!hitBarCursor.IsActive && IsOnHitBar(cursor, out float pos))
                            TriggerCursorPress(cursor, pos);
                        // If not hit on hit bar, this is treated as a key stoke.
                        else
                            TriggerKeyPress(cursor, cursor);
                    }

                    // Record replay input data
                    if (replayInputSaver != null)
                        RecordInputData(curTime, cursor);
                }
            }
            return true;
        }

        public void JudgePassive(float curTime, HitObjectView view)
        {
            foreach(var judgement in view.JudgePassive(curTime))
                AddJudgement(judgement);
        }

        int IComparable<IInputReceiver>.CompareTo(IInputReceiver other) => other.InputLayer.CompareTo(InputLayer);

        void IInputReceiver.PrepareInputSort() { }

        /// <summary>
        /// Initializes the replay input saver instance.
        /// </summary>
        private void InitReplayInputSaver(ManualTask task)
        {
            // TODO: Handle only if saving replay option is enabled.
            // {
            //     task.SetFinished();
            //     return;
            // }

            replayInputSaver = new DataStreamSaver<ReplayableInput>(InputManager.MaxTouchCount * 60, 500);
            for (int i = 0; i < replayInputSaver.RawBuffer.Length; i++)
                replayInputSaver.RawBuffer[i] = new ReplayableInput();
            task.SetFinished();
        }

        /// <summary>
        /// Records the specified input for replay feature.
        /// </summary>
        private void RecordInputData(float curTime, ICursor cursor)
        {
            var nextData = replayInputSaver.RawBuffer[replayInputSaver.NextPushIndex];
            nextData.SetFromCursor(curTime, cursor);
            replayInputSaver.PushData(nextData);
        }

        /// <summary>
        /// Triggers a new hit bar cursor aiming event.
        /// </summary>
        private void TriggerCursorPress(ICursor cursor, float pos)
        {
            hitBarCursor.OnRecycleNew();
            hitBarCursor.Input = cursor;
            hitBarCursor.HitBarPos = pos;
            OnCursorPress?.Invoke(hitBarCursor);

            // Cursor press should be treated as key stroke
            // TODO: This can be overridden by configurations.
            TriggerKeyPress(cursor);
        }

        /// <summary>
        /// Triggers a new key stroke press event for specified input.
        /// </summary>
        private void TriggerKeyPress(IInput input, ICursor cursor = null)
        {
            var beatsKey = keyRecycler.GetNext();
            beatsKey.Input = input;
            beatsKey.SetInputAsCursor(cursor);
            // Associate the key with the hit cursor.
            if(hitBarCursor.IsActive)
                beatsKey.SetHitCursor(hitBarCursor);

            OnKeyPress?.Invoke(beatsKey);
            JudgeKeyPress(beatsKey);
        }

        /// <summary>
        /// Returns whether the specified cursor was on hit bar on press.
        /// Outputs position relative to the hit bar sprite, if the cursor is on the hit bar.
        /// </summary>
        private bool IsOnHitBar(ICursor cursor, out float position)
        {
            position = 0;
            pointerEvent.position = cursor.RawPosition;

            Root3D.Raycaster.Raycast(pointerEvent, raycastResults);

            foreach (var result in raycastResults)
            {
                if (result.gameObject == hitBar.gameObject)
                {
                    position = hitBar.transform.InverseTransformPoint(result.worldPosition).x;
                    raycastResults.Clear();
                    return true;
                }
            }
            raycastResults.Clear();
            return false;
        }

        /// <summary>
        /// Creates a new beats cursor instance.
        /// </summary>
        private BeatsCursor CreateCursor()
        {
            var cursor = new BeatsCursor();
            cursor.OnRelease += () => OnCursorStateRelease(cursor);
            return cursor;
        }

        /// <summary>
        /// Creates a new beats key instance.
        /// </summary>
        private BeatsKey CreateKey()
        {
            var key = new BeatsKey();
            key.OnRelease += () => OnKeyStateRelease(key);
            return key;
        }

        /// <summary>
        /// Judges the specified key press against hit objects.
        /// </summary>
        private void JudgeKeyPress(BeatsKey key)
        {
            // Return if no cursor is active.
            if(!hitBarCursor.IsActive)
                return;

            // Find the first hit object where the cursor is within the X range.
            foreach (var objView in hitObjectHolder.GetActiveObjects())
            {
                if (objView.IsCursorInRange(hitBarCursor.HitBarPos))
                {
                    // Associate the hit object view with the key stroke.
                    if(objView is DraggerView draggerView)
                        key.DraggerView = draggerView;
                    AddJudgement(objView.JudgeInput(hitObjectHolder.CurrentTime, key.Input));
                    break;
                }
            }
        }

        /// <summary>
        /// Judges the specified key release against hit objects.
        /// </summary>
        private void JudgeKeyRelease(BeatsKey key)
        {
            if (key.DraggerView == null)
                return;

            key.DraggerView.StartCircle.SetHold(false, hitObjectHolder.CurrentTime);
        }

        /// <summary>
        /// Adds the specified judgement result to the score processor.
        /// </summary>
        private void AddJudgement(JudgementResult result)
        {
            if (result != null)
            {
                if(hitBarCursor.IsActive)
                    hitBarCursor.ReportNewResult(result);

                GameSession?.ScoreProcessor.ProcessJudgement(result);
            }
        }

        /// <summary>
        /// Event called on cursor release state.
        /// </summary>
        private void OnCursorStateRelease(BeatsCursor cursor)
        {
            OnCursorRelease?.Invoke(cursor);
            cursor.OnRecycleDestroy();
        }

        /// <summary>
        /// Event called on key stroke release state.
        /// </summary>
        private void OnKeyStateRelease(BeatsKey key)
        {
            OnKeyRelease?.Invoke(key);
            JudgeKeyRelease(key);
            
            keyRecycler.Return(key);
        }

        /// <summary>
        /// Event called on game session soft initialization.
        /// </summary>
        private void OnSoftInit()
        {
            InputManager.AddReceiver(this);
            
            // Setup input saver
            if (replayInputSaver != null)
            {
                replayFile = TemporaryStore.GetReplayDataFile(Guid.NewGuid().ToString());
                replayWriteStream = new StreamWriter(replayFile.OpenWrite());
                replayInputSaver.StartStream(replayWriteStream);
            }
        }

        /// <summary>
        /// Event called on game session soft disposal.
        /// </summary>
        private void OnSoftDispose()
        {
            hitBarCursor.OnRecycleDestroy();
            keyRecycler.ReturnAll();
            raycastResults.Clear();
            InputManager.RemoveReceiver(this);

            if (replayInputSaver != null)
                replayInputSaver.StopStream();
            if (replayWriteStream != null)
                replayWriteStream.Dispose();
            replayWriteStream = null;
        }

        /// <summary>
        /// Event called on game session hard disposal.
        /// </summary>
        private void OnHardDispose()
        {
            hitBar.UnlinkCursor();
            
            hitBar = null;
            hitObjectHolder = null;
            hitBarCursor = null;
            keyRecycler = null;
            pointerEvent = null;
            raycastResults = null;

            if (replayFile != null)
            {
                var lastRecord = Model.LastRecord;
                if (lastRecord != null && lastRecord.IsClear)
                {
                    var replayFileDest = RecordStore.GetReplayFile(lastRecord);
                    replayFile.MoveTo(replayFileDest.FullName);
                }
            }

            GameSession.OnSoftInit -= OnSoftInit;
            GameSession.OnSoftDispose -= OnSoftDispose;
            GameSession.OnHardDispose -= OnHardDispose;
        }
    }
}