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
        Task Reload(IEventProgress progress);

        /// <summary>
        /// Returns all records for the specified map.
        /// </summary>
        Task GetRecords(IMap map, IReturnableProgress<IEnumerable<IRecord>> progress);

        /// <summary>
        /// Returns the number of records for the specified map and user.
        /// </summary>
        int GetPlayCount(IMap map, IUser user);

        /// <summary>
        /// Returns whether there is a replay data for specified record.
        /// </summary>
        bool HasReplay(IRecord record);

        // TODO: Create a method for retrieving replay.
    }
}