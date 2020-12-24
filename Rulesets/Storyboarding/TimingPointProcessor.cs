using System;
using System.Collections.Generic;
using PBGame.Rulesets.Maps;
using PBGame.Rulesets.Maps.Timing;
using PBGame.Rulesets.Maps.ControlPoints;
using PBFramework.Data.Bindables;

namespace PBGame.Rulesets.Storyboarding
{
    public class TimingPointProcessor : IDisposable
    {
        private IPlayableMap map;
        private IList<BreakPoint> breakPoints;
        private IList<EffectControlPoint> effectPoints;

        private int curBreakIndex;
        private int curEffectIndex;
        private BreakPoint nextBreakPoint;
        private EffectControlPoint nextEffectPoint;

        private BindableBool isBreakPoint = new BindableBool(false);
        private BindableBool isHighlight = new BindableBool(false);


        /// <summary>
        /// Whether the processor is currently initialized with a playable map.
        /// </summary>
        public bool IsInitialized { get; private set; }

        /// <summary>
        /// Returns whether it's currently break point.
        /// </summary>
        public IReadOnlyBindable<bool> IsBreakPoint => isBreakPoint;

        /// <summary>
        /// Returns whether it's currently highlight mode.
        /// </summary>
        public IReadOnlyBindable<bool> IsHighlight => isHighlight;


        /// <summary>
        /// Initializes the processor for the specified map.
        /// </summary>
        public void Initialize(IPlayableMap map)
        {
            IsInitialized = true;

            this.map = map;
            this.breakPoints = map.BreakPoints;
            this.effectPoints = map.ControlPoints.EffectPoints;

            Reset();
        }

        /// <summary>
        /// Resets the storyboard playback to initial state.
        /// </summary>
        public void Reset()
        {
            curBreakIndex = 0;
            curEffectIndex = 0;
            nextBreakPoint = breakPoints.Count > 0 ? breakPoints[0] : null;
            nextEffectPoint = effectPoints.Count > 0 ? effectPoints[0] : null;
            isBreakPoint.Value = false;
            isHighlight.Value = false;
        }

        /// <summary>
        /// Disposes the processor so it can't be used until re-initialization.
        /// </summary>
        public void Dispose()
        {
            IsInitialized = false;

            map = null;
            breakPoints = null;
            effectPoints = null;
            nextBreakPoint = null;
            nextEffectPoint = null;
        }

        /// <summary>
        /// Updates the timing point storyboard for the specified current time.
        /// </summary>
        public void Update(float currentTime)
        {
            if (!IsInitialized)
                return;
            
            if (nextBreakPoint != null)
            {
                if (currentTime >= nextBreakPoint.EndTime)
                {
                    isBreakPoint.Value = false;
                    AdvanceBreakPoint();
                }
                else if (currentTime >= nextBreakPoint.StartTime && !isBreakPoint.Value)
                {
                    isBreakPoint.Value = true;
                }
            }

            if (nextEffectPoint != null)
            {
                if (currentTime >= nextEffectPoint.Time)
                {
                    isHighlight.Value = nextEffectPoint.IsHighlight;
                    AdvanceEffectPoint();
                }
            }
        }

        /// <summary>
        /// Advances to the next break point.
        /// </summary>
        private void AdvanceBreakPoint()
        {
            curBreakIndex++;
            nextBreakPoint = curBreakIndex < breakPoints.Count ? breakPoints[curBreakIndex] : null;
        }

        /// <summary>
        /// Advances to the next effect point.
        /// </summary>
        private void AdvanceEffectPoint()
        {
            curEffectIndex++;
            nextEffectPoint = curEffectIndex < effectPoints.Count ? effectPoints[curEffectIndex] : null;
        }
    }
}