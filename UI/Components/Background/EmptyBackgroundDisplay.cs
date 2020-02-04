using PBGame.Maps;
using PBFramework.Graphics;
using UnityEngine;

namespace PBGame.UI.Components.Background
{
    public class EmptyBackgroundDisplay : UguiObject, IBackgroundDisplay {

        public Color Color { get; set; }

        public float Alpha { get; set; }


        public void MountBackground(IMapBackground background)
        {
            // Nothing to do!
        }

        public void ToggleDisplay(bool enable) => Active = enable;
    }
}