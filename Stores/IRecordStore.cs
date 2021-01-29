using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using PBGame.Data.Users;
using PBGame.Data.Records;
using PBGame.Rulesets.Maps;
using PBFramework.Threading;

namespace PBGame.Stores
{
    public interface IRecordStore {

        /// <summary>
        /// Reloads the store's cached data for a fresh use.
        /// </summary>
        Task Reload(TaskListener listener = null);

        /// <summary>
        /// Returns the list of top records for the specified map.
        /// </summary>
        Task<List<IRecord>> GetTopRecords(IPlayableMap map, int? limit = null, TaskListener<List<IRecord>> listener = null);

        /// <summary>
        /// Returns the list of top records for the specified map and user.
        /// </summary>
        Task<List<IRecord>> GetTopRecords(IPlayableMap map, IUser user, int? limit = null, TaskListener<List<IRecord>> listener = null);

        /// <summary>
        /// Returns the number of records for the specified map and user.
        /// </summary>
        int GetRecordCount(IPlayableMap map, IUser user);

        /// <summary>
        /// Saves the specified record to the store.
        /// </summary>
        void SaveRecord(IRecord record);

        /// <summary>
        /// Deletes all records of specified map.
        /// </summary>
        void DeleteRecords(IPlayableMap map);

        /// <summary>
        /// Returns the file info of the specified record's replay data.
        /// </summary>
        FileInfo GetReplayFile(IRecord record);

        /// <summary>
        /// Returns whether there is a replay data for the specified record.
        /// </summary>
        bool HasReplayData(IRecord record);
    }
}