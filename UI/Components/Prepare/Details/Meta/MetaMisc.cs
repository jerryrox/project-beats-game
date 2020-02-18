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
    public class MetaMisc : UguiSprite, IMetaMisc {

        private IMetaMiscEntry source;
        private IMetaMiscEntry tags;


        [ReceivesDependency]
        private IMapSelection MapSelection { get; set; }


        [InitWithDependency]
        private void Init()
        {
            Color = new Color(1f, 1f, 1f, 0.0625f);

            source = CreateChild<MetaMiscEntry>("source", 0);
            {
                source.Anchor = Anchors.TopStretch;
                source.Pivot = Pivots.Top;
                source.OffsetLeft = source.OffsetRight = 0f;
                source.Y = 0f;
                source.Height = 120f;

                source.LabelText = "Source";
            }
            tags = CreateChild<MetaMiscEntry>("tags", 1);
            {
                tags.Anchor = Anchors.Fill;
                tags.OffsetLeft = tags.OffsetRight = tags.OffsetBottom = 0f;
                tags.OffsetTop = 120f;

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
            MapSelection.OnMapChange += OnMapChange;
            OnMapChange(MapSelection.Map);
        }
        
        /// <summary>
        /// Unbinds from external dependency events.
        /// </summary>
        private void UnbindEvents()
        {
            MapSelection.OnMapChange -= OnMapChange;
        }

        /// <summary>
        /// Event called on map change.
        /// </summary>
        private void OnMapChange(IMap map)
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