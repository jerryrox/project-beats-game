using System;
using System.IO;
using System.Threading;

namespace PBGame.IO
{
    public class DataStreamReader<T>
        where T : class, IStreamableData
    {
        private Func<T> instantiator;
        private int poolSize;
        private int readInterval;
        private T[] pool;

        private BinaryReader reader;
        private Thread streamingThread;
        private bool isStarted = false;
        /// <summary>
        /// Total number of times a data has been retrieved by user via ReadData method.
        /// </summary>
        private int readCount = 0;
        /// <summary>
        /// Total number of times a data has been loaded from the stream.
        /// </summary>
        private int bufferedCount = 0;

        private object locker = new object();


        /// <summary>
        /// Returns the number of data prepared in the pool buffer.
        /// </summary>
        public int BufferedCount => bufferedCount - readCount;


        public DataStreamReader(Func<T> instantiator, int poolSize, int readInterval = 60)
        {
            this.instantiator = instantiator;
            this.poolSize = poolSize;
            this.readInterval = readInterval;
            this.pool = new T[poolSize];
        }

        /// <summary>
        /// Starts the streaming process for the specified target.
        /// </summary>
        public void StartStream(BinaryReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));
            if (!reader.BaseStream.CanRead)
                throw new Exception("The specified stream cannot be read from.");
            if (this.reader != null)
                throw new Exception("There is already a reader initialized to this instance.");

            this.reader = reader;
            isStarted = true;
            readCount = 0;
            bufferedCount = 0;

            streamingThread = new Thread(HandleStreaming);
            streamingThread.Start(reader);
        }

        /// <summary>
        /// Stops currently on-going streaming process.
        /// </summary>
        public void StopStream()
        {
            reader = null;
            isStarted = false;
            if (streamingThread != null)
                streamingThread.Join();
            streamingThread = null;
            readCount = 0;
            bufferedCount = 0;
        }

        /// <summary>
        /// Reads the next available data from the pool without advancing read index.
        /// Returns null if no new data is currently available.
        /// </summary>
        public T PeekData()
        {
            if (reader == null)
                throw new Exception("There is no stream to read the data from.");

            if (readCount == bufferedCount)
                return null;

            return pool[readCount % poolSize];
        }

        /// <summary>
        /// Reads the next available data from the pool while advancing the index to read the next value when called again.
        /// Returns null if no new data is currently available.
        /// </summary>
        public T ReadData()
        {
            T value = PeekData();
            if(value != null)
                AdvanceIndex();
            return value;
        }

        /// <summary>
        /// Increments the read index by 1 to read the next available value.
        /// </summary>
        public void AdvanceIndex()
        {
            readCount++;
        }

        /// <summary>
        /// Handles the actual reading of the incoming data.
        /// </summary>
        private void HandleStreaming(object reader)
        {
            for (int i = 0; i < pool.Length; i++)
            {
                if (pool[i] == null)
                    pool[i] = instantiator.Invoke();
            }

            var myStream = reader as BinaryReader;

            while (isStarted)
            {
                int lastReadCount = readCount;

                while (isStarted && bufferedCount - lastReadCount < poolSize && myStream.BaseStream.Position < myStream.BaseStream.Length)
                {
                    var item = pool[bufferedCount % poolSize];
                    item.ReadStreamData(myStream);
                    bufferedCount++;
                }

                try
                {
                    Thread.Sleep(readInterval);
                }
                catch {}
            }
        }
    }
}