using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Maps;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Background
{
    public class ImageBackgroundDisplay : BaseBackgroundDisplay, IBackgroundDisplay {

        private MapImageDisplay mapImage;


        public override Color Color
        {
            get => mapImage.Color;
            set => mapImage.Color = value;
        }


        [InitWithDependency]
        private void Init()
        {
            mapImage = CreateChild<MapImageDisplay>("bg");
            {
                mapImage.Anchor = Anchors.Fill;
                mapImage.RawSize = Vector2.zero;
            }
        }

        public override void MountBackground(IMapBackground background)
        {
            base.MountBackground(background);

            mapImage.SetBackground(background);
        }
    }
}