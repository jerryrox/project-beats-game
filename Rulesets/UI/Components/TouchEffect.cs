using PBGame.Graphics;
using PBGame.Rulesets.Inputs;
using PBGame.Rulesets.Judgements;
using PBFramework.Inputs;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Allocation.Recyclers;
using PBFramework.Dependencies;

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
            UnbindCursor();
            BindCursor(cursor, resultReporter);

            // Set initial position.
            Position = cursor.Position;

            showAni?.PlayFromStart();
        }

        /// <summary>
        /// Forcefully hides the touch effect while disintegrating with the current cursor.
        /// </summary>
        public void Hide()
        {
            UnbindCursor();

            if (Active)
            {
                showAni?.Pause();
                hideAni?.PlayFromStart();
            }
        }

        void IRecyclable.OnRecycleNew()
        {
            Active = true;
        }

        void IRecyclable.OnRecycleDestroy()
        {
            Active = false;
            UnbindCursor();
            showAni?.Pause();
            hideAni?.Pause();
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
            HitResultType type = result.HitResult;
            if(type == HitResultType.Miss)
                return;

            // Show a pulse effect at the current position.
            var effect = PulseRecycler.GetNext();
            effect.Show(myTransform.position, ColorPreset.GetHitResultColor(type));
        }

        protected void Update()
        {
            if(cursor == null)
                return;

            Position = cursor.Position;
        }

        /// <summary>
        /// Binds cursor and result reporter events and stores their reference.
        /// </summary>
        private void BindCursor(ICursor cursor, IInputResultReporter resultReporter)
        {
            this.cursor = cursor;
            cursor.State.Bind(OnCursorStateChange);

            this.resultReporter = resultReporter;
            if (resultReporter != null)
                resultReporter.OnResult += OnInputResult;
        }

        /// <summary>
        /// Unbinds cursor and result reporter events and removes references to them.
        /// </summary>
        private void UnbindCursor()
        {
            if(cursor != null)
                cursor.State.Unbind(OnCursorStateChange);
            cursor = null;

            if (resultReporter != null)
                resultReporter.OnResult -= OnInputResult;
            resultReporter = null;
        }
    }
}