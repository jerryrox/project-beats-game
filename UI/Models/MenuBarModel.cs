using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Models.MenuBar;
using PBGame.UI.Navigations.Screens;
using PBFramework.UI;
using PBFramework.UI.Navigations;
using PBFramework.Data.Bindables;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Models
{
    public class MenuBarModel : BaseModel {

        private Bindable<MenuType> focusedMenu = new Bindable<MenuType>(MenuType.None);


        /// <summary>
        /// Returns the menu currently focused.
        /// </summary>
        public IReadOnlyBindable<MenuType> FocusedMenu => focusedMenu;

        [ReceivesDependency]
        private IScreenNavigator ScreenNavigator { get; set; }


        /// <summary>
        /// Sets the current menu type.
        /// </summary>
        public void SetMenu(MenuType type)
        {
            focusedMenu.Value = type;
        }

        protected override void OnPreShow()
        {
            base.OnPreShow();
            focusedMenu.Value = MenuType.None;
        }
    }
}