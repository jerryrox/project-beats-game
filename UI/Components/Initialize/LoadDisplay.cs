using PBGame.UI;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Initialize
{
    public class LoadDisplay : UguiObject {

        /// <summary>
        /// Returns the status displayer label.
        /// </summary>
        public ILabel Status { get; private set; }

        /// <summary>
        /// Returns the progress displayer bar.
        /// </summary>
        public IProgressBar Progress { get; private set; }


        [InitWithDependency]
        private void Init()
        {
            Status = CreateChild<Label>("status", 0);
            {
                Status.Anchor = AnchorType.Left;
                Status.Pivot = PivotType.Left;
                Status.Position = new Vector3(20f, 20f);
                Status.Alignment = TextAnchor.MiddleLeft;
                Status.FontSize = 20;
            }
            Progress = CreateChild<UguiProgressBar>("progress", 1);
            {
                Progress.Anchor = AnchorType.MiddleStretch;
                Progress.SetOffsetHorizontal(0f);
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

        /// <summary>
        /// Sets the status to display.
        /// </summary>
        public void SetStatus(string status)
        {
            Status.Text = status;
        }

        /// <summary>
        /// Sets the progress to display.
        /// </summary>
        public void SetProgress(float progress)
        {
            Progress.Value = progress;
        }
    }
}