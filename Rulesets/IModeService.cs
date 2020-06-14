using PBGame.Rulesets.Maps;
using PBGame.Rulesets.Difficulty;
using PBGame.Rulesets.Judgements;
using PBFramework.Graphics;
using PBFramework.Dependencies;

namespace PBGame.Rulesets
{
    /// <summary>
    /// Interface which provides access to mode-specific evaluations and functions.
    /// </summary>
    public interface IModeService {

        /// <summary>
        /// Returns the displayed name of the game mode.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Returns the type of the game mode.
        /// </summary>
        GameModeType GameMode { get; }

        /// <summary>
        /// Returns whether this game mode is available for play.
        /// </summary>
        bool IsPlayable { get; }


        /// <summary>
        /// Returns the icon name suitable for specified size.
        /// </summary>
        string GetIconName(int size);

        /// <summary>
        /// Creates a new map converter instance for this game mode.
        /// </summary>
        IMapConverter CreateConverter(IOriginalMap map);

        /// <summary>
        /// Creates a new map pre/post processor instance for this game mode.
        /// </summary>
        IMapProcessor CreateProcessor(IPlayableMap map);

        /// <summary>
        /// Creates a new map difficulty calculator instance for this game mode.
        /// </summary>
        IDifficultyCalculator CreateDifficultyCalculator(IPlayableMap map);

        /// <summary>
        /// Creates a new hit object timing information of this game mode.
        /// </summary>
        HitTiming CreateTiming();

        /// <summary>
        /// Returns a new or existing session of this game mode.
        /// </summary>
        IGameSession GetSession(IGraphicObject container, IDependencyContainer dependency = null);
    }
}