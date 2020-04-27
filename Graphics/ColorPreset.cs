using System.Collections.Generic;
using PBGame.Rulesets.Difficulty;
using PBFramework.Utils;
using PBFramework.Graphics;
using UnityEngine;

using Logger = PBFramework.Debugging.Logger;

namespace PBGame.Graphics
{
    public class ColorPreset : IColorPreset {

        private Dictionary<DifficultyTypes, ColorPalette> difficultyColors = new Dictionary<DifficultyTypes, ColorPalette>()
        {
            { DifficultyTypes.Easy, new ColorPalette(HexColor.Create("A1FFB9")) },
            { DifficultyTypes.Normal, new ColorPalette(HexColor.Create("EAFFA1")) },
            { DifficultyTypes.Hard, new ColorPalette(HexColor.Create("FFCEA1")) },
            { DifficultyTypes.Insane, new ColorPalette(HexColor.Create("FFA1A1")) },
            { DifficultyTypes.Extreme, new ColorPalette(HexColor.Create("B8AFFF")) },
        };


        public ColorPalette PrimaryFocus { get; private set; } = new ColorPalette(HexColor.Create("A7D7FFFF"));

        public ColorPalette SecondaryFocus { get; private set; } = new ColorPalette(HexColor.Create("FFFFA0FF"));

        public ColorPalette Positive { get; private set; } = new ColorPalette(HexColor.Create("00BC00"));

        public ColorPalette Negative { get; private set; } = new ColorPalette(HexColor.Create("D10000"));

        public ColorPalette Warning { get; private set; } = new ColorPalette(HexColor.Create("D19500"));

        public ColorPalette Passive { get; private set; } = new ColorPalette(HexColor.Create("1F77BC"));

        public Color DarkBackground { get; private set; } = HexColor.Create("0E1216");


        public ColorPalette GetDifficultyColor(DifficultyTypes type)
        {
            if(difficultyColors.TryGetValue(type, out ColorPalette value))
                return value;
            Logger.LogWarning($"ColorPreset.GetDifficultyColor - Unknown type: {type}");
            return new ColorPalette(Color.black);
        }
    }
}