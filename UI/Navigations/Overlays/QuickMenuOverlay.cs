using System.Collections.Generic;
using PBGame.UI.Models;
using PBGame.UI.Models.QuickMenu;
using PBGame.UI.Components.QuickMenu;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Navigations.Overlays
{
    public class QuickMenuOverlay : BaseSubMenuOverlay<QuickMenuModel> {

        public static readonly Vector2 ButtonSize = new Vector2(150f, 72f);

        private IScrollView scrollView;
        private IGrid grid;
        private List<MenuButton> menuButtons = new List<MenuButton>();


        protected override int ViewDepth => ViewDepths.QuickMenuOverlay;


        [InitWithDependency]
        private void Init()
        {
            container.Anchor = AnchorType.TopStretch;
            container.Pivot = PivotType.Top;
            container.SetOffsetHorizontal(16f);
            container.Y = -16f;
            container.Height = ButtonSize.y;

            scrollView = container.CreateChild<UguiScrollView>("scrollview", 0);
            {
                scrollView.Anchor = AnchorType.Fill;
                scrollView.Offset = Offset.Zero;
                scrollView.Background.Color = new Color(0f, 0f, 0f, 0.5f);

                grid = scrollView.Container.AddComponentInject<UguiGrid>();
                {
                    grid.Anchor = AnchorType.LeftStretch;
                    grid.SetOffsetVertical(0f);
                    grid.X = 0f;
                    grid.CellSize = ButtonSize;

                    foreach (var info in model.GetMenus())
                    {
                        CreateMenuButton(info);
                    }

                    // Resize scrollview container
                    InvokeAfterTransformed(1, () => scrollView.Container.Width = Mathf.Max(scrollView.Width, grid.ChildCount * ButtonSize.x));
                }
            }
        }
        
        /// <summary>
        /// Creates a new menu button for the specified info.
        /// </summary>
        private void CreateMenuButton(MenuInfo info)
        {
            var button = grid.CreateChild<MenuButton>(info.Label, grid.ChildCount);
            button.SetMenuInfo(info);
            menuButtons.Add(button);
        }
    }
}