using PBGame.Rulesets.Maps;
using PBGame.Rulesets.Maps.ControlPoints;
using PBGame.Rulesets.Beats.Standard.Judgements;
using PBGame.Rulesets.Objects;

namespace PBGame.Rulesets.Beats.Standard.Objects
{
	/// <summary>
	/// Base hit object in Beats game mode.
	/// </summary>
	public abstract class HitObject : Rulesets.Objects.BaseHitObject, IHasCombo, IHasPositionX {

		/// <summary>
		/// The base radius of the hit object at circle size 0.
		/// </summary>
		public const float BaseRadius = 125f;

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

			ApproachDuration = MapDifficulty.GetDifficultyValue(difficulty.ApproachRate, 1800, 1200, 600);
			Scale = MapDifficulty.GetDifficultyValue(difficulty.CircleSize, 1, 0.85f, 0.7f);
		}

		protected override Rulesets.Judgements.HitTiming CreateHitTiming() { return new HitTiming(); }
	}
}