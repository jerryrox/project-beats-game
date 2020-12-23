using System;

namespace PBGame.Rulesets
{
    /// <summary>
    /// Types of game modes supported.
    /// </summary>
    public enum GameModeType {
    
        OsuStandard = GameProviderType.Osu,
        OsuTaiko = GameProviderType.Osu + 1,
        OsuCatch = GameProviderType.Osu + 2,
        OsuMania = GameProviderType.Osu + 3,

        BeatsStandard = GameProviderType.Beats,
    }

    public static class GameModeTypeExtension
    {
        /// <summary>
        /// Returns the actual index value of this game mode relative to game provider's mode index offset.
        /// </summary>
        public static int GetIndex(this GameModeType context)
        {
            string contextStr = context.ToString();
            foreach (var provider in (GameProviderType[])Enum.GetValues(typeof(GameProviderType)))
            {
                if (contextStr.StartsWith(provider.ToString(), StringComparison.OrdinalIgnoreCase))
                    return (int)context - (int)provider;
            }
            throw new Exception("Failed to determine the index value for game mode: " + context);
        }
    }
}