using PBGame.Assets.Caching;
using PBFramework.UI;
using PBFramework.Allocation.Caching;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Common
{
    public class WebTexture : UguiTexture {

        private CanvasGroup canvasGroup;

        private CacherAgent<string, Texture2D> cacherAgent;

        private IAnime showAni;


        /// <summary>
        /// Whether the cacher agent unload should be done with delay.
        /// </summary>
        public bool IsDelayedUnload
        {
            get => cacherAgent.UseDelayedRemove;
            set => cacherAgent.UseDelayedRemove = value;
        }

        [ReceivesDependency]
        protected IWebImageCacher WebImageCacher { get; set; }


        [InitWithDependency]
        private void Init()
        {
            canvasGroup = myObject.AddComponent<CanvasGroup>();

            cacherAgent = new CacherAgent<string, Texture2D>(WebImageCacher);
            cacherAgent.RemoveDelay = 2f;
            cacherAgent.OnFinished += OnImageLoaded;
            cacherAgent.OnProgress += OnLoadProgress;

            canvasGroup.alpha = 0f;
            this.IsDelayedUnload = true;

            showAni = new Anime();
            showAni.AnimateFloat(a => canvasGroup.alpha = a)
                .AddTime(0f, () => canvasGroup.alpha)
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
            canvasGroup.alpha = 0f;
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