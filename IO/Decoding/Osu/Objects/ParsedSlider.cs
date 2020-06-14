using System;
using System.Collections.Generic;
using PBGame.Audio;
using PBGame.Rulesets.Maps;
using PBGame.Rulesets.Maps.ControlPoints;
using PBGame.Rulesets.Objects;

namespace PBGame.IO.Decoding.Osu.Objects
{
	/// <summary>
	/// Base hit object variant used for parsing sliders.
	/// </summary>
	public abstract class ParsedSlider : BaseHitObject, IHasCurve {

		/// <summary>
		/// Base distance of slider for 1 Beat per second.
		/// </summary>
		private const float BaseDistance = 100;


		public SliderPath Path { get; set; }

		public float Distance { get { return Path.Distance; } }

		public List<List<SoundInfo>> NodeSamples { get; set; }

		public int RepeatCount { get; set; }

		public float EndTime { get { return StartTime + this.SpanCount() * Distance / Speed; } }

		public float Duration { get { return EndTime - StartTime; } }

		public float Speed { get; set; }


		protected override void ApplyMapPropertiesSelf (ControlPointGroup controlPoints, MapDifficulty difficulty)
		{
			base.ApplyMapPropertiesSelf (controlPoints, difficulty);

			TimingControlPoint timingPoint = controlPoints.TimingPointAt(StartTime);
			DifficultyControlPoint difficultyPoint = controlPoints.DifficultyPointAt(StartTime);

			float distance = BaseDistance * difficultyPoint.SpeedMultiplier * difficulty.SliderMultiplier;
			Speed = distance / timingPoint.BeatLength;
		}
	}
}

