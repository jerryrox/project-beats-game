using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Rulesets.Beats.Standard.Maps;
using PBGame.Rulesets.Beats.Standard.UI.Components;
using PBGame.Graphics;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Graphics.Effects.CoffeeUI;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;
using Coffee.UIExtensions;

namespace PBGame.Rulesets.Beats.Standard.UI
{
    public class PlayAreaContainer : Rulesets.UI.PlayAreaContainer {

        private const float DraggerBodyQuality = 2f;
        private const float DefaultBodyInterval = 0.1f;


        private ISprite bgSprite;
        private ISprite hitBarSprite;
        private HitObjectHolder hitObjectHolder;
        private PlayAreaFader playAreaFader;

        private float distancePerTime;
        private float draggerBodyInterval = DefaultBodyInterval;


        /// <summary>
        /// Returns the Y position from the bottom of the play area where the note should be hit.
        /// </summary>
        public float HitPosition => 150f;

        /// <summary>
        /// Returns the Y position which the hit object fade starts from.
        /// </summary>
        public float FadeStartPos => 3000f;

        /// <summary>
        /// Returns the position where hit objects spawn and approach from.
        /// </summary>
        public float FallStartPos => FadeStartPos + playAreaFader.FadeSize;

        /// <summary>
        /// Returns the amount of Y position approached per millisecond.
        /// </summary>
        public float DistancePerTime => distancePerTime;

        /// <summary>
        /// Returns the interval at which the dragger body is sampled from path.
        /// </summary>
        public float DraggerBodyInterval => draggerBodyInterval;

        [ReceivesDependency]
        private IRoot3D Root3D { get; set; }

        [ReceivesDependency]
        private IGameSession GameSession { get; set; }


        [InitWithDependency]
        private void Init()
        {
            Dependencies = Dependencies.Clone();
            Dependencies.Cache(this);

            GameSession.OnHardInit += OnHardInit;
            GameSession.OnHardDispose += OnHardDispose;

            bgSprite = CreateChild<UguiSprite>("bg", 0);
            {
                bgSprite.Anchor = AnchorType.Fill;
                bgSprite.Offset = Offset.Zero;
                bgSprite.SpriteName = "gradation-bottom";
                bgSprite.Color = new Color(0f, 0f, 0f, 0.75f);

                var shadow = bgSprite.CreateChild<UguiSprite>("shadow", 1);
                {
                    shadow.Anchor = AnchorType.Fill;
                    shadow.Offset = new Offset(-30f);
                    shadow.SpriteName = "glow-square-32-x2";
                    shadow.ImageType = Image.Type.Sliced;

                    var gradient = shadow.AddEffect(new GradientEffect()).Component;
                    gradient.direction = UIGradient.Direction.Vertical;
                    gradient.color1 = new Color(0f, 0f, 0f, 0f);
                    gradient.color2 = Color.black;
                }
            }
            hitBarSprite = CreateChild<UguiSprite>("hit-bar", 1);
            {
                hitBarSprite.Anchor = AnchorType.BottomStretch;
                hitBarSprite.SetOffsetHorizontal(0f);
                hitBarSprite.Y = HitPosition;
                hitBarSprite.Height = 34;
                hitBarSprite.SpriteName = "glow-bar";
                hitBarSprite.ImageType = Image.Type.Sliced;
            }
            hitObjectHolder = CreateChild<HitObjectHolder>("obj-holder", 2);
            {
                hitObjectHolder.Anchor = AnchorType.Bottom;
                hitObjectHolder.Pivot = PivotType.Bottom;
                hitObjectHolder.Y = 0f;
            }
            playAreaFader = CreateChild<PlayAreaFader>("fader", 3);
            {
                playAreaFader.Anchor = AnchorType.BottomStretch;
                playAreaFader.Pivot = PivotType.Bottom;
                playAreaFader.SetOffsetHorizontal(0f);
                playAreaFader.Y = FadeStartPos;
                playAreaFader.Height = 10000f;
            }
        }

        /// <summary>
        /// Event called on session hard init.
        /// </summary>
        private void OnHardInit()
        {
            var map = GameSession.CurrentMap as Map;
            var dummyObj = map.HitObjects[0];

            distancePerTime = (FallStartPos - HitPosition) / dummyObj.ApproachDuration;
            draggerBodyInterval = dummyObj.Radius / (distancePerTime * DraggerBodyQuality);
        }

        /// <summary>
        /// Event called on session hard dispose.
        /// </summary>
        private void OnHardDispose()
        {
            distancePerTime = 0f;
            draggerBodyInterval = DefaultBodyInterval;
        }
    }
}