using System;

namespace PBGame.Rulesets
{
    /// <summary>
    /// Types of game modes supported.
    /// </summary>
    public enum GameModes {
    
        OsuStandard = GameProviders.Osu,
        // OsuTaiko = 1,
        // OsuCatch = 2,
        // OsuMania = 3,

        BeatsStandard = GameProviders.Beats,
    }

    public static class GameModeExtensions
    {
        /// <summary>
        /// Returns the actual index value of this game mode relative to game provider's mode index offset.
        /// </summary>
        public static int GetIndex(this GameModes context)
        {
            string contextStr = context.ToString();
            foreach (var provider in (GameProviders[])Enum.GetValues(typeof(GameProviders)))
            {
                if (contextStr.StartsWith(provider.ToString(), StringComparison.OrdinalIgnoreCase))
                    return (int)context - (int)provider;
            }
            throw new Exception("Failed to determine the index value for game mode: " + context);
        }
    }
}