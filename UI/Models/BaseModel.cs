using System;
using System.Collections;
using System.Collections.Generic;

namespace PBGame.UI.Models
{
    public abstract class BaseModel : IModel {
    
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
        protected virtual void OnPostHide() { }
        void IModel.OnPostHide() => OnPostHide();
    }
}