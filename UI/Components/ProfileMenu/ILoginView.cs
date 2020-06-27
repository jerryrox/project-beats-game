using PBGame.Networking.API;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.ProfileMenu
{
    public interface ILoginView : IHasAlpha {

        /// <summary>
        /// The desired height of this view required to fully display all components.
        /// </summary>
        float DesiredHeight { get; }


        /// <summary>
        /// Sets up the view for logging in with credentials for the specified API provider.
        /// </summary>
        void Setup(IApi api);

        /// <summary>
        /// Starts showing this login view.
        /// </summary>
        void Show();

        /// <summary>
        /// Starts hiding this login view.
        /// </summary>
        void Hide();
    }
}