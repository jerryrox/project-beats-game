using System;
using System.Collections.Generic;

namespace PBGame.Networking.Maps
{
    public class OnlineMapset {
        
        /// <summary>
        /// Returns the identifier of the mapset assigned in a provider.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Returns the title of the mapset.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Returns the artist of the mapset.
        /// </summary>
        public string Artist { get; set; }

        /// <summary>
        /// Returns the name of the creator.
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// Returns the source of the music.
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Returns the tags attached to the mapset.
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// Returns the url to the cover image of the mapset.
        /// </summary>
        public string CoverImage { get; set; }

        /// <summary>
        /// Returns the url to the card image of the mapset.
        /// </summary>
        public string CardImage { get; set; }

        /// <summary>
        /// Returns the url to the preview audio of the mapset.
        /// </summary>
        public string PreviewAudio { get; set; }

        /// <summary>
        /// Returns whether the mapset has a video.
        /// </summary>
        public bool HasVideo { get; set; }

        /// <summary>
        /// Returns whether the mapset has a storyboard.
        /// </summary>
        public bool HasStoryboard { get; set; }

        /// <summary>
        /// Returns the primary bpm of the mapset.
        /// </summary>
        public float Bpm { get; set; }

        /// <summary>
        /// Returns the played count of the map.
        /// </summary>
        public int PlayCount { get; set; }

        /// <summary>
        /// Returns the number of favorites on the mapset.
        /// </summary>
        public int FavoriteCount { get; set; }

        /// <summary>
        /// Returns the last updated date of the mapset.
        /// </summary>
        public DateTime LastUpdate { get; set; }

        /// <summary>
        /// Returns the status of the map.
        /// </summary>
        public MapStatus Status { get; set; }

        /// <summary>
        /// Returns whether the downloading of this map is disabled.
        /// </summary>
        public bool IsDisabled { get; set; }

        /// <summary>
        /// Returns the detailed information of download disability.
        /// </summary>
        public string DisabledInformation { get; set; }

        /// <summary>
        /// Returns the list of maps included in the mapset.
        /// </summary>
        public List<OnlineMap> Maps { get; set; } = new List<OnlineMap>();
    }
}