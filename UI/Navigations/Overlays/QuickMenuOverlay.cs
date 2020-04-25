using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.QuickMenu;
using PBGame.UI.Navigations.Screens;
using PBFramework.UI;
using PBFramework.UI.Navigations;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Navigations.Overlays
{
    public class QuickMenuOverlay : BaseSubMenuOverlay, IQuickMenuOverlay {

        public static readonly Vector2 ButtonSize = new Vector2(150f, 72f);

        private IScrollView scrollView;
        private IGrid grid;
        private List<BaseMenuButton> menuButtons = new List<BaseMenuButton>();


        protected override int OverlayDepth => ViewDepths.QuickMenuOverlay;

        [ReceivesDependency]
        private IGame Game { get; set; }

        [ReceivesDependency]
        private IOverlayNavigator OverlayNavigator { get; set; }


        [InitWithDependency]
        private void Init()
        {
            container.Anchor = Anchors.TopStretch;
            container.Pivot = Pivots.Top;
            container.SetOffsetHorizontal(16f);
            container.Y = -16f;
            container.Height = ButtonSize.y;

            scrollView = container.CreateChild<UguiScrollView>("scrollview", 0);
            {
                scrollView.Anchor = Anchors.Fill;
                scrollView.Offset = Offset.Zero;
                scrollView.Background.Color = new Color(0f, 0f, 0f, 0.5f);

                grid = scrollView.Container.AddComponentInject<UguiGrid>();
                {
                    grid.Anchor = Anchors.LeftStretch;
                    grid.SetOffsetVertical(0f);
                    grid.X = 0f;
                    grid.CellSize = ButtonSize;

                    // Create buttons.
                    CreateScreenButton<HomeScreen>("Home", "icon-home");
                    CreateScreenButton<SongsScreen>("Play", "icon-play");
                    CreateScreenButton<PrepareScreen>("Prepare", "icon-game");
                    CreateScreenButton<DownloadScreen>("Download", "icon-download");
                    CreateBasicButton("Quit", "icon-power", () => Game.GracefulQuit());

                    // Resize scrollview container
                    InvokeAfterTransformed(1, () => scrollView.Container.Width = Mathf.Max(scrollView.Width, grid.ChildCount * ButtonSize.x));
                }
            }

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            menuButtons.ForEach(b => b.OnShowQuickMenu());
        }

        /// <summary>
        /// Creates a new screen menu button.
        /// </summary>
        private void CreateScreenButton<T>(string label, string iconName)
            where T : BaseScreen
        {
            CreateButton<ScreenMenuButton>(label, iconName).SetScreen<T>();
        }

        /// <summary>
        /// Creates a new overlay menu button.
        /// </summary>
        private void CreateOverlayButton<T>(string label, string iconName)
            where T : BaseOverlay
        {
            CreateButton<OverlayMenuButton>(label, iconName).SetOverlay<T>();
        }

        /// <summary>
        /// Creates a new basic menu button.
        /// </summary>
        private void CreateBasicButton(string label, string iconName, Action action)
        {
            CreateButton<BasicMenuButton>(label, iconName).SetAction(action);
        }

        /// <summary>
        /// Creates a new button T with label and icon name set up.
        /// </summary>
        private T CreateButton<T>(string label, string iconName)
            where T : BaseMenuButton
        {
            var button = grid.CreateChild<T>(label, grid.ChildCount);
            button.LabelText = label;
            button.IconName = iconName;
            button.OnTriggered += () => OverlayNavigator.Hide(this);
            menuButtons.Add(button);
            return button;
        }
    }
}