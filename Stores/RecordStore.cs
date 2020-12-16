using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using PBGame.IO;
using PBGame.Data.Users;
using PBGame.Data.Records;
using PBGame.Rulesets.Maps;
using PBFramework.DB;
using PBFramework.Stores;
using PBFramework.Threading;

namespace PBGame.Stores
{
    public class RecordStore : DatabaseBackedStore<Record>, IRecordStore {


        public Task Reload(TaskListener listener = null)
        {
            return Task.Run(() =>
            {
                base.Reload();
                listener?.SetFinished();
            });
        }

        public Task<List<IRecord>> GetTopRecords(IPlayableMap map, int? limit = null, TaskListener<List<IRecord>> listener = null)
        {
            return Task.Run(() =>
            {
                using (var query = Database.Query())
                {
                    ApplyFilterMap(query, map);

                    List<IRecord> records = query.GetResult().Cast<IRecord>().ToList();
                    records.SortByTop();
                    return records;
                }
            });
        }

        public Task<List<IRecord>> GetTopRecords(IPlayableMap map, IUser user, int? limit = null, TaskListener<List<IRecord>> listener = null)
        {
            return Task.Run(() =>
            {
                using (var query = Database.Query())
                {
                    ApplyFilterMap(query, map);
                    ApplyFilterUser(query, user);

                    List<IRecord> records = query.GetResult().Cast<IRecord>().ToList();
                    records.SortByTop();
                    return records;
                }
            });
        }

        public int GetRecordCount(IPlayableMap map, IUser user)
        {
            using (var results = Database.Query())
            {
                ApplyFilterMap(results, map);
                ApplyFilterUser(results, user);
                return results.GetResult().Count;
            }
        }

        public void SaveRecord(IRecord record)
        {
            if(!(record is Record rec))
                throw new ArgumentException($"The specified record interface is not a type of {nameof(Record)}.");

            Database.Edit().Write(rec).Commit();
        }

        public void DeleteRecords(IPlayableMap map)
        {
            using(var records = Database.Query())
            {
                ApplyFilterMap(records, map);
                Database.Edit().RemoveRange(records.GetResult()).Commit();
            }
        }

        protected override IDatabase<Record> CreateDatabase()
        {
            return new Database<Record>(GameDirectory.Records);
        }

        /// <summary>
        /// Applies DB query selection of records for the specified map.
        /// </summary>
        private void ApplyFilterMap(IDatabaseQuery<Record> query, IPlayableMap map)
        {
            var mapHash = map.Detail.Hash;
            var gameMode = ((int)map.PlayableMode).ToString();
            query.Where(d => d["MapHash"].ToString().Equals(mapHash, StringComparison.Ordinal))
                .Where(d => d["GameMode"].ToString().Equals(gameMode, StringComparison.Ordinal));
        }

        /// <summary>
        /// Applies DB query selection of records for the specified user.
        /// </summary>
        private void ApplyFilterUser(IDatabaseQuery<Record> query, IUser user)
        {
            var userId = user.Id.ToString();
            query.Where(d => d["UserId"].ToString().Equals(userId, StringComparison.Ordinal));
        }
    }
}