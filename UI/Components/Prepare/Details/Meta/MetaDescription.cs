using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Maps;
using PBGame.Rulesets.Maps;
using PBGame.Networking.API;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBGame.UI.Components.Prepare.Details.Meta
{
    public class MetaDescription : UguiObject {

        private ILabel label;
        private IScrollView scrollView;
        private ILabel contentLabel;


        [ReceivesDependency]
        private IMapSelection MapSelection { get; set; }

        [ReceivesDependency]
        private IApiManager ApiManager { get; set; }


        [InitWithDependency]
        private void Init()
        {
            label = CreateChild<Label>("label", 0);
            {
                label.Anchor = AnchorType.TopLeft;
                label.Pivot = PivotType.TopLeft;
                label.X = 32f;
                label.Y = -32f;
                label.IsBold = true;
                label.FontSize = 18;
                label.Alignment = TextAnchor.UpperLeft;

                label.Text = "Description";
            }
            scrollView = CreateChild<UguiScrollView>("scrollview", 1);
            {
                scrollView.Anchor = AnchorType.Fill;
                scrollView.Offset = new Offset(32f, 64f, 32f, 32f);

                scrollView.Background.Alpha = 0f;

                contentLabel = scrollView.Container.CreateChild<Label>("content", 0);
                {
                    contentLabel.Anchor = AnchorType.Fill;
                    contentLabel.RawSize = Vector2.zero;
                    contentLabel.FontSize = 16;
                    contentLabel.Alignment = TextAnchor.UpperLeft;
                }
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
        /// Sets the content text on the label.
        /// </summary>
        private void SetContent(string content)
        {
            contentLabel.Text = content;
            scrollView.Container.Height = contentLabel.PreferredHeight;
            scrollView.ResetPosition();
        }

        /// <summary>
        /// Event called on map selection change.
        /// </summary>
        private void OnMapChange(IPlayableMap map)
        {
            if (map == null)
            {
                SetContent("");
            }
            else
            {
                SetContent("");
                // TODO: Fetch map description from server. There is currently no easy way to do this, so I'll do this some other time.
                // var api = ApiManager.GetRelevantApi(map);
                // if (api == null)
                // {
                //     SetContent("");
                // }
                // else
                // {
                //     // api.Request();
                // }
            }
        }
    }
}