using PBGame.UI.Components.Common;
using PBGame.Networking.Maps;
using PBFramework.Threading;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Download.Result
{
    public class CoverImage : WebTexture {

        private OnlineMapset mapset;
        private SynchronizedTimer loadDelay;


        [InitWithDependency]
        private void Init()
        {
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
            if(mapset == null)
                return;

            // Load the image after delay to reduce performance implications when scrolling quickly.
            loadDelay = new SynchronizedTimer()
            {
                Limit = 1f
            };
            loadDelay.OnFinished += () =>
            {
                loadDelay = null;
                Load(mapset.CardImage);
            };
        }

        /// <summary>
        /// Hides the cover image.
        /// </summary>
        public void Reset()
        {
            this.mapset = null;
            Unload();

            if (loadDelay != null)
            {
                loadDelay.Stop();
                loadDelay = null;
            }
        }
    }
}