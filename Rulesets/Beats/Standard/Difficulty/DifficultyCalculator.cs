using System.Linq;
using System.Collections.Generic;
using PBGame.Rulesets.Maps;
using PBGame.Rulesets.Beats.Standard.Objects;
using PBGame.Rulesets.Beats.Standard.Difficulty.Skills;
using PBGame.Rulesets.Beats.Standard.Difficulty.Objects;
using PBGame.Rulesets.Difficulty.Skills;

namespace PBGame.Rulesets.Beats.Standard.Difficulty
{
    public class DifficultyCalculator : Rulesets.Difficulty.DifficultyCalculator {

		private const float StrainAdjustment = 0.075f;

		/// <summary>
		/// Amount of time to additionally wait before dragger is no longer considered being held.
		/// </summary>
		private const float DraggerRemovalDelay = 100; // 150 BPM


		public DifficultyCalculator(IMap map) : base(map) {}

        protected override Rulesets.Difficulty.DifficultyInfo CreateDifficultyInfo(IMap map, Skill[] skills, float clockRate)
        {
            if(!map.HitObjects.Any())
                return new DifficultyInfo();

			float speedStrain = skills[0].GetDifficultyScale() * StrainAdjustment;

            return new DifficultyInfo() {
                Scale = speedStrain,
                SpeedStrain = speedStrain
            };
        }

		protected override IEnumerable<Rulesets.Difficulty.Objects.DifficultyHitObject> CreateHitObjects(IMap beatmap, float clockRate)
        {
			var map = beatmap as Maps.Map;
			if(map != null)
			{
	            bool isFirst = true;
				HitObject prevObject = null;
				List<Dragger> draggers = new List<Dragger>();
				foreach(var obj in map.HitObjects)
	            {
					// Remove draggers that would have ended at current object's start time.
					for(int i=draggers.Count-1; i>=0; i--)
					{
						if(draggers[i].EndTime + DraggerRemovalDelay < obj.StartTime)
							draggers.RemoveAt(i);
					}

					// Create hit object.
	                if(!isFirst)
						yield return new DifficultyHitObject(obj, prevObject, draggers.Count, clockRate);
	                isFirst = false;

					// Store previous object.
	                prevObject = obj;

					// Add dragger.
					Dragger dragger = obj as Dragger;
					if(dragger != null)
						draggers.Add(dragger);
	            }
			}
        }

        protected override Skill[] CreateSkills()
        {
            return new Skill[] {
                new Speed()
            };
        }
    }
}