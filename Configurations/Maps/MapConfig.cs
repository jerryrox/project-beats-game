using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Audio;
using PBFramework.DB.Entities;
using PBFramework.Data.Bindables;

namespace PBGame.Configurations.Maps
{
    public class MapConfig : DatabaseEntity, IMusicOffset {

        /// <summary>
        /// MD5 hash of the map for identification.
        /// </summary>
        [Indexed]
        public string MapHash { get; set; }

        /// <summary>
        /// Whether this configuration should override the global configuration.
        /// </summary>
        public bool OverrideSettings { get; set; } = false;

        /// <summary>
        /// Overriding storyboard flag.
        /// </summary>
        public bool UseStoryboard { get; set; } = true;

        /// <summary>
        /// Overriding video flag.
        /// </summary>
        public bool UseVideo { get; set; } = true;

        /// <summary>
        /// Overriding background dim.
        /// </summary>
        public float BackgroundDim { get; set; } = 0.5f;

        public BindableInt Offset { get; set; } = new BindableInt(0);


        public MapConfig() : this("")
        {

        }

        public MapConfig(string mapHash)
        {
            MapHash = mapHash;
        }
    }
}