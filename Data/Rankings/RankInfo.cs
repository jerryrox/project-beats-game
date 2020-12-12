using PBGame.Data.Records;

namespace PBGame.Data.Rankings
{
    public class RankInfo {

        /// <summary>
        /// The rank number within a ranking set.
        /// </summary>
        public int Rank { get; private set; }

        /// <summary>
        /// Record holding the rank details.
        /// </summary>
        public IRecord Record { get; private set; }


        public RankInfo(int rank, IRecord record)
        {
            Rank = rank;
            Record = record;
        }
    }
}