using System;

namespace PBGame.Rulesets.Osu.Objects
{
	/// <summary>
	/// Types of hit objects defined in osu format.
	/// </summary>
	[Flags]
	public enum HitObjectTypes {
		
		Circle = 1 << 0,
		Slider = 1 << 1,
		NewCombo = 1 << 2,
		Spinner = 1 << 3,
		ComboOffset = (1 << 4) | (1 << 5) | (1 << 6),
		Hold = 1 << 7
	}
}

