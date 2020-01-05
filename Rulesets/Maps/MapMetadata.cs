using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.Data.Queries;

namespace PBGame.Rulesets.Maps
{
    /// <summary>
    /// Represents the general information of a map or a mapset.
    /// </summary>
    public class MapMetadata : IQueryableData {

        /// <summary>
        /// English text of the title.
        /// May be in unicode for some map formats.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Unicode text of the title.
        /// </summary>
        public string TitleUnicode { get; set; }

        /// <summary>
        /// English text of the artist.
        /// May be in unicode for some map formats.
        /// </summary>
        public string Artist { get; set; }

        /// <summary>
        /// Unicode text of the artist.
        /// </summary>
        public string ArtistUnicode { get; set; }

        /// <summary>
        /// Mapper nickname.
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// The original source of the music.
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Search tags associated with the map.
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// Name of the background image file.
        /// </summary>
        public string BackgroundFile { get; set; }

        /// <summary>
        /// Name of the music file.
        /// </summary>
        public string AudioFile { get; set; }

        /// <summary>
        /// Music preview playback time in milliseconds.
        /// </summary>
        public int PreviewTime { get; set; }


        public IEnumerable<string> GetQueryables()
        {
            yield return Title;
            yield return TitleUnicode;
            yield return Artist;
            yield return ArtistUnicode;
            yield return Creator;
            yield return Source;
            yield return Tags;
        }

        /// <summary>
        /// Returns the correct title based on unicode preference.
        /// </summary>
        public string GetTitle(bool preferUnicode)
        {
            return preferUnicode ? (TitleUnicode ?? Title) : Title;
        }

        /// <summary>
        /// Returns the correct artist based on unicode preference.
        /// </summary>
        public string GetArtist(bool preferUnicode)
        {
            return preferUnicode ? (ArtistUnicode ?? Artist) : Artist;
        }
    }
}