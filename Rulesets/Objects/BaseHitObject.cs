using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Audio;
using PBGame.Rulesets.Maps;
using PBGame.Rulesets.Maps.ControlPoints;
using PBGame.Rulesets.Judgements;

namespace PBGame.Rulesets.Objects
{
    public class BaseHitObject {
    
		/// <summary>
		/// Offset applied when querying control point using the object's time.
		/// This is to prevent potential precision errors that may occur when comparing time values.
		/// </summary>
		public const float ControlPointOffset = 1;

		private List<SoundInfo> samples;

		private List<BaseHitObject> nestedObjects = new List<BaseHitObject>();


        /// <summary>
        /// The duration of the approach since appearance.
        /// </summary>
        public float ApproachDuration { get; set; } = 1000f;

        /// <summary>
        /// The starting time of this hit object in milliseconds.
        /// </summary>
        public float StartTime { get; set; }

		/// <summary>
		/// The sample control point which this hit object is grouped in.
		/// </summary>
		public SampleControlPoint SamplePoint { get; set; }

		/// <summary>
		/// Whether this object is an object within the highlight point.
		/// </summary>
		public bool IsHighlight { get; private set; }

		/// <summary>
		/// The timing details of this object.
		/// </summary>
		public HitTiming Timing { get; set; }

		/// <summary>
		/// List of samples to play on hit object hit.
		/// </summary>
		public List<SoundInfo> Samples
		{
			get { return samples ?? (samples = new List<SoundInfo>()); }
			set { samples = value; }
		}

		/// <summary>
		/// Returns the list of hit objects nested inside this object.
		/// </summary>
		public List<BaseHitObject> NestedObjects { get { return nestedObjects; } }


		/// <summary>
		/// Applies specified map properties to this and all nested hit object.
		/// </summary>
		public void ApplyMapProperties(ControlPointGroup controlPoints, MapDifficulty difficulty)
		{
			// Apply map properties to this object first.
			ApplyMapPropertiesSelf(controlPoints, difficulty);

			// Determine which sample point this object belongs to.
			IHasEndTime endTimeObject = this as IHasEndTime;
			SamplePoint = controlPoints.SamplePointAt(
				(endTimeObject != null ? endTimeObject.EndTime : StartTime) + ControlPointOffset
			);

			// Create nested objects if necessary
			NestedObjects.Clear();
			CreateNestedObjects();
			NestedObjects.Sort((x, y) => x.StartTime.CompareTo(y.StartTime));

			// Apply map properties to all nested objects
			foreach(var obj in NestedObjects)
			{
				obj.ApplyMapProperties(controlPoints, difficulty);
			}
		}

		/// <summary>
		/// Creates the judgement information this hit object provides.
		/// </summary>
		public virtual JudgementInfo CreateJudgementInfo() { return null; }

		/// <summary>
		/// Applies map properties to this object only.
		/// </summary>
		protected virtual void ApplyMapPropertiesSelf(ControlPointGroup controlPoints, MapDifficulty difficulty)
		{
			IsHighlight = controlPoints.EffectPointAt(StartTime + ControlPointOffset).IsHighlight;

            ApproachDuration = MapDifficulty.GetDifficultyValue(difficulty.ApproachRate, 1000f, 750f, 500f);

            if(Timing == null)
				Timing = CreateHitTiming();
			if(Timing != null)
				Timing.SetDifficulty(difficulty.OverallDifficulty);
		}

		/// <summary>
		/// Optional function for creating nested hit objects.
		/// </summary>
		protected virtual void CreateNestedObjects() {}

		/// <summary>
		/// Creates a new hit timing info for this object.
		/// </summary>
		protected virtual HitTiming CreateHitTiming() { return new HitTiming(); }

		/// <summary>
		/// Adds specified hit object as nested object.
		/// </summary>
		protected void AddNestedObject(BaseHitObject obj) { NestedObjects.Add(obj); }

    }
}