using System;
using System.Linq;
using System.Collections.Generic;
using PBGame.Rulesets.Osu.Objects;
using PBGame.Rulesets.Objects;
using PBGame.Audio;
using PBFramework.Utils;
using UnityEngine;

namespace PBGame.IO.Decoding.Osu
{
	/// <summary>
	/// Class which parses legacy osu hit objects.
	/// </summary>
	public abstract class HitObjectParser : IHitObjectParser {

		/// <summary>
		/// Offset to apply on hit objects (format version < 5 have +24 greater offset).
		/// </summary>
		protected float offset;

		/// <summary>
		/// Version of the beatmap format.
		/// </summary>
		protected int formatVersion;

		/// <summary>
		/// Whether the first object has been parsed.
		/// </summary>
		protected bool isFirstObject;


		public HitObjectParser(float offset, int formatVersion)
		{
			isFirstObject = true;
			this.offset = offset;
			this.formatVersion = formatVersion;
		}

		public HitObject Parse (string text)
		{
			try
			{
				string[] splits = text.Split(',');

				Vector2 pos = new Vector2(ParseUtils.ParseFloat(splits[0]), ParseUtils.ParseFloat(splits[1]));
				float startTime = ParseUtils.ParseFloat(splits[2]) + offset;
				HitObjectTypes type = (HitObjectTypes)ParseUtils.ParseInt(splits[3]);

				int comboOffset = (int)(type & HitObjectTypes.ComboOffset) >> 4;
				type &= ~HitObjectTypes.ComboOffset;

				bool isNewCombo = (int)(type & HitObjectTypes.NewCombo) != 0;
				type &= ~HitObjectTypes.NewCombo;

				var soundType = (SoundTypes)ParseUtils.ParseInt(splits[4]);
				var customSample = new CustomSampleInfo();

				// Now parse the actual hit objects.
				HitObject result = null;
				// If this object is a hit circle
				if((type & HitObjectTypes.Circle) != 0)
				{
					result = CreateCircle(pos, isNewCombo, comboOffset);

					if(splits.Length > 5)
						ParseCustomSample(splits[5], customSample);
				}
				else if((type & HitObjectTypes.Slider) != 0)
				{
					PathTypes pathType = PathTypes.Catmull;
					float length = 0;
					string[] pointSplits = splits[5].Split('|');

					// Find the number of valid slider node points.
					int pointCount = 1;
					foreach(var p in pointSplits)
					{
						if(p.Length > 1)
							pointCount ++;
					}

					// Parse node points
					var nodePoints = new Vector2[pointCount];
					nodePoints[0] = Vector2.zero;

					int pointIndex = 1;
					foreach(var p in pointSplits)
					{
						// Determine which path type was found.
						if(p.Length == 1)
						{
							switch(p)
							{
							case "C":
								pathType = PathTypes.Catmull;
								break;
							case "B":
								pathType = PathTypes.Bezier;
								break;
							case "L":
								pathType = PathTypes.Linear;
								break;
							case "P":
								pathType = PathTypes.PerfectCurve;
								break;
							}
							continue;
						}
						// Parse point position
						string[] pointPos = p.Split(':');
						nodePoints[pointIndex++] = new Vector2(ParseUtils.ParseFloat(pointPos[0]), ParseUtils.ParseFloat(pointPos[1])) - pos;
					}

					// Change perfect curve to linear if certain conditions meet.
					if(nodePoints.Length == 3 && pathType == PathTypes.PerfectCurve && IsLinearPerfectCurve(nodePoints))
						pathType = PathTypes.Linear;

					// Parse slider repeat count
					int repeatCount = ParseUtils.ParseInt(splits[6]);
					if(repeatCount > 9000)
						throw new Exception();
					// Osu file has +1 addition to the actual number of repeats.
					repeatCount = Math.Max(0, repeatCount - 1);

					if(splits.Length > 7)
						length = Math.Max(0, ParseUtils.ParseFloat(splits[7]));

					if(splits.Length > 10)
						ParseCustomSample(splits[10], customSample);

					// Number of repeats + start(1) + end(1)
					int nodeCount = repeatCount + 2;

					// Parse per-node sound samples
					var nodeCustomSamples = new List<CustomSampleInfo>();
					for(int i=0; i<nodeCount; i++)
						nodeCustomSamples.Add(customSample.Clone());

					if(splits.Length > 9 && splits[9].Length > 0)
					{
						string[] sets = splits[9].Split('|');
						for(int i=0; i<nodeCount; i++)
						{
							if(i >= sets.Length)
								break;

							ParseCustomSample(sets[i], nodeCustomSamples[i]);
						}
					}

					// Set all nodes' sample types to default.
					var nodeSampleTypes = new List<SoundTypes>();
					for(int i=0; i<nodeCount; i++)
						nodeSampleTypes.Add(soundType);

					// Parse per-node sample types
					if(splits.Length > 8 && splits[8].Length > 0)
					{
						string[] nodeSampleSplits = splits[8].Split('|');
						for(int i=0; i<nodeCount; i++)
						{
							if(i > nodeSampleSplits.Length)
								break;

							nodeSampleTypes[i] = (SoundTypes)ParseUtils.ParseInt(nodeSampleSplits[i]);
						}
					}

					// Map sample types to custom sample infos.
					var nodeSamples = new List<List<SoundInfo>>(nodeCount);
					for(int i=0; i<nodeCount; i++)
						nodeSamples.Add(GetSamples(nodeSampleTypes[i], nodeCustomSamples[i]));

					result = CreateSlider(pos, isNewCombo, comboOffset, nodePoints, length, pathType, repeatCount, nodeSamples);
					// Hit sound for the root slider should be played at the end.
					result.Samples = nodeSamples[nodeSamples.Count - 1];
				}
				else if((type & HitObjectTypes.Spinner) != 0)
				{
					float endTime = Math.Max(startTime, ParseUtils.ParseFloat(splits[5]) + offset);
					result = CreateSpinner(pos, isNewCombo, comboOffset, endTime);
					if(splits.Length > 6)
						ParseCustomSample(splits[6], customSample);
				}
				else if((type & HitObjectTypes.Hold) != 0)
				{
					float endTime = Math.Max(startTime, ParseUtils.ParseFloat(splits[2] + offset));

					// I can understand all others except this, because Hold type only exists for Mania mode.
					if(splits.Length > 5 && !string.IsNullOrEmpty(splits[5]))
					{
						string[] sampleSplits = splits[5].Split(':');
						endTime = Math.Max(startTime, ParseUtils.ParseFloat(sampleSplits[0]));
						ParseCustomSample(string.Join(":", sampleSplits.Skip(1).ToArray()), customSample);
					}

					result = CreateHold(pos, isNewCombo, comboOffset, endTime + offset);
				}

				if(result == null)
				{
//					Debug.LogError("HitObjectParser.Parse - Unknown hit object for line: " + text);
					return null;
				}

				result.StartTime = startTime;
				if(result.Samples.Count == 0)
					result.Samples = GetSamples(soundType, customSample);
				isFirstObject = false;
				return result;
			}
			catch(Exception e)
			{
				Debug.LogErrorFormat(
					"HitObjectParser.Parse - Failed to parse line: {0}, Error: {1}",
					text,
					e.Message
				);
			}
			return null;
		}

