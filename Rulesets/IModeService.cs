using PBGame.Rulesets.Maps;
using PBGame.Rulesets.Difficulty;
using PBGame.Rulesets.Judgements;

namespace PBGame.Rulesets
{
    // TODO: Provide implementation
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
        GameModes GameMode { get; }


        /// <summary>
        /// Creates a new map converter instance for this game mode.
        /// </summary>
        IMapConverter CreateConverter(IMap map);

        /// <summary>
        /// Creates a new map pre/post processor instance for this game mode.
        /// </summary>
        IMapProcessor CreateProcessor(IMap map);

        /// <summary>
        /// Creates a new map difficulty calculator instance for this game mode.
        /// </summary>
        IDifficultyCalculator CreateDifficultyCalculator(IMap map);

        /// <summary>
        /// Creates a new map performance calculator instance of this game mode.
        /// </summary>
        IPerformanceCalculator CreatePerformanceCalculator(IMap map);

        /// <summary>
        /// Creates a new hit object timing information of this game mode.
        /// </summary>
        HitTiming CreateTiming();

        /// <summary>
        /// Returns a new or existing session of this game mode.
        /// </summary>
        IGameSession GetSession();
    }
}