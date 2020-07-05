using PBGame.UI.Models.Background;
using PBGame.Maps;
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

        public override BackgroundType Type => BackgroundType.Empty;


        [InitWithDependency]
        private void Init()
        {
            mapImage = CreateChild<MapImageDisplay>("bg");
            {
                mapImage.Anchor = AnchorType.Fill;
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