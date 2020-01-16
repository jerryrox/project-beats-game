using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using PBGame.IO;
using PBFramework;
using PBFramework.Storages;

namespace PBGame.Stores
{
    public class DownloadStore : IDownloadStore {

        private DirectoryInfo baseDirectory;


        public IFileStorage MapStorage { get; private set; }


        public DownloadStore()
        {
            baseDirectory = GameDirectory.Downloads;

            MapStorage = new FileStorage(baseDirectory.GetSubdirectory("maps"));
        }
    }
}