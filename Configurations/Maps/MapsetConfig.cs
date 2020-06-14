using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Audio;
using PBFramework.DB.Entities;
using PBFramework.Data.Bindables;

namespace PBGame.Configurations.Maps
{
    public class MapsetConfig : DatabaseEntity, IMusicOffset {

        /// <summary>
        /// Hashcode of the mapset for identification.
        /// </summary>
        [Indexed]
        public int MapsetHash { get; set; }

        public BindableInt Offset { get; set; } = new BindableInt(0);


        public MapsetConfig() : this(0) {}

        public MapsetConfig(int hash)
        {
            MapsetHash = hash;
        }
    }
}