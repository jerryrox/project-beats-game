using PBGame.UI;
using PBGame.Assets.Fonts;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Initialize
{
    public class LoadDisplay : UguiObject, ILoadDisplay {

        public ILabel Status { get; private set; }

        public IProgressBar Progress { get; private set; }


        [ReceivesDependency]
        private IFontManager FontManager { get; set; }


        [InitWithDependency]
        private void Init()
        {
            Status = CreateChild<Label>("status", 0);
            {
                Status.Anchor = Anchors.Left;
                Status.Pivot = Pivots.Left;
                Status.Position = new Vector3(20f, 20f);
                Status.Alignment = TextAnchor.MiddleLeft;
                Status.FontSize = 20;
                Status.Font = FontManager.DefaultFont;
            }
            Progress = CreateChild<UguiProgressBar>("progress", 1);
            {
                Progress.Anchor = Anchors.MiddleStretch;
                Progress.OffsetLeft = 0f;
                Progress.OffsetRight = 0f;
                Progress.Y = 2f;
                Progress.Height = 10f;

                var background = Progress.Background;
                {
                    background.SpriteName = "box";
                    background.Color = Color.black;
                }
                var foreground = Progress.Foreground;
                {
                    foreground.SpriteName = "box";
                }
            }

            // Reset display.
            SetStatus(null);
            SetProgress(0f);
        }

        public void SetStatus(string status)
        {
            Status.Text = status;
        }

        public void SetProgress(float progress)
        {
            Progress.Value = progress;
        }
    }
}