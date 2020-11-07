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
        /// Specify a non-null user to retrieve results of that user only.
        /// </summary>
        Task<List<IRecord>> GetRecords(IPlayableMap map, IUser user = null, TaskListener<List<IRecord>> listener = null);

        /// <summary>
        /// Returns the number of records for the specified map and user.
        /// </summary>
        int GetPlayCount(IPlayableMap map, IUser user);

        /// <summary>
        /// Saves the specified record to the database.
        /// </summary>
        void SaveRecord(IRecord record);

        /// <summary>
        /// Deletes all the records of specified map.
        /// </summary>
        void DeleteRecords(IPlayableMap map);

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