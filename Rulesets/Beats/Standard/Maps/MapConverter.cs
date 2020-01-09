using System;
using System.Collections.Generic;
using PBGame.Rulesets.Beats.Standard.Objects;
using PBGame.Rulesets.Objects;
using PBGame.Audio;
using UnityEngine;

using HitObject = PBGame.Rulesets.Beats.Standard.Objects.HitObject;
using BaseHitObject = PBGame.Rulesets.Objects.HitObject;

namespace PBGame.Rulesets.Beats.Standard.Maps
{
	public class MapConverter : Rulesets.Maps.MapConverter<HitObject> {

		public override GameModes TargetMode { get { return GameModes.BeatsStandard; } }

		protected override IEnumerable<Type> RequiredTypes { get { yield return typeof(IHasPositionX); } }


		public MapConverter(Rulesets.Maps.IMap map) : base(map) {}

		protected override Rulesets.Maps.Map<HitObject> CreateMap() => new Map();

		protected override IEnumerable<HitObject> ConvertHitObjects(BaseHitObject hitObject)
		{
			IHasCurve curve = hitObject as IHasCurve;
			IHasPositionX posX = hitObject as IHasPositionX;
			IHasEndTime endTime = hitObject as IHasEndTime;
			IHasCombo combo = hitObject as IHasCombo;

            float posAdjustment = 0f;
            // If the original map is an osu map, shift the hit object positions by -50% of the play area width.
            if (Map.Detail.GameMode == GameModes.OsuStandard)
            {
                // TODO: Refer to Osu playarea's static variable instead!!
                posAdjustment = -512f / 2f;
            }

            if(curve != null)
			{
				yield return new Dragger() {
					StartTime = hitObject.StartTime,
					Samples = hitObject.Samples,
					X = posX.X + posAdjustment,
					RepeatCount = curve.RepeatCount,
					IsNewCombo = (combo == null ? false : combo.IsNewCombo),
					ComboOffset = (combo == null ? 0 : combo.ComboOffset),
					Path = curve.Path,
					NodeSamples = curve.NodeSamples,
					EndTime = curve.EndTime
				};
			}
			else if(endTime != null)
			{
				yield return new Dragger() {
					StartTime = hitObject.StartTime,
					Samples = new List<SoundInfo>(),
					X = posX.X + posAdjustment,
					RepeatCount = 0,
					IsNewCombo = (combo == null ? false : combo.IsNewCombo),
					ComboOffset = (combo == null ? 0 : combo.ComboOffset),
					Path = new SliderPath(PathTypes.Linear, new Vector2[] {
						new Vector2(0, 0),
						new Vector2(0, 1)
					}, null),
					NodeSamples = new List<List<SoundInfo>>() {
						new List<SoundInfo>(),
						hitObject.Samples
					},
					EndTime = endTime.EndTime
				};
			}
			else
			{
				yield return new HitCircle() {
					StartTime = hitObject.StartTime,
					Samples = hitObject.Samples,
					X = posX.X + posAdjustment,
					IsNewCombo = (combo == null ? false : combo.IsNewCombo),
					ComboOffset = (combo == null ? 0 : combo.ComboOffset)
				};
			}
		}
	}
}

