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
    public abstract class BaseHitObjectView : UguiObject, IRecyclable<BaseHitObjectView>, IHasAlpha {

        protected CanvasGroup canvasGroup;

        protected float startTime;
        protected float endTime;
        protected float duration;
        protected float approachDuration;
        protected float approachTime;
        protected float judgeEndTime;

        private BaseHitObject hitObject;
        private IHasEndTime hasEndTime;

        /// <summary>
        /// List of nested objects under this object.
        /// </summary>
        private List<BaseHitObjectView> nestedObjects = new List<BaseHitObjectView>();


        /// <summary>
        /// Returns the base hit object info this view is representing.
        /// </summary>
        public BaseHitObject BaseHitObject => hitObject;

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
        /// Returns whether all nested objects have been judged.
        /// </summary>
        public virtual bool IsNestedJudged => nestedObjects.Count > 0 ? nestedObjects.TrueForAll(o => o.IsJudged) : true;

        /// <summary>
        /// Returns whether this object has been fully judged.
        /// </summary>
        public virtual bool IsFullyJudged => IsJudged && IsNestedJudged;

        public float Alpha
        {
            get => canvasGroup.alpha;
            set => canvasGroup.alpha = value;
        }

        /// <summary>
        /// List of nested objects returned as base hit object view type.
        /// If you'd like to retrieve a more specialized type of nested objects, they should be stored by the derived classes.
        /// </summary>
        protected List<BaseHitObjectView> BaseNestedObjects => nestedObjects;

        /// <summary>
        /// Just a dummy implementation of IRecyclable, only to define the other interface methods virtual.
        /// </summary>
        IRecycler<BaseHitObjectView> IRecyclable<BaseHitObjectView>.Recycler { get; set; }


        [InitWithDependency]
        private void Init()
        {
            canvasGroup = RawObject.AddComponent<CanvasGroup>();
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }

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

            // Judge self only after all nested objects are judged.
            if (!IsJudged && IsNestedJudged)
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
        /// Disposes object state in a way that can be reused immediately later.
        /// </summary>
        public virtual void SoftDispose()
        {
            Active = false;

            if (Result != null)
            {
                Result.Reset();
            }
        }

        /// <summary>
        /// Disposes object state completely for a new session at later time.
        /// </summary>
        public virtual void HardDispose()
        {
            SoftDispose();

            startTime = 0f;
            endTime = 0f;
            duration = 0f;
            approachDuration = 0f;
            approachTime = 0f;
            judgeEndTime = 0f;

            hitObject = null;
            hasEndTime = null;

            nestedObjects.Clear();
            Result = null;
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

        /// <summary>
        /// Returns the progress of the hit object since start time at specified time.
        /// </summary>
        public float GetHitProgress(float curTime)
        {
            if(hasEndTime == null)
                return 0f;
            return (curTime - startTime) / duration;
        }

        public virtual void OnRecycleNew() => HardDispose();

        public virtual void OnRecycleDestroy() => HardDispose();

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

            Result = new JudgementResult(hitObject.CreateJudgementInfo());
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
        protected abstract void EvalPassiveJudgement();
    }
}