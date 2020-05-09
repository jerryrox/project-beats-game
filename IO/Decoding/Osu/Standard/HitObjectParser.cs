using System;
using System.Collections.Generic;
using UnityEngine;
using PBGame.Audio;
using PBGame.Rulesets.Objects;
using PBGame.IO.Decoding.Osu.Standard.Objects;

namespace PBGame.IO.Decoding.Osu.Standard
{
	/// <summary>
	/// Class that parses hit objects for Osu standard mode.
	/// </summary>
	public class HitObjectParser : Osu.HitObjectParser {

		private bool forceNewCombo;
		private int extraComboOffset;


		public HitObjectParser(float offset, int formatVersion) : base(offset, formatVersion) {}

		protected override BaseHitObject CreateCircle (Vector2 pos, bool isNewCombo, int comboOffset)
		{
			isNewCombo |= forceNewCombo;
			comboOffset += extraComboOffset;

			forceNewCombo = false;
			extraComboOffset = 0;

			return new ParsedHitCircle() {
				Position = pos,
				IsNewCombo = isFirstObject || isNewCombo,
				ComboOffset = comboOffset
			};
		}

		protected override BaseHitObject CreateSlider (Vector2 pos, bool isNewCombo, int comboOffset, Vector2[] controlPoints,
            float length, PathType pathType, int repeatCount, List<List<SoundInfo>> nodeSamples)
		{
			isNewCombo |= forceNewCombo;
			comboOffset += extraComboOffset;

			forceNewCombo = false;
			extraComboOffset = 0;

			return new ParsedSlider() {
				Position = pos,
				IsNewCombo = isFirstObject || isNewCombo,
				ComboOffset = comboOffset,
				Path = new SliderPath(pathType, controlPoints, Math.Max(0, length)),
				NodeSamples = nodeSamples,
				RepeatCount = repeatCount
			};
		}

		protected override BaseHitObject CreateSpinner (Vector2 pos, bool isNewCombo, int comboOffset, float endTime)
		{
			forceNewCombo |= (formatVersion <= 8 || isNewCombo);
			extraComboOffset += comboOffset;

			return new ParsedSpinner() {
				Position = pos,
				EndTime = endTime
			};
		}

		protected override BaseHitObject CreateHold (Vector2 pos, bool isNewCombo, int comboOffset, float endTime)
		{
			return null;
		}
	}
}

