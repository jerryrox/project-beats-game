using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Maps;
using PBGame.Rulesets.Maps;
using PBFramework.UI;
using PBFramework.Utils;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components
{
    public class MapImageDisplay : UguiObject, IHasColor {

        private ITexture[] textures;

        private IMapBackground curBackground;
        private IAnime transitionAni;
        private int curIndex = 0;
        private Color tintColor = Color.white;



        public Color Color
        {
            get => tintColor;
            set => SetTint(value);
        }

        /// <summary>
        /// Unsupported property.
        /// </summary>
        public float Alpha { get; set; }

        /// <summary>
        /// Returns the texture displayer at current index.
        /// </summary>
        private ITexture CurTexture => textures[curIndex];


        [InitWithDependency]
        private void Init()
        {
            textures = new ITexture[2];
            for (int i = 0; i < textures.Length; i++)
            {
                var texture = textures[i] = CreateChild<UguiTexture>($"texture{i}", i);
                texture.Anchor = Anchors.Fill;
                texture.RawSize = Vector2.zero;
                texture.Alpha = 0f;
            }

            transitionAni = new Anime();
            transitionAni.AnimateFloat(progress =>
            {
                for (int i = 0; i < textures.Length; i++)
                {
                    var alpha = textures[i].Alpha;
                    textures[i].Alpha = Easing.Linear(progress, alpha, i == curIndex ? 1 - alpha : -alpha, 0f);
                }
            })
                .AddTime(0f, 0f)
                .AddTime(0.35f, 1f)
                .Build();
            transitionAni.AddEvent(transitionAni.Duration, () =>
            {
                // Remove texture reference from all displays.
                for (int i = 0; i < textures.Length; i++)
                {
                    if(i != curIndex)
                        textures[i].Texture = null;
                }
            });
        }

        /// <summary>
        /// Sets the background to display.
        /// </summary>
        public void SetBackground(IMapBackground background)
        {
            // We should draw the image on the next texture.
            PrepareNextTexture();

            // Set texture
            SetCurTexture(background.Image);

            // Do transition.
            transitionAni.PlayFromStart();
        }

        /// <summary>
        /// Dispatches FillTexture call to the current texture.
        /// </summary>
        public void FillTexture() => CurTexture.FillTexture();

        /// <summary>
        /// Sets the texture specified texture reference to the current display.
        /// </summary>
        private void SetCurTexture(Texture2D tex)
        {
            CurTexture.Texture = tex;
            if(tex == null)
                CurTexture.Active = false;
            else
            {
                CurTexture.Active = true;
                // Because it takes at least a frame for Unity to refresh its RectTransformation values, we must fill the texture after a frame.
                InvokeAfterFrames(1, CurTexture.FillTexture);
            }
        }

        /// <summary>
        /// Sets texture index to next.
        /// </summary>
        private void PrepareNextTexture()
        {
            // Use current index if null texture anyway.
            if(CurTexture.Texture == null)
                return;

            curIndex++;
            if(curIndex >= textures.Length)
                curIndex = 0;
        }

        /// <summary>
        /// Sets tint color on all textures.
        /// </summary>
        private void SetTint(Color color)
        {
            tintColor = color;
            foreach (var t in textures)
            {
                Color tColor = t.Color;
                tColor.r = color.r;
                tColor.g = color.g;
                tColor.b = color.b;
                t.Color = tColor;
            }
        }
    }
}