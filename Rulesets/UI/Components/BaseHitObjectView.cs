using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Rulesets.Objects;
using PBGame.Rulesets.Judgements;
using PBFramework.UI;
using PBFramework.Data;
using PBFramework.Graphics;
using PBFramework.Allocation.Recyclers;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.Rulesets.UI.Components
{
    public abstract class BaseHitObjectView : UguiObject, IRecyclable<BaseHitObjectView> {

        private BaseHitObject hitObject;
        private IHasEndTime hasEndTime;

        protected float startTime;
        protected float endTime;
        protected float duration;
        protected float approachDuration;
        protected float approachTime;
        protected float judgeEndTime;

        /// <summary>
        /// List of nested objects under this object.
        /// </summary>
        private List<BaseHitObjectView> nestedObjects = new List<BaseHitObjectView>();


        /// <summary>
        /// Returns the hit start time of the object.
        /// </summary>
        public float StartTime => startTime;

        /// <summary>
        /// Returns the hit end time of the object.
        /// </summary>
        public float EndTime => endTime;

        /// <summary>
        /// Returns the duratino of the object, if applicable.
        /// </summary>
        public float Duration => duration;

        /// <summary>
        /// Returns the time at which the object should start approaching.
        /// </summary>
        public float ApproachTime => approachTime;

        /// <summary>
        /// Returns the maximum time at which a "hit" judgement can be made.
        /// </summary>
        public float JudgeEndTime => judgeEndTime;

        /// <summary>
        /// The result of judgement for this object.
        /// </summary>
        public JudgementResult Result { get; private set; }

        /// <summary>
        /// Returns whether this object has been judged.
        /// </summary>
        public bool IsJudged => Result == null ? true : Result.HasResult;

        /// <summary>
        /// Returns whether this object has an ending time interface.
        /// </summary>
        public virtual bool HasEndTime => hasEndTime != null;

        /// <summary>
        /// Returns whether this object has been fully judged.
        /// </summary>
        public virtual bool IsFullyJudged => IsJudged && (nestedObjects.Count > 0 ? nestedObjects.TrueForAll(o => o.IsJudged) : true);

        /// <summary>
        /// Just a dummy implementation of IRecyclable, only to define the other interface methods virtual.
        /// </summary>
        IRecycler<BaseHitObjectView> IRecyclable<BaseHitObjectView>.Recycler { get; set; }


        /// <summary>
        /// Returns the judgement result instance after assigning the specified values.
        /// </summary>
        public virtual JudgementResult SetResult(HitResultType hitResult, float offset)
        {
            Result.HitResult = hitResult;
            Result.HitOffset = offset;
            return Result;
        }

        /// <summary>
        /// Evaluates this and all nested objects for judgements that occurred as a result of simply passing time.
        /// </summary>
        public virtual IEnumerable<JudgementResult> JudgePassive(float curTime)
        {
            // Judge all inner objects recursively.
            for (int i = 0; i < nestedObjects.Count; i++)
            {
                foreach (var result in nestedObjects[i].JudgePassive(curTime))
                {
                    if(result != null)
                        yield return result;
                }
            }

            // Judge self
            if (!IsJudged)
            {
                if (IsPastJudgeEnd(curTime))
                {
                    EvalPassiveJudgement();
                    if(!IsJudged)
                        throw new Exception("Evaluation of passive judgement must output a valid judgement result!");
                    yield return Result;
                }
            }
        }

        /// <summary>
        /// Returns the offset from perfect hit timing toward the specified time.
        /// </summary>
        public virtual float GetHitOffset(float curTime)
        {
            return curTime - endTime;
        }

        /// <summary>
        /// Returns whether the specified value is a time past the last judgeable time that returns a successful hit.
        /// </summary>
        public virtual bool IsPastJudgeEnd(float curTime)
        {
            return curTime > judgeEndTime;
        }

        /// <summary>
        /// Returns the progress at which the object is approaching its perfect hit timing at 1.0
        /// </summary>
        public float GetApproachProgress(float curTime)
        {
            return (curTime - approachTime) / approachDuration;
        }

        public virtual void OnRecycleNew()
        {
            ClearStates();
        }

        public virtual void OnRecycleDestroy()
        {
            ClearStates();
        }

        /// <summary>
        /// Sets the base hit object to be represented by this view.
        /// </summary>
        protected void SetBaseHitObject(BaseHitObject hitObject)
        {
            this.hitObject = hitObject;
            hasEndTime = hitObject as IHasEndTime;

            startTime = hitObject.StartTime;
            endTime = hasEndTime == null ? startTime : hasEndTime.EndTime;
            duration = hasEndTime == null ? 0f : hasEndTime.Duration;
            approachDuration = hitObject.ApproachDuration;
            approachTime = startTime - approachDuration;
            judgeEndTime = hasEndTime == null ? startTime + hitObject.Timing.LowestSuccessTiming() : endTime;
        }

        /// <summary>
        /// Adds the specified base object view as a nested object under this object.
        /// </summary>
        protected void AddBaseNestedObject(BaseHitObjectView objectView)
        {
            nestedObjects.Add(objectView);
        }

        /// <summary>
        /// Evaluates the judgement of this object in passive context.
        /// It is expected that this method MUST assign judgement result manualy or via SetResult method.
        /// </summary>
        protected virtual void EvalPassiveJudgement()
        {
            SetResult(HitResultType.Miss, judgeEndTime);
        }

        /// <summary>
        /// Clears all states so the object can be reused for another game.
        /// </summary>
        protected virtual void ClearStates()
        {
            if(!Active)
                return;

            Active = false;
            // TODO:
        }
    }
}