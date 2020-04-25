using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.Common;
using PBGame.Rulesets;
using PBGame.Networking.Maps;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Graphics.Effects.Components;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.Download.Search
{
    public class BannerContainer : UguiSprite {

        private WebTexture bannerTexture;
        private IGrid grid;
        private UguiSprite blocker;


        public bool IsInteractible
        {
            get => !blocker.Active;
            set => blocker.Active = !value;
        }


        [InitWithDependency]
        private void Init(DownloadState state)
        {
            var mask = AddEffect(new MaskEffect());
            mask.Component.showMaskGraphic = false;

            bannerTexture = CreateChild<WebTexture>("banner", 0);
            {
                bannerTexture.Anchor = Anchors.Fill;
                bannerTexture.Offset = Offset.Zero;
                bannerTexture.Tint = new Color(0.25f, 0.25f, 0.25f);
            }
            grid = CreateChild<UguiGrid>("grid", 1);
            {
                grid.Anchor = Anchors.Fill;
                grid.Offset = new Offset(0f, 32f);
                grid.SpaceHeight = 16f;

                var modeFilter = grid.CreateChild<DropdownFilter>("mode", 0);
                {
                    modeFilter.LabelText = "Mode";
                    modeFilter.Setup<GameModes>(state.Mode);
                }
                var categoryFilter = grid.CreateChild<DropdownFilter>("category", 1);
                {
                    categoryFilter.LabelText = "Category";
                    categoryFilter.Setup<MapCategories>(state.Category);
                }
                var genreFilter = grid.CreateChild<DropdownFilter>("genre", 2);
                {
                    genreFilter.LabelText = "Genre";
                    genreFilter.Setup<MapGenres>(state.Genre);
                }
                var languageFilter = grid.CreateChild<DropdownFilter>("language", 3);
                {
                    languageFilter.LabelText = "Language";
                    languageFilter.Setup<MapLanguages>(state.Language);
                }
                var hasVideoFilter = grid.CreateChild<ToggleFilter>("hasVideo", 4);
                {
                    hasVideoFilter.LabelText = "Has Video";
                    hasVideoFilter.Setup(state.HasVideo);
                }
                var hasStoryboardFilter = grid.CreateChild<ToggleFilter>("hasStoryboard", 5);
                {
                    hasStoryboardFilter.LabelText = "Has Storyboard";
                    hasStoryboardFilter.Setup(state.HasStoryboard);
                }
            }
            blocker = CreateChild<UguiSprite>("blocker", 2);
            {
                blocker.Anchor = Anchors.Fill;
                blocker.Offset = Offset.Zero;
                blocker.Alpha = 0f;
            }

            InvokeAfterFrames(1, () =>
            {
                grid.CellSize = GetFilterCellSize();
            });
        }

        /// <summary>
        /// Adjusts banner texture so the image can be displayed correctly on current aspect ratio.
        /// </summary>
        public void AdjustBannerTexture() => bannerTexture.FillTexture();

        /// <summary>
        /// Returns the appropriate filter cell size based on the grid's width.
        /// </summary>
        private Vector2 GetFilterCellSize()
        {
            float width = grid.Width;
            if(width > 1400)
                return new Vector2(width / 4f, 64f);
            return new Vector2(width / 3f, 64f);
        }
    }
}