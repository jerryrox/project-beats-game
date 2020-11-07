using PBGame.UI.Models;
using PBGame.Rulesets.Maps;
using PBGame.Graphics;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Result
{
    public class InfoBlock : UguiObject {

        private Label titleLabel;
        private Label artistLabel;
        private Label versionLabel;
        private Label mapperLabel;


        [ReceivesDependency]
        private IColorPreset ColorPreset { get; set; }

        [ReceivesDependency]
        private IRoot Root { get; set; }

        [ReceivesDependency]
        private ResultModel Model { get; set; }


        [InitWithDependency]
        private void Init()
        {
            var bg = CreateChild<UguiSprite>("bg");
            {
                bg.Anchor = AnchorType.Fill;
                bg.Color = ColorPreset.DarkBackground;
                bg.Offset = Offset.Zero;
            }
            titleLabel = CreateChild<Label>("title");
            {
                titleLabel.Anchor = AnchorType.Bottom;
                titleLabel.Position = new Vector3(0f, 104f, 0f);
                titleLabel.IsBold = true;
                titleLabel.FontSize = 28;
                titleLabel.Height = 30;
                titleLabel.Width = Root.Width - 100;
                titleLabel.Alignment = TextAnchor.MiddleCenter;
            }
            artistLabel = CreateChild<Label>("artist");
            {
                artistLabel.Anchor = AnchorType.Bottom;
                artistLabel.Position = new Vector3(0f, 74f, 0f);
                artistLabel.FontSize = 22;
                artistLabel.Height = 30;
                artistLabel.Width = Root.Width - 100;
                artistLabel.Alignment = TextAnchor.MiddleCenter;
            }
            versionLabel = CreateChild<Label>("version");
            {
                versionLabel.Anchor = AnchorType.BottomLeft;
                versionLabel.Pivot = PivotType.BottomLeft;
                versionLabel.Position = new Vector3(16f, 30f);
                versionLabel.IsBold = true;
                versionLabel.FontSize = 22;
                versionLabel.Alignment = TextAnchor.LowerLeft;
            }
            mapperLabel = CreateChild<Label>("mapper");
            {
                mapperLabel.Anchor = AnchorType.BottomLeft;
                mapperLabel.Pivot = PivotType.BottomLeft;
                mapperLabel.Position = new Vector3(16f, 8f);
                mapperLabel.FontSize = 20;
                mapperLabel.Alignment = TextAnchor.LowerLeft;
            }

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            Model.Map.Bind(OnMapChanged);
            Model.PreferUnicode.Bind(OnPreferUnicode);
            Refresh();
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();

            Model.Map.Unbind(OnMapChanged);
            Model.PreferUnicode.Unbind(OnPreferUnicode);
        }

        /// <summary>
        /// Refreshes display components.
        /// </summary>
        private void Refresh()
        {
            var map = Model.Map.Value;
            var preferUnicode = Model.PreferUnicode.Value;

            titleLabel.Text = map?.Metadata.GetTitle(preferUnicode);
            artistLabel.Text = map?.Metadata.GetArtist(preferUnicode);
            versionLabel.Text = map?.Detail.Version;
            mapperLabel.Text = $"mapped by {map?.Metadata.Creator ?? ""}";
        }

        /// <summary>
        /// Event called when the map instance has changed.
        /// </summary>
        private void OnMapChanged(IPlayableMap map) => Refresh();

        /// <summary>
        /// Event called when the prefer unicode settings has changed.
        /// </summary>
        private void OnPreferUnicode(bool preferUnicode) => Refresh();
    }
}