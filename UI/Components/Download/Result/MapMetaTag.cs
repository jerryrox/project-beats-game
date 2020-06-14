using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using PBGame.Rulesets;
using PBGame.Networking.Maps;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Allocation.Recyclers;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.Download.Result
{
    public class MapMetaTag : BaseMetaTag, IRecyclable<MapMetaTag> {

        private ISprite iconSprite;


        public IRecycler<MapMetaTag> Recycler { get; set; }

        [ReceivesDependency]
        private IModeManager ModeManager { get; set; }


        [InitWithDependency]
        private void Init()
        {
            iconSprite = CreateChild<UguiSprite>("icon", 1);
            {
                iconSprite.Anchor = AnchorType.Left;
                iconSprite.Pivot = PivotType.Left;
                iconSprite.Size = new Vector2(20f, 20f);
                iconSprite.X = 8f;
                iconSprite.Color = Color.black;
            }

            label.SetOffsetHorizontal(8 + iconSprite.Width + 4, 8);
        }

        /// <summary>
        /// Sets the count display for specified game mode.
        /// </summary>
        public void SetMapCount(GameModeType gameMode, int count)
        {
            var service = ModeManager.GetService(gameMode);
            if (service == null)
            {
                Debug.LogWarning($"MapMetaTag.SetMapset - Unsupported gameMode: {gameMode}");
                return;
            }

            iconSprite.SpriteName = service.GetIconName(32);
            label.Text = count.ToString("N0");
        }

        public void OnRecycleNew() => Active = true;

        public void OnRecycleDestroy() => Active = false;
    }
}