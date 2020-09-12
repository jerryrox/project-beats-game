using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Assets.Caching;
using PBFramework.UI;
using PBFramework.Assets.Caching;
using PBFramework.Graphics;
using PBFramework.Allocation.Caching;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.Common
{
    public class WebTexture : UguiTexture {

        private CacherAgent<string, Texture2D> cacherAgent;

        private IAnime showAni;


        [ReceivesDependency]
        private IWebImageCacher WebImageCacher { get; set; }


        [InitWithDependency]
        private void Init()
        {
            cacherAgent = new CacherAgent<string, Texture2D>(WebImageCacher);
            cacherAgent.OnFinished += OnImageLoaded;
            cacherAgent.OnProgress += OnLoadProgress;

            this.Alpha = 0f;

            showAni = new Anime();
            showAni.AnimateFloat(a => this.Alpha = a)
                .AddTime(0f, () => this.Alpha)
                .AddTime(0.25f, 1f)
                .Build();
        }

        /// <summary>
        /// Starts loading the image from specified location onto this texture.
        /// </summary>
        public void Load(string url)
        {
            Unload();

            if (string.IsNullOrEmpty(url))
                return;
            OnLoadStart();
            cacherAgent.Request(url);
        }

        /// <summary>
        /// Unloads the image from the texture.
        /// </summary>
        public void Unload()
        {
            cacherAgent.Remove();
            OnUnload();
        }

        /// <summary>
        /// Event called on load start.
        /// This is called before the cacher agent actually starts loading.
        /// </summary>
        protected virtual void OnLoadStart() {}

        /// <summary>
        /// Event called on image unload.
        /// </summary>
        protected virtual void OnUnload()
        {
            this.Texture = null;
            this.Alpha = 0f;
            showAni.Stop();
        }

        /// <summary>
        /// Event called on web image load.
        /// </summary>
        protected virtual void OnImageLoaded(Texture2D image)
        {
            this.Texture = image;
            showAni.PlayFromStart();

            InvokeAfterTransformed(1, FillTexture);
        }

        /// <summary>
        /// Event called on loading progress change.
        /// </summary>
        protected virtual void OnLoadProgress(float progress) {}
    }
}