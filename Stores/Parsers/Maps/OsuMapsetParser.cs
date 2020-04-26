using System.IO;
using PBGame.Rulesets;
using PBGame.Rulesets.Maps;
using PBFramework.IO.Decoding;
using PBFramework.Utils;
using PBFramework.Debugging;

namespace PBGame.Stores.Parsers.Maps
{
    public class OsuMapsetParser : IMapsetParser {

        private IModeManager modeManager;


        public OsuMapsetParser(IModeManager modeManager)
        {
            this.modeManager = modeManager;
        }

        public Mapset Parse(DirectoryInfo directory, Mapset mapset)
        {
            mapset.Maps.Clear();
            mapset.Files.Clear();
            mapset.Files.AddRange(directory.GetFiles());

            // Search for map files in the directory.
            foreach(var file in mapset.Files)
			{
                // If a valid beatmap file extension
                if(file.Extension.Equals(".osu"))
				{
					// Open stream for read
					using(var stream = file.OpenText())
					{
						// Get decoder.
						var decoder = Decoders.GetDecoder<OriginalMap>(stream);
						if(decoder != null)
						{
                            // Decode file into beatmap.
                            OriginalMap map = decoder.Decode(stream);
							if(map != null)
							{
								// Store file info.
								map.Detail.MapFile = file;

								// Assign beatmap set id.
								if(!mapset.MapsetId.HasValue)
									mapset.MapsetId = map.Detail.MapsetId;

								// Assign beatmap set.
								map.Detail.Mapset = mapset;
								// Add beatmap to beatmap set.
								mapset.Maps.Add(map);
							}
						}
					}
				}
            }

            // If there is no map included, this is an invalid map.
            if (mapset.Maps.Count == 0)
            {
                Logger.LogWarning($"OsuMapsetParser - No map file found for directory: {directory.FullName}");
                return null;
            }

			foreach(var map in mapset.Maps)
			{
                // Prepare converted maps for different modes.
                map.CreatePlayable(modeManager);
				
				// Calculate beatmap file hash.
				map.Detail.Hash = FileUtils.GetHash(map.Detail.MapFile);
            }

			return mapset;
        }
    }
}