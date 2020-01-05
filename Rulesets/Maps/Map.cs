using System;
using System.Linq;
using System.Collections.Generic;
using PBGame.Rulesets.Maps.Timing;
using PBGame.Rulesets.Maps.ControlPoints;
using PBGame.Rulesets.Objects;
using PBGame.Rulesets.Difficulty;
using UnityEngine;

namespace PBGame.Rulesets.Maps
{
    public class Map<T> : IMap
        where T : HitObject
    {
        public MapDetail Detail { get; set; } = new MapDetail()
        {
            Metadata = new MapMetadata()
            {
                Artist = "Unknown",
                Title = "Unknown",
                Creator = "Unknown",
            },
            Version = "Unknown",
            Difficulty = new MapDifficulty()
        };

        public ControlPointGroup ControlPoints { get; set; } = new ControlPointGroup();

        public GameModes PlayableMode { get; set; }

        public List<BreakPoint> BreakPoints { get; set; } = new List<BreakPoint>();

        public List<T> HitObjects { get; set; } = new List<T>();

        IEnumerable<HitObject> IMap.HitObjects
        {
            get
            {
                foreach(var obj in HitObjects)
                    yield return obj;
            }
        }

        public MapMetadata Metadata => Detail == null ? null : (Detail.Metadata ?? (Detail.Mapset == null ? null : Detail.Mapset.Metadata));

        public List<DifficultyInfo> Difficulties { get; set; } = new List<DifficultyInfo>();

        public double BreakDuration => BreakPoints.Sum(p => p.Duration);

        public List<Color> ComboColors { get; set; } = new List<Color>();


        public IEnumerable<string> GetQueryables() => Metadata.GetQueryables();

        public DifficultyInfo GetDifficulty(GameModes gameMode)
        {
            // Find difficulty for requested game mode.
            var diff = Difficulties.Find(d => d.GameMode == gameMode);
            // If diff is null, fall back to original game mode's difficulty.
            return diff ?? Difficulties.Find(d => d.GameMode == Detail.GameMode);
        }

        public IMap CreatePlayable(IModeService service)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));

            // TODO: Implement
            return null;
        }

        public Map<T> Clone() => (Map<T>)MemberwiseClone();

        IMap IMap.Clone() => Clone();
    }

    /// <summary>
    /// Assumption of T parameter of Map as HitObject for convenience.
    /// </summary>
    public class Map : Map<HitObject>
    {
    }
}