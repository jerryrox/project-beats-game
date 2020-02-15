using PBGame.Rulesets;
using PBGame.Rulesets.Maps;
using PBGame.Rulesets.Scoring;

namespace PBGame.Data.Users
{
    public interface IUserStatistics {
    
        /// <summary>
        /// Returns the game mode this statistics apply for.
        /// </summary>
        GameModes GameMode { get; }

        /// <summary>
        /// Returns the level of the user.
        /// </summary>
        int Level { get; }

        /// <summary>
        /// Returns the progress of the exp for current level.
        /// </summary>
        float ExpProgress { get; }

        /// <summary>
        /// Returns the current exp in current level.
        /// </summary>
        long CurExp { get; }

        /// <summary>
        /// Returns the target exp for levelling up at current level.
        /// </summary>
        long MaxExp { get; }

        /// <summary>
        /// Returns the total score earned from plays.
        /// </summary>
        long TotalScore { get; }

        /// <summary>
        /// Total score of all maps' best records.
        /// </summary>
        long RankedScore { get; }

        /// <summary>
        /// Returns the overall accuracy of the user using the best record of each map.
        /// </summary>
        float Accuracy { get; }

        /// <summary>
        /// Returns the number of plays completed.
        /// </summary>
        int CompletedPlay { get; }

        /// <summary>
        /// Returns the total number of plays done.
        /// </summary>
        int PlayCount { get; }

        /// <summary>
        /// Returns the number of seconds spent during play.
        /// </summary>
        int PlayTime { get; }

        /// <summary>
        /// Returns the total number of hits made.
        /// </summary>
        int TotalHits { get; }

        /// <summary>
        /// Returns the highest combo achieved among all maps.
        /// </summary>
        int MaxCombo { get; }


        /// <summary>
        /// Returns the amount of score required at specified level to level up.
        /// </summary>
        long GetRequiredScore(int level);

        /// <summary>
        /// Returns the number of plays done for the specified map.
        /// </summary>
        int GetPlayCount(IMap map);

        /// <summary>
        /// Returns the number of ranks achieved for specified rank type.
        /// </summary>
        int GetRankCount(RankTypes type);

        // TODO: Take in 1 record
        /// <summary>
        /// Records an incompleted play to the statistics.
        /// </summary>
        void RecordIncompletePlay();

        // TODO: Take in 2 records, current and previous best.
        /// <summary>
        /// Records a complete play to the statistics.
        /// </summary>
        void RecordPlay();
    }
}