using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Graphics;
using PBFramework.UI;
using PBFramework.Utils;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.ProfileMenu
{
    public class StatDisplay : UguiObject, IStatDisplay {

        private ILabel label;
        private ISprite bg;
        private ISprite fg;
        private ISprite center;
        private ILabel centerLabel;


        public float Progress
        {
            get => fg.FillAmount;
            set => fg.FillAmount = value;
        }

        public string LabelText
        {
            get => label.Text;
            set => label.Text = value;
        }

        public string CenterText
        {
            get => centerLabel.Text;
            set => centerLabel.Text = value;
        }


        [InitWithDependency]
        private void Init(IColorPreset colorPreset)
        {
            label = CreateChild<Label>("label", 0);
            {
                label.Anchor = Anchors.Bottom;
                label.Pivot = Pivots.Top;
                label.Y = -4f;
                label.Alignment = TextAnchor.UpperCenter;
                label.FontSize = 17;
            }
            bg = CreateChild<UguiSprite>("bg", 1);
            {
                bg.Anchor = Anchors.Fill;
                bg.RawSize = Vector2.zero;
                bg.SpriteName = "circle-320";
                bg.Color = Color.black;
            }
            fg = CreateChild<UguiSprite>("fg", 2);
            {
                fg.Anchor = Anchors.Fill;
                fg.RawSize = Vector2.zero;
                fg.SpriteName = "circle-320";
                fg.Color = colorPreset.PrimaryFocus;
                fg.ImageType = Image.Type.Filled;

                fg.FillAmount = 0f;
                fg.SetRadial360Fill(Image.Origin360.Top);
            }
            center = CreateChild<UguiSprite>("center", 3);
            {
                center.Anchor = Anchors.Fill;
                center.RawSize = new Vector2(-8f, -8f);
                center.SpriteName = "circle-320";
                center.Color = HexColor.Create("1D2126");

                centerLabel = center.CreateChild<Label>("label");
                {
                    centerLabel.Anchor = Anchors.Fill;
                    centerLabel.RawSize = Vector2.zero;
                    centerLabel.IsBold = true;
                    centerLabel.FontSize = 18;
                }
            }
        }
    }
}