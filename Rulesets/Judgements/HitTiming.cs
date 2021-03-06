using System;
using System.Linq;
using System.Collections.Generic;
using PBGame.Rulesets.Maps;
using PBFramework.Debugging;

namespace PBGame.Rulesets.Judgements
{
    using TimingTuple = Tuple<float, float, float>;

    /// <summary>
    /// Contains information about hit timing values.
    /// </summary>
    public class HitTiming {

        protected static IReadOnlyDictionary<HitResultType, TimingTuple> DefaultRanges = new Dictionary<HitResultType, TimingTuple>()
        {
            { HitResultType.Perfect, new TimingTuple(44.8f, 38.8f, 27.8f) },
			{ HitResultType.Great, new TimingTuple(128, 98, 68) },
			{ HitResultType.Good, new TimingTuple(194, 164, 134) },
			{ HitResultType.Ok, new TimingTuple(254, 224, 194) },
			{ HitResultType.Bad, new TimingTuple(302, 272, 242) },
			{ HitResultType.Miss, new TimingTuple(376, 346, 316) }
        };

		/// <summary>
		/// Time value for a perfect hit result.
		/// </summary>
		public float Perfect { get; protected set; }

		/// <summary>
		/// Time value for a great hit result.
		/// </summary>
		public float Great { get; protected set; }

		/// <summary>
		/// Time value for a good hit result.
		/// </summary>
		public float Good { get; protected set; }

		/// <summary>
		/// Time value for a ok hit result.
		/// </summary>
		public float Ok { get; protected set; }

		/// <summary>
		/// Time value for a bad hit result.
		/// </summary>
		public float Bad { get; protected set; }

		/// <summary>
		/// Time value for a miss hit result.
		/// </summary>
		public float Miss { get; protected set; }


        /// <summary>
        /// Sets the difficulty value to calculate the timing values.
        /// </summary>
        public virtual void SetDifficulty(float difficulty)
        {
			Perfect = MapDifficulty.GetDifficultyValue(difficulty, DefaultRanges[HitResultType.Perfect]);
			Great = MapDifficulty.GetDifficultyValue(difficulty, DefaultRanges[HitResultType.Great]);
			Good = MapDifficulty.GetDifficultyValue(difficulty, DefaultRanges[HitResultType.Good]);
			Ok = MapDifficulty.GetDifficultyValue(difficulty, DefaultRanges[HitResultType.Ok]);
			Bad = MapDifficulty.GetDifficultyValue(difficulty, DefaultRanges[HitResultType.Bad]);
			Miss = MapDifficulty.GetDifficultyValue(difficulty, DefaultRanges[HitResultType.Miss]);
        }

		/// <summary>
		/// Returns the timing value for the lowest possible successful hit.
		/// </summary>
		public float LowestSuccessTiming() => GetHalfTiming(LowestSuccessfulHitResult());

		/// <summary>
		/// Returns whether specified hit result is supported.
		/// </summary>
		public virtual bool IsHitResultSupported(HitResultType result)
		{
			return SupportedHitResults().Any(r => r == result);
		}

		/// <summary>
		/// Returns the hit result type for specified hit offset.
		/// </summary>
		public HitResultType GetHitResult(float offset)
		{
			if(offset < 0)
				offset = -offset;

			foreach(var result in SupportedHitResults())
			{
				if(offset <= GetHalfTiming(result))
					return result;
			}
			return HitResultType.None;
		}

		/// <summary>
		/// Returns the halved value of the timing value for specified result.
		/// </summary>
		public float GetHalfTiming(HitResultType type)
		{
			switch(type)
			{
			case HitResultType.Perfect: return Perfect / 2;
			case HitResultType.Great: return Great / 2;
			case HitResultType.Good: return Good / 2;
			case HitResultType.Ok: return Ok / 2;
			case HitResultType.Bad: return Bad / 2;
			case HitResultType.Miss: return Miss / 2;
			}
            Logger.LogWarning($"HitTiming.GetHalfTiming - Unsupported hit result type: {type}");
            return 0;
        }

		/// <summary>
		/// Returns whether a hit object can be hit after specified offset, resulting in a non-miss result.
		/// </summary>
		public bool CanBeHit(float offset) => offset <= GetHalfTiming(LowestSuccessfulHitResult());

		/// <summary>
		/// Returns the types of hit results which are supported by this hit timing in current game mode.
		/// Should be ordered from Perfect to Miss.
		/// </summary>
		public virtual IEnumerable<HitResultType> SupportedHitResults()
		{
			yield return HitResultType.Perfect;
			yield return HitResultType.Great;
			yield return HitResultType.Good;
			yield return HitResultType.Ok;
			yield return HitResultType.Bad;
			yield return HitResultType.Miss;
		}

		/// <summary>
		/// Returns the timing value for the lowest possible successful hit.
		/// </summary>
		protected HitResultType LowestSuccessfulHitResult()
		{
			HitResultType lastResult = HitResultType.None;
			foreach(var result in SupportedHitResults())
			{
				if(result != HitResultType.Miss)
					lastResult = result;
			}
			return lastResult;
		}
    }
}