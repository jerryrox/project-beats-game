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
		private SortedList<float> strainPeaks = new SortedList<float>(2);

		/// <summary>
		/// Current amount of strain on hold.
		/// </summary>
		private float currentStrain;

		/// <summary>
		/// Current highest strain peak during this section.
		/// </summary>
		private float currentStrainPeak;


		/// <summary>
		/// The amount of scoring this skill holds in comparison to other skills within the same game mode.
		/// </summary>
		protected abstract float StrainScore { get; }

		/// <summary>
		/// The amount of decay applied on current strain every strain section, per second.
		/// </summary>
		protected abstract float StrainDecay { get; }

		/// <summary>
		/// The amount of decay applied on weight factor for every next highest strain.
		/// </summary>
		protected virtual float WeightDecay { get { return 0.9f; } }


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
		public void StartNewSection(float time)
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
		public float GetDifficultyScale()
		{
			float difficulty = 0;
			float curWeight = 1;

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
		protected abstract float CalculateStrain(DifficultyHitObject obj);

		/// <summary>
		/// Returns the amount of decay applied to the current strain value for specified time difference in milliseconds.
		/// </summary>
		private float GetStrainDecay(float time) { return Mathf.Pow(StrainDecay, time / 1000); }
	}
}