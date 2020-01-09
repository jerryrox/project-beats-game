using PBFramework.Graphics;
using PBFramework.Dependencies;

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
            }
            Hud = CreateHud();
            {
                Hud.Depth = 1;
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