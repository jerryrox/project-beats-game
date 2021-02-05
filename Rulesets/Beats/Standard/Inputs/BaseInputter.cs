using System;
using System.IO;
using System.Collections.Generic;
using PBGame.UI.Models;
using PBGame.Graphics;
using PBGame.Rulesets.Beats.Standard.UI;
using PBGame.Rulesets.Beats.Standard.UI.Components;
using PBFramework.Inputs;
using PBFramework.Allocation.Recyclers;
using PBFramework.Dependencies;
using UnityEngine.EventSystems;

namespace PBGame.Rulesets.Beats.Standard.Inputs
{
    public abstract class BaseInputter : IGameInputter {
    
        public event Action<BeatsCursor> OnCursorPress;
        public event Action<BeatsCursor> OnCursorRelease;
        public event Action<BeatsKey> OnKeyPress;
        public event Action<BeatsKey> OnKeyRelease;

        protected HitBarDisplay hitBar;
        protected HitObjectHolder hitObjectHolder;

        protected ManagedRecycler<BeatsKey> keyRecycler;
        protected PointerEventData pointerEvent;
        protected List<RaycastResult> raycastResults = new List<RaycastResult>();

        protected BeatsCursor hitBarCursor;
        protected FileInfo replayFile;

        protected BeatsStandardProcessor gameProcessor;


        public BeatsCursor HitBarCursor => hitBarCursor;

        public List<BeatsKey> KeyInputs => keyRecycler.ActiveObjects;

        public FileInfo ReplayFile => replayFile;

        [ReceivesDependency]
        protected GameModel Model { get; set; }

        [ReceivesDependency]
        protected IGameSession GameSession { get; set; }

        [ReceivesDependency]
        protected IRoot3D Root3D { get; set; }


        protected BaseInputter(HitBarDisplay hitBar, HitObjectHolder hitObjectHolder)
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
        }

        public void SetGameProcessor(BeatsStandardProcessor gameProcessor)
        {
            this.gameProcessor = gameProcessor;
        }

        /// <summary>
        /// Invokes the OnCursorPress event.
        /// </summary>
        protected void InvokeCursorPress(BeatsCursor cursor) => OnCursorPress?.Invoke(cursor);

        /// <summary>
        /// Invokes the OnCursorRelease event.
        /// </summary>
        protected void InvokeCursorRelease(BeatsCursor cursor) => OnCursorRelease?.Invoke(cursor);

        /// <summary>
        /// Invokes the OnKeyPress event.
        /// </summary>
        protected void InvokeKeyPress(BeatsKey key) => OnKeyPress?.Invoke(key);

        /// <summary>
        /// Invokes the OnKeyRelease event.
        /// </summary>
        protected void InvokeKeyRelease(BeatsKey key) => OnKeyRelease?.Invoke(key);

        /// <summary>
        /// Event called on game session soft initialization.
        /// </summary>
        protected virtual void OnSoftInit() { }

        /// <summary>
        /// Event called on game session soft disposal.
        /// </summary>
        protected virtual void OnSoftDispose()
        {
            hitBarCursor.OnRecycleDestroy();
            keyRecycler.ReturnAll();
            raycastResults.Clear();
        }

        /// <summary>
        /// Event called on game session hard disposal.
        /// </summary>
        protected virtual void OnHardDispose()
        {
            GameSession.OnSoftInit -= OnSoftInit;
            GameSession.OnSoftDispose -= OnSoftDispose;
            GameSession.OnHardDispose -= OnHardDispose;

            hitBar.UnlinkCursor();
            
            hitBar = null;
            hitObjectHolder = null;
            hitBarCursor = null;
            keyRecycler = null;
            pointerEvent = null;
            raycastResults = null;
        }

        /// <summary>
        /// Returns whether the specified cursor was on hit bar on press.
        /// Outputs position relative to the hit bar sprite, if the cursor is on the hit bar.
        /// </summary>
        protected bool IsOnHitBar(ICursor cursor, out float position)
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
        /// Triggers a new hit bar cursor aiming event.
        /// Returns the key that was newly pressed.
        /// </summary>
        protected BeatsKey TriggerCursorPress(float time, ICursor cursor, float pos)
        {
            hitBarCursor.OnRecycleNew();
            hitBarCursor.Input = cursor;
            hitBarCursor.HitBarPos = pos;
            InvokeCursorPress(hitBarCursor);

            // Cursor press should be treated as key stroke
            // TODO: This can be overridden by configurations.
            return TriggerKeyPress(time, cursor);
        }

        /// <summary>
        /// Triggers a new key stroke press event for specified input.
        /// Returns the key that was newly pressed.
        /// </summary>
        protected BeatsKey TriggerKeyPress(float time, IInput input, ICursor cursor = null)
        {
            var beatsKey = keyRecycler.GetNext();
            beatsKey.Input = input;
            beatsKey.SetInputAsCursor(cursor);
            // Associate the key with the hit cursor.
            if(hitBarCursor.IsActive)
                beatsKey.SetHitCursor(hitBarCursor);

            InvokeKeyPress(beatsKey);
            return beatsKey;
        }

        /// <summary>
        /// Event called on key stroke release state.
        /// </summary>
        protected virtual void OnKeyStateRelease(BeatsKey key)
        {
            InvokeKeyRelease(key);
            
            keyRecycler.Return(key);
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
        /// Event called on cursor release state.
        /// </summary>
        private void OnCursorStateRelease(BeatsCursor cursor)
        {
            InvokeCursorRelease(cursor);
            cursor.OnRecycleDestroy();
        }
    }
}