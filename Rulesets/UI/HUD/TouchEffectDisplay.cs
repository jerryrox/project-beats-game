using PBGame.Rulesets.UI.Components;
using PBGame.Rulesets.Inputs;
using PBFramework.Inputs;
using PBFramework.Graphics;
using PBFramework.Allocation.Recyclers;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.Rulesets.UI.HUD
{
    public class TouchEffectDisplay : UguiObject
    {
        private IGraphicObject primaryContainer;
        private IGraphicObject secondaryContainer;
        private IGraphicObject pulseContainer;

        private ManagedRecycler<PrimaryTouchEffects> primaryRecycler;
        private ManagedRecycler<SecondaryTouchEffects> secondaryRecycler;
        private ManagedRecycler<TouchPulseEffect> pulseRecycler;


        [InitWithDependency]
        private void Init(IGameSession gameSession)
        {
            secondaryContainer = CreateChild("secondary");
            {
                secondaryContainer.Size = Vector2.zero;
            }
            primaryContainer = CreateChild("primary");
            {
                primaryContainer.Size = Vector2.zero;
            }
            pulseContainer = CreateChild("pulse");
            {
                pulseContainer.Size = Vector2.zero;
            }

            gameSession.OnSoftDispose += () =>
            {
                DestroyAllEffects();
            };

            primaryRecycler = new ManagedRecycler<PrimaryTouchEffects>(CreatePrimaryEffect);
            primaryRecycler.Precook(4);

            secondaryRecycler = new ManagedRecycler<SecondaryTouchEffects>(CreateSecondaryEffect);
            secondaryRecycler.Precook(20);

            pulseRecycler = new ManagedRecycler<TouchPulseEffect>(CreatePulseEffect);
            pulseRecycler.Precook(5);
        }

        /// <summary>
        /// Shows the primary effect for specified cursor.
        /// </summary>
        public void ShowPrimary(ICursor cursor, IInputResultReporter resultReporter)
        {
            var effect = primaryRecycler.GetNext();
            effect.Show(cursor, resultReporter);
        }

        /// <summary>
        /// Shows the secondary effect for specified cursor.
        /// </summary>
        public void ShowSecondary(ICursor cursor, IInputResultReporter resultReporter)
        {
            var effect = secondaryRecycler.GetNext();
            effect.Show(cursor, resultReporter);
        }

        /// <summary>
        /// Immediately returns all effects to recyclers.
        /// </summary>
        private void DestroyAllEffects()
        {
            primaryRecycler.ReturnAll();
            secondaryRecycler.ReturnAll();
            pulseRecycler.ReturnAll();
        }

        /// <summary>
        /// Creates a new primary touch effect.
        /// </summary>
        private PrimaryTouchEffects CreatePrimaryEffect()
        {
            var effect = primaryContainer.CreateChild<PrimaryTouchEffects>();
            effect.PulseRecycler = pulseRecycler;
            return effect;
        }

        /// <summary>
        /// Creates a new secondary touch effect.
        /// </summary>
        private SecondaryTouchEffects CreateSecondaryEffect()
        {
            var effect = secondaryContainer.CreateChild<SecondaryTouchEffects>();
            effect.PulseRecycler = pulseRecycler;
            return effect;
        }

        /// <summary>
        /// Creates a new touch pulse effect.
        /// </summary>
        private TouchPulseEffect CreatePulseEffect()
        {
            return pulseContainer.CreateChild<TouchPulseEffect>();
        }
    }
}