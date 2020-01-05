using System.Collections;
using System.Collections.Generic;
using PBGame.Rulesets.Beats.Standard.Objects;
using UnityEngine;

namespace PBGame.Rulesets.Beats.Standard.Difficulty.Objects
{
	public class DifficultyHitObject : Rulesets.Difficulty.Objects.DifficultyHitObject {

		public new HitObject BaseObject { get; private set; }

		public new HitObject PrevObject { get; private set; }

		/// <summary>
		/// Number of draggers that should be held by the play at the start time of BaseObject.
		/// </summary>
		public int DraggingCount { get; private set; }


		public DifficultyHitObject(HitObject obj, HitObject prevObject, int draggingCount, double clockRate) :
			base(obj, prevObject, clockRate)
		{
			BaseObject = obj;
			PrevObject = prevObject;
			DraggingCount = draggingCount;
		}
	}
}