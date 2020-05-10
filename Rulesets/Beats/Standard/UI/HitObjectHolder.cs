using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.Game;
using PBGame.Data;
using PBGame.Rulesets.Beats.Standard.UI.Components;
using PBGame.Rulesets.Beats.Standard.Objects;
using PBGame.Rulesets.Judgements;
using PBFramework.UI;
using PBFramework.Audio;
using PBFramework.Graphics;
using PBFramework.Allocation.Recyclers;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.Rulesets.Beats.Standard.UI
{
    public class HitObjectHolder : UguiObject {

        private ManagedRecycler<HitCircleView> hitCircleRecycler;
        private ManagedRecycler<DraggerCircleView> draggerCircleRecycler;
        private ManagedRecycler<DraggerTickView> tickRecycler;
        private ManagedRecycler<DraggerView> draggerRecycler;

        private RangedList<HitObjectView> hitObjectViews;


        [ReceivesDependency]
        private IGameSession GameSession { get; set; }

        [ReceivesDependency]
        private GameState State { get; set; }

        [ReceivesDependency]
        private IMusicController MusicController { get; set; }


        [InitWithDependency]
        private void Init()
        {
            Dependencies = Dependencies.Clone();
            Dependencies.Cache(this);

            hitCircleRecycler = new ManagedRecycler<HitCircleView>(CreateHitCircle);
            draggerCircleRecycler = new ManagedRecycler<DraggerCircleView>(CreateDraggerCircle);
            tickRecycler = new ManagedRecycler<DraggerTickView>(CreateTick);
            draggerRecycler = new ManagedRecycler<DraggerView>(CreateDragger);

            hitObjectViews = new RangedList<HitObjectView>(400);

            // Add tick and drag circle as dependencies for dragger view to draw its nested objects.
            Dependencies.CacheAs<IRecycler<DraggerCircleView>>(draggerCircleRecycler);
            Dependencies.CacheAs<IRecycler<DraggerTickView>>(tickRecycler);

            if (GameSession != null)
            {
                GameSession.OnHardInit += OnHardInit;
                GameSession.OnSoftDispose += OnSoftDispose;
                GameSession.OnHardDispose += OnHardDispose;
            }
        }

        protected void Update()
        {
            float curTime = MusicController.CurrentTime;
            bool advanceLowIndex = true;
            for (int i = hitObjectViews.LowIndex; i < hitObjectViews.Count; i++)
            {
                var view = hitObjectViews[i];

                // Process any passive judgements to be made.
                foreach(var judgement in view.JudgePassive(curTime))
                    AddJudgement(judgement);

                if (view.IsFullyJudged)
                {
                    // Advance low index?
                    if (advanceLowIndex)
                        hitObjectViews.AdvanceLowIndex(true);
                }
                else
                {
                    advanceLowIndex = false;

                    // If not fully judged and disabled, this should be an upcoming hit object.
                    if (!view.Active)
                    {
                        // If should show, make it active and increase high index.
                        if (view.GetApproachProgress(curTime) >= 0f)
                        {
                            view.Active = true;
                            hitObjectViews.AdvanceHighIndex(true);
                        }
                        // If not, all other objects wouldn't be shown since the objects are sorted by start time.
                        else
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Adds the specified judgement info to score processor.
        /// </summary>
        private void AddJudgement(JudgementResult result)
        {
            GameSession?.ScoreProcessor.ProcessJudgement(result);
        }

        /// <summary>
        /// Creates a new hit circle view.
        /// </summary>
        private HitCircleView CreateHitCircle() => CreateChild<HitCircleView>();

        /// <summary>
        /// Creates a new dragger circle view.
        /// </summary>
        private DraggerCircleView CreateDraggerCircle() => CreateChild<DraggerCircleView>();

        /// <summary>
        /// Creates a new tick view.
        /// </summary>
        private DraggerTickView CreateTick() => CreateChild<DraggerTickView>();

        /// <summary>
        /// Creates a new dragger view.
        /// </summary>
        private DraggerView CreateDragger() => CreateChild<DraggerView>();

        /// <summary>
        /// Event called on game session hard initialization.
        /// </summary>
        private void OnHardInit()
        {
            foreach (var obj in GameSession.CurrentMap.HitObjects)
            {
                if (obj is HitCircle hitCircle)
                {
                    var hitCircleView = hitCircleRecycler.GetNext();
                    hitCircleView.Depth = hitObjectViews.Count;
                    hitCircleView.SetHitObject(hitCircle);

                    hitObjectViews.Add(hitCircleView);
                }
                else if (obj is Dragger dragger)
                {
                    var draggerView = draggerRecycler.GetNext();
                    draggerView.Depth = hitObjectViews.Count;
                    draggerView.SetHitObject(dragger);

                    hitObjectViews.Add(draggerView);
                }
            }
        }

        /// <summary>
        /// Event called on game session soft disposal.
        /// </summary>
        private void OnSoftDispose()
        {
            hitCircleRecycler.ActiveObjects.ForEach(o => o.SoftDispose());
            draggerCircleRecycler.ActiveObjects.ForEach(o => o.SoftDispose());
            tickRecycler.ActiveObjects.ForEach(o => o.SoftDispose());
            draggerRecycler.ActiveObjects.ForEach(o => o.SoftDispose());

            hitObjectViews.ResetIndex();
        }

        /// <summary>
        /// Event called on game session hard disposal.
        /// </summary>
        private void OnHardDispose()
        {
            // Return all hit objects to recycler.
            hitCircleRecycler.ReturnAll();
            draggerCircleRecycler.ReturnAll();
            tickRecycler.ReturnAll();
            draggerRecycler.ReturnAll();
            
            hitObjectViews.Clear();
        }
    }
}