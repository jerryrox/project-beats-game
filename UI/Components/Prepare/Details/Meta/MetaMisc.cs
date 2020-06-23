using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Maps;
using PBGame.Rulesets.Maps;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Prepare.Details.Meta
{
    public class MetaMisc : UguiSprite {

        private MetaMiscEntry source;
        private MetaMiscEntry tags;


        [ReceivesDependency]
        private IMapSelection MapSelection { get; set; }


        [InitWithDependency]
        private void Init()
        {
            Color = new Color(1f, 1f, 1f, 0.0625f);

            source = CreateChild<MetaMiscEntry>("source", 0);
            {
                source.Anchor = AnchorType.TopStretch;
                source.Pivot = PivotType.Top;
                source.SetOffsetHorizontal(0f);
                source.Y = 0f;
                source.Height = 120f;

                source.LabelText = "Source";
            }
            tags = CreateChild<MetaMiscEntry>("tags", 1);
            {
                tags.Anchor = AnchorType.Fill;
                tags.Offset = new Offset(0f, 120f, 0f, 0f);

                tags.LabelText = "Tags";
            }

            OnEnableInited();
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();
            BindEvents();
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            UnbindEvents();
        }

        /// <summary>
        /// Binds to external dependency events.
        /// </summary>
        private void BindEvents()
        {
            MapSelection.Map.BindAndTrigger(OnMapChange);
        }
        
        /// <summary>
        /// Unbinds from external dependency events.
        /// </summary>
        private void UnbindEvents()
        {
            MapSelection.Map.OnNewValue -= OnMapChange;
        }

        /// <summary>
        /// Event called on map change.
        /// </summary>
        private void OnMapChange(IPlayableMap map)
        {
            if (map == null)
            {
                source.Content = "";
                tags.Content = "";
            }
            else
            {
                source.Content = map.Metadata.Source;
                tags.Content = map.Metadata.Tags;
            }
        }
    }
}