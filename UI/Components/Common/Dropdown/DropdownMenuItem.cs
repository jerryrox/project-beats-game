using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Graphics;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Allocation.Recyclers;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.Common.Dropdown
{
    public class DropdownMenuItem : FocusableTrigger, IRecyclable<DropdownMenuItem> {

        private ILabel label;


        public IRecycler<DropdownMenuItem> Recycler { get; set; }

        /// <summary>
        /// The dropdown data represented by this item.
        /// </summary>
        public DropdownData Data { get; set; }

        protected override int FocusSpriteDepth => 1;

        protected override int HoverSpriteDepth => 2;

        protected override bool ChangeFocusIfDifferent => false;


        [InitWithDependency]
        private void Init(IColorPreset colorPreset)
        {
            hoverSprite.SpriteName = "circle-16";
            hoverSprite.ImageType = Image.Type.Sliced;

            focusSprite.SpriteName = "circle-16";
            focusSprite.ImageType = Image.Type.Sliced;
            focusSprite.Tint = colorPreset.Passive;

            label = CreateChild<Label>("label", 10);
            {
                label.Anchor = Anchors.Fill;
                label.Offset = new Offset(16f, 0f);
                label.Alignment = TextAnchor.MiddleLeft;
                label.FontSize = 16;
            }

            UseDefaultHoverAni();

            focusAni = new Anime();
            focusAni.AnimateFloat(a => focusSprite.Alpha = a)
                .AddTime(0f, () => focusSprite.Alpha)
                .AddTime(0.01f, 1f)
                .Build();

            unfocusAni = new Anime();
            unfocusAni.AnimateFloat(a => focusSprite.Alpha = a)
                .AddTime(0f, () => focusSprite.Alpha)
                .AddTime(0.01f, 0f)
                .Build();
        }

        /// <summary>
        /// Sets up the dropdown item using specified data.
        /// </summary>
        public void Setup(DropdownData data, bool isSelected)
        {
            this.Data = data;
            IsFocused = isSelected;
            label.Text = data.Text;
        }

        public void OnRecycleNew()
        {
            SetFocused(false, false);
            hoverSprite.Alpha = 0f;
            Active = true;
        }

        public void OnRecycleDestroy()
        {
            Active = false;
        }
    }
}