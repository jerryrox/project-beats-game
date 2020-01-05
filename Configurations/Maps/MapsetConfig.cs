using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.DB.Entities;

namespace PBGame.Configurations.Maps
{
    public class MapsetConfig : DatabaseEntity {

        /// <summary>
        /// Hashcode of the mapset for identification.
        /// </summary>
        [Indexed]
        int MapsetHash { get; set; }

        /// <summary>
        /// Music offset in the mapset context.
        /// </summary>
        int Offset { get; set; } = 0;


        public MapsetConfig() : this(0) {}

        public MapsetConfig(int hash)
        {
            MapsetHash = hash;
        }
    }
}