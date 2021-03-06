using PBGame.UI.Models;
using PBGame.UI.Components.Common;
using PBGame.UI.Components.MusicMenu;
using PBGame.Maps;
using PBGame.Rulesets.Maps;
using PBGame.Graphics;
using PBGame.Animations;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Navigations.Overlays
{
    public class MusicMenuOverlay : BaseSubMenuOverlay<MusicMenuModel> {

        private new ISprite mask;
        private MapImageDisplay imageDisplay;
        private ISprite gradient;
        private ILabel title;
        private ILabel artist;
        private ControlButton randomButton;
        private ControlButton prevButton;
        private ControlButton playButton;
        private ControlButton nextButton;
        private TimeBar timeBar;


        protected override int ViewDepth => ViewDepths.MusicMenuOverlay;


        [InitWithDependency]
        private void Init(IColorPreset colorPreset)
        {
            container.Anchor = AnchorType.TopRight;
            container.Pivot = PivotType.TopRight;
            container.X = -16f;
            container.Y = -16f;
            container.Width = 400f;
            container.Height = 140f;

            mask = container.CreateChild<UguiSprite>("mask", 0);
            {
                mask.Anchor = AnchorType.Fill;
                mask.RawSize = Vector2.zero;
                mask.Position = Vector2.zero;
                mask.SpriteName = "box";
                mask.Color = Color.black;

                imageDisplay = mask.CreateChild<MapImageDisplay>("imageDisplay", 0);
                {
                    imageDisplay.Anchor = AnchorType.Fill;
                    imageDisplay.RawSize = Vector2.zero;
                }
                gradient = mask.CreateChild<UguiSprite>("gradient", 1);
                {
                    gradient.Anchor = AnchorType.Fill;
                    gradient.Offset = new Offset(0f, -22f, 0f, 0f);
                    gradient.SpriteName = "gradation-bottom";
                    gradient.Color = new Color(0f, 0f, 0f, 0.9f);
                }
                title = mask.CreateChild<Label>("title", 2);
                {
                    title.Anchor = AnchorType.BottomStretch;
                    title.SetOffsetHorizontal(16f);
                    title.Y = 92f;
                    title.Height = 30f;
                    title.IsBold = true;
                    title.WrapText = true;
                    title.FontSize = 18;
                }
                artist = mask.CreateChild<Label>("artist", 3);
                {
                    artist.Anchor = AnchorType.BottomStretch;
                    artist.SetOffsetHorizontal(16f);
                    artist.Y = 70f;
                    artist.Height = 30f;
                    artist.WrapText = true;
                    artist.FontSize = 16;
                }
                randomButton = mask.CreateChild<ControlButton>("random", 4);
                {
                    randomButton.Anchor = AnchorType.BottomLeft;
                    randomButton.X = 36f;
                    randomButton.Y = 36f;
                    randomButton.Size = new Vector2(48f, 48f);
                    randomButton.IconName = "icon-random";
                    randomButton.IconSize = 24f;

                    randomButton.OnTriggered += model.RandomizeMusic;
                }
                prevButton = mask.CreateChild<ControlButton>("prev", 5);
                {
                    prevButton.Anchor = AnchorType.Bottom;
                    prevButton.X = -56f;
                    prevButton.Y = 36f;
                    prevButton.Size = new Vector2(48f, 48f);
                    prevButton.IconName = "icon-backward";
                    prevButton.IconSize = 24f;

                    prevButton.OnTriggered += model.PrevMusic;
                }
                playButton = mask.CreateChild<ControlButton>("play", 6);
                {
                    playButton.Anchor = AnchorType.Bottom;
                    playButton.Y = 36f;
                    playButton.Size = new Vector2(48f, 48f);
                    playButton.IconName = "icon-play";
                    playButton.IconSize = 32f;

                    playButton.OnTriggered += model.TogglePlaying;
                }
                nextButton = mask.CreateChild<ControlButton>("next", 7);
                {
                    nextButton.Anchor = AnchorType.Bottom;
                    nextButton.X = 56f;
                    nextButton.Y = 36f;
                    nextButton.Size = new Vector2(48f, 48f);
                    nextButton.IconName = "icon-forward";
                    nextButton.IconSize = 24f;

                    nextButton.OnTriggered += model.NextMusic;
                }
                timeBar = mask.CreateChild<TimeBar>("timebar", 8);
                {
                    timeBar.Anchor = AnchorType.BottomStretch;
                    timeBar.Pivot = PivotType.Bottom;
                    timeBar.SetOffsetHorizontal(0f);
                    timeBar.Y = 0f;
                    timeBar.Height = 8f;
                }
            }
            
            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            model.IsPlaying.OnNewValue += OnPlayingChange;
            model.SelectedMap.BindAndTrigger(OnMapChange);
            model.Background.BindAndTrigger(OnBackgroundChange);
            model.PreferUnicode.OnNewValue += OnPreferUnicode;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            model.IsPlaying.OnNewValue -= OnPlayingChange;
            model.SelectedMap.OnNewValue -= OnMapChange;
            model.Background.OnNewValue -= OnBackgroundChange;
            model.PreferUnicode.OnNewValue -= OnPreferUnicode;
        }

        protected override IAnime CreateShowAnime(IDependencyContainer dependencies)
        {
            return dependencies.Get<IAnimePreset>().GetSubMenuOverlayPopupShow(this, () => container);
        }

        protected override IAnime CreateHideAnime(IDependencyContainer dependencies)
        {
            return dependencies.Get<IAnimePreset>().GetSubMenuOverlayPopupHide(this, () => container);
        }

        /// <summary>
        /// Refreshes the song info label texts.
        /// </summary>
        private void SetLabelText()
        {
            var map = model.SelectedMap.Value;
            if (map != null)
            {
                var preferUnicode = model.PreferUnicode.Value;

                title.Text = map.Metadata.GetTitle(preferUnicode);
                artist.Text = map.Metadata.GetArtist(preferUnicode);
            }
            else
            {
                title.Text = "";
                artist.Text = "";
            }
        }

        /// <summary>
        /// Event called when the map background has changed.
        /// </summary>
        private void OnBackgroundChange(IMapBackground background)
        {
            imageDisplay.SetBackground(background);
        }

        /// <summary>
        /// Event called when the selected map has changed.
        /// </summary>
        private void OnMapChange(IPlayableMap map) => SetLabelText();

        /// <summary>
        /// Event called when the unicode preference option has changed.
        /// </summary>
        private void OnPreferUnicode(bool preferUnicode) => SetLabelText();

        /// <summary>
        /// Event called when the music playing state has changed.
        /// </summary>
        private void OnPlayingChange(bool isPlaying)
        {
            playButton.IconName = isPlaying ? "icon-pause" : "icon-play";
        }
    }
}