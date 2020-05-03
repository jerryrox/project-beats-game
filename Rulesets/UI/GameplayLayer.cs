using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.Rulesets.UI
{
    public abstract class GameplayLayer : UguiObject, IGameplayLayer {

        public IPlayAreaContainer PlayArea { get; private set; }

        public IHudContainer Hud { get; private set; }


        [InitWithDependency]
        private void Init()
        {
            PlayArea = CreatePlayArea();
            {
                PlayArea.Depth = 0;
                PlayArea.Anchor = AnchorType.Fill;
                PlayArea.RawSize = Vector2.zero;
            }
            Hud = CreateHud();
            {
                Hud.Depth = 1;
                Hud.Anchor = AnchorType.Fill;
                Hud.RawSize = Vector2.zero;
            }
        }

        /// <summary>
        /// Creates a new play area container.
        /// </summary>
        protected abstract IPlayAreaContainer CreatePlayArea();

        /// <summary>
        /// Creates a new hud container.
        /// </summary>
        protected abstract IHudContainer CreateHud();
    }
}