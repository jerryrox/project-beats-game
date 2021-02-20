using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using PBFramework.Threading;

namespace PBGame.Threading
{
    public class ThreadedLoader<TInput, TOutput>
    {
        private Func<TInput, TOutput> loadHandler;


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

            taskCount = Math.Min(taskCount, inputs.Count);

            if (outputs == null)
                outputs = new TOutput[inputs.Count];

            return Task.Run<TOutput[]>(() =>
            {
                object locker = new object();
                int curInputIndex = 0;
                for (int i = 0; i < taskCount; i++)
                {
                    Task.Run(() =>
                    {
                        while (true)
                        {
                            // Retrieve the index of next input to process.
                            int inx = -1;
                            lock (locker)
                            {
                                inx = curInputIndex++;
                            }
                            if (inx >= inputs.Count)
                                break;

                            outputs[inx] = loadHandler.Invoke(inputs[inx]);
                        }
                    });
                }

                listener?.SetFinished(outputs);
                return outputs;
            });
        }
    }
}