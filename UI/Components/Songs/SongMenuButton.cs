using PBGame.UI.Components.Common;
using PBFramework.UI;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Songs
{
    public class SongMenuButton : HoverableTrigger, IHasIcon {

        private ISprite iconSprite;


        public string IconName
        {
            get => iconSprite.SpriteName;
            set => iconSprite.SpriteName = value;
        }


        [InitWithDependency]
        private void Init()
        {
            iconSprite = CreateChild<UguiSprite>("icon", 10);
            {
                iconSprite.Size = new Vector2(36f, 36f);
                iconSprite.Alpha = 0.65f;
            }

            UseDefaultHoverAni();
        }
    }
}