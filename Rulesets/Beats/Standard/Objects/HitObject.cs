using System;
using System.Collections.Generic;
using PBGame.Audio;
using PBGame.Rulesets.Maps;
using PBGame.Rulesets.Maps.ControlPoints;
using PBGame.Rulesets.Beats.Standard.Judgements;
using PBGame.Rulesets.Objects;

namespace PBGame.Rulesets.Beats.Standard.Objects
{
	/// <summary>
	/// Base hit object in Beats game mode.
	/// </summary>
	public abstract class HitObject : Rulesets.Objects.HitObject, IHasCombo, IHasPositionX {

		/// <summary>
		/// The base radius of the hit object at circle size 0.
		/// </summary>
		public const float BaseRadius = 108;

		/// <summary>
		/// Amount of time to fall before reaching a perfect timing for hit.
		/// </summary>
		public double FallTime { get; set; } = 1800;

		/// <summary>
		/// Returns the radius of the object.
		/// </summary>
		public float Radius => BaseRadius * Scale;

		public float X { get; set; }

		public float Scale { get; set; }

		public bool IsNewCombo { get; set; }

		public int ComboOffset { get; set; }


		protected override void ApplyMapPropertiesSelf (ControlPointGroup controlPoints, MapDifficulty difficulty)
		{
			base.ApplyMapPropertiesSelf (controlPoints, difficulty);

			// Angle 50: 1800, 1200, 600);
			// Angle 55: 2400, 1800, 900);
			FallTime = MapDifficulty.GetDifficultyValue(difficulty.ApproachRate, 1800, 1200, 600);

			Scale = (float)MapDifficulty.GetDifficultyValue(difficulty.CircleSize, 1, 0.85, 0.7);
		}

		protected override Rulesets.Judgements.HitTiming CreateHitTiming() { return new HitTiming(); }
	}
}