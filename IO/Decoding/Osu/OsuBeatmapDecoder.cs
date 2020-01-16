using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using PBGame.Rulesets.Maps;
using PBGame.Rulesets.Maps.Timing;
using PBGame.Rulesets.Maps.ControlPoints;
using PBGame.Rulesets.Objects;
using PBGame.Rulesets;
using PBFramework.IO.Decoding;
using PBFramework.Utils;
using UnityEngine;

namespace PBGame.IO.Decoding.Osu
{
	/// <summary>
	/// Decoder for osu standard beatmaps.
	/// </summary>
	public class OsuBeatmapDecoder : OsuDecoder<Map> {

		/// <summary>
		/// The latest osu beatmap format version.
		/// </summary>
		public const int LatestVersion = 14;

		/// <summary>
		/// Parser which parses hit objects for the specified game mode.
		/// </summary>
		private IHitObjectParser objectParser;

		/// <summary>
		/// The beatmap currently being decoded into.
		/// </summary>
		private Map map;

		/// <summary>
		/// The default sample type defined in metadata.
		/// </summary>
		private SampleTypes defaultSampleType;

		/// <summary>
		/// The default sample volume defined in metadata.
		/// </summary>
		private float defaultSampleVolume;

		/// <summary>
		/// Osu maps before version 5 has wrong offset by +24.
		/// </summary>
		private int offset;


		public OsuBeatmapDecoder(int formatVersion) : base(formatVersion)
		{
			offset = (formatVersion < 5 ? 24 : 0);
		}

		/// <summary>
		/// Registers this decoder to the Decoder provider.
		/// </summary>
		public static void RegisterDecoder()
		{
			Decoders.AddDecoder<Map>(
				"osu file format v",
				(header) => new OsuBeatmapDecoder(ParseUtils.ParseInt(header.Split('v').Last(), LatestVersion))
			);
		}

		public override void Decode (StreamReader stream, Map result)
		{
			this.map = result;
			this.map.Detail.FormatVersion = formatVersion;

			base.Decode (stream, result);

			// Apparently, some osu maps don't seem to have hit objects in order.
			this.map.HitObjects.Sort((x, y) => x.StartTime.CompareTo(y.StartTime));

			// Apply map properties to all hit objects.
			foreach(var obj in this.map.HitObjects)
				obj.ApplyMapProperties(this.map.ControlPoints, this.map.Detail.Difficulty);
		}

		/// <summary>
		/// Creates a new timing point for different modes.
		/// </summary>
		protected virtual TimingControlPoint CreateTimingPoint() => new TimingControlPoint();

		protected override bool ShouldSkipLine (string line)
		{
			// Storyboard parsing should not be supported in beatmap parsing.
			return base.ShouldSkipLine (line) ||
				line.StartsWith(" ", StringComparison.Ordinal) || line.StartsWith("_", StringComparison.Ordinal);
		}

		protected override void DecodeLine (Map result, Sections section, string line)
		{
			var l = TrimLine(line);

			switch(section)
			{
			case Sections.General:
				DecodeGeneral(line);
				break;
			case Sections.Metadata:
				DecodeMetadata(line);
				break;
			case Sections.Difficulty:
				DecodeDifficulty(line);
				break;
			case Sections.Events:
				DecodeEvents(line);
				break;
			case Sections.TimingPoints:
				DecodeTimingPoints(line);
				break;
			case Sections.HitObjects:
				DecodeHitObjects(line);
				break;
			}

			base.DecodeLine(result, section, line);
		}

		/// <summary>
		/// Decodes general data.
		/// </summary>
		private void DecodeGeneral(string line)
		{
			var data = GetKeyValue(line);
			var detail = map.Detail;
			var metadata = detail.Metadata;

			switch(data.Key)
			{
			case "AudioFilename":
				metadata.AudioFile = PathUtils.StandardPath(data.Value);
				break;
			case "AudioLeadIn":
				detail.AudioLeadIn = ParseUtils.ParseInt(data.Value);
				break;
			case "PreviewTime":
				metadata.PreviewTime = ParseUtils.ParseInt(data.Value) + offset;
				break;
			case "Countdown":
				detail.Countdown = ParseUtils.ParseInt(data.Value) == 1;
				break;
			case "SampleSet":
				defaultSampleType = (SampleTypes)Enum.Parse(typeof(SampleTypes), data.Value);
				break;
			case "SampleVolume":
				defaultSampleVolume = ParseUtils.ParseFloat(data.Value, 100) / 100f;
				break;
			case "StackLeniency":
				detail.StackLeniency = ParseUtils.ParseFloat(data.Value);
				break;
			case "Mode":
				detail.GameMode = (GameModes)(ParseUtils.ParseInt(data.Value) + GameProviders.Osu);

				switch(detail.GameMode)
				{
						// Osu Standard mode
					case GameModes.OsuStandard:
						objectParser = new Standard.HitObjectParser(offset, formatVersion);
						break;
						// TODO: Osu Taiko mode
					// case GameModes.OsuTaiko:
					// 	break;
						// TODO: Osu Catch mode
					// case GameModes.OsuCatch:
					// 	break;
						// TODO: Osu Mania mode
					// case GameModes.OsuMania:
					// 	break;
				}
				break;
			case "LetterboxInBreaks":
				detail.LetterboxInBreaks = ParseUtils.ParseInt(data.Value) == 1;
				break;
				// TODO: Osu file contains a SpecialStyle property but it seems it's not being used anywhere.
				// May have to come back in future once I confirm this.
			case "SpecialStyle":
				break;
			case "WidescreenStoryboard":
				detail.WidescreenStoryboard = ParseUtils.ParseInt(data.Value) == 1;
				break;
			}
		}

