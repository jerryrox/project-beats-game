using System.Collections.Generic;
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


        /// <summary>
        /// Returns the current mapset selected.
        /// </summary>
        private IMapset CurMapset => MapSelection.Mapset.Value;

        /// <summary>
        /// Returns the list of original maps in current mapset.
        /// </summary>
        private List<IOriginalMap> MapList => CurMapset == null ? null : CurMapset.Maps;

        [ReceivesDependency]
        private IMapSelection MapSelection { get; set; }

        [ReceivesDependency]
        private IGameConfiguration GameConfiguration { get; set; }


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
            var timer = new SynchronizedTimer() { Limit = 0f, WaitFrameOnStart = true };
            timer.OnFinished += delegate { OnEnableInited(); };
            timer.Start();
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
            MapSelection.Mapset.OnNewValue += OnMapsetChange;
            GameConfiguration.RulesetMode.OnValueChanged += OnModeChange;

            RefreshList();
        }
        
        /// <summary>
        /// Unbinds from external dependency events.
        /// </summary>
        private void UnbindEvents()
        {
            MapSelection.Mapset.OnNewValue -= OnMapsetChange;
            GameConfiguration.RulesetMode.OnValueChanged -= OnModeChange;
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
            var maps = MapList;
            var cell = item as VersionButton;
            var gameMode = GameConfiguration.RulesetMode.Value;

            cell.Setup(maps[cell.ItemIndex].GetPlayable(gameMode));
        }

        /// <summary>
        /// Refreshes the list items.
        /// </summary>
        private void RefreshList()
        {
            // Sort the maps list.
            if(CurMapset != null)
                CurMapset.SortMapsByMode(GameConfiguration.RulesetMode.Value);

            versionList.TotalItems = MapList.Count;
            versionList.ForceUpdate();
        }

        /// <summary>
        /// Event called on mapset selection change.
        /// </summary>
        private void OnMapsetChange(IMapset mapset) => RefreshList();

        /// <summary>
        /// Event called on game mode configuration change.
        /// </summary>
        private void OnModeChange(GameModeType gameMode, GameModeType _ = GameModeType.BeatsStandard) => RefreshList();
    }
}