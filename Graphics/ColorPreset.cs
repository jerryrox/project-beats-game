using System.Collections.Generic;
using PBGame.Rulesets.Scoring;
using PBGame.Rulesets.Difficulty;
using PBGame.Rulesets.Judgements;
using PBFramework.Utils;
using PBFramework.Graphics;
using UnityEngine;

using Logger = PBFramework.Debugging.Logger;

namespace PBGame.Graphics
{
    public class ColorPreset : IColorPreset {

        private Dictionary<DifficultyType, ColorPalette> difficultyColors = new Dictionary<DifficultyType, ColorPalette>()
        {
            { DifficultyType.Easy, new ColorPalette(HexColor.Create("A1FFB9")) },
            { DifficultyType.Normal, new ColorPalette(HexColor.Create("EAFFA1")) },
            { DifficultyType.Hard, new ColorPalette(HexColor.Create("FFCEA1")) },
            { DifficultyType.Insane, new ColorPalette(HexColor.Create("FFA1A1")) },
            { DifficultyType.Extreme, new ColorPalette(HexColor.Create("B8AFFF")) },
        };

        private Dictionary<HitResultType, ColorPalette> hitResultColors = new Dictionary<HitResultType, ColorPalette>()
        {
            { HitResultType.Perfect, new ColorPalette(HexColor.Create("1F77BC")) },
            { HitResultType.Great, new ColorPalette(HexColor.Create("1FBBBC")) },
            { HitResultType.Good, new ColorPalette(HexColor.Create("1FB972")) },
            { HitResultType.Ok, new ColorPalette(HexColor.Create("8BB972")) },
            { HitResultType.Bad, new ColorPalette(HexColor.Create("FFB972")) },
            { HitResultType.Miss, new ColorPalette(HexColor.Create("FF271D")) },
        };

        private Dictionary<RankType, ColorPalette> rankColors = new Dictionary<RankType, ColorPalette>()
        {
            { RankType.XH, new ColorPalette(HexColor.Create("CCD9E3")) },
            { RankType.X, new ColorPalette(HexColor.Create("CCD9E3")) },
            { RankType.SH, new ColorPalette(HexColor.Create("4EB1FF")) },
            { RankType.S, new ColorPalette(HexColor.Create("4EB1FF")) },
            { RankType.A, new ColorPalette(HexColor.Create("00C300")) },
            { RankType.B, new ColorPalette(HexColor.Create("EAD500")) },
            { RankType.C, new ColorPalette(HexColor.Create("FF8400")) },
            { RankType.D, new ColorPalette(HexColor.Create("D20000")) },
        };

        public List<Color> DefaultComboColors { get; private set; } = new List<Color>()
        {
            new Color(1f, 0.25f, 25f),
            new Color(0.25f, 1f, 0.25f),
            new Color(0.25f, 0.25f, 1f)
        };

        public ColorPalette PrimaryFocus { get; private set; } = new ColorPalette(HexColor.Create("A7D7FFFF"));

        public ColorPalette SecondaryFocus { get; private set; } = new ColorPalette(HexColor.Create("FFFFA0FF"));

        public ColorPalette Positive { get; private set; } = new ColorPalette(HexColor.Create("00BC00"));

        public ColorPalette Negative { get; private set; } = new ColorPalette(HexColor.Create("D10000"));

        public ColorPalette Warning { get; private set; } = new ColorPalette(HexColor.Create("D19500"));

        public ColorPalette Passive { get; private set; } = new ColorPalette(HexColor.Create("1F77BC"));

        public Color DarkBackground { get; private set; } = HexColor.Create("0E1216");

        public Color Background { get; private set; } = HexColor.Create("1D2126");


        public ColorPalette GetDifficultyColor(DifficultyType type)
        {
            if(difficultyColors.TryGetValue(type, out ColorPalette value))
                return value;
            Logger.LogWarning($"ColorPreset.GetDifficultyColor - Unknown type: {type}");
            return new ColorPalette(Color.black);
        }

        public ColorPalette GetHitResultColor(HitResultType type)
        {
            if (hitResultColors.TryGetValue(type, out ColorPalette value))
                return value;
            Logger.LogWarning($"ColorPreset.GetHitResultColor - Unknown type: {type}");
            return new ColorPalette(Color.white);
        }

        public ColorPalette GetRankColor(RankType type)
        {
            if(rankColors.TryGetValue(type, out ColorPalette value))
                return value;
            Logger.LogWarning($"ColorPreset.GetRankColor - Unknown type: {type}");
            return new ColorPalette(Color.white);
        }
    }
}