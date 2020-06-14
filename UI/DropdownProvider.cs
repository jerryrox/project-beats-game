using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.Common.Dropdown;
using PBGame.Graphics;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Allocation.Recyclers;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI
{
    public class DropdownProvider : IDropdownProvider {

        private ManagedRecycler<DropdownMenu> recycler;

        private IRootMain rootMain;


        public DropdownProvider(IRootMain rootMain)
        {
            if(rootMain == null)
                throw new ArgumentNullException(nameof(rootMain));
                
            this.rootMain = rootMain;
            recycler = new ManagedRecycler<DropdownMenu>(CreateMenu);
        }

        public DropdownMenu Open(DropdownContext context) => OpenInternal(context);

        public DropdownMenu OpenAt(DropdownContext context, Vector2 worldPos)
        {
            DropdownMenu menu = OpenInternal(context);
            menu.PositionMenu(worldPos, Space.World);
            return menu;
        }

        /// <summary>
        /// Internally handles opening of a dropdown menu.
        /// </summary>
        private DropdownMenu OpenInternal(DropdownContext context)
        {
            var menu = recycler.GetNext();
            menu.OpenMenu(context);
            int depth = GetNextMenuDepth();
            menu.Depth = depth;
            var canvas = menu.GetComponent<Canvas>();
            canvas.sortingOrder = depth;
            return menu;
        }

        /// <summary>
        /// Return the depth for the next menu to be shown.
        /// </summary>
        private int GetNextMenuDepth() => DepthPresets.DropdownPopup + recycler.ActiveCount;

        /// <summary>
        /// Creates a new dropdown menu.
        /// </summary>
        private DropdownMenu CreateMenu()
        {
            DropdownMenu menu = rootMain.CreateChild<DropdownMenu>("menu", DepthPresets.DropdownPopup);
            menu.OnHidden += recycler.Return;
            menu.Active = false;

            var dropdownCanvas = menu.Canvas = menu.RawObject.AddComponent<Canvas>();
            dropdownCanvas.overrideSorting = true;
            dropdownCanvas.sortingOrder = DepthPresets.DropdownPopup;

            var raycaster = menu.RawObject.AddComponent<GraphicRaycaster>();
            raycaster.ignoreReversedGraphics = true;
            return menu;
        }
    }
}