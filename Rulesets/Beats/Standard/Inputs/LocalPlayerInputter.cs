using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI;
using PBGame.Rulesets.Beats.Standard.UI.Components;
using PBGame.Graphics;
using PBFramework.UI;
using PBFramework.Inputs;
using PBFramework.Allocation.Recyclers;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PBGame.Rulesets.Beats.Standard.Inputs
{
    public class LocalPlayerInputter : IGameInputter, IInputReceiver
    {
        public event Action<BeatsCursor> OnCursorPress;
        public event Action<BeatsCursor> OnCursorRelease;
        public event Action<BeatsKey> OnKeyPress;
        public event Action<BeatsKey> OnKeyRelease;

        private BeatsCursor hitBarCursor;
        private ManagedRecycler<BeatsKey> keyRecycler;

        private HitBarDisplay hitBar;
        private PointerEventData pointerEvent;
        private List<RaycastResult> raycastResults = new List<RaycastResult>();


        public BeatsCursor HitBarCursor => hitBarCursor;
        public List<BeatsKey> KeyInputs => keyRecycler.ActiveObjects;

        public int InputLayer => InputLayers.GameScreenComponents;

        [ReceivesDependency]
        private IGameSession GameSession { get; set; }

        [ReceivesDependency]
        private IInputManager InputManager { get; set; }

        [ReceivesDependency]
        private IRoot3D Root3D { get; set; }


        public LocalPlayerInputter(HitBarDisplay hitBar)
        {
            this.hitBar = hitBar;

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

        public bool ProcessInput()
        {
            var cursors = InputManager.GetCursors();

            // Process beats cursor aiming.
            if (hitBarCursor.IsActive)
            {
                if (IsOnHitBar(hitBarCursor.Input, out float pos))
                {
                    hitBarCursor.HitBarPos = pos;
                    hitBarCursor.IsOnHitBar.Value = true;
                }
                else
                {
                    hitBarCursor.IsOnHitBar.Value = false;
                }
            }

            // Process new input presses.
            foreach (var cursor in cursors)
            {
                if (cursor.State.Value == InputState.Press)
                {
                    // If hit on the hit bar, register this as a new BeatsCursor.
                    if (IsOnHitBar(cursor, out float pos))
                        TriggerCursorPress(cursor, pos);
                    // If not hit on hit bar, this is treated as a key stoke.
                    else
                        TriggerKeyPress(cursor);
                }
            }
            return true;
        }

        int IComparable<IInputReceiver>.CompareTo(IInputReceiver other) => other.InputLayer.CompareTo(InputLayer);

        void IInputReceiver.PrepareInputSort() { }

        /// <summary>
        /// Triggers a new hit bar cursor aiming event.
        /// </summary>
        private void TriggerCursorPress(ICursor cursor, float pos)
        {
            hitBarCursor.OnRecycleNew();
            hitBarCursor.Input = cursor;
            hitBarCursor.HitBarPos = pos;
            OnCursorPress?.Invoke(hitBarCursor);
        }

        /// <summary>
        /// Triggers a new key stroke press event for specified input.
        /// </summary>
        private void TriggerKeyPress(IInput input)
        {
            var beatsKey = keyRecycler.GetNext();
            beatsKey.Input = input;
            OnKeyPress?.Invoke(beatsKey);
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
            keyRecycler.Return(key);
        }

        /// <summary>
        /// Event called on game session soft initialization.
        /// </summary>
        private void OnSoftInit()
        {
            InputManager.AddReceiver(this);
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
        }

        /// <summary>
        /// Event called on game session hard disposal.
        /// </summary>
        private void OnHardDispose()
        {
            hitBar = null;
            hitBarCursor = null;
            keyRecycler = null;
            pointerEvent = null;
            raycastResults = null;
            hitBar.UnlinkCursor();

            GameSession.OnSoftInit -= OnSoftInit;
            GameSession.OnSoftDispose -= OnSoftDispose;
            GameSession.OnHardDispose -= OnHardDispose;
        }
    }
}