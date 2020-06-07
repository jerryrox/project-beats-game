using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Audio;
using PBFramework.DB.Entities;

namespace PBGame.Configurations.Maps
{
    public class MapsetConfig : DatabaseEntity, IMusicOffset {

        /// <summary>
        /// Hashcode of the mapset for identification.
        /// </summary>
        [Indexed]
        public int MapsetHash { get; set; }

        public int Offset { get; set; } = 0;


        public MapsetConfig() : this(0) {}

        public MapsetConfig(int hash)
        {
            MapsetHash = hash;
        }
    }
}