		/// <summary>
		/// Creates a new parsed circle object.
		/// </summary>
		protected abstract HitObject CreateCircle(Vector2 pos, bool isNewCombo, int comboOffset);

		/// <summary>
		/// Creates a new parsed slider object.
		/// </summary>
		protected abstract HitObject CreateSlider(Vector2 pos, bool isNewCombo, int comboOffset, Vector2[] controlPoints,
			float length, PathTypes pathType, int repeatCount, List<List<SoundInfo>> nodeSamples);

		/// <summary>
		/// Creates a new parsed spinner object.
		/// </summary>
		protected abstract HitObject CreateSpinner(Vector2 pos, bool isNewCombo, int comboOffset, float endTime);

		/// <summary>
		/// Creates a new parsed hold object.
		/// </summary>
		protected abstract HitObject CreateHold(Vector2 pos, bool isNewCombo, int comboOffset, float endTime);

		/// <summary>
		/// Whether specified control points are labelled PerfectCurve path type but it's actually linear in terms of coordinates.
		/// </summary>
		private bool IsLinearPerfectCurve(Vector2[] p)
		{
			return MathUtils.AlmostEquals(
				0,
				(p[1].y - p[0].y) * (p[2].x - p[0].x) - (p[1].x - p[0].x) * (p[2].y - p[0].y)
			);
		}

