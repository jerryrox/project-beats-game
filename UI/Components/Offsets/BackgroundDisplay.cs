using PBGame.UI.Models;
using PBGame.UI.Components.Common;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Offsets
{
    public class BackgroundDisplay : UguiSprite {

        private BasicTrigger closeTrigger;


        [ReceivesDependency]
        private OffsetsModel Model { get; set; }


        [InitWithDependency]
        private void Init()
        {
            SpriteName = "gradation-bottom";
            Color = Color.black;

            closeTrigger = CreateChild<BasicTrigger>("close", 0);
            {
                closeTrigger.Anchor = AnchorType.Fill;
                closeTrigger.Offset = new Offset(0f, 0f, 0f, 180f);

                closeTrigger.OnTriggered += Model.CloseOffsets;
            }
        }
    }
}