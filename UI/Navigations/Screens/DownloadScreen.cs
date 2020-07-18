using PBGame.UI.Models;
using PBGame.UI.Components.Download;
using PBGame.Graphics;
using PBGame.Networking.API.Requests;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Graphics.Effects.CoffeeUI;
using PBFramework.Dependencies;
using Coffee.UIExtensions;
using UnityEngine;

namespace PBGame.UI.Navigations.Screens
{
    public class DownloadScreen : BaseScreen<DownloadModel>
    {
        private UguiSprite bgSprite;
        private SearchMenu searchMenu;
        private UguiObject resultArea;
        private ResultList resultList;
        private ResultLoader resultLoader;


        protected override int ViewDepth => ViewDepths.DownloadScreen;


        [InitWithDependency]
        private void Init(IColorPreset colorPreset)
        {
            bgSprite = CreateChild<UguiSprite>("bg", -1);
            {
                bgSprite.Anchor = AnchorType.Fill;
                bgSprite.Offset = Offset.Zero;

                var gradient = bgSprite.AddEffect(new GradientEffect());
                gradient.Component.direction = UIGradient.Direction.Vertical;
                gradient.Component.color1 = colorPreset.Passive;
                gradient.Component.color2 = colorPreset.DarkBackground;
            }
            searchMenu = CreateChild<SearchMenu>("search-menu", 1);
            {
                searchMenu.Anchor = AnchorType.Fill;
                searchMenu.Pivot = PivotType.Top;
                searchMenu.Offset = new Offset(0f, MenuBarHeight, 0f, 0f);
            }
            resultArea = CreateChild<UguiObject>("result-area", 1);
            {
                resultArea.Anchor = AnchorType.Fill;
                resultArea.Offset = new Offset(0f, searchMenu.FoldedHeight + MenuBarHeight, 0f, 0f);

                resultList = resultArea.CreateChild<ResultList>("list", 0);
                {
                    resultList.Anchor = AnchorType.Fill;
                    resultList.Offset = new Offset(8f, 0f);
                }
                resultLoader = resultArea.CreateChild<ResultLoader>("loader", 1);
                {
                    resultLoader.Anchor = AnchorType.Fill;
                    resultLoader.Offset = Offset.Zero;
                }
            }

            model.MapsetsRequest.OnNewValue += OnRequestChange;
        }

        /// <summary>
        /// Event called on mapset list request object change.
        /// </summary>
        private void OnRequestChange(MapsetsRequest request)
        {
            if(request == null)
                resultLoader.Hide();
            else
                resultLoader.Show();
        }
    }
}