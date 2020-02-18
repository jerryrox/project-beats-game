using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Rulesets.Objects;

using HitObject = PBGame.Rulesets.Osu.Standard.Objects.HitObject;
using BaseHitObject = PBGame.Rulesets.Objects.HitObject;

namespace PBGame.Rulesets.Osu.Standard.Maps
{
    public class MapConverter : Rulesets.Maps.MapConverter<HitObject> {

        public override GameModes TargetMode => GameModes.OsuStandard;

        protected override IEnumerable<Type> RequiredTypes { get { yield return typeof(IHasPosition); } }


        public MapConverter(Rulesets.Maps.IOriginalMap map) : base(map) {}

        protected override Rulesets.Maps.PlayableMap<HitObject> CreateMap(Rulesets.Maps.IOriginalMap map) => null;//new Map();

        protected override IEnumerable<HitObject> ConvertHitObjects(BaseHitObject hitObject)
        {
            var position = hitObject as IHasPosition;
            var combo = hitObject as IHasCombo;
            var curve = hitObject as IHasCurve;
            var endTime = hitObject as IHasEndTime;

            // TODO:
            yield return null;

            // if (curve != null)
            // {
            //     yield return new Slider()
            //     {

            //     };
            // }
            // else if (endTime != null)
            // {
            //     yield return new Spinner()
            //     {
            //     };
            // }
            // else
            // {
            //     yield return new HitCircle()
            //     {

            //     };
            // }
        }
    }
}

/*

    public class OsuBeatmapConverter : BeatmapConverter<OsuHitObject>
    {
        public OsuBeatmapConverter(IBeatmap beatmap, Ruleset ruleset)
            : base(beatmap, ruleset)
        {
        }

        public override bool CanConvert() => Beatmap.HitObjects.All(h => h is IHasPosition);

        protected override IEnumerable<OsuHitObject> ConvertHitObject(HitObject original, IBeatmap beatmap)
        {
            var positionData = original as IHasPosition;
            var comboData = original as IHasCombo;

            switch (original)
            {
                case IHasCurve curveData:
                    return new Slider
                    {
                        StartTime = original.StartTime,
                        Samples = original.Samples,
                        Path = curveData.Path,
                        NodeSamples = curveData.NodeSamples,
                        RepeatCount = curveData.RepeatCount,
                        Position = positionData?.Position ?? Vector2.Zero,
                        NewCombo = comboData?.NewCombo ?? false,
                        ComboOffset = comboData?.ComboOffset ?? 0,
                        LegacyLastTickOffset = (original as IHasLegacyLastTickOffset)?.LegacyLastTickOffset,
                        // prior to v8, speed multipliers don't adjust for how many ticks are generated over the same distance.
                        // this results in more (or less) ticks being generated in <v8 maps for the same time duration.
                        TickDistanceMultiplier = beatmap.BeatmapInfo.BeatmapVersion < 8 ? 1f / beatmap.ControlPointInfo.DifficultyPointAt(original.StartTime).SpeedMultiplier : 1
                    }.Yield();

                case IHasEndTime endTimeData:
                    return new Spinner
                    {
                        StartTime = original.StartTime,
                        Samples = original.Samples,
                        EndTime = endTimeData.EndTime,
                        Position = positionData?.Position ?? OsuPlayfield.BASE_SIZE / 2,
                        NewCombo = comboData?.NewCombo ?? false,
                        ComboOffset = comboData?.ComboOffset ?? 0,
                    }.Yield();

                default:
                    return new HitCircle
                    {
                        StartTime = original.StartTime,
                        Samples = original.Samples,
                        Position = positionData?.Position ?? Vector2.Zero,
                        NewCombo = comboData?.NewCombo ?? false,
                        ComboOffset = comboData?.ComboOffset ?? 0,
                    }.Yield();
            }
        }

        protected override Beatmap<OsuHitObject> CreateBeatmap() => new OsuBeatmap();
*/