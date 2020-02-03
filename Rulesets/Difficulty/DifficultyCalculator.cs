using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using PBGame.Rulesets.Maps;
using PBGame.Rulesets.Objects;
using PBGame.Rulesets.Difficulty.Objects;
using PBGame.Rulesets.Difficulty.Skills;
using UnityEngine;

namespace PBGame.Rulesets.Difficulty
{
	public abstract class DifficultyCalculator : IDifficultyCalculator {

		/// <summary>
		/// Map converted for the mode which difficulty is calculated for.
		/// </summary>
		private IMap map;


        /// <summary>
        /// The length of a single strain section.
        /// </summary>
        protected virtual double SectionLength => 400;


        protected DifficultyCalculator(IMap map)
		{
            this.map = map;
		}

		public DifficultyInfo Calculate() => CalculateInternal(1);

		/// <summary>
		/// Creates difficulty info object.
		/// </summary>
		protected abstract DifficultyInfo CreateDifficultyInfo(IMap beatmap, Skill[] skills, double clockRate);

        /// <summary>
        /// Creates hit objects to be used for difficulty calculation.
        /// </summary>
		protected abstract IEnumerable<DifficultyHitObject> CreateHitObjects(IMap map, double clockRate);

        /// <summary>
        /// Creates the skills required for current game mode.
        /// </summary>
        protected abstract Skill[] CreateSkills();

		/// <summary>
		/// Calculates difficulty of the map and outputs difficulty information.
		/// </summary>
		private DifficultyInfo CalculateInternal(double clockRate)
		{
			var skills = CreateSkills();
			if(map.HitObjects.Any())
			{
				var convertedObjects = CreateHitObjects(map, clockRate);
				// The actual amount of time that has been past when playing on given clockrate.
				var realSectionLength = SectionLength * clockRate;
				var nextSectionTime = Math.Ceiling(convertedObjects.First().BaseObject.StartTime / realSectionLength) * realSectionLength;

				foreach(var obj in convertedObjects)
				{
					while(obj.BaseObject.StartTime > nextSectionTime)
					{
						foreach(var skill in skills)
						{
							skill.StoreStrainPeak();
							skill.StartNewSection(nextSectionTime);
						}
						nextSectionTime += realSectionLength;
					}

					foreach(var skill in skills)
						skill.Process(obj);
				}

				// Store strain peak for last section.
				foreach(var skill in skills)
					skill.StoreStrainPeak();
			}

			return CreateDifficultyInfo(map, skills, clockRate);
		}
	}
}