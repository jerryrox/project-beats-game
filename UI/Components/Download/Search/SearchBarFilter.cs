using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.Common;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.Download.Search
{
    public class SearchBarFilter : BaseSearchFilter {

        private BasicInput input;


        [ReceivesDependency]
        private DownloadState State { get; set; }


        [InitWithDependency]
        private void Init()
        {
            label.Text = "Search";

            input = CreateChild<BasicInput>("input", 1);
            {
                input.Anchor = AnchorType.Fill;
                input.Offset = new Offset(0f, 24f, 0f, 0f);
                input.Background.Color = new Color(1f, 1f, 1f, 0.25f);
                input.CreateIconSprite(spriteName: "icon-search", size: 24f);

                input.OnSubmitted += (value) =>
                {
                    value = value.Trim();
                    if (!string.IsNullOrEmpty(value) || value != State.SearchTerm.Value)
                        State.SearchTerm.Value = value;
                };
            }
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();
            State.SearchTerm.BindAndTrigger(OnSearchTermChange);
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            State.SearchTerm.OnValueChanged -= OnSearchTermChange;
        }

        /// <summary>
        /// Event called on search term change.
        /// </summary>
        private void OnSearchTermChange(string term, string _)
        {
            input.Text = term;
        }
    }
}