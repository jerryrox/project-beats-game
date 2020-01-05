using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using PBGame.Maps;
using PBGame.Rulesets;
using PBGame.Rulesets.Maps;
using PBGame.Rulesets.Osu.Maps;
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

        public Mapset Parse(DirectoryInfo directory)
        {
            OsuMapset mapset = new OsuMapset();
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
						var decoder = Decoders.GetDecoder<Map>(stream);
						if(decoder != null)
						{
							// Decode file into beatmap.
							Map map = decoder.Decode(stream);
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
                Logger.Log($"OsuMapsetParser - No map file found for directory: {directory.FullName}");
                return null;
            }

			foreach(var map in mapset.Maps)
			{
				// Calculate difficulties.
				foreach(var servicer in modeManager.AllServices())
				{
					var calculator = servicer.CreateDifficultyCalculator(map);
					map.Difficulties.Add(calculator.Calculate());
				}

				// Calculate beatmap file hash.
				map.Detail.Hash = FileUtils.GetHash(map.Detail.MapFile);
			}

			return mapset;
        }
    }
}