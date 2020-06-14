using System.IO;

namespace PBGame.Rulesets.Maps
{
	/// <summary>
	/// Detailed information of a map.
	/// </summary>
	public class MapDetail {

		private int? mapId;
        private int? mapsetId;


        /// <summary>
        /// Map identifier.
        /// </summary>
        public int? MapId
		{
            get => mapId;
            set => mapId = (value > 0 ? value : null);
		}

        /// <summary>
        /// Mapset identifier.
        /// </summary>
        public int? MapsetId
        {
			get => mapsetId ?? Mapset?.MapsetId;
            set => mapsetId = value > 0 ? value : null;
        }

        /// <summary>
        /// Mapset which contains the map of this detail.
        /// </summary>
        public IMapset Mapset { get; set; }

		/// <summary>
		/// Metadata of the map.
		/// </summary>
		public MapMetadata Metadata { get; set; }

		/// <summary>
		/// Difficulty values of the map.
		/// </summary>
		public MapDifficulty Difficulty { get; set; }

		/// <summary>
		/// Version of the map.
		/// </summary>
		public int FormatVersion { get; set; }

		/// <summary>
		/// File name of the map, relative to its map set.
		/// </summary>
		public string FileName { get; set; }

		/// <summary>
		/// Hash of the map for integrity.
		/// </summary>
		public string Hash { get; set; }

		/// <summary>
		/// File info of the map file.
		/// </summary>
		public FileInfo MapFile { get; set; }

		/// <summary>
		/// Amount of delay before actual audio start.
		/// </summary>
		public int AudioLeadIn { get; set; }

		/// <summary>
		/// Whether countdown should be displayed at the start.
		/// </summary>
		public bool Countdown { get; set; }

        /// <summary>
        /// Amount of offset to apply on hitobject position that are stacked on a single location.
        /// </summary>
        public float StackLeniency { get; set; } = 0.7f;

        /// <summary>
        /// The game mode which this map is initially created for.
        /// The actual playable game mode should be referenced from map, not mapDetail.
        /// </summary>
        public GameModeType GameMode { get; set; }

		/// <summary>
		/// Whether break periods should display letterbox on the background image.
		/// </summary>
		public bool LetterboxInBreaks { get; set; }

		/// <summary>
		/// Whether storyboard support widescreen.
		/// </summary>
		public bool WidescreenStoryboard { get; set; }

		/// <summary>
		/// Name of the map version.
		/// </summary>
		public string Version { get; set; }


		/// <summary>
		/// Returns whether the specified map detail provides the same audio.
		/// </summary>
		public bool IsSameAudio(MapDetail other)
		{
			return other != null && Mapset != null && other.Mapset != null &&
				Mapset.Id == other.Mapset.Id &&
				(Metadata ?? Mapset.Metadata).AudioFile == (other.Metadata ?? other.Mapset.Metadata).AudioFile;
		}

		/// <summary>
		/// Returns whether the specified map detail provides the same background.
		/// </summary>
		public bool IsSameBackground(MapDetail other)
		{
			return other != null && Mapset != null && other.Mapset != null &&
				Mapset.Id == other.Mapset.Id &&
				(Metadata ?? Mapset.Metadata).BackgroundFile == (other.Metadata ?? other.Mapset.Metadata).BackgroundFile;
		}

		/// <summary>
		/// Returns the full path to the audio file.
		/// </summary>
        public string GetFullAudioPath()
        {
            return Path.Combine(Mapset.Directory.FullName, (Metadata ?? Mapset.Metadata).AudioFile);
        }

		/// <summary>
		/// Returns the full path to the background.
		/// </summary>
        public string GetFullBackgroundPath()
        {
            return Path.Combine(Mapset.Directory.FullName, (Metadata ?? Mapset.Metadata).BackgroundFile);
        }
    }
}

