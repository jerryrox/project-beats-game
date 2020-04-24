using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Assets.Caching;
using PBGame.Networking.Maps;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Allocation.Caching;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.Download.Result
{
    public class CoverImage : UguiTexture {

        private IAnime showAni;

        private CacherAgent<Texture2D> cacherAgent;
        private OnlineMapset mapset;


        [ReceivesDependency]
        private IWebImageCacher Cacher { get; set; }


        [InitWithDependency]
        private void Init()
        {
            cacherAgent = new CacherAgent<Texture2D>(Cacher);
            cacherAgent.OnFinished += OnImageLoaded;

            Color = new Color(0.75f, 0.75f, 0.75f);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            Reset();
        }

        /// <summary>
        /// Starts displaying the cover image for the specified mapset.
        /// </summary>
        public void Setup(OnlineMapset mapset)
        {
            Reset();
            
            this.mapset = mapset;
            cacherAgent.Request(mapset.CoverImage);
        }

        /// <summary>
        /// Hides the cover image.
        /// </summary>
        public void Reset()
        {
            this.mapset = null;
            Alpha = 0f;
            showAni.Stop();
            cacherAgent.Remove();
        }

        /// <summary>
        /// Event called from cacher agent when the map image has been loaded.
        /// </summary>
        private void OnImageLoaded(Texture2D image)
        {
            this.Texture = image;
            if (image != null)
            {
                InvokeAfterFrames(1, FillTexture);
                showAni.PlayFromStart();
            }
        }
    }
}