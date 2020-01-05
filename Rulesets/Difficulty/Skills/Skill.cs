using System;
using System.Linq;
using System.Collections;
using PBGame.Rulesets.Difficulty.Objects;
using PBFramework.Data;
using UnityEngine;

namespace PBGame.Rulesets.Difficulty.Skills
{
	/// <summary>
	/// An attribute of the player required to play a specific game mode.
	/// </summary>
	public abstract class Skill {

		/// <summary>
		/// List of previous objects that have been evaluated.
		/// </summary>
		protected LimitedStack<DifficultyHitObject> previousObjects = new LimitedStack<DifficultyHitObject>(2);

		/// <summary>
		/// List of all peak strain factors recorded per strain section.
		/// </summary>
		private SortedList<double> strainPeaks = new SortedList<double>(2);

		/// <summary>
		/// Current amount of strain on hold.
		/// </summary>
		private double currentStrain;

		/// <summary>
		/// Current highest strain peak during this section.
		/// </summary>
		private double currentStrainPeak;


		/// <summary>
		/// The amount of scoring this skill holds in comparison to other skills within the same game mode.
		/// </summary>
		protected abstract double StrainScore { get; }

		/// <summary>
		/// The amount of decay applied on current strain every strain section, per second.
		/// </summary>
		protected abstract double StrainDecay { get; }

		/// <summary>
		/// The amount of decay applied on weight factor for every next highest strain.
		/// </summary>
		protected virtual double WeightDecay { get { return 0.9; } }


		/// <summary>
		/// Stores current strain value into strains list.
		/// </summary>
		public void StoreStrainPeak()
		{
			if(previousObjects.Count > 0)
				strainPeaks.Add(currentStrainPeak);
		}

		/// <summary>
		/// Starts a new section at specified time.
		/// </summary>
		public void StartNewSection(double time)
		{
			// Since strain is carried over from last section, the new strain peak is not reset to 0.
			if(previousObjects.Count > 0)
				currentStrainPeak = currentStrain * GetStrainDecay(time - previousObjects.Last().BaseObject.StartTime);
		}

		/// <summary>
		/// Processes the specified hit object for current strain factor.
		/// </summary>
		public void Process(DifficultyHitObject obj)
		{
			currentStrain *= GetStrainDecay(obj.DeltaTime);
			currentStrain += CalculateStrain(obj);

			currentStrainPeak = Math.Max(currentStrain, currentStrainPeak);

			previousObjects.Push(obj);
		}

		/// <summary>
		/// Returns the calculated difficulty scale.
		/// </summary>
		public double GetDifficultyScale()
		{
			double difficulty = 0;
			double curWeight = 1;

			for(int i=strainPeaks.Count-1; i>=0; i--)
			{
				difficulty += strainPeaks[i] * curWeight;
				curWeight *= WeightDecay;
			}
			return difficulty;
		}

		/// <summary>
		/// Calculates the actual strain value of specified object.
		/// </summary>
		protected abstract double CalculateStrain(DifficultyHitObject obj);

		/// <summary>
		/// Returns the amount of decay applied to the current strain value for specified time difference in milliseconds.
		/// </summary>
		private double GetStrainDecay(double time) { return Math.Pow(StrainDecay, time / 1000); }
	}
}