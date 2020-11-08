using PBGame.Graphics;
using PBGame.Rulesets.Inputs;
using PBGame.Rulesets.Judgements;
using PBFramework.Inputs;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Allocation.Recyclers;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.Rulesets.UI.Components
{
    public abstract class TouchEffect : UguiObject, IRecyclable
    {

        protected ICursor cursor;
        protected IInputResultReporter resultReporter;

        protected IAnime showAni;
        protected IAnime hideAni;


        /// <summary>
        /// Recycler instance for touch pulse effects.
        /// </summary>
        public IRecycler<TouchPulseEffect> PulseRecycler { get; set; }

        [ReceivesDependency]
        private IColorPreset ColorPreset { get; set; }


        /// <summary>
        /// Shows the touch effect for the specified cursor.
        /// </summary>
        public void Show(ICursor cursor, IInputResultReporter resultReporter)
        {
            this.cursor = cursor;
            this.resultReporter = resultReporter;

            // Set initial position.
            myTransform.position = cursor.RawPosition;

            // Listen to input release state.
            cursor.State.Bind(OnCursorStateChange);

            // Listen to new judgement results from this input.
            if (resultReporter != null)
                resultReporter.OnResult += OnInputResult;

            showAni?.PlayFromStart();
        }

        /// <summary>
        /// Forcefully hides the touch effect while disintegrating with the current cursor.
        /// </summary>
        public void Hide()
        {
            if (cursor == null)
                return;

            cursor.State.Unbind(OnCursorStateChange);
            cursor = null;

            if (resultReporter != null)
                resultReporter.OnResult -= OnInputResult;
            resultReporter = null;

            showAni?.Stop();
            hideAni?.PlayFromStart();
        }

        void IRecyclable.OnRecycleNew()
        {
            Active = true;
        }

        void IRecyclable.OnRecycleDestroy()
        {
            Active = false;
            showAni?.Stop();
            hideAni?.Stop();
        }

        /// <summary>
        /// Event called on cursor input state change.
        /// </summary>
        protected virtual void OnCursorStateChange(InputState state)
        {
            if(state == InputState.Release)
                Hide();
        }

        /// <summary>
        /// Event called on new input-triggerd judgement.
        /// </summary>
        protected virtual void OnInputResult(JudgementResult result)
        {
            // Show a pulse effect at the current position.
            var effect = PulseRecycler.GetNext();
            effect.Show(myTransform.position, ColorPreset.GetHitResultColor(result.HitResult));
        }

        protected void Update()
        {
            if(cursor == null)
                return;

            myTransform.position = cursor.RawPosition;
        }
    }
}