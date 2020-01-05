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


		public HitObjectParser(double offset, int formatVersion) : base(offset, formatVersion) {}

		protected override HitObject CreateCircle (Vector2 pos, bool isNewCombo, int comboOffset)
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

		protected override HitObject CreateSlider (Vector2 pos, bool isNewCombo, int comboOffset, Vector2[] controlPoints,
            double length, PathTypes pathType, int repeatCount, List<List<SoundInfo>> nodeSamples)
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

		protected override HitObject CreateSpinner (Vector2 pos, bool isNewCombo, int comboOffset, double endTime)
		{
			forceNewCombo |= (formatVersion <= 8 || isNewCombo);
			extraComboOffset += comboOffset;

			return new ParsedSpinner() {
				Position = pos,
				EndTime = endTime
			};
		}

		protected override HitObject CreateHold (Vector2 pos, bool isNewCombo, int comboOffset, double endTime)
		{
			return null;
		}
	}
}

