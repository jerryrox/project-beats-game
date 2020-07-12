using PBGame.UI.Models;
using PBGame.Audio;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.Offsets
{
    public class MetronomeContainer : UguiObject {

        private MetronomeDisplay metronomeDisplay;
        private IGrid modeGrid;
        private MetronomeMode fullMode;
        private MetronomeMode halfMode;


        /// <summary>
        /// View which is displayed when the metronome is currently available.
        /// </summary>
        public IGraphicObject AvailableView { get; private set; }

        /// <summary>
        /// View which is displayed when the metronome cannot be displayed currently.
        /// </summary>
        public IGraphicObject UnavailableView { get; private set; }

        [ReceivesDependency]
        private OffsetsModel Model { get; set; }


        [InitWithDependency]
        private void Init()
        {
            AvailableView = CreateChild("available", 0);
            {
                AvailableView.Anchor = AnchorType.Fill;
                AvailableView.Offset = Offset.Zero;

                metronomeDisplay = AvailableView.CreateChild<MetronomeDisplay>("metronome", 0);
                {
                    metronomeDisplay.Anchor = AnchorType.CenterStretch;
                    metronomeDisplay.X = 120f;
                    metronomeDisplay.SetOffsetVertical(0f);
                    metronomeDisplay.Width = 500f;
                    metronomeDisplay.SetMetronome(metronome);
                }
                modeGrid = AvailableView.CreateChild<UguiGrid>("modes", 1);
                {
                    modeGrid.Anchor = AnchorType.CenterStretch;
                    modeGrid.X = -200f;
                    modeGrid.SetOffsetVertical(0f);
                    modeGrid.Width = 500f;
                    modeGrid.Spacing = new Vector2(44f, 0f);
                    modeGrid.Axis = GridLayoutGroup.Axis.Horizontal;
                    modeGrid.Alignment = TextAnchor.MiddleCenter;

                    fullMode = modeGrid.CreateChild<MetronomeMode>("full", 0);
                    {
                        fullMode.Frequency = BeatFrequency.Full;
                        fullMode.SetMetronome(metronome);
                    }
                    halfMode = modeGrid.CreateChild<MetronomeMode>("half", 1);
                    {
                        halfMode.Frequency = BeatFrequency.Half;
                        halfMode.SetMetronome(metronome);
                    }
                }
            }
            UnavailableView = CreateChild("unavailable", 1);
            {
                UnavailableView.Anchor = AnchorType.Fill;
                UnavailableView.Offset = Offset.Zero;

                var message = UnavailableView.CreateChild<Label>("message", 0);
                {
                    message.FontSize = 20;
                    message.Anchor = AnchorType.Fill;
                    message.Offset = Offset.Zero;
                    message.Alignment = TextAnchor.MiddleCenter;
                    message.Text = "Metronome currently unavailable.";
                }
            }

            InvokeAfterTransformed(2, () =>
            {
                modeGrid.CellSize = new Vector2(68f, this.Height);
            });

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            Model.IsMetronomeAvailable.OnNewValue += OnMetronomeAvailable;
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();

            Model.IsMetronomeAvailable.OnNewValue -= OnMetronomeAvailable;
        }

        /// <summary>
        /// Event called when the metronome availability has changed.
        /// </summary>
        private void OnMetronomeAvailable(bool isAvailable)
        {
            if (isAvailable)
            {
                AvailableView.Active = true;
                UnavailableView.Active = false;
            }
            else
            {
                AvailableView.Active = false;
                UnavailableView.Active = true;
            }
        }
    }
}