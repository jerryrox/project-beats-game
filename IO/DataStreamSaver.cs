using System;
using System.IO;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

namespace PBGame.IO
{
    /// <summary>
    /// Class which saves incoming data as a stream to the destination file.
    /// </summary>
    public class DataStreamSaver<T>
        where T : IStreamableData
    {
        private int index;
        private int poolSize;
        private int saveInterval;
        private T[] pool;

        private StreamWriter writer;
        private Thread streamingThread;

        private object locker = new object();

        public DataStreamSaver(int poolSize, int saveInterval = 60)
        {
            this.poolSize = poolSize;
            this.index = 0;
            this.saveInterval = saveInterval;
            this.pool = new T[poolSize];
        }

        /// <summary>
        /// Starts the streaming process for the specified target.
        /// </summary>
        public void StartStream(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            if (!stream.CanWrite)
                throw new Exception("The specified stream cannot be written to.");
            if (this.writer != null)
                throw new Exception("There is already a stream initialized to the saver.");

            this.writer = new StreamWriter(stream);

            streamingThread = new Thread(HandleStreaming);
            streamingThread.Start();
        }

        /// <summary>
        /// Stops currently on-going streaming process.
        /// </summary>
        public void StopStream()
        {
            writer = null;
            streamingThread.Join();

            index = 0;
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
                index = index >= poolSize - 1 ? 0 : index + 1;
                pool[index] = data;
            }
        }

        /// <summary>
        /// Handles the actual saving of the incoming data.
        /// </summary>
        private void HandleStreaming()
        {
            var myStream = writer;
            int curIndex = 0;
            while (writer != null)
            {
                int targetIndex = 0;
                lock (locker)
                {
                    targetIndex = index;
                }
                while (true)
                {
                    if (curIndex == targetIndex + 1)
                        break;

                    var item = pool[curIndex];
                    myStream.Write(item.ToStreamData());

                    curIndex++;
                    if (curIndex >= poolSize)
                        curIndex = 0;
                }

                // Wait a delay until there is potentially enough data.
                try
                {
                    Thread.Sleep(saveInterval);
                }
                catch {}
            }
            myStream.Flush();
            myStream.Close();
        }
    }
}