using System;
using System.IO;
using System.Threading;

namespace PBGame.IO
{
    /// <summary>
    /// Class which saves incoming data as a stream to the destination file.
    /// </summary>
    public class DataStreamWriter<T>
        where T : class, IStreamableData, new()
    {
        private int poolSize;
        private int writeInterval;
        private T[] pool;

        private StreamWriter writer;
        private Thread streamingThread;
        private bool isStarted = false;
        private int writeCount = 0;

        private object locker = new object();


        /// <summary>
        /// Returns the next item in the internal buffer where the next data will be set to.
        /// </summary>
        public T NextWriteItem => pool[writeCount % poolSize];


        public DataStreamWriter(int poolSize, int writeInterval = 60)
        {
            this.poolSize = poolSize;
            this.writeInterval = writeInterval;
            this.pool = new T[poolSize];
        }

        /// <summary>
        /// Starts the streaming process for the specified target.
        /// </summary>
        public void StartStream(StreamWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));
            if (!writer.BaseStream.CanWrite)
                throw new Exception("The specified stream cannot be written to.");
            if (this.writer != null)
                throw new Exception("There is already a writer initialized to this instance.");

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
            writeCount = 0;
        }

        /// <summary>
        /// Writes the specified data to stream.
        /// </summary>
        public void WriteData(T data)
        {
            if (writer == null)
                throw new Exception("There is no stream to write the data to.");

            lock (locker)
            {
                pool[writeCount % poolSize] = data;
                writeCount++;
            }
        }

        /// <summary>
        /// Handles the actual writing of the incoming data.
        /// </summary>
        private void HandleStreaming(object writer)
        {
            for (int i = 0; i < pool.Length; i++)
            {
                if (pool[i] == null)
                    pool[i] = new T();
            }

            var myStream = writer as StreamWriter;
            int curCount = 0;

            Action seekAndWrite = () =>
            {
                int targetCount = 0;
                lock (locker)
                {
                    targetCount = writeCount;
                }
                while (true)
                {
                    if (curCount == targetCount)
                        break;

                    var item = pool[curCount % poolSize];
                    myStream.WriteLine(item.ToStreamData());
                    curCount++;
                }
            };
            
            while (isStarted)
            {
                seekAndWrite();
                // Wait a delay until there is potentially enough data.
                try
                {
                    Thread.Sleep(writeInterval);
                }
                catch {}
            }
            seekAndWrite();
            myStream.Flush();
        }
    }
}