		/// <summary>
		/// Converts specified custom sample info into a list of sample infos that will be used in the game play.
		/// </summary>
		private List<SoundInfo> GetSamples(SoundTypes type, CustomSampleInfo customInfo)
		{
			// Include normal sound as default
			var samples = new List<SoundInfo>() {
				new LegacySampleInfo() {
					Sample = customInfo.NormalSample,
					Sound = SoundInfo.HitNormal,
					Volume = customInfo.Volume,
					VariantNumber = customInfo.Variant
				}
			};

			if((type & SoundTypes.Finish) != 0)
			{
				samples.Add(new LegacySampleInfo() {
					Sample = customInfo.AdditionalSample,
					Sound = SoundInfo.HitFinish,
					Volume = customInfo.Volume,
					VariantNumber = customInfo.Variant
				});
			}

			if((type & SoundTypes.Whistle) != 0)
			{
				samples.Add(new LegacySampleInfo() {
					Sample = customInfo.AdditionalSample,
					Sound = SoundInfo.HitWhistle,
					Volume = customInfo.Volume,
					VariantNumber = customInfo.Variant
				});
			}

			if((type & SoundTypes.Clap) != 0)
			{
				samples.Add(new LegacySampleInfo() {
					Sample = customInfo.AdditionalSample,
					Sound = SoundInfo.HitClap,
					Volume = customInfo.Volume,
					VariantNumber = customInfo.Variant
				});
			}

			return samples;
		}

		/// <summary>
		/// Parses custom sample information to support per-object sound variations.
		/// </summary>
		private void ParseCustomSample(string text, CustomSampleInfo info)
		{
			if(string.IsNullOrEmpty(text))
				return;

			string[] splits = text.Split(':');

			var normalType = (OsuBeatmapDecoder.SampleTypes)ParseUtils.ParseInt(splits[0]);
			var additionalType = (OsuBeatmapDecoder.SampleTypes)ParseUtils.ParseInt(splits[1]);

			string normalTypeStr = normalType.ToString().ToLowerInvariant();
			if(normalTypeStr == "none")
				normalTypeStr = null;
			string additionalTypeStr = additionalType.ToString().ToLowerInvariant();
			if(additionalTypeStr == "none")
				additionalTypeStr = null;

			info.NormalSample = normalTypeStr;
			info.AdditionalSample = string.IsNullOrEmpty(additionalTypeStr) ? normalTypeStr : additionalTypeStr;

			if(splits.Length > 2)
				info.Variant = ParseUtils.ParseInt(splits[2]);
			if(splits.Length > 3)
				info.Volume = ParseUtils.ParseFloat(splits[3]) / 100f;
			info.FileName = splits.Length > 4 ? splits[4] : null;
		}

		/// <summary>
		/// For per-object sample customization.
		/// </summary>
		private class CustomSampleInfo {

			public string FileName;
			public string NormalSample;
			public string AdditionalSample;
			public int Variant;
			public float Volume;


			/// <summary>
			/// Clones this sample info.
			/// </summary>
			public CustomSampleInfo Clone() { return (CustomSampleInfo)MemberwiseClone(); }
		}

		/// <summary>
		/// Sample info used for parsing out hit sounds from objects.
		/// </summary>
		private class LegacySampleInfo : SoundInfo {
			
			/// <summary>
			/// Sets variant value in SampleInfo with specified value.
			/// </summary>
			public int VariantNumber { set { if(value > 1) Variant = value.ToString(); } }
		}

		// TODO: Support file sample when necessary.
		/// <summary>
		/// Sample info used for supporting file-override hit sound.
		/// </summary>
		private class FileSampleInfo : SoundInfo {

			public string FileName;


			public override IEnumerable<string> LookupNames
			{
				get
				{
					yield return FileName;
					foreach(var name in base.LookupNames)
						yield return name;
				}
			}
		}
	}
}

