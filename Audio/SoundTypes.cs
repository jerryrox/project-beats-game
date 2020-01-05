using System;

namespace PBGame.Audio
{
	/// <summary>
	/// Types of sounds included in a sample group.
	/// </summary>
	[Flags]
	public enum SoundTypes {

		None = 0,
		Normal = 1,
		Whistle = 2,
		Finish = 4,
		Clap = 8
	}
}