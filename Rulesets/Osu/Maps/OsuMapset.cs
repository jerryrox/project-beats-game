using System;
using System.IO;
using System.Linq;
using PBGame.Rulesets.Maps;

namespace PBGame.Rulesets.Osu.Maps
{
    public class OsuMapset : Mapset {

        public override FileInfo StoryboardFile
            => Files.Where(f => f.Extension.Equals(".osb", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
    }
}