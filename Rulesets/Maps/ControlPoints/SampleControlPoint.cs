using System;
using PBGame.Audio;

namespace PBGame.Rulesets.Maps.ControlPoints
{
	/// <summary>
	/// Control point which manipulates hit samples.
	/// </summary>
	public class SampleControlPoint : ControlPoint, IComparable<SampleControlPoint> {

		/// <summary>
		/// The default sample name to use if not specified.
		/// </summary>
		public const string DefaultSampleName = "normal";

		/// <summary>
		/// The sample to use within this point.
		/// </summary>
		public string Sample { get; set; } = DefaultSampleName;

		/// <summary>
		/// The variant number of the sample.
		/// </summary>
		public int Variant { get; set; } = 1;

		/// <summary>
		/// The volume of the hit sound.
		/// </summary>
		public float Volume { get; set; }


        /// <summary>
        /// Returns a new sound info object based on this control point.
        /// </summary>
        public SoundInfo GetSoundInfo(string soundName)
		{
			return new SoundInfo() {
				Sample = this.Sample,
				Sound = soundName,
				Variant = this.Variant.ToString(),
				Volume = this.Volume
			};
		}

		/// <summary>
		/// Returns a NEW sound info object based on this control point and specified sound info.
		/// This simply applies default sound settings if specified info has missing values.
		/// </summary>
		public SoundInfo CreateAppliedSample(SoundInfo other)
		{
			var applied = other.Clone();
			applied.Sample = other.Sample ?? Sample;
			applied.Variant = other.Variant ?? Variant.ToString();
			applied.Volume = other.Volume > 0 ? other.Volume : Volume;
			return applied;
		}

		/// <summary>
		/// Applies default sample settings on the specified sound info if there are any missing values.
		/// </summary>
		public void ApplySample(SoundInfo info)
		{
			info.Sample = info.Sample ?? Sample;
			info.Variant = info.Variant ?? Variant.ToString();
			info.Volume = info.Volume > 0 ? info.Volume : Volume;
		}

		public override bool IsEquivalentTo (ControlPoint other)
		{
			SampleControlPoint samplePoint = other as SampleControlPoint;
			if(samplePoint == null)
				return false;
			return base.IsEquivalentTo (other) &&
				Sample.Equals(samplePoint.Sample) &&
				Variant.Equals(samplePoint.Variant) &&
				Volume.Equals(samplePoint.Volume);
		}

		public int CompareTo(SampleControlPoint other)
		{
			return base.CompareTo(other);
		}
	}
}

