using PBGame.Rulesets.UI.Components;
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
            return primaryContainer.CreateChild<PrimaryTouchEffects>();
        }

        /// <summary>
        /// Creates a new secondary touch effect.
        /// </summary>
        private SecondaryTouchEffects CreateSecondaryEffect()
        {
            return secondaryContainer.CreateChild<SecondaryTouchEffects>();
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