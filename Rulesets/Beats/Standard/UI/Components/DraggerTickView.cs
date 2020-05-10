using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Rulesets.Beats.Standard.Objects;
using PBGame.Rulesets.Judgements;
using PBFramework.UI;
using PBFramework.Inputs;
using PBFramework.Graphics;
using PBFramework.Allocation.Recyclers;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.Rulesets.Beats.Standard.UI.Components
{
    public class DraggerTickView : HitObjectView<DraggerTick>, IRecyclable<DraggerTickView>, IDraggerComponent {

        private DraggerView dragger;


        public DraggerView Dragger => dragger;

        IRecycler<DraggerTickView> IRecyclable<DraggerTickView>.Recycler { get; set; }
        

        public override JudgementResult JudgeInput(float curTime, IInput input)
        {
            // Ticks shouldn't be judged by direct input.
            return null;
        }


        /// <summary>
        /// Sets the parent dragger view.
        /// Specifying a null dragger should be equivalent to calling RemoveDragger.
        /// </summary>
        public void SetDragger(DraggerView dragger)
        {
            RemoveDragger();
            if(dragger == null)
                return;

            this.dragger = dragger;
        }

        /// <summary>
        /// Disassociates this object from parent dragger.
        /// </summary>
        public void RemoveDragger()
        {
            if(dragger == null)
                return;

            dragger = null;
        }
    }
}