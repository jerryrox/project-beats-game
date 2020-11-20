using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Rulesets.Beats.Standard.Inputs;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.Rulesets.Beats.Standard.UI
{
    public class TouchEffectDisplay : Rulesets.UI.HUD.TouchEffectDisplay {

        private IGameInputter gameInputter;


        [InitWithDependency]
        private void Init(IGameSession gameSession)
        {
            Dependencies.Cache(this);

            gameSession.OnHardDispose += () =>
            {
                UnbindInputter();
            };
        }

        /// <summary>
        /// Assigns the inputter instance to listen for new touches.
        /// </summary>
        public void SetInputter(IGameInputter inputter)
        {
            UnbindInputter();

            gameInputter = inputter;
            
            gameInputter.OnCursorPress += OnCursorPress;
            gameInputter.OnKeyPress += OnKeyPress;
        }

        /// <summary>
        /// Unbinds association with current game inputter.
        /// </summary>
        private void UnbindInputter()
        {
            if(gameInputter == null)
                return;

            gameInputter.OnCursorPress -= OnCursorPress;
            gameInputter.OnKeyPress -= OnKeyPress;

            gameInputter = null;
        }

        /// <summary>
        /// Event called on new cursor input.
        /// </summary>
        private void OnCursorPress(BeatsCursor cursor)
        {
            ShowPrimary(cursor.Input, cursor);
        }

        /// <summary>
        /// Event called on new key input.
        /// </summary>
        private void OnKeyPress(BeatsKey key)
        {
            var cursor = key.GetInputAsCursor();
            if(cursor != null)
                ShowSecondary(cursor, key);
        }
    }
}