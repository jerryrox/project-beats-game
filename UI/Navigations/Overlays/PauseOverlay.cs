using PBGame.UI.Models;

namespace PBGame.UI.Navigations.Overlays
{
    public class PauseOverlay : BaseOverlay<PauseModel>, IPauseOverlay
    {
        protected override int ViewDepth => 0;
    }
}