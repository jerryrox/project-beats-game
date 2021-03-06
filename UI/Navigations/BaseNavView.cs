using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Models;
using PBGame.UI.Navigations.Overlays;
using PBGame.Graphics;
using PBFramework;
using PBFramework.UI;
using PBFramework.UI.Navigations;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Navigations
{
    public abstract class BaseNavView<TModel> : UguiNavigationView, INavigationView
        where TModel : class, IModel, new()
    {
        protected TModel model;

        private float? menuBarHeight = null;


        /// <summary>
        /// Returns the model of the view.
        /// </summary>
        public TModel Model => model;

        /// <summary>
        /// Returns the height of the menu bar overlay's container.
        /// </summary>
        public float MenuBarHeight
        {
            get
            {
                if(menuBarHeight.HasValue)
                    return menuBarHeight.Value;
                var overlay = OverlayNavigator?.Get<MenuBarOverlay>();
                if(overlay == null)
                    return 0f;
                return (menuBarHeight = overlay.ContainerHeight).Value;
            }
        }

        public override HideActionType HideAction => HideActionType.Recycle;

        /// <summary>
        /// Returns whether the screen should be displayed on 3D root.
        /// </summary>
        protected virtual bool IsRoot3D => false;

        /// <summary>
        /// Returns the depth of the navigation view.
        /// </summary>
        protected abstract int ViewDepth { get; }

        [ReceivesDependency]
        private IOverlayNavigator OverlayNavigator { get; set; }


        [InitWithDependency]
        private void Init(IRoot3D root3D)
        {
            Dependencies = Dependencies.Clone();

            // Cache this navigation view.
            Dependencies.Cache(this);

            if (IsRoot3D)
            {
                SetParent(root3D);
                myTransform.ResetTransform();
            }

            model = CreateModel() ?? new TModel();
            Dependencies.CacheAndInject(model);

            Depth = ViewDepth;
        }

        /// <summary>
        /// Manually creates a new navigation view model.
        /// </summary>
        protected virtual TModel CreateModel() => null;

        protected override void OnPreShow()
        {
            base.OnPreShow();
            model.OnPreShow();
        }

        protected override void OnPostShow()
        {
            base.OnPostShow();
            model.OnPostShow();
        }

        protected override void OnPreHide()
        {
            base.OnPreHide();
            model.OnPreHide();
        }

        protected override void OnPostHide()
        {
            base.OnPostHide();
            model.OnPostHide();
        }
    }
}