		/// <summary>
		/// Decodes metadata.
		/// </summary>
		private void DecodeMetadata(string line)
		{
			var data = GetKeyValue(line);
			var detail = map.Detail;
			var metadata = detail.Metadata;

			switch(data.Key)
			{
			case "Title":
				metadata.Title = data.Value;
				break;
			case "TitleUnicode":
				metadata.TitleUnicode = data.Value;
				break;
			case "Artist":
				metadata.Artist = data.Value;
				break;
			case "ArtistUnicode":
				metadata.ArtistUnicode = data.Value;
				break;
			case "Creator":
				metadata.Creator = data.Value;
				break;
			case "Version":
				detail.Version = data.Value;
				break;
			case "Source":
				metadata.Source = data.Value;
				break;
			case "Tags":
				metadata.Tags = data.Value;
				break;
			case "BeatmapID":
				detail.MapId = ParseUtils.ParseInt(data.Value);
				break;
			case "BeatmapSetID":
				detail.MapsetId = ParseUtils.ParseInt(data.Value);
				break;
			}
		}

		/// <summary>
		/// Decodes difficulty data.
		/// </summary>
		private void DecodeDifficulty(string line)
		{
			var data = GetKeyValue(line);
			var difficulty = map.Detail.Difficulty;

			switch(data.Key)
			{
			case "HPDrainRate":
				difficulty.HpDrainRate = ParseUtils.ParseFloat(data.Value);
				break;
			case "CircleSize":
				difficulty.CircleSize = ParseUtils.ParseFloat(data.Value);
				break;
			case "OverallDifficulty":
				difficulty.OverallDifficulty = ParseUtils.ParseFloat(data.Value);
				break;
			case "ApproachRate":
				difficulty.ApproachRate = ParseUtils.ParseFloat(data.Value);
				break;
			case "SliderMultiplier":
				difficulty.SliderMultiplier = ParseUtils.ParseDouble(data.Value);
				break;
			case "SliderTickRate":
				difficulty.SliderTickRate = ParseUtils.ParseDouble(data.Value);
				break;
			}
		}

		/// <summary>
		/// Decodes event data.
		/// </summary>
		private void DecodeEvents(string line)
		{
			var splits = line.Split(',');

			EventTypes type = (EventTypes)Enum.Parse(typeof(EventTypes), splits[0]);

			// Only two event types will be checked, as the rest should be decoded by storyboard decoder.
			switch(type)
			{
			case EventTypes.Background:
				string fileName = splits[2].Trim('"');
				map.Detail.Metadata.BackgroundFile = PathUtils.StandardPath(fileName);
				break;
			case EventTypes.Break:
				double start = ParseUtils.ParseDouble(splits[1]) + offset;
				double end = Math.Max(start, ParseUtils.ParseDouble(splits[2]) + offset);

				if(!BreakPoint.CanBeValid(start, end))
					return;

				map.BreakPoints.Add(new BreakPoint() {
					StartTime = start,
					EndTime = end
				});
				break;
			}
		}

