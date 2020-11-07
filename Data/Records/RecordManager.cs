using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using PBGame.Data.Users;
using PBGame.Stores;
using PBGame.Rulesets.Maps;
using PBFramework.Threading;
using PBFramework.Debugging;

namespace PBGame.Data.Records
{
    public class RecordManager : IRecordManager {

        private IRecordStore recordStore;
        // TODO: Replay store


        public RecordManager()
        {
            recordStore = new RecordStore();
        }

        public Task Reload(TaskListener listener = null)
        {
            return Task.Run(() =>
            {
                recordStore.Reload();
                // TODO: Reload replay store.

                listener?.SetFinished();
            });
        }

        public Task<List<IRecord>> GetRecords(IPlayableMap map, IUser user = null, TaskListener<List<IRecord>> listener = null)
        {
            return Task.Run(() =>
            {
                using (var records = recordStore.GetRecords(map, user))
                {
                    var recordList = records.Cast<IRecord>().ToList();
                    listener?.SetFinished(recordList);
                    return recordList;
                }
            });
        }

        public int GetPlayCount(IPlayableMap map, IUser user) => recordStore.GetPlayCount(map, user);

        public void SaveRecord(IRecord record)
        {
            if (!(record is Record r))
            {
                Logger.LogWarning($"The record trying to save is not a type of {nameof(Record)}!");
                return;
            }
            recordStore.SaveRecord(r);
        }

        public void DeleteRecords(IPlayableMap map)
        {
            recordStore.DeleteRecords(map);
        }

        // TODO: Implement when replay store is implemented.
        public bool HasReplay(IRecord record) => false;

        public IRecord GetBestRecord(List<IRecord> records)
        {
            if(records.Count == 0)
                return null;

            IRecord bestRecord = records[0];
            for (int i = 1; i < records.Count; i++)
            {
                IRecord record = records[i];
                int comp = record.Score.CompareTo(bestRecord.Score);
                if (comp > 0)
                    bestRecord = record;
                else if (comp == 0)
                {
                    comp = record.Accuracy.CompareTo(bestRecord.Accuracy);
                    if(comp > 0 || (comp == 0 && record.Date <= bestRecord.Date))
                        bestRecord = record;
                }
            }
            return bestRecord;
        }
    }
}