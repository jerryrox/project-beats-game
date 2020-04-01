using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Navigations.Screens;
using PBGame.Graphics;
using PBFramework.UI;
using PBFramework.UI.Navigations;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Prepare.Details
{
    public class MenuHolder : UguiSprite, IMenuHolder {

        private IGrid grid;

        private IMenuButton backButton;
        private IMenuButton infoButton;
        private IMenuButton playButton;


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

            var resolution = rootMain.Resolution;

            grid = CreateChild<UguiGrid>("grid", 0);
            {
                grid.Anchor = Anchors.Fill;
                grid.RawSize = Vector2.zero;
                grid.CellSize = new Vector2(resolution.x / 3f, 56f);

                backButton = grid.CreateChild<MenuButton>("back", 0);
                {
                    backButton.IconName = "icon-arrow-left";
                    backButton.LabelText = "Back";

                    backButton.OnPointerDown += () =>
                    {
                        ScreenNavigator.Show<SongsScreen>();
                    };
                }
                infoButton = grid.CreateChild<MenuButton>("info", 1);
                {
                    infoButton.IconName = "icon-info";
                    infoButton.LabelText = "Details";

                    infoButton.OnPointerDown += () =>
                    {
                        // Toggle detail display via info container.
                        PrepareScreen.ToggleInfoDetail();
                    };
                }
                playButton = grid.CreateChild<MenuButton>("play", 2);
                {
                    playButton.IconName = "icon-play";
                    playButton.LabelText = "Play";

                    playButton.OnPointerDown += () =>
                    {
                        // TODO: Show GameLoadOverlay
                        ScreenNavigator.Hide<PrepareScreen>();
                        // OverlayNavigator.Show<GameLoadOverlay>();
                    };
                }
            }
        }
    }
}