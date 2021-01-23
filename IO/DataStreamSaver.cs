using System;
using System.IO;
using System.Threading;

namespace PBGame.IO
{
    /// <summary>
    /// Class which saves incoming data as a stream to the destination file.
    /// </summary>
    public class DataStreamSaver<T>
        where T : IStreamableData
    {
        private int poolSize;
        private int saveInterval;
        private T[] pool;

        private StreamWriter writer;
        private Thread streamingThread;
        private bool isStarted = false;
        private int dataCount = 0;

        private object locker = new object();


        /// <summary>
        /// Returns the raw value array.
        /// This shouldn't be modified unless you know what you're doing.
        /// </summary>
        public T[] RawBuffer => pool;

        /// <summary>
        /// Returns the next index of the internal buffer where the next data will be set to.
        /// </summary>
        public int NextPushIndex => dataCount % poolSize;


        public DataStreamSaver(int poolSize, int saveInterval = 60)
        {
            this.poolSize = poolSize;
            this.saveInterval = saveInterval;
            this.pool = new T[poolSize];
        }

        /// <summary>
        /// Starts the streaming process for the specified target.
        /// </summary>
        public void StartStream(StreamWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));
            if (writer.BaseStream == null)
                throw new Exception("The base stream of specifier writer is null.");
            if (!writer.BaseStream.CanWrite)
                throw new Exception("The specified stream cannot be written to.");
            if (this.writer != null)
                throw new Exception("There is already a writer initialized to the saver.");

            this.writer = writer;
            isStarted = true;

            streamingThread = new Thread(HandleStreaming);
            streamingThread.Start(writer);
        }

        /// <summary>
        /// Stops currently on-going streaming process.
        /// </summary>
        public void StopStream()
        {
            writer = null;
            isStarted = false;
            if (streamingThread != null)
                streamingThread.Join();
            streamingThread = null;
            dataCount = 0;
        }

        /// <summary>
        /// Pushes the specified data to stream.
        /// </summary>
        public void PushData(T data)
        {
            if (writer == null)
                throw new Exception("There is no stream to push the data to.");

            lock (locker)
            {
                pool[dataCount % poolSize] = data;
                dataCount++;
            }
        }

        /// <summary>
        /// Handles the actual saving of the incoming data.
        /// </summary>
        private void HandleStreaming(object writer)
        {
            var myStream = writer as StreamWriter;
            int curCount = 0;

            Action seekAndWrite = () =>
            {
                int targetCount = 0;
                lock (locker)
                {
                    targetCount = dataCount;
                }
                while (true)
                {
                    if (curCount == targetCount)
                        break;

                    var item = pool[curCount % poolSize];
                    myStream.Write(item.ToStreamData());
                    curCount++;
                }
            };
            
            while (isStarted)
            {
                seekAndWrite();
                // Wait a delay until there is potentially enough data.
                try
                {
                    Thread.Sleep(saveInterval);
                }
                catch {}
            }
            seekAndWrite();
            myStream.Flush();
        }
    }
}