using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Components.Common;
using PBGame.UI.Components.Download.Search;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.Download
{
    public class SearchMenu : UguiObject {

        private const float ProviderControllerHeight = 54f;
        private const float SearchBarHeight = 96f;

        private UguiObject container;
        private ProviderContainer providerContainer;
        private BannerContainer bannerContainer;
        private SearchBarContainer searchBarContainer;
        private ShadowButton shadowButton;

        private bool isFolded = true;

        private IAnime foldAni;
        private IAnime unfoldAni;


        /// <summary>
        /// Returns the height of the search menu on folded state.
        /// </summary>
        public float FoldedHeight => ProviderControllerHeight + SearchBarHeight;

        /// <summary>
        /// Returns the height of the search menu on unfolded state.
        /// </summary>
        public float UnfoldedHeight => 360f;


        [InitWithDependency]
        private void Init()
        {
            container = CreateChild<UguiObject>("container", 0);
            {
                container.Anchor = AnchorType.TopStretch;
                container.Pivot = PivotType.Top;
                container.Y = 0f;
                container.SetOffsetHorizontal(0f);
                container.Height = FoldedHeight;

                providerContainer = container.CreateChild<ProviderContainer>("provider", 1);
                {
                    providerContainer.Anchor = AnchorType.TopStretch;
                    providerContainer.Pivot = PivotType.Top;
                    providerContainer.SetOffsetHorizontal(0f);
                    providerContainer.Y = 0f;
                    providerContainer.Height = ProviderControllerHeight;
                }
                bannerContainer = container.CreateChild<BannerContainer>("banner", 0);
                {
                    bannerContainer.Anchor = AnchorType.Fill;
                    bannerContainer.Offset = new Offset(0f, 54f, 0f, 96f);
                    bannerContainer.IsInteractible = false;
                }
                searchBarContainer = container.CreateChild<SearchBarContainer>("search-bar", 2);
                {
                    searchBarContainer.Anchor = AnchorType.BottomStretch;
                    searchBarContainer.Pivot = PivotType.Bottom;
                    searchBarContainer.SetOffsetHorizontal(0f);
                    searchBarContainer.Y = 0f;
                    searchBarContainer.Height = SearchBarHeight;

                    searchBarContainer.AdvancedButton.OnTriggered += OnAdvancedButton;
                }
            }
            shadowButton = CreateChild<ShadowButton>("shadow", 1);
            {
                shadowButton.Anchor = AnchorType.Fill;
                shadowButton.Offset = new Offset(0f, FoldedHeight, 0f, 0f);
                shadowButton.Active = false;
                shadowButton.Alpha = 0f;

                shadowButton.OnTriggered += OnShadowButton;
            }

            foldAni = new Anime();
            foldAni.AddEvent(0f, () => bannerContainer.IsInteractible = false);
            foldAni.AnimateFloat(h => 
            {
                container.Height = h;
                shadowButton.SetOffsetTop(h);
                bannerContainer.AdjustBannerTexture();
            })
                .AddTime(0f, () => container.Height)
                .AddTime(0.25f, FoldedHeight)
                .Build();
            foldAni.AnimateFloat(a => shadowButton.Alpha = a)
                .AddTime(0f, () => shadowButton.Alpha)
                .AddTime(0.25f, 0f)
                .Build();
            foldAni.AddEvent(0f, () => shadowButton.Active = false);

            unfoldAni = new Anime();
            unfoldAni.AddEvent(0f, () => shadowButton.Active = true);
            unfoldAni.AnimateFloat(h =>
            {
                container.Height = h;
                shadowButton.SetOffsetTop(h);
                bannerContainer.AdjustBannerTexture();
            })
                .AddTime(0f, () => container.Height)
                .AddTime(0.25f, UnfoldedHeight)
                .Build();
            unfoldAni.AnimateFloat(a => shadowButton.Alpha = a)
                .AddTime(0f, () => shadowButton.Alpha)
                .AddTime(0.25f, 0.5f)
                .Build();
            unfoldAni.AddEvent(unfoldAni.Duration, () => bannerContainer.IsInteractible = true);
        }

        /// <summary>
        /// Sets folded state on the menu.
        /// </summary>
        public void SetFold(bool isFolded)
        {
            if(this.isFolded == isFolded)
                return;

            this.isFolded = isFolded;

            foldAni.Stop();
            unfoldAni.Stop();

            if(isFolded)
                foldAni.PlayFromStart();
            else
                unfoldAni.PlayFromStart();
        }

        /// <summary>
        /// Event called on triggering advanced button.
        /// </summary>
        private void OnAdvancedButton() => SetFold(!isFolded);

        /// <summary>
        /// Event called on triggering shadow button.
        /// </summary>
        private void OnShadowButton() => SetFold(true);
    }
}