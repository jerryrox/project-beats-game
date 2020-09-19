using System.Collections.Generic;
using PBGame.Data.Users;
using PBGame.Data.Records;
using PBGame.Rulesets.Maps;
using PBFramework.DB;
using PBFramework.Stores;

namespace PBGame.Stores
{
    public interface IRecordStore : IDatabaseBackedStore<Record> {

        /// <summary>
        /// Returns the database result for the records of specified map.
        /// </summary>
        IDatabaseResult<Record> GetRecords(IPlayableMap map);

        /// <summary>
        /// Saves the specified record to the store.
        /// </summary>
        void SaveRecord(Record record);

        /// <summary>
        /// Returns the number of records made for the specified map and the user.
        /// </summary>
        int GetPlayCount(IPlayableMap map, IUser user);
    }
}