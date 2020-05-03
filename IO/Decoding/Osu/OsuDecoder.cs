using System;
using System.IO;
using System.Collections.Generic;
using PBGame.Rulesets.Maps;
using PBGame.Skins;
using PBFramework.IO.Decoding;
using UnityEngine;

using Logger = PBFramework.Debugging.Logger;

namespace PBGame.IO.Decoding.Osu
{
	/// <summary>
	/// Base decoder for osu files.
	/// </summary>
	public abstract class OsuDecoder<T> : Decoder<T> where T : new() {

		/// <summary>
		/// Version number of the osu file.
		/// </summary>
		protected int formatVersion;

		/// <summary>
		/// Whether combo color has been defined in the map.
		/// </summary>
		private bool hasComboColor;


		protected OsuDecoder(int formatVersion)
		{
			this.formatVersion = formatVersion;
		}

		public override void Decode(StreamReader stream, T result)
		{
			SectionType section = SectionType.None;

			string line;
			while((line = stream.ReadLine()) != null)
			{
				if(ShouldSkipLine(line))
					continue;

				// If found a new section
				if(line.StartsWith("[", StringComparison.Ordinal) && line.EndsWith("]", StringComparison.Ordinal))
				{
					// Parse the section type
					string sectionString = line.Substring(1, line.Length-2);
					try
					{
						section = (SectionType)Enum.Parse(typeof(SectionType), sectionString);
					}
					catch(Exception e)
					{
						// If an error occurred, throw an exception.
						section = SectionType.None;
						throw e;
					}
					continue;
				}

				try
				{
					DecodeLine(result, section, line);
				}
				catch(Exception e)
				{
                    Logger.LogError(
                        $"OsuDecoder.Decode - Failed to decode line: {line}, Error: {e.Message}"
                    );
				}
			}
		}

		/// <summary>
		/// Decodes the specified line for current section.
		/// </summary>
		protected virtual void DecodeLine(T result, SectionType section, string line)
		{
			string l = TrimLine(line);

			switch(section)
			{
			case SectionType.Colours:
				DecodeColor(result, l);
				return;
			}
		}

		/// <summary>
		/// Decodes color value for specified line.
		/// </summary>
		protected void DecodeColor(T result, string line)
		{
			var pair = GetKeyValue(line);
			bool isCombo = pair.Key.StartsWith("Combo", StringComparison.Ordinal);
			string[] colorData = pair.Value.Split(',');

			// Invalid format
			if(colorData.Length < 3 || colorData.Length > 4)
			{
                Logger.LogWarning(
                    $"OsuDecoder.DecodeColor - Invalid color data format. Must be (R, G, B) or (R, G, B, A). ({pair.Value})"
                );
				return;
			}

			// Try parse color
			Color color;
			try
			{
				color = new Color(
					byte.Parse(colorData[0]) / 255f,
					byte.Parse(colorData[1]) / 255f,
					byte.Parse(colorData[2]) / 255f,
					(colorData.Length > 3 ? byte.Parse(colorData[3]) : 255) / 255f
				);
			}
			catch(Exception)
			{
                Logger.LogWarning(
                    $"OsuDecoder.DecodeColor -  Invalid color element. Each element must be a valid byte value. ({pair.Value})"
                );
				return;
			}

			// If a combo color property
			if(isCombo)
			{
				IComboColorable colorable = result as IComboColorable;
				if(colorable == null)
					return;

				// Remove default combo colors first.
				if(!hasComboColor)
				{
					colorable.ComboColors.Clear();
					hasComboColor = true;
				}
				// Add color
				colorable.ComboColors.Add(color);
			}
			else
			{
				ICustomColorable colorable = result as ICustomColorable;
				if(colorable == null)
					return;
				
				// Add color
				colorable.CustomColors[pair.Key] = color;
			}
		}

		/// <summary>
		/// Returns the trimmed line which doesn't contain comments.
		/// </summary>
		protected string TrimLine(string line)
		{
			int index = line.IndexOf("//", StringComparison.Ordinal);
			if(index > 0)
				return line.Substring(0, index);
			return line;
		}

		/// <summary>
		/// Returns whether specified line should be skipped from decoding..
		/// </summary>
		protected virtual bool ShouldSkipLine(string line)
		{
			return string.IsNullOrEmpty(line) || line.StartsWith("//", StringComparison.Ordinal) || line.StartsWith(" ", StringComparison.Ordinal);
		}

		/// <summary>
		/// Returns the key value representation for specified line.
		/// </summary>
		protected KeyValuePair<string, string> GetKeyValue(string line, char splitter = ':')
		{
			var splits = line.Trim().Split(new char[] {splitter}, 2);
			return new KeyValuePair<string, string>(
				splits[0].Trim(),
				splits.Length > 1 ? splits[1].Trim() : string.Empty
			);
		}


		/// <summary>
		/// Types of sound samples attributed to an object or control point.
		/// </summary>
		public enum SampleType {

			None = 0,
			Normal = 1,
			Soft = 2,
			Drum = 3
		}

		/// <summary>
		/// Types of sections in any osu file.
		/// </summary>
		protected enum SectionType {

			None,
			General,
			Editor,
			Metadata,
			Difficulty,
			Events,
			TimingPoints,
			Colours,
			HitObjects,
			Variables,
			Fonts
		}

		/// <summary>
		/// Types of events in an osu beatmap or storyboard..
		/// </summary>
		protected enum EventType {
			
			Background = 0,
			Video = 1,
			Break = 2,
			Colour = 3,
			Sprite = 4,
			Sample = 5,
			Animation = 6
		}

		/// <summary>
		/// Types of pivot points used for osu storyboards.
		/// </summary>
		protected enum PivotPointType {
			
			TopLeft,
			Centre,
			CentreLeft,
			TopRight,
			BottomCentre,
			TopCentre,
			Custom,
			CentreRight,
			BottomLeft,
			BottomRight
		}

		/// <summary>
		/// Types of layers used in an osu storyboard.
		/// </summary>
		protected enum StoryboardLayers
		{
			Background = 0,
			Fail = 1,
			Pass = 2,
			Foreground = 3
		}
	}
}

