using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Models;
using PBGame.Data;
using PBGame.Graphics;
using PBGame.Rulesets.Objects;
using PBGame.Rulesets.Beats.Standard.UI.Components;
using PBGame.Rulesets.Beats.Standard.Objects;
using PBFramework.Audio;
using PBFramework.Graphics;
using PBFramework.Threading;
using PBFramework.Allocation.Recyclers;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.Rulesets.Beats.Standard.UI
{
    public class HitObjectHolder : UguiObject
    {
        private ManagedRecycler<HitCircleView> hitCircleRecycler;
        private ManagedRecycler<DraggerCircleView> draggerCircleRecycler;
        private ManagedRecycler<DraggerTickView> tickRecycler;
        private ManagedRecycler<DraggerView> draggerRecycler;

        private RangedList<HitObjectView> hitObjectViews;

        private int curComboOffset;
        private List<Color> comboColors;

        private BeatsStandardProcessor gameProcessor;


        /// <summary>
        /// Returns the list of views currently being managed.
        /// </summary>
        public RangedList<HitObjectView> HitObjectViews => hitObjectViews;

        [ReceivesDependency]
        private IGameSession GameSession { get; set; }

        [ReceivesDependency]
        private GameModel Model { get; set; }

        [ReceivesDependency]
        private IMusicController MusicController { get; set; }

        [ReceivesDependency]
        private PlayAreaContainer PlayArea { get; set; }

        [ReceivesDependency]
        private IColorPreset ColorPreset { get; set; }


        [InitWithDependency]
        private void Init()
        {
            Dependencies.Cache(this);

            if (GameSession != null)
            {
                GameSession.OnHardInit += OnHardInit;
                GameSession.OnSoftInit += OnSoftInit;
                GameSession.OnSoftDispose += OnSoftDispose;
                GameSession.OnHardDispose += OnHardDispose;
            }

            hitCircleRecycler = new ManagedRecycler<HitCircleView>(CreateHitCircle);
            draggerCircleRecycler = new ManagedRecycler<DraggerCircleView>(CreateDraggerCircle);
            tickRecycler = new ManagedRecycler<DraggerTickView>(CreateTick);
            draggerRecycler = new ManagedRecycler<DraggerView>(CreateDragger);

            hitObjectViews = new RangedList<HitObjectView>(400);

            // Add tick and drag circle as dependencies for dragger view to draw its nested objects.
            Dependencies.CacheAs<IRecycler<DraggerCircleView>>(draggerCircleRecycler);
            Dependencies.CacheAs<IRecycler<DraggerTickView>>(tickRecycler);
        }

        /// <summary>
        /// Sets the current game processor instance.
        /// </summary>
        public void SetGameProcessor(BeatsStandardProcessor gameProcessor)
        {
            this.gameProcessor = gameProcessor;
        }

        /// <summary>
        /// Returns all active hit object views in the holder.
        /// </summary>
        public IEnumerable<HitObjectView> GetActiveObjects()
        {
            for (int i = hitObjectViews.LowIndex; i < hitObjectViews.HighIndex; i++)
                yield return hitObjectViews[i];
        }

        /// <summary>
        /// Updates the contained hit objects based on the specified time.
        /// </summary>
        public void UpdateObjects(float curTime)
        {
            bool advanceLowIndex = true;
            for (int i = hitObjectViews.LowIndex; i < hitObjectViews.Count; i++)
            {
                var view = hitObjectViews[i];

                // Record dragging flag.
                // Ensure we don't apply the generous release handicap here as it'll screw up the replay score sync.
                if(view.IsHoldable)
                    gameProcessor.RecordDraggerHoldFlag(view.ObjectIndex, view.IsHolding(null));

                // Process any passive judgements to be made.
                gameProcessor.JudgePassive(curTime, view);

                if (view.IsFullyJudged)
                {
                    // Advance low index?
                    if (advanceLowIndex)
                        hitObjectViews.AdvanceLowIndex(true);
                }
                else
                {
                    advanceLowIndex = false;

                    float approachProgress = view.GetApproachProgress(curTime);

                    // If not fully judged and disabled, this should be an upcoming hit object.
                    if (!view.Active)
                    {
                        // If should show, make it active and increase high index.
                        if (approachProgress >= 0f)
                        {
                            view.Active = true;
                            hitObjectViews.AdvanceHighIndex(true);
                        }
                        // If not, all other objects wouldn't be shown since the objects are sorted by start time.
                        else
                            break;
                    }
                    // Move object.
                    view.Y = Mathf.LerpUnclamped(PlayArea.FallStartPos, PlayArea.HitPosition, approachProgress);
                }
            }
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
        /// Returns the color for specified combo information.
        /// </summary>
        private Color GetComboColor(IHasCombo combo)
        {
            if(combo.IsNewCombo)
                curComboOffset += combo.ComboOffset + 1;
            return comboColors[curComboOffset % comboColors.Count];
        }

        /// <summary>
        /// Starts loading hit object to resolve for specified future.
        /// </summary>
        private IEnumerator LoadHitObjects(ManualTask task)
        {
            int createCount = 0;
            int lastLoads = 0;

            foreach (var obj in GameSession.CurrentMap.HitObjects)
            {
                // If create count reached 0, determine new creation count.
                if (createCount <= 0)
                {
                    createCount = Mathf.Max((int)((1f / Time.deltaTime) + lastLoads) / 4, 1);
                    lastLoads = createCount;
                    yield return null;
                }
                createCount--;

                HitObjectView hitObjView = null;
                if (obj is HitCircle hitCircle)
                {
                    var hitCircleView = hitCircleRecycler.GetNext();
                    hitCircleView.Depth = hitObjectViews.Count;
                    hitCircleView.ObjectIndex = hitObjectViews.Count;
                    hitCircleView.SetHitObject(hitCircle);

                    hitObjectViews.Add(hitCircleView);
                    hitObjView = hitCircleView;
                }
                else if (obj is Dragger dragger)
                {
                    var draggerView = draggerRecycler.GetNext();
                    draggerView.Depth = hitObjectViews.Count;
                    draggerView.ObjectIndex = hitObjectViews.Count;
                    draggerView.SetHitObject(dragger);

                    hitObjectViews.Add(draggerView);
                    hitObjView = draggerView;
                }

                if (hitObjView != null)
                {
                    // Apply combo color
                    var combo = obj as IHasCombo;
                    if (combo != null)
                        hitObjView.Tint = GetComboColor(combo);
                }
            }
            task.SetFinished();
        }

        /// <summary>
        /// Event called on game session hard initialization.
        /// </summary>
        private void OnHardInit()
        {
            curComboOffset = 0;
            comboColors = GameSession.CurrentMap.ComboColors;
            if(comboColors == null || comboColors.Count == 0)
                comboColors = ColorPreset.DefaultComboColors;

            Coroutine loadRoutine = null;
            ManualTask task = new ManualTask((t) => loadRoutine = UnityThread.StartCoroutine(LoadHitObjects(t)));
            task.IsRevoked.OnNewValue += (revoked) =>
            {
                if (revoked && loadRoutine != null)
                    UnityThread.StopCoroutine(loadRoutine);
            };
            Model.AddAsLoader(task);
        }

        /// <summary>
        /// Event called on game session soft initialization.
        /// </summary>
        private void OnSoftInit()
        {
            hitCircleRecycler.ActiveObjects.ForEach(o => o.SoftInit());
            draggerCircleRecycler.ActiveObjects.ForEach(o => o.SoftInit());
            tickRecycler.ActiveObjects.ForEach(o => o.SoftInit());
            draggerRecycler.ActiveObjects.ForEach(o => o.SoftInit());
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

            comboColors = null;
        }
    }
}