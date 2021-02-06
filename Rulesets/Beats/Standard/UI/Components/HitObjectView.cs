using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Rulesets.UI.Components;
using PBGame.Rulesets.Beats.Standard.Objects;
using PBGame.Rulesets.Judgements;
using PBFramework.UI;
using PBFramework.Inputs;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.Rulesets.Beats.Standard.UI.Components
{
    public abstract class HitObjectView : BaseHitObjectView
    {
        protected float xPos;
        protected float radius;

        [ReceivesDependency]
        private PlayAreaContainer PlayArea { get; set; }


        /// <summary>
        /// Sets the specified hit object to be represented by this view.
        /// </summary>
        public void SetHitObject(HitObject hitObject)
        {
            base.SetBaseHitObject(hitObject);

            this.xPos = hitObject.X;
            this.radius = hitObject.Radius;

            this.X = xPos;
            this.Size = new Vector2(radius * 2f, radius * 2f);
        }

        public override JudgementResult SetResult(HitResultType hitResult, float offset)
        {
            base.SetResult(hitResult, offset);
            PlayArea.HitBar.ShowJudgementEffect(GetPosOnJudgement(), this);
            return Result;
        }

        /// <summary>
        /// Returns whether the specified cursor position X is within the hit object range.
        /// </summary>
        public virtual bool IsCursorInRange(float x)
        {
            return x > xPos - radius && x < xPos + radius;
        }

        /// <summary>
        /// Feeds the specified input to the object to try evaluating a judgement result under given time.
        /// May return a judgement result if it has been made.
        /// </summary>
        public abstract JudgementResult JudgeInput(float curTime, IInput input);

        public override void HardDispose()
        {
            base.HardDispose();
            xPos = 0f;
            radius = 0f;
        }

        /// <summary>
        /// Returns the desired X position of the object upon judgement.
        /// </summary>
        protected virtual float GetPosOnJudgement() => BaseParentView == null ? X : BaseParentView.X + X;
    }

    public abstract class HitObjectView<T> : HitObjectView
        where T : HitObject
    {
        protected T hitObject;


        /// <summary>
        /// Returns the hit object info this view is representing.
        /// </summary>
        public T HitObject => hitObject;


        /// <summary>
        /// Sets the specific type of hit object to be represented by this view.
        /// </summary>
        public virtual void SetHitObject(T hitObject)
        {
            base.SetHitObject(hitObject);

            this.hitObject = hitObject;
        }

        /// <summary>
        /// Adds the specified object view as nested object under this object.
        /// </summary>
        public virtual void AddNestedObject(HitObjectView hitObject)
        {
            hitObject.ObjectIndex = BaseNestedObjects.Count;
            base.AddBaseNestedObject(hitObject);
        }

        public override void HardDispose()
        {
            base.HardDispose();

            hitObject = null;
        }
    }
}