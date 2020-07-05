using System.Collections.Generic;
using PBGame.UI.Models;
using PBGame.UI.Navigations.Screens;
using PBGame.Maps;
using PBGame.Rulesets;
using PBGame.Rulesets.Maps;
using PBGame.Configurations;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Threading;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.Prepare
{
    public class VersionContainer : UguiObject {

        private ISprite gradient;
        private IGraphicObject listContainer;
        private IListView versionList;


        [ReceivesDependency]
        private PrepareModel Model { get; set; }


        [InitWithDependency]
        private void Init(IPrepareScreen prepareScreen)
        {
            gradient = CreateChild<UguiSprite>("gradient", 0);
            {
                gradient.Anchor = AnchorType.Fill;
                gradient.RawSize = Vector2.zero;
                gradient.SpriteName = "gradation-top";
                gradient.Color = new Color(0f, 0f, 0f, 0.75f);
            }
            listContainer = CreateChild<UguiObject>("list-container", 1);
            {
                listContainer.Anchor = AnchorType.TopStretch;
                listContainer.Pivot = PivotType.Top;
                listContainer.RawWidth = 0f;
                listContainer.Height = 64f;
                listContainer.Y = -(prepareScreen as PrepareScreen).MenuBarHeight;

                versionList = listContainer.CreateChild<UguiListView>("version-list", 0);
                {
                    versionList.Anchor = AnchorType.Fill;
                    versionList.RawSize = new Vector2(-64f, 0f);
                    versionList.SetOffsetVertical(0f);

                    versionList.Background.Alpha = 0f;
                    versionList.UseMask = false;
                    versionList.IsVertical = false;

                    versionList.Initialize(CreateVersionCell, SetupVersionCell);
                    versionList.Axis = GridLayoutGroup.Axis.Horizontal;
                    versionList.CellSize = new Vector2(64f, 64f);
                }
            }

            // Call after a frame due to unity ui limitations.
            InvokeAfterFrames(1, OnEnableInited);
        }

        protected override void OnEnableInited()
        {
            base.OnEnableInited();

            Model.MapList.BindAndTrigger(OnMapListChange);
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();

            Model.MapList.OnNewValue -= OnMapListChange;
        }

        /// <summary>
        /// Creates a new version cell for the version list.
        /// </summary>
        private IListItem CreateVersionCell()
        {
            int index = versionList.Container.RawTransform.childCount;
            var cell = versionList.Container.CreateChild<VersionButton>($"cell{index}", index);
            cell.Size = versionList.CellSize;
            return cell;
        }

        /// <summary>
        /// Initializes the specified list item.
        /// </summary>
        private void SetupVersionCell(IListItem item)
        {
            var maps = Model.MapList.Value;
            var cell = item as VersionButton;
            var gameMode = Model.GameMode.Value;

            cell.Setup(maps[cell.ItemIndex].GetPlayable(gameMode));
        }

        /// <summary>
        /// Refreshes the list items.
        /// </summary>
        private void RefreshList()
        {
            versionList.TotalItems = Model.MapList.Value.Count;
            versionList.ForceUpdate();
        }

        /// <summary>
        /// Event called on map list change.
        /// </summary>
        private void OnMapListChange(List<IOriginalMap> maps) => RefreshList();
    }
}