using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Data.Users;
using PBFramework.UI;
using PBFramework.Data.Bindables;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Models
{
    public class ProfileMenuModel : BaseModel {


        /// <summary>
        /// Returns the current user profile.
        /// </summary>
        public IReadOnlyBindable<IUser> CurrentUser => UserManager.CurrentUser;

        [ReceivesDependency]
        private IUserManager UserManager { get; set; }
    }
}