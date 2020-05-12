using System;
using System.Collections.Generic;
using PBGame.Rulesets.Beats.Standard.Objects;
using PBGame.Rulesets.Objects;
using PBGame.Audio;
using UnityEngine;

using HitObject = PBGame.Rulesets.Beats.Standard.Objects.HitObject;
using BaseHitObject = PBGame.Rulesets.Objects.BaseHitObject;

namespace PBGame.Rulesets.Beats.Standard.Maps
{
	public class MapConverter : Rulesets.Maps.MapConverter<HitObject> {

		/// <summary>
		/// Size of the area where the objects are spawned.
		/// </summary>
        public const float AreaSize = 1400f - HitObject.BaseRadius * 2f;

        private Func<float, float> convertPosX;
        private Func<float, float> convertPathPosX;
        private bool hasCustomPosConverter = false;


        public override GameModeType TargetMode { get { return GameModeType.BeatsStandard; } }

		protected override IEnumerable<Type> RequiredTypes { get { yield return typeof(IHasPositionX); } }


		public MapConverter(Rulesets.Maps.IOriginalMap map) : base(map)
		{
            switch (map.Detail.GameMode)
            {
				case GameModeType.OsuStandard:
					convertPosX = ConvertFromOsu;
                    convertPathPosX = ConvertPathFromOsu;
                    hasCustomPosConverter = true;
                    break;
				default:
					convertPosX = ConvertDefault;
                    convertPathPosX = ConvertDefault;
                    break;
            }
        }

		protected override Rulesets.Maps.PlayableMap<HitObject> CreateMap(Rulesets.Maps.IOriginalMap map) => new Map(map);

		protected override IEnumerable<HitObject> ConvertHitObjects(BaseHitObject hitObject)
		{
			IHasCurve curve = hitObject as IHasCurve;
			IHasPositionX posX = hitObject as IHasPositionX;
			IHasEndTime endTime = hitObject as IHasEndTime;
			IHasCombo combo = hitObject as IHasCombo;

            if(curve != null)
			{
                // Regenerate path using conversion method.
                SliderPath newPath = curve.Path;
                if (hasCustomPosConverter)
                {
                    Vector2[] origPoints = newPath.Points;
                    Vector2[] points = new Vector2[origPoints.Length];
                    for (int i = 0; i < points.Length; i++)
                    {
                        points[i] = origPoints[i];
                        points[i].x = convertPathPosX(points[i].x);
                    }
					// Final path
                    newPath = new SliderPath(newPath.PathType, points);
                }

                yield return new Dragger() {
					StartTime = hitObject.StartTime,
					Samples = hitObject.Samples,
					X = convertPosX(posX.X),
					RepeatCount = curve.RepeatCount,
					IsNewCombo = (combo == null ? false : combo.IsNewCombo),
					ComboOffset = (combo == null ? 0 : combo.ComboOffset),
					Path = newPath,
					NodeSamples = curve.NodeSamples,
					EndTime = curve.EndTime
				};
			}
			else if(endTime != null)
			{
				yield return new Dragger() {
					StartTime = hitObject.StartTime,
					Samples = new List<SoundInfo>(),
					X = convertPosX(posX.X),
					RepeatCount = 0,
					IsNewCombo = (combo == null ? false : combo.IsNewCombo),
					ComboOffset = (combo == null ? 0 : combo.ComboOffset),
					Path = new SliderPath(PathType.Linear, new Vector2[] {
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
					X = convertPosX(posX.X),
					IsNewCombo = (combo == null ? false : combo.IsNewCombo),
					ComboOffset = (combo == null ? 0 : combo.ComboOffset)
				};
			}
		}

		/// <summary>
		/// Converts the specified X position to Beats standard coordinate, assuming an osu context.
		/// </summary>
        private float ConvertFromOsu(float x)
        {
			// TODO: Replace 512 with osu's play area size constant value.
            x *= AreaSize / 512f;
            x -= AreaSize * 0.5f;
            return x;
        }

		/// <summary>
		/// Converts the specified path X posiiton to Beats standard coordinate, assuming an osu context.
		/// </summary>
		private float ConvertPathFromOsu(float x)
        {
            // TODO: Replace 512 with osu's play area size constant value.
            return x * AreaSize / 512f;
        }

		/// <summary>
		/// Default conversion provider.
		/// </summary>
        private float ConvertDefault(float x) => x;
    }
}

