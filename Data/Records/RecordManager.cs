using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using PBGame.Data.Users;
using PBGame.Stores;
using PBGame.Rulesets.Maps;
using PBFramework.Threading;
using PBFramework.Dependencies;

namespace PBGame.Data.Records
{
    public class RecordManager : IRecordManager {

        private IRecordStore recordStore;
        // TODO: Replay store
        private IDependencyContainer dependency;


        public RecordManager(IDependencyContainer dependency)
        {
            if(dependency == null) throw new ArgumentNullException(nameof(dependency));

            this.dependency = dependency;

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

        public Task<List<IRecord>> GetRecords(IPlayableMap map, TaskListener<List<IRecord>> listener = null)
        {
            return Task.Run(() =>
            {
                using (var records = recordStore.GetRecords(map))
                {
                    var injectedRecords = GetInjectedRecords(records).ToList();
                    listener?.SetFinished(injectedRecords);
                    return injectedRecords;
                }
            });
        }

        public int GetPlayCount(IPlayableMap map, IUser user) => recordStore.GetPlayCount(map, user);

        public void SaveRecord(IRecord record)
        {
            if(!(record is Record r))
                return;
            recordStore.SaveRecord(r);
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

        /// <summary>
        /// Injects dependencies to specified records and returns them.
        /// </summary>
        private IEnumerable<IRecord> GetInjectedRecords(IEnumerable<IRecord> records)
        {
            foreach (var r in records)
            {
                dependency.Inject(r);
                yield return r;
            }
        }
    }
}