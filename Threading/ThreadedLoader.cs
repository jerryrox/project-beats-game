using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using PBFramework.Threading;

namespace PBGame.Threading
{
    public class ThreadedLoader<TInput, TOutput>
    {
        private Func<TInput, TOutput> loadHandler;


        /// <summary>
        /// Number of milliseconds which the loading completion check will be run at.
        /// Default is (1000 / 60).
        /// </summary>
        public int CompletionCheckInterval { get; set; } = 1000 / 60;

        /// <summary>
        /// If non-null, the max number of milliseconds to wait for the loading process to finish.
        /// </summary>
        public int? Timeout { get; set; } = null;


        public ThreadedLoader(Func<TInput, TOutput> loadHandler)
        {
            this.loadHandler = loadHandler;
        }

        public Task<TOutput[]> StartLoad(int taskCount, List<TInput> inputs, TOutput[] outputs = null, TaskListener<TOutput[]> listener = null)
        {
            if (taskCount < 1)
                throw new Exception("Task count must be 1 or greater.");
            if (inputs == null)
                throw new ArgumentNullException(nameof(inputs));
            if (outputs != null && outputs.Length < inputs.Count)
                throw new ArgumentException("The outputs array length is less than the input count.");

            return Task.Run<TOutput[]>(() =>
            {
                taskCount = Math.Min(taskCount, inputs.Count);
                if (outputs == null)
                    outputs = new TOutput[inputs.Count];

                object inputLocker = new object();
                object finishLocker = new object();
                int curInputIndex = 0;
                int finishedCount = 0;
                int loadStartTime = DateTime.UtcNow.Millisecond;
                for (int i = 0; i < taskCount; i++)
                {
                    Task.Run(() =>
                    {
                        while (true)
                        {
                            // Retrieve the index of next input to process.
                            int inx = -1;
                            lock (inputLocker)
                            {
                                inx = curInputIndex++;
                            }
                            if (inx >= inputs.Count)
                                break;

                            outputs[inx] = loadHandler.Invoke(inputs[inx]);
                            lock (finishLocker)
                            {
                                listener?.SetProgress((float)(finishedCount + 1) / inputs.Count);
                                finishedCount++;
                            }
                    }
                    });
                }
                while (true)
                {
                    Thread.Sleep(CompletionCheckInterval);
                    if (Timeout.HasValue)
                    {
                        int elapsed = DateTime.UtcNow.Millisecond - loadStartTime;
                        if (elapsed > Timeout.Value)
                            throw new TimeoutException("The loading process has taken longer than expected.");
                    }
                    if (finishedCount >= inputs.Count)
                        break;
                }

                listener?.SetFinished(outputs);
                return outputs;
            });
        }
    }
}