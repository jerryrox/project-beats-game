using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Models;
using PBGame.UI.Models.Game;
using PBGame.UI.Components.Common;
using PBGame.UI.Components.GameLoad;
using PBGame.UI.Navigations.Screens;
using PBGame.Maps;
using PBGame.Rulesets;
using PBGame.Configurations;
using PBFramework.UI;
using PBFramework.UI.Navigations;
using PBFramework.Audio;
using PBFramework.Utils;
using PBFramework.Inputs;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Navigations.Overlays
{
    public class GameLoadOverlay : BaseOverlay<GameLoadModel> {

        private InfoDisplayer infoDisplayer;
        private LoadIndicator loadIndicator;

        private IAnime componentShowAni;
        private IAnime componentHideAni;


        protected override int ViewDepth => ViewDepths.GameLoadOverlay;


        [InitWithDependency]
        private void Init()
        {
            var blur = CreateChild<BlurDisplay>("blur", 0);
            {
                blur.Anchor = AnchorType.Fill;
                blur.Offset = Offset.Zero;

                var dark = blur.CreateChild<UguiSprite>("dark", 0);
                {
                    dark.Anchor = AnchorType.Fill;
                    dark.Offset = Offset.Zero;
                    dark.Color = new Color(0f, 0f, 0f, 0.75f);
                }
            }
            infoDisplayer = CreateChild<InfoDisplayer>("info", 1);
            {
            }
            loadIndicator = CreateChild<LoadIndicator>("load", 2);
            {
                loadIndicator.Position = new Vector3(0f, -260f);
                loadIndicator.Size = new Vector2(88f, 88f);
            }

            float showDur = Mathf.Max(infoDisplayer.ShowAniDuration, loadIndicator.ShowAniDuration);
            componentShowAni = new Anime();
            componentShowAni.AddEvent(0f, () =>
            {
                infoDisplayer.Show();
                loadIndicator.Show();
            });
            componentShowAni.AddEvent(showDur + model.MinimumLoadTime, OnShowAniEnd);

            float hideDur = Mathf.Max(infoDisplayer.HideAniDuration, loadIndicator.HideAniDuration);
            componentHideAni = new Anime();
            componentHideAni.AddEvent(0f, () =>
            {
                infoDisplayer.Hide();
                loadIndicator.Hide();
            });
            componentHideAni.AnimateFloat(v => model.MusicController.SetFade(v))
                .AddTime(0f, 0.5f, EaseType.QuadEaseOut)
                .AddTime(hideDur, 0f)
                .Build();
            componentHideAni.AddEvent(hideDur, () =>
            {
                model.MusicController.SetFade(1f);
                OnHideAniEnd();
            });

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            model.OnLoadSucceed += OnLoadSucceed;
            model.OnLoadFail += OnLoadFail;

            componentShowAni.PlayFromStart();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            model.OnLoadSucceed -= OnLoadSucceed;
            model.OnLoadFail -= OnLoadFail;
        }

        /// <summary>
        /// Event called when the game loading has succeeded.
        /// </summary>
        private void OnLoadSucceed()
        {
            // If the hide animation is playing, the player must've navigated out before this was executed.
            if (HideAnime.IsPlaying)
                return;
            componentHideAni.PlayFromStart();
        }

        /// <summary>
        /// Event called when the game loading has failed.
        /// </summary>
        private void OnLoadFail()
        {
            componentShowAni.Stop();
            componentHideAni.Stop();
        }

        /// <summary>
        /// Event called from component show ani when it has finished animating.
        /// </summary>
        private void OnShowAniEnd() => model.OnShowAniEnd();

        /// <summary>
        /// Event called from component hide ani when it has finished animating.
        /// </summary>
        private void OnHideAniEnd() => model.NavigateToGame();
    }
}