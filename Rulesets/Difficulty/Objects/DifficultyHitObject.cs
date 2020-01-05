using PBGame.Rulesets.Objects;

namespace PBGame.Rulesets.Difficulty.Objects
{
	/// <summary>
	/// Representation of hit object for difficulty calculation.
	/// </summary>
	public class DifficultyHitObject {

        /// <summary>
        /// The HitObject this refers to.
        /// </summary>
        public HitObject BaseObject { get; private set; }

        /// <summary>
        /// Previous hit object that appears before the BaseObject.
        /// </summary>
        public HitObject PrevObject { get; private set; }

		/// <summary>
		/// The actual time between current object and previous object after applying clock rate.
		/// </summary>
		public double DeltaTime { get; protected set; }


        public DifficultyHitObject(HitObject hitObject, HitObject prevObject, double clockRate)
        {
            BaseObject = hitObject;
            PrevObject = prevObject;
            DeltaTime = (hitObject.StartTime - prevObject.StartTime) / clockRate;
        }
	}
}