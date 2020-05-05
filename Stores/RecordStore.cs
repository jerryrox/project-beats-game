using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.IO;
using PBGame.Data.Users;
using PBGame.Data.Records;
using PBGame.Rulesets.Maps;
using PBFramework.DB;
using PBFramework.Stores;

namespace PBGame.Stores
{
    public class RecordStore : DatabaseBackedStore<Record>, IRecordStore {


        public IEnumerable<IRecord> GetRecords(IPlayableMap map)
        {
            using (var results = Database.Query()
                .FilterMap(map)
                .Preload()
                .GetResult())
            {
                return results;
            }
        }

        public void SaveRecord(Record record)
        {
            Database.Edit().Write(record).Commit();
        }

        public int GetPlayCount(IPlayableMap map, IUser user)
        {
            using (var results = Database.Query()
                .FilterMap(map)
                .FilterUser(user)
                .GetResult())
            {
                return results.Count;
            }
        }

        protected override IDatabase<Record> CreateDatabase()
        {
            return new Database<Record>(GameDirectory.Records);
        }
    }

    public static class RecordStoreExtension
    {
        /// <summary>
        /// Selects records for the specified map.
        /// </summary>
        public static IDatabaseQuery<Record> FilterMap(this IDatabaseQuery<Record> context, IPlayableMap map)
        {
            var mapHash = map.Detail.Hash;
            var gameMode = map.PlayableMode.ToString();
            return context
                .Where(d => d["MapHash"].ToString().Equals(mapHash, StringComparison.Ordinal))
                .Where(d => d["GameMode"].ToString().Equals(gameMode, StringComparison.Ordinal));
        }

        /// <summary>
        /// Selects records for the specified user.
        /// </summary>
        public static IDatabaseQuery<Record> FilterUser(this IDatabaseQuery<Record> context, IUser user)
        {
            var userId = user.Id.ToString();
            return context
                .Where(d => d["UserId"].ToString().Equals(userId, StringComparison.Ordinal));
        }
    }
}