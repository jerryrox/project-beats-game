using PBFramework.Utils;
using UnityEngine;

namespace PBGame.Graphics
{
    public class ColorPreset : IColorPreset {

        public Color PrimaryFocus { get; private set; } = HexColor.Create("A7D7FFFF");

        public Color SecondaryFocus { get; private set; } = HexColor.Create("FFFFA0FF");

        public Color Positive { get; private set; } = HexColor.Create("00BC00");

        public Color Negative { get; private set; } = HexColor.Create("D10000");

        public Color Warning { get; private set; } = HexColor.Create("D19500");
    }
}