using PBGame.Data.Records;

namespace PBGame.Data.Rankings
{
    public interface IRankInfo {

        /// <summary>
        /// The rank number within a ranking set.
        /// </summary>
        int Rank { get; set; }

        /// <summary>
        /// Record holding the rank details.
        /// </summary>
        IRecord Record { get; set; }
    }
}