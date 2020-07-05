using PBGame.UI.Models;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Prepare.Details.Meta
{
    public class MetaDescription : UguiObject {

        private ILabel label;
        private IScrollView scrollView;
        private ILabel contentLabel;


        [ReceivesDependency]
        private PrepareModel Model { get; set; }


        [InitWithDependency]
        private void Init()
        {
            label = CreateChild<Label>("label", 0);
            {
                label.Anchor = AnchorType.TopLeft;
                label.Pivot = PivotType.TopLeft;
                label.X = 32f;
                label.Y = -32f;
                label.IsBold = true;
                label.FontSize = 18;
                label.Alignment = TextAnchor.UpperLeft;

                label.Text = "Description";
            }
            scrollView = CreateChild<UguiScrollView>("scrollview", 1);
            {
                scrollView.Anchor = AnchorType.Fill;
                scrollView.Offset = new Offset(32f, 64f, 32f, 32f);

                scrollView.Background.Alpha = 0f;

                contentLabel = scrollView.Container.CreateChild<Label>("content", 0);
                {
                    contentLabel.Anchor = AnchorType.Fill;
                    contentLabel.RawSize = Vector2.zero;
                    contentLabel.FontSize = 16;
                    contentLabel.Alignment = TextAnchor.UpperLeft;
                }
            }

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            Model.MapsetDescription.BindAndTrigger(OnDescriptionChange);
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();

            Model.MapsetDescription.OnNewValue -= OnDescriptionChange;
        }

        /// <summary>
        /// Sets the content text on the label.
        /// </summary>
        private void SetContent(string content)
        {
            contentLabel.Text = content;
            scrollView.Container.Height = contentLabel.PreferredHeight;
            scrollView.ResetPosition();
        }

        /// <summary>
        /// Event called on mapset description change.
        /// </summary>
        private void OnDescriptionChange(string description) => SetContent(description);
    }
}