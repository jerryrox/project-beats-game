using System.Collections.Generic;
using PBGame.Audio;

namespace PBGame.Rulesets.Objects
{
	/// <summary>
	/// Interface indicating that the implementing hit object has repeats.
	/// </summary>
	public interface IHasRepeats : IHasEndTime {

		/// <summary>
		/// Returns the number of repeats included.
		/// </summary>
		int RepeatCount { get; }

		/// <summary>
		/// Returns the list of sounds to be played at each node.
		/// Example: Repeat point of sliders.
		/// </summary>
		List<List<SoundInfo>> NodeSamples { get; }
	}

	public static class HasRepeatsExtension {

		/// <summary>
		/// Returns the total number of passes that can be made on a path.
		/// </summary>
		public static int SpanCount(this IHasRepeats context) { return context.RepeatCount + 1; }
	}
}

