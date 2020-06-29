using System;
using Newtonsoft.Json;

namespace PBGame.Networking.Maps
{
    public class OnlineMapset {
        
        /// <summary>
        /// Returns the identifier of the mapset assigned in a provider.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Returns the title of the mapset.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Returns the artist of the mapset.
        /// </summary>
        [JsonProperty("artist")]
        public string Artist { get; set; }

        /// <summary>
        /// Returns the name of the creator.
        /// </summary>
        [JsonProperty("creator")]
        public string Creator { get; set; }

        /// <summary>
        /// Returns the source of the music.
        /// </summary>
        [JsonProperty("source")]
        public string Source { get; set; }

        /// <summary>
        /// Returns the tags attached to the mapset.
        /// </summary>
        [JsonProperty("tags")]
        public string Tags { get; set; }

        /// <summary>
        /// Returns the url to the cover image of the mapset.
        /// </summary>
        [JsonProperty("coverImage")]
        public string CoverImage { get; set; }

        /// <summary>
        /// Returns the url to the card image of the mapset.
        /// </summary>
        [JsonProperty("cardImage")]
        public string CardImage { get; set; }

        /// <summary>
        /// Returns the url to the preview audio of the mapset.
        /// </summary>
        [JsonProperty("previewAudio")]
        public string PreviewAudio { get; set; }

        /// <summary>
        /// Returns whether the mapset has a video.
        /// </summary>
        [JsonProperty("hasVideo")]
        public bool HasVideo { get; set; }

        /// <summary>
        /// Returns whether the mapset has a storyboard.
        /// </summary>
        [JsonProperty("hasStoryboard")]
        public bool HasStoryboard { get; set; }

        /// <summary>
        /// Returns the primary bpm of the mapset.
        /// </summary>
        [JsonProperty("bpm")]
        public float Bpm { get; set; }

        /// <summary>
        /// Returns the played count of the map.
        /// </summary>
        [JsonProperty("playCount")]
        public int PlayCount { get; set; }

        /// <summary>
        /// Returns the number of favorites on the mapset.
        /// </summary>
        [JsonProperty("favoriteCount")]
        public int FavoriteCount { get; set; }

        /// <summary>
        /// Returns the last updated date of the mapset.
        /// </summary>
        [JsonProperty("lastUpdate")]
        public DateTime LastUpdate { get; set; }

        /// <summary>
        /// Returns the status of the map.
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <summary>
        /// Returns whether the downloading of this map is disabled.
        /// </summary>
        [JsonProperty("isDisabled")]
        public bool IsDisabled { get; set; }

        /// <summary>
        /// Returns the detailed information of download disability.
        /// </summary>
        [JsonProperty("disabledInformation")]
        public string DisabledInformation { get; set; }

        /// <summary>
        /// Returns the list of maps included in the mapset.
        /// </summary>
        [JsonProperty("maps")]
        public OnlineMap[] Maps { get; set; }
    }
}