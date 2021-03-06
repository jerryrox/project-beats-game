using PBGame.UI.Models;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Navigations.Overlays
{
    public class QuitOverlay : BaseOverlay<QuitModel> {

        private ISprite darkSprite;


        protected override int ViewDepth => ViewDepths.QuitOverlay;


        [InitWithDependency]
        private void Init()
        {
            darkSprite = CreateChild<UguiSprite>("dark", 0);
            {
                darkSprite.Anchor = AnchorType.Fill;
                darkSprite.Offset = Offset.Zero;
                darkSprite.Color = Color.black;
                darkSprite.Alpha = 0f;
            }
        }

        protected override IAnime CreateShowAnime(IDependencyContainer dependencies)
        {
            var anime = new Anime();
            anime.AnimateFloat(a => darkSprite.Alpha = a)
                .AddTime(0f, 0f)
                .AddTime(2f, 1f)
                .AddTime(2.5f, 1f)
                .Build();
            anime.AddEvent(anime.Duration, () => model.Quit());
            return anime;
        }
    }
}