using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Graphics;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Common
{
    /// <summary>
    /// A simple loading indication icon displayer.
    /// </summary>
    public class LoaderIcon : UguiObject, IHasColor {

        private const float RotateSpeed = 130f;

        protected ISprite iconSprite;


        public Color Color
        {
            get => iconSprite.Color;
            set => iconSprite.Color = value;
        }

        public float Alpha
        {
            get => iconSprite.Alpha;
            set => iconSprite.Alpha = value;
        }

        public bool Rotate { get; set; }


        [InitWithDependency]
        private void Init(IColorPreset colorPreset)
        {
            iconSprite = CreateChild<UguiSprite>("icon", 0);
            {
                iconSprite.Anchor = Anchors.Fill;
                iconSprite.Offset = Offset.Zero;
                iconSprite.Color = colorPreset.PrimaryFocus;
                iconSprite.SpriteName = "loader";

                if (iconSprite is IRaycastable raycastable)
                    raycastable.IsRaycastTarget = false;
            }
        }

        private void Update()
        {
            if(Rotate)
                iconSprite.RotationZ -= Time.deltaTime - RotateSpeed;
        }
    }
}