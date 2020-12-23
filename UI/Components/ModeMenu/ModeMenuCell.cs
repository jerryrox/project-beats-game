using PBGame.UI.Models;
using PBGame.UI.Components.Common;
using PBGame.Graphics;
using PBGame.Rulesets;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.ModeMenu
{
    public class ModeMenuCell : FocusableTrigger
    {
        private Label label;


        /// <summary>
        /// The mode service this cell represents for.
        /// </summary>
        public IModeService ModeService { get; private set; }

        [ReceivesDependency]
        private ModeMenuModel Model { get; set; }

        [ReceivesDependency]
        private IColorPreset ColorPreset { get; set; }
    

        [InitWithDependency]
        private void Init()
        {
            label = CreateChild<Label>("label");
            {
                label.Anchor = AnchorType.Fill;
                label.Offset = new Offset(56f, 0f, 16f, 0f);
                label.FontSize = 18;
                label.Alignment = TextAnchor.MiddleLeft;
            }
            CreateIconSprite(size: 28, alpha: 0.75f);
            {
                iconSprite.Anchor = AnchorType.Left;
                iconSprite.Pivot = PivotType.Left;
                iconSprite.Position = new Vector3(16f, 0f);
            }

            focusSprite.Tint = ColorPreset.PrimaryFocus;

            UseDefaultHoverAni();
            UseDefaultFocusAni();

            focusAni.AnimateColor((color) => iconSprite.Color = label.Color = color)
                .AddTime(0f, () => iconSprite.Color)
                .AddTime(focusAni.Duration, ColorPreset.PrimaryFocus)
                .Build();

            unfocusAni.AnimateColor((color) => iconSprite.Color = label.Color = color)
                .AddTime(0f, () => iconSprite.Color)
                .AddTime(unfocusAni.Duration, new Color(1f, 1f, 1f, 0.75f))
                .Build();

            OnEnableInited();
        }

        /// <summary>
        /// Initializes the cell state using the specified mode service.
        /// </summary>
        public void SetModeService(IModeService modeService)
        {
            label.Text = modeService.Name;
            IconName = modeService.GetIconName(32);
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            Model.GameMode.BindAndTrigger(OnGameModeChanged);
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();

            Model.GameMode.Unbind(OnGameModeChanged);
        }

        /// <summary>
        /// Event called when the current game mode has changed.
        /// </summary>
        private void OnGameModeChanged(GameModeType type)
        {
            IsFocused = type == ModeService?.GameMode;
        }
    }
}