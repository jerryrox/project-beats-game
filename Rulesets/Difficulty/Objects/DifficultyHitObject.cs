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
        public BaseHitObject BaseObject { get; private set; }

        /// <summary>
        /// Previous hit object that appears before the BaseObject.
        /// </summary>
        public BaseHitObject PrevObject { get; private set; }

		/// <summary>
		/// The actual time between current object and previous object after applying clock rate.
		/// </summary>
		public float DeltaTime { get; protected set; }


        public DifficultyHitObject(BaseHitObject hitObject, BaseHitObject prevObject, float clockRate)
        {
            BaseObject = hitObject;
            PrevObject = prevObject;
            DeltaTime = (hitObject.StartTime - prevObject.StartTime) / clockRate;
        }
	}
}