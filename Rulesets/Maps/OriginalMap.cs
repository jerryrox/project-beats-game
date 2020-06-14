using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Rulesets.Maps.Timing;
using PBGame.Rulesets.Maps.ControlPoints;
using PBGame.Rulesets.Objects;
using UnityEngine;

using Logger = PBFramework.Debugging.Logger;

namespace PBGame.Rulesets.Maps
{
    public class OriginalMap : Map<BaseHitObject>, IOriginalMap {

        /// <summary>
        /// Table of playable map variants for each supported game mode.
        /// </summary>
        private Dictionary<GameModeType, IPlayableMap> playableMaps = new Dictionary<GameModeType, IPlayableMap>();


        public override MapDetail Detail { get; } = new MapDetail()
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

        public override ControlPointGroup ControlPoints { get; } = new ControlPointGroup();

        public override List<BreakPoint> BreakPoints { get; } = new List<BreakPoint>();

        public override bool IsPlayable => false;

        public override List<Color> ComboColors { get; } = new List<Color>();


        public OriginalMap()
        {

        }
        public void CreatePlayable(IModeManager modeManager)
        {
            // Playable maps shouldn't support this action.
            if (IsPlayable)
            {
                Logger.LogWarning($"OriginalMap.CreatePlayable - This action is not supported for playable maps!");
                return;
            }

            this.playableMaps.Clear();
            foreach (var service in modeManager.PlayableServices())
            {
                var playableMap = CreatePlayable(service);
                if (playableMap != null)
                {
                    this.playableMaps[service.GameMode] = playableMap;
                }
            }
        }

        public IPlayableMap GetPlayable(GameModeType gameMode)
        {
            // Prioritize the queried game mode. Else, fallback.
            if (this.playableMaps.TryGetValue(gameMode, out IPlayableMap map))
                return map;
            // Make sure the original game mode is available for play.
            if (this.playableMaps.TryGetValue(Detail.GameMode, out map))
                return map;
            // Else, just return the first playable map.
            return this.playableMaps.Count > 0 ? this.playableMaps[0] : null;
        }

        /// <summary>
        /// Creates a playable map for specified service.
        /// </summary>
        private IPlayableMap CreatePlayable(IModeService service)
        {
            if (service == null)
            {
                Logger.LogWarning($"OriginalMap.CreatePlayable - A null servicer is passed. Skipping this mode.");
                return null;
            }

            // Conversion
            var converter = service.CreateConverter(this);
            if (!converter.IsConvertible)
                return null;
            var convertedMap = converter.Convert();
            if (convertedMap == null)
            {
                Logger.LogWarning($"OriginalMap.CreatePlayable - Failed to convert map for mode: {service.GameMode}");
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
                Logger.LogWarning($"OriginalMap.CreatePlayable - Difficulty calculator is null for mode: {service.GameMode}");
                return null;
            }
            convertedMap.Difficulty = difficultyCalculator.Calculate();
            if (convertedMap.Difficulty == null)
            {
                Logger.LogWarning($"OriginalMap.CreatePlayable - Failed to calculate difficulty for mode: {service.GameMode}");
                return null;
            }

            // Finished
            convertedMap.PlayableMode = service.GameMode;
            return convertedMap;
        }
    }
}