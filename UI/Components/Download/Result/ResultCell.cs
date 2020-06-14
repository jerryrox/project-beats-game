using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Networking.Maps;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Graphics.Effects.Components;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.Download.Result
{
    public class ResultCell : UguiObject, IListItem {

        private UguiSprite container;
        private CoverImage coverImage;
        private ISprite shadow;
        private ILabel titleLabel;
        private ILabel artistLabel;
        private ILabel mapperLabel;
        private PreviewBar previewBar;
        private MetaDisplayer metaDisplayer;
        private ActionBar actionBar;

        private OnlineMapset mapset;


        public int ItemIndex { get; set; }


        [InitWithDependency]
        private void Init()
        {
            container = CreateChild<UguiSprite>("container", 0);
            {
                container.Anchor = AnchorType.Fill;
                container.Offset = new Offset(8f);
                container.SpriteName = "circle-32";
                container.ImageType = Image.Type.Sliced;
                container.Color = new Color(1f, 1f, 1f, 0.25f);
                container.AddEffect(new MaskEffect());

                coverImage = container.CreateChild<CoverImage>("cover", 0);
                {
                    coverImage.Anchor = AnchorType.Fill;
                    coverImage.Offset = Offset.Zero;
                }
                shadow = container.CreateChild<UguiSprite>("shadow", 1);
                {
                    shadow.Anchor = AnchorType.Fill;
                    shadow.Offset = Offset.Zero;
                    shadow.Color = new Color(0f, 0f, 0f, 0.5f);
                    shadow.SpriteName = "gradation-bottom";
                }
                titleLabel = container.CreateChild<Label>("title", 2);
                {
                    titleLabel.Anchor = AnchorType.BottomStretch;
                    titleLabel.Pivot = PivotType.Bottom;
                    titleLabel.SetOffsetHorizontal(8f);
                    titleLabel.Y = 74f;
                    titleLabel.IsBold = true;
                    titleLabel.FontSize = 18;
                    titleLabel.WrapText = true;
                    titleLabel.Alignment = TextAnchor.LowerLeft;

                    titleLabel.AddEffect(new ShadowEffect()).Component.effectColor = Color.black;
                }
                artistLabel = container.CreateChild<Label>("artist", 3);
                {
                    artistLabel.Anchor = AnchorType.BottomStretch;
                    artistLabel.Pivot = PivotType.Bottom;
                    artistLabel.SetOffsetHorizontal(8f);
                    artistLabel.Y = 58f;
                    artistLabel.IsBold = true;
                    artistLabel.FontSize = 16;
                    artistLabel.WrapText = true;
                    artistLabel.Alignment = TextAnchor.LowerLeft;

                    artistLabel.AddEffect(new ShadowEffect()).Component.effectColor = Color.black;
                }
                mapperLabel = container.CreateChild<Label>("mapper", 4);
                {
                    mapperLabel.Anchor = AnchorType.BottomStretch;
                    mapperLabel.Pivot = PivotType.Bottom;
                    mapperLabel.SetOffsetHorizontal(8f);
                    mapperLabel.Y = 42f;
                    mapperLabel.IsBold = true;
                    mapperLabel.FontSize = 16;
                    mapperLabel.WrapText = true;
                    mapperLabel.Alignment = TextAnchor.LowerLeft;

                    mapperLabel.AddEffect(new ShadowEffect()).Component.effectColor = Color.black;
                }
                previewBar = container.CreateChild<PreviewBar>("preview", 5);
                {
                    previewBar.Anchor = AnchorType.BottomStretch;
                    previewBar.Pivot = PivotType.Bottom;
                    previewBar.SetOffsetHorizontal(0f);
                    previewBar.Y = 36f;
                    previewBar.Height = 2f;
                }
                metaDisplayer = container.CreateChild<MetaDisplayer>("meta", 6);
                {
                    metaDisplayer.Anchor = AnchorType.TopStretch;
                    metaDisplayer.Pivot = PivotType.Top;
                    metaDisplayer.SetOffsetHorizontal(8f);
                    metaDisplayer.Y = -8f;
                    metaDisplayer.Height = 24f;
                }
                actionBar = container.CreateChild<ActionBar>("actions", 7);
                {
                    actionBar.Anchor = AnchorType.BottomStretch;
                    actionBar.Pivot = PivotType.Bottom;
                    actionBar.SetOffsetHorizontal(0f);
                    actionBar.Y = 0f;
                    actionBar.Height = 36f;
                }
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            this.mapset = null;
        }

        /// <summary>
        /// Initializes the display for cell using the specified mapset.
        /// </summary>
        public void Setup(OnlineMapset mapset)
        {
            this.mapset = mapset;

            coverImage.Setup(mapset);
            previewBar.Setup(mapset);
            metaDisplayer.Setup(mapset);
            actionBar.Setup(mapset);

            titleLabel.Text = mapset.Title;
            artistLabel.Text = mapset.Artist;
            mapperLabel.Text = mapset.Creator;
        }

#if UNITY_EDITOR
        [ContextMenu("Log mapset info")]
        private void LogMapsetInfo()
        {
            if(mapset == null)
                Debug.Log("null");
            else
                Debug.Log(Newtonsoft.Json.JsonConvert.SerializeObject(mapset).ToString());
        }
#endif
    }
}