		/// <summary>
		/// Decodes timing points.
		/// </summary>
		private void DecodeTimingPoints(string line)
		{
			try
			{
				string[] splits = line.Split(',');

				double time = ParseUtils.ParseDouble(splits[0].Trim()) + offset;
				double beatLength = ParseUtils.ParseDouble(splits[1].Trim());
				double speedMultiplier = beatLength < 0 ? 100 / -beatLength : 1;

				TimeSignatures timeSignature = TimeSignatures.Quadruple;
				if(splits.Length >= 3)
					timeSignature = splits[2][0] == '0' ? TimeSignatures.Quadruple : (TimeSignatures)ParseUtils.ParseInt(splits[2]);

				SampleTypes sampleType = defaultSampleType;
				if(splits.Length >= 4)
					sampleType = (SampleTypes)ParseUtils.ParseInt(splits[3]);
				
				string sampleTypeString = sampleType.ToString().ToLowerInvariant();
				if(sampleTypeString.Equals("none", StringComparison.Ordinal))
					sampleTypeString = "normal";

				int sampleVariation = 0;
				if(splits.Length >= 5)
					sampleVariation = ParseUtils.ParseInt(splits[4]);

				float sampleVolume = defaultSampleVolume;
				if(splits.Length >= 6)
					sampleVolume = ParseUtils.ParseFloat(splits[5]) / 100f;

				bool isTimingChange = true;
				if(splits.Length >= 7)
					isTimingChange = splits[6][0] == '1';

				bool isHighlight = false;
				if(splits.Length >= 8)
				{
					int flags = ParseUtils.ParseInt(splits[7]);
					isHighlight = (flags & (int)EffectFlags.IsHighlight) != 0;
				}

				if(isTimingChange)
				{
					var timingPoint = CreateTimingPoint();
					timingPoint.Time = time;
					timingPoint.BeatLength = beatLength;
					timingPoint.TimeSignature = timeSignature;

					AddTimingPoint(timingPoint);
				}

				AddDifficultyPoint(new DifficultyControlPoint() {
					Time = time,
					SpeedMultiplier = speedMultiplier
				});

				AddEffectPoint(new EffectControlPoint() {
					Time = time,
					IsHighlight = isHighlight
				});

				AddSamplePoint(new SampleControlPoint() {
					Time = time,
					Sample = sampleTypeString,
					Variant = sampleVariation,
					Volume = sampleVolume,
				});
			}
			catch(Exception e)
			{
				throw e;
			}
		}

		/// <summary>
		/// Decodes hit objects.
		/// </summary>
		private void DecodeHitObjects(string line)
		{
			// If parser not found, use Osu standard mode as default.
			if(objectParser == null)
				objectParser = new IO.Decoding.Osu.Standard.HitObjectParser(offset, formatVersion);

			var obj = objectParser.Parse(line);
			if(obj != null)
				map.HitObjects.Add(obj);
		}

		/// <summary>
		/// Adds specified timing point to the beatmap.
		/// </summary>
		private void AddTimingPoint(TimingControlPoint point)
		{
			// Replace existing point if equal time.
			var existingPoint = map.ControlPoints.TimingPointAt(point.Time);
			if(existingPoint.Time == point.Time)
				map.ControlPoints.TimingPoints.Remove(existingPoint);

			map.ControlPoints.TimingPoints.Add(point);
		}

		/// <summary>
		/// Adds specified difficulty point to the beatmap.
		/// </summary>
		private void AddDifficultyPoint(DifficultyControlPoint point)
		{
			// Replace existing point if equal time.
			var existingPoint = map.ControlPoints.DifficultyPointAt(point.Time);
			if(point.IsEquivalentTo(existingPoint))
				return;
			if(existingPoint.Time == point.Time)
				map.ControlPoints.DifficultyPoints.Remove(existingPoint);

			map.ControlPoints.DifficultyPoints.Add(point);
		}

		/// <summary>
		/// Adds specified effect point to the beatmap.
		/// </summary>
		private void AddEffectPoint(EffectControlPoint point)
		{
			// Replace existing point if equal time.
			var existingPoint = map.ControlPoints.EffectPointAt(point.Time);
			if(point.IsEquivalentTo(existingPoint))
				return;
			if(existingPoint.Time == point.Time)
				map.ControlPoints.EffectPoints.Remove(existingPoint);

			map.ControlPoints.EffectPoints.Add(point);
		}

		/// <summary>
		/// Adds specified sample point to the beatmap.
		/// </summary>
		private void AddSamplePoint(SampleControlPoint point)
		{
			// Replace existing point if equal time.
			var existingPoint = map.ControlPoints.SamplePointAt(point.Time);
			if(point.IsEquivalentTo(existingPoint))
				return;
			if(existingPoint.Time == point.Time)
				map.ControlPoints.SamplePoints.Remove(existingPoint);

			map.ControlPoints.SamplePoints.Add(point);
		}

		/// <summary>
		/// Types of flags included in effect control point.
		/// </summary>
		[Flags]
		private enum EffectFlags {

			None = 0,
			IsHighlight = 1,
			OmitFirstBarLine = 8
		}
	}
}

