using System;
using System.Linq;
using System.Collections.Generic;
using PBGame.Rulesets.Maps.Timing;
using PBGame.Rulesets.Maps.ControlPoints;
using PBGame.Rulesets.Objects;
using PBGame.Rulesets.Difficulty;
using UnityEngine;

using Logger = PBFramework.Debugging.Logger;

namespace PBGame.Rulesets.Maps
{
    public class Map<T> : IMap
        where T : HitObject
    {
        /// <summary>
        /// Table of playable map variants for each supported game mode.
        /// </summary>
        private Dictionary<GameModes, IMap> playableMaps;


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
                foreach (var obj in HitObjects)
                    yield return obj;
            }
        }

        public MapMetadata Metadata => Detail == null ? null : (Detail.Metadata ?? (Detail.Mapset == null ? null : Detail.Mapset.Metadata));

        public DifficultyInfo Difficulty { get; private set; } = null;

        public double BreakDuration => BreakPoints.Sum(p => p.Duration);

        public bool IsPlayable { get; private set; } = false;

        public List<Color> ComboColors { get; set; } = new List<Color>();


        public IEnumerable<string> GetQueryables() => Metadata.GetQueryables();

        public void CreatePlayable(IModeManager modeManager)
        {
            // Playable maps shouldn't support this action.
            if (IsPlayable)
            {
                Logger.LogWarning($"Map.CreatePlayable - This action is not supported for playable maps!");
                return;
            }
            // Skip if already created.
            if(this.playableMaps != null)
                return;

            this.playableMaps = new Dictionary<GameModes, IMap>();
            foreach (var service in modeManager.AllServices())
            {
                var playableMap = CreatePlayable(service);
                if(playableMap != null)
                    this.playableMaps[service.GameMode] = playableMap;
            }
        }

        public IMap GetPlayable(GameModes gameMode)
        {
            if (IsPlayable)
            {
                Logger.Log($"Map.GetPlayable - This map is already at a playable state.");
                return this;
            }

            // Prioritize the queried game mode. Else, fallback.
            if (this.playableMaps.TryGetValue(gameMode, out IMap map))
                return map;
            return this.playableMaps[Detail.GameMode];
        }

        public Map<T> Clone() => (Map<T>)MemberwiseClone();

        IMap IMap.Clone() => Clone();

        /// <summary>
        /// Creates a playable map for specified service.
        /// </summary>
        private IMap CreatePlayable(IModeService service)
        {
            if (service == null)
            {
                Logger.LogWarning($"Map.CreatePlayable - A null servicer is passed. Skipping this mode.");
                return null;
            }

            // Conversion
            var converter = service.CreateConverter(this);
            if (!converter.IsConvertible)
                return null;
            var convertedMap = converter.Convert() as Map<T>;
            if (convertedMap == null)
            {
                Logger.LogWarning($"Map.CreatePlayable - Failed to convert map for mode: {service.GameMode}");
                return null;
            }

            // Processing
            var processor = service.CreateProcessor(convertedMap);
            processor.PreProcess();
            foreach (var o in convertedMap.HitObjects)
                o.ApplyMapProperties(convertedMap.ControlPoints, convertedMap.Detail.Difficulty);
            processor.PostProcess();

            // Calculate difficulty.
            var difficultyCalculator = service.CreateDifficultyCalculator(convertedMap);
            if (difficultyCalculator == null)
            {
                Logger.LogWarning($"Map.CreatePlayable - Difficulty calculator is null for mode: {service.GameMode}");
                return null;
            }
            convertedMap.Difficulty = difficultyCalculator.Calculate();
            if (convertedMap.Difficulty == null)
            {
                Logger.LogWarning($"Map.CreatePlayable - Failed to calculate difficulty for mode: {service.GameMode}");
                return null;
            }

            // Finished
            convertedMap.PlayableMode = service.GameMode;
            convertedMap.IsPlayable = true;
            return convertedMap;
        }
    }

    /// <summary>
    /// Assumption of T parameter of Map as HitObject for convenience.
    /// </summary>
    public class Map : Map<HitObject>
    {
    }
}