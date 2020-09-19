using System.Threading.Tasks;
using System.Collections.Generic;
using PBGame.Data.Users;
using PBGame.Rulesets.Maps;
using PBFramework.Threading;

namespace PBGame.Data.Records
{
    public interface IRecordManager {

        /// <summary>
        /// Reloads all data stores required by the manager.
        /// </summary>
        Task Reload(TaskListener listener = null);

        /// <summary>
        /// Returns all records for the specified map.
        /// </summary>
        Task<List<IRecord>> GetRecords(IPlayableMap map, TaskListener<List<IRecord>> listener = null);

        /// <summary>
        /// Returns the number of records for the specified map and user.
        /// </summary>
        int GetPlayCount(IPlayableMap map, IUser user);

        /// <summary>
        /// Saves the specified record to the database.
        /// </summary>
        void SaveRecord(IRecord record);

        /// <summary>
        /// Returns whether there is a replay data for specified record.
        /// </summary>
        bool HasReplay(IRecord record);

        /// <summary>
        /// Returns the best record among the specified series of records.
        /// </summary>
        IRecord GetBestRecord(List<IRecord> records);

        // TODO: Create a method for retrieving replay.
    }
}