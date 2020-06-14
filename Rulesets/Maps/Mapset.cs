using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using PBFramework.DB;
using PBFramework.DB.Entities;
using Newtonsoft.Json;

namespace PBGame.Rulesets.Maps
{
    public class Mapset : DatabaseEntity, IMapset {

        private int? mapsetId;


        [Indexed]
        public int HashCode { get; set; }

        public DateTime ImportedDate { get; set; }

        [JsonIgnore]
        public int? MapsetId
        {
            get => mapsetId;
            set => mapsetId = value > 0 ? value : null;
        }

        [JsonIgnore]
        public List<IOriginalMap> Maps { get; set; } = new List<IOriginalMap>();

        [JsonIgnore]
        public FileInfo StoryboardFile { get; set; }

        [JsonIgnore]
        public MapMetadata Metadata => Maps[0].Metadata;

        [JsonIgnore]
        public List<FileInfo> Files { get; } = new List<FileInfo>();

        [JsonIgnore]
        public DirectoryInfo Directory { get; set; }


        public void SortMapsByMode(GameModeType gameMode)
        {
            Maps.Sort((x, y) => {
				var diffX = x.GetPlayable(gameMode).Difficulty;
				var diffY = y.GetPlayable(gameMode).Difficulty;
				var scaleX = diffX.Scale;
				var scaleY = diffY.Scale;
				if(diffX.GameMode != gameMode)
					scaleX += ((int)diffX.GameMode + 1) * 1000;
				if(diffY.GameMode != gameMode)
					scaleY += ((int)diffY.GameMode + 1) * 1000;
				return scaleX.CompareTo(scaleY);
            });
        }

        public IEnumerable GetHashParams()
        {
            var metadata = Metadata;
            if (metadata != null)
            {
                yield return metadata.Title;
                yield return metadata.Artist;
                yield return metadata.Creator;
            }
            yield return mapsetId;
        }

        public IEnumerable<string> GetQueryables()
        {
            foreach (var map in Maps)
            {
                foreach (var field in map.GetQueryables())
                    yield return field;
            }
        }
    }
}