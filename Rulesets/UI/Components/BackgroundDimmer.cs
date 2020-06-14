using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Configurations;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.Rulesets.UI.Components
{
    public class BackgroundDimmer : UguiSprite {

        private Anime dimAni;
        private Anime brightenAni;


        [ReceivesDependency]
        private IGameSession GameSession { get; set; }

        [ReceivesDependency]
        private IGameConfiguration GameConfiguration { get; set; }


        [InitWithDependency]
        private void Init()
        {
            GameSession.OnSoftInit += OnSoftInit;
            GameSession.OnCompletion += OnCompletion;

            this.Color = new Color();

            dimAni = new Anime();
            dimAni.AnimateFloat(a => this.Alpha = a)
                .AddTime(0f, 0f)
                .AddTime(0.25f, () => GameConfiguration.BackgroundDim.Value)
                .Build();

            brightenAni = new Anime();
            brightenAni.AnimateFloat(a => this.Alpha = a)
                .AddTime(0f, () => GameConfiguration.BackgroundDim.Value)
                .AddTime(0.25f, 0f)
                .Build();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (dimAni != null)
                dimAni.Stop();
            if (brightenAni != null)
                brightenAni.Stop();
        }

        /// <summary>
        /// Event called on game session soft initialization.
        /// </summary>
        private void OnSoftInit()
        {
            brightenAni.Stop();
            dimAni.PlayFromStart();
        }

        /// <summary>
        /// Event called on game completion.
        /// </summary>
        private void OnCompletion()
        {
            dimAni.Stop();
            brightenAni.PlayFromStart();
        }
    }
}