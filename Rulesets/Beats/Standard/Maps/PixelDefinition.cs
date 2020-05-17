using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Rulesets.Beats.Standard.Objects;
using UnityEngine;

namespace PBGame.Rulesets.Beats.Standard.Maps
{
    public class PixelDefinition {

        /// <summary>
        /// Width of the play area.
        /// </summary>
        public const float PlayAreaWidth = 1400f - HitObject.BaseRadius * 2f;


        public GameModeType FromMode { get; private set; }

        /// <summary>
        /// Returns the positional scale from other game mode to beats standard.
        /// </summary>
        public float Scale { get; private set; } = 1f;

        /// <summary>
        /// Returns the offset which adjusts the X position of peer mode to match beats standard.
        /// </summary>
        public float Offset { get; private set; } = 0f;


        public PixelDefinition(GameModeType fromMode)
        {
            this.FromMode = fromMode;

            switch (fromMode)
            {
                case GameModeType.OsuStandard:
                    // TODO: Refer from osu pixel definition.
                    Scale = PlayAreaWidth / 512;
                    Offset = -PlayAreaWidth * 0.5f;
                    break;
            }
        }

        /// <summary>
        /// Returns the converted X position of the object at specified original pos.
        /// </summary>
        public float GetX(float original) => original * Scale + Offset;
    }
}