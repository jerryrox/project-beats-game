using PBGame.UI.Models;
using UnityEngine;

namespace PBGame.UI.Navigations.Screens
{
    public class GameScreen : BaseScreen<GameModel>, IGameScreen {

        public override int InputLayer => InputLayers.GameScreen;

        protected override int ViewDepth => ViewDepths.GameScreen;

        protected override bool IsRoot3D => true;
    }
}