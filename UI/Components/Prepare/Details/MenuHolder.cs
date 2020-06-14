using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Navigations.Screens;
using PBGame.UI.Navigations.Overlays;
using PBGame.Graphics;
using PBFramework.UI;
using PBFramework.UI.Navigations;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Prepare.Details
{
    public class MenuHolder : UguiSprite {

        private IGrid grid;

        private MenuButton backButton;
        private MenuButton infoButton;
        private MenuButton playButton;


        [ReceivesDependency]
        private IScreenNavigator ScreenNavigator { get; set; }

        [ReceivesDependency]
        private IOverlayNavigator OverlayNavigator { get; set; }

        [ReceivesDependency]
        private IPrepareScreen PrepareScreen { get; set; }


        [InitWithDependency]
        private void Init(IRootMain rootMain)
        {
            Color = new Color(1f, 1f, 1f, 0.25f);

            grid = CreateChild<UguiGrid>("grid", 0);
            {
                grid.Anchor = AnchorType.Fill;
                grid.RawSize = Vector2.zero;
                InvokeAfterTransformed(1, () => grid.CellSize = new Vector2(Width / 3f, 56f));

                backButton = grid.CreateChild<MenuButton>("back", 0);
                {
                    backButton.IconName = "icon-arrow-left";
                    backButton.LabelText = "Back";

                    backButton.OnTriggered += () =>
                    {
                        ScreenNavigator.Show<SongsScreen>();
                    };
                }
                infoButton = grid.CreateChild<MenuButton>("info", 1);
                {
                    infoButton.IconName = "icon-info";
                    infoButton.LabelText = "Details";

                    infoButton.OnTriggered += () =>
                    {
                        // Toggle detail display via info container.
                        PrepareScreen.ToggleInfoDetail();
                    };
                }
                playButton = grid.CreateChild<MenuButton>("play", 2);
                {
                    playButton.IconName = "icon-play";
                    playButton.LabelText = "Play";

                    playButton.OnTriggered += () =>
                    {
                        // TODO: Show GameLoadOverlay
                        ScreenNavigator.Hide<PrepareScreen>();
                        OverlayNavigator.Show<GameLoadOverlay>();
                    };
                }
            }
        }
    }
}