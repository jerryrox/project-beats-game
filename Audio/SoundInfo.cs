using System;
using System.Collections.Generic;

namespace PBGame.Audio
{
	/// <summary>
	/// Class which represents information about a sound within a sample group.
	/// </summary>
	public class SoundInfo {

		public const string HitWhistle = "hitwhistle";
		public const string HitFinish = "hitfinish";
		public const string HitNormal = "hitnormal";
		public const string HitClap = "hitclap";
		public const string Slidertick = "slidertick";


		/// <summary>
		/// Top-level optional container for separate grouping of samples and sounds.
		/// </summary>
		public string Namespace { get; set; }

		/// <summary>
		/// Name of the sample group.
		/// </summary>
		public string Sample { get; set; }

		/// <summary>
		/// Name of the sound within a sample.
		/// </summary>
		public string Sound { get; set; }

		/// <summary>
		/// Variant number of the same sample & sound group.
		/// </summary>
		public string Variant { get; set; }

		/// <summary>
		/// Volume of the sample to be played.
		/// </summary>
		public float Volume { get; set; }


		/// <summary>
		/// Returns all possible name combinations for retrieving the sound asset..
		/// </summary>
		public virtual IEnumerable<string> LookupNames
		{
			get
			{
				if(!string.IsNullOrEmpty(Namespace))
				{
					if(!string.IsNullOrEmpty(Variant))
						yield return string.Format("{0}/{1}-{2}{3}", Namespace, Sample, Sound, Variant);
					yield return string.Format("{0}/{1}-{2}", Namespace, Sample, Sound);
				}
				if(!string.IsNullOrEmpty(Variant))
					yield return string.Format("{0}-{1}{2}", Sample, Sound, Variant);
				yield return string.Format("{0}-{1}", Sample, Sound);
			}
		}

		/// <summary>
		/// Returns the shallow clone of this object.
		/// </summary>
		public SoundInfo Clone() { return (SoundInfo)MemberwiseClone(); }
	}
}

