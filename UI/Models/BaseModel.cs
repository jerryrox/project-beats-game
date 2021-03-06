using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.Threading;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Models
{
    public abstract class BaseModel : IModel {

        private bool? isTestEnvironment;

        private Coroutine updateCoroutine;


        /// <summary>
        /// Returns whether the model is currently in test environment.
        /// </summary>
        public bool IsTestMode
        {
            get
            {
                if(!isTestEnvironment.HasValue)
                    isTestEnvironment = Dependency.Get<IGame>().IsTestMode;
                return isTestEnvironment.Value;
            }
        }

        [ReceivesDependency]
        protected IDependencyContainer Dependency { get; set; }


        /// <summary>
        /// Handles pre-show event called from the host view.
        /// </summary>
        protected virtual void OnPreShow() { }
        void IModel.OnPreShow() => OnPreShow();

        /// <summary>
        /// Handles post-show event called from the host view.
        /// </summary>
        protected virtual void OnPostShow() { }
        void IModel.OnPostShow() => OnPostShow();

        /// <summary>
        /// Handles pre-hide event called from the host view.
        /// </summary>
        protected virtual void OnPreHide() { }
        void IModel.OnPreHide() => OnPreHide();

        /// <summary>
        /// Handles post-hide event called from the host view.
        /// </summary>
        protected virtual void OnPostHide()
        {
            StopUpdate();
        }
        void IModel.OnPostHide() => OnPostHide();

        /// <summary>
        /// Processes update routine logic.
        /// </summary>
        protected virtual void Update() { }

        /// <summary>
        /// Starts the update routine.
        /// </summary>
        protected void StartUpdate()
        {
            if(updateCoroutine != null)
                return;

            updateCoroutine = UnityThread.StartCoroutine(UpdateRoutine());
        }

        /// <summary>
        /// Stops the update routine.
        /// Automatically stopped when OnPostHide is called too.
        /// </summary>
        protected void StopUpdate()
        {
            if(updateCoroutine == null)
                return;

            UnityThread.StopCoroutine(updateCoroutine);
            updateCoroutine = null;
        }

        /// <summary>
        /// Handles update routine.
        /// </summary>
        private IEnumerator UpdateRoutine()
        {
            while (true)
            {
                yield return null;
                Update();
            }
        }
    }
}