using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using PBGame.Skins;
using PBFramework.Debugging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PBGame.Stores.Parsers.Skins
{
    public class BeatsSkinParser : ISkinParser {

        public Skin Parse(DirectoryInfo directory, Skin skin)
        {
            var info = directory.GetFiles("info.txt").FirstOrDefault();
            if(info == null) return null;

            try
            {
                var json = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(info.FullName));

                skin.Metadata.Name = json["Name"].ToString();
                skin.Metadata.Creator = json["Creator"].ToString();
                return skin;
            }
            catch (Exception e)
            {
                Logger.Log($"BeatsSkinParser.Parse - Failed to parse skin info at: {info.FullName}");
                return null;
            }
        }
    }
}