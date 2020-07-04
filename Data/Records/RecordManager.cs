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

        public Task Reload(IEventProgress progress)
        {
            progress?.Report(0f);
            return Task.Run(() =>
            {
                recordStore.Reload();
                // TODO: Reload replay store.

                UnityThread.DispatchUnattended(() =>
                {
                    if (progress != null)
                    {
                        progress.Report(1f);
                        progress.InvokeFinished();
                    }
                    return null;
                });
            });
        }

        public Task GetRecords(IPlayableMap map, IReturnableProgress<IEnumerable<IRecord>> progress)
        {
            progress?.Report(0f);
            return Task.Run(() =>
            {
                var records = GetInjectedRecords(recordStore.GetRecords(map));

                UnityThread.DispatchUnattended(() => {
                    if (progress != null)
                    {
                        progress.Report(1f);
                        progress.InvokeFinished(records);
                    }
                    return null;
                });
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

        public IRecord GetBestRecord(IEnumerable<IRecord> records)
        {
            return records.Aggregate((x, y) =>
            {
                int scoreComp = x.Score.CompareTo(y.Score);
                if(scoreComp != 0)
                    return scoreComp > 0 ? x : y;
                int accComp = x.Accuracy.CompareTo(y.Accuracy);
                if(accComp != 0)
                    return accComp > 0 ? x : y;
                return x.Date.CompareTo(y.Date) <= 0 ? x : y;
            });
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