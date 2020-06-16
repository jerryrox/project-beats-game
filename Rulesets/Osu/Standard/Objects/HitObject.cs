using System;
using System.Linq;
using PBGame.Rulesets.Maps;
using PBGame.Rulesets.Maps.ControlPoints;
using PBGame.Rulesets.Objects;
// using PBGame.Rulesets.Osu.Judgements;
using PBFramework.Data.Bindables;
using UnityEngine;

namespace PBGame.Rulesets.Osu.Standard.Objects
{
    public abstract class HitObject : Rulesets.Objects.BaseHitObject, IHasComboExtended, IHasPosition {

        /// <summary>
        /// The base radius of the hit object at cs 0.
        /// </summary>
        public const float BaseRadius = 64;

        /// <summary>
        /// Distance of speed-adjusted 1 second.
        /// </summary>
        public const float BaseScoreDistance = 100f;

        private BindableInt bindableStackHeight = new BindableInt();


        public virtual Vector2 Position { get; set; }

        public float X => Position.x;

        public float Y => Position.y;

        public bool IsNewCombo { get; set; }

        public int ComboOffset { get; set; }

        public int IndexInCombo { get; set; }

        public int ComboIndex { get; set; }

        public bool IsLastInCombo { get; set; }

        /// <summary>
        /// Amount of time it takes an object to reach hit time since appearance.
        /// </summary>
        public float ApproachTime { get; set; } = 600f;

        /// <summary>
        /// The base time for object fade in.
        /// </summary>
        public float FadeInTime { get; set; } = 400f;

        /// <summary>
        /// Returns the position of the object at its end time.
        /// </summary>
        public virtual Vector2 EndPosition => Position;

        /// <summary>
        /// Returns the position after applying stack offset.
        /// </summary>
        public Vector2 StackedPosition => Position + StackOffset;

        /// <summary>
        /// Returns the end position after applying stack offset.
        /// </summary>
        public Vector2 StackedEndPosition => EndPosition + StackOffset;

        /// <summary>
        /// Object stacking factor.
        /// </summary>
        public int StackHeight
        {
            get => bindableStackHeight.Value;
            set => bindableStackHeight.Value = value;
        }

        /// <summary>
        /// Returns the offset applied to next stacked object at the same position.
        /// </summary>
        public Vector2 StackOffset
        {
            get
            {
                var offset = StackHeight * Scale * -6.4f;
                return new Vector2(offset, offset);
            }
        }

        /// <summary>
        /// Returns the actual radius after applying CS.
        /// </summary>
        public float Radius => BaseRadius * Scale;

        /// <summary>
        /// Object scale factor.
        /// </summary>
        public float Scale { get; set; }


        protected HitObject()
        {
            // Make sure stack height is applied to all children.
            bindableStackHeight.OnNewValue += (height) =>
            {
                foreach(var nested in NestedObjects.OfType<HitObject>())
                    nested.StackHeight = height;
            };
        }

        protected override void ApplyMapPropertiesSelf(ControlPointGroup controlPoints, MapDifficulty difficulty)
        {
            base.ApplyMapProperties(controlPoints, difficulty);

            ApproachTime = (float)MapDifficulty.GetDifficultyValue(difficulty.ApproachRate, 1800f, 1200f, 450f);
            FadeInTime = 400f;

            Scale = (1f - 0.7f * ((float)difficulty.CircleSize - 5) / 5) / 2;
        }

        // TODO:
        protected override Rulesets.Judgements.HitTiming CreateHitTiming() => null;//new HitTiming();
    }
}
