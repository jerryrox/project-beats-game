using System.Linq;
using System.Collections.Generic;
using PBGame.Rulesets.Maps.Timing;
using PBGame.Rulesets.Maps.ControlPoints;
using PBGame.Rulesets.Objects;
using UnityEngine;

namespace PBGame.Rulesets.Maps
{
    public abstract class Map<T> : IMap
        where T : BaseHitObject
    {
        public abstract MapDetail Detail { get; }

        public MapMetadata Metadata => Detail == null ? null : (Detail.Metadata ?? (Detail.Mapset == null ? null : Detail.Mapset.Metadata));

        public abstract ControlPointGroup ControlPoints { get; }

        public abstract List<BreakPoint> BreakPoints { get; }

        public List<T> HitObjects { get; set; } = new List<T>();

        IEnumerable<BaseHitObject> IMap.HitObjects
        {
            get
            {
                foreach (var obj in HitObjects)
                    yield return obj;
            }
        }

        public int ObjectCount => HitObjects.Count;

        public int Duration
        {
            get
            {
                if(HitObjects.Count < 2)
                    return 0;
                var firstObject = HitObjects[0];
                var lastObject = HitObjects[HitObjects.Count - 1];
                if(lastObject is IHasEndTime endTime)
                    return (int)(endTime.EndTime - firstObject.StartTime);
                return (int)(lastObject.StartTime - firstObject.StartTime);
            }
        }

        public float BreakDuration => BreakPoints.Sum(p => p.Duration);

        public abstract bool IsPlayable { get; }

        public abstract List<Color> ComboColors { get; }


        public IEnumerable<string> GetQueryables() => Metadata.GetQueryables();
    }
}