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
        private Blocker blocker;


        public bool IsInteractible
        {
            get => !blocker.Active;
            set => blocker.Active = !value;
        }

        [ReceivesDependency]
        private DownloadState State { get; set; }


        [InitWithDependency]
        private void Init()
        {
            var mask = AddEffect(new MaskEffect());
            Color = new Color(0f, 0f, 0f, 0.75f);

            bannerTexture = CreateChild<WebTexture>("banner", 0);
            {
                bannerTexture.Anchor = AnchorType.Fill;
                bannerTexture.Offset = Offset.Zero;
                bannerTexture.Tint = new Color(0.25f, 0.25f, 0.25f);
            }
            grid = CreateChild<UguiGrid>("grid", 1);
            {
                grid.Anchor = AnchorType.Fill;
                grid.Offset = new Offset(0f, 32f);
                grid.SpaceHeight = 16f;
                grid.Limit = 0;

                var modeFilter = grid.CreateChild<DropdownFilter>("mode", 0);
                {
                    modeFilter.LabelText = "Mode";
                    modeFilter.Setup<GameModeType>(State.Mode);
                }
                var genreFilter = grid.CreateChild<DropdownFilter>("genre", 2);
                {
                    genreFilter.LabelText = "Genre";
                    genreFilter.Setup<MapGenreType>(State.Genre);
                }
                var languageFilter = grid.CreateChild<DropdownFilter>("language", 3);
                {
                    languageFilter.LabelText = "Language";
                    languageFilter.Setup<MapLanguageType>(State.Language);
                }
                var hasVideoFilter = grid.CreateChild<ToggleFilter>("hasVideo", 4);
                {
                    hasVideoFilter.LabelText = "Has Video";
                    hasVideoFilter.Setup(State.HasVideo);
                }
                var hasStoryboardFilter = grid.CreateChild<ToggleFilter>("hasStoryboard", 5);
                {
                    hasStoryboardFilter.LabelText = "Has Storyboard";
                    hasStoryboardFilter.Setup(State.HasStoryboard);
                }
            }
            blocker = CreateChild<Blocker>("blocker", 2);

            InvokeAfterTransformed(1, () =>
            {
                grid.CellSize = GetFilterCellSize();
            });

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();
            State.Results.BindAndTrigger(OnResultsChange);
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            State.Results.OnValueChanged -= OnResultsChange;
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

        /// <summary>
        /// Event called on result mapset list change.
        /// </summary>
        private void OnResultsChange(List<OnlineMapset> mapsets, List<OnlineMapset> _)
        {
            if(State.IsRequestingNextPage)
                return;
                
            bannerTexture.Unload();
            if(mapsets != null && mapsets.Count > 0)
                bannerTexture.Load(mapsets[0].CoverImage);
        }
    }
}