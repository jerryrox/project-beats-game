using PBGame.Rulesets.Difficulty;
using PBFramework.Utils;
using UnityEngine;

using Logger = PBFramework.Debugging.Logger;

namespace PBGame.Graphics
{
    public class ColorPreset : IColorPreset {

        public Color PrimaryFocus { get; private set; } = HexColor.Create("A7D7FFFF");

        public Color SecondaryFocus { get; private set; } = HexColor.Create("FFFFA0FF");

        public Color Positive { get; private set; } = HexColor.Create("00BC00");

        public Color Negative { get; private set; } = HexColor.Create("D10000");

        public Color Warning { get; private set; } = HexColor.Create("D19500");


        public Color GetDifficultyColor(DifficultyTypes type)
        {
            switch (type)
            {
                case DifficultyTypes.Easy: return HexColor.Create("A1FFB9");
                case DifficultyTypes.Normal: return HexColor.Create("EAFFA1");
                case DifficultyTypes.Hard: return HexColor.Create("FFCEA1");
                case DifficultyTypes.Insane: return HexColor.Create("FFA1A1");
                case DifficultyTypes.Extreme: return HexColor.Create("B8AFFF");
            }
            Logger.LogWarning($"ColorPreset.GetDifficultyColor - Unknown type: {type}");
            return Color.black;
        }
    }
}