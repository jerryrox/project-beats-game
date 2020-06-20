using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.UI;
using PBFramework.Utils;
using PBFramework.Graphics;
using PBFramework.Allocation.Recyclers;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.Common.Dropdown
{
    public class DropdownMenu : UguiObject, IRecyclable<DropdownMenu> {

        /// <summary>
        /// The width of the menu container.
        /// </summary>
        public const float ContainerWidth = 200f;

        /// <summary>
        /// Amount of Y to move around during menu show/hide animations.
        /// </summary>
        private const float MoveAniAmount = -10f;

        /// <summary>
        /// Amount of padding to apply when constraining menu position.
        /// </summary>
        private const float ConstraintPadding = 10f;

        /// <summary>
        /// The size of each menu item.
        /// </summary>
        public static readonly Vector2 ItemSize = new Vector2(ContainerWidth, 40f);

        /// <summary>
        /// EVent called when this menu is hidden.
        /// </summary>
        public event Action<DropdownMenu> OnHidden;

        private IGraphicObject holder;
        private IGraphicObject aniHolder;
        private CanvasGroup canvasGroup;
        private ISprite shadow;
        private IListView listContainer;

        private IAnime showAni;
        private IAnime hideAni;

        private DropdownContext context;


        /// <summary>
        /// Returns the menu holder object.
        /// </summary>
        public IGraphicObject Holder => holder;

        public IRecycler<DropdownMenu> Recycler { get; set; }

        /// <summary>
        /// Returns whether the show or hide animation is currently playing.
        /// </summary>
        private bool IsAnimating => showAni.IsPlaying || hideAni.IsPlaying;

        [ReceivesDependency(true)]
        private IRoot Root { get; set; }


        [InitWithDependency]
        private void Init()
        {
            // Make the menu fit within the parent's object.
            Anchor = AnchorType.Fill;
            Offset = Offset.Zero;

            var blocker = CreateChild<BasicTrigger>("blocker", 0);
            {
                blocker.Anchor = AnchorType.Fill;
                blocker.Offset = Offset.Zero;

                blocker.OnTriggered += () => CloseMenu();
            }
            holder = CreateChild("holder", 1);
            {
                holder.Size = new Vector2(ContainerWidth, 0f);

                aniHolder = holder.CreateChild("ani-holder", 0);
                {
                    aniHolder.Anchor = AnchorType.Fill;
                    aniHolder.Offset = Offset.Zero;

                    canvasGroup = aniHolder.RawObject.AddComponent<CanvasGroup>();
                    canvasGroup.alpha = 0f;

                    shadow = aniHolder.CreateChild<UguiSprite>("shadow", 0);
                    {
                        shadow.Anchor = AnchorType.Fill;
                        shadow.Offset = new Offset(-7f);
                        shadow.SpriteName = "glow-circle-16";
                        shadow.ImageType = Image.Type.Sliced;
                        shadow.Color = Color.black;
                    }
                    listContainer = aniHolder.CreateChild<UguiListView>("list", 1);
                    {
                        listContainer.Anchor = AnchorType.Fill;
                        listContainer.Offset = Offset.Zero;
                        listContainer.Background.SpriteName = "circle-16";
                        listContainer.Background.ImageType = Image.Type.Sliced;
                        listContainer.Background.Color = new Color(0f, 0f, 0f, 0.75f);

                        listContainer.Initialize(OnCreateMenuItem, OnUpdateMenuItem);
                        listContainer.CellSize = ItemSize;
                        listContainer.Corner = GridLayoutGroup.Corner.UpperLeft;
                        listContainer.Axis = GridLayoutGroup.Axis.Vertical;
                    }
                }
            }

            showAni = new Anime();
            showAni.AddEvent(0f, () => Active = true);
            showAni.AnimateFloat(a => canvasGroup.alpha = a)
                .AddTime(0f, () => canvasGroup.alpha)
                .AddTime(0.25f, 1f)
                .Build();
            showAni.AnimateFloat(y => aniHolder.Y = y)
                .AddTime(0f, MoveAniAmount, EaseType.QuadEaseOut)
                .AddTime(0.25f, 0f)
                .Build();

            hideAni = new Anime();
            hideAni.AnimateFloat(a => canvasGroup.alpha = a)
                .AddTime(0f, () => canvasGroup.alpha)
                .AddTime(0.25f, 0f)
                .Build();
            hideAni.AnimateFloat(y => aniHolder.Y = y)
                .AddTime(0f, 0f, EaseType.QuadEaseOut)
                .AddTime(0.25f, MoveAniAmount)
                .Build();
            hideAni.AddEvent(hideAni.Duration, () =>
            {
                OnHidden?.Invoke(this);
                Active = false;
            });
        }

        /// <summary>
        /// Sets up the dropdown menu for specified context and makes the menu visible.
        /// </summary>
        public void OpenMenu(DropdownContext context)
        {
            if(Active || IsAnimating)
                return;

            this.context = context;

            // Clamp menu height so it doesn't go over half of screen height.
            holder.Height = Mathf.Min(ItemSize.y * context.Datas.Count, GetMaxHolderHeight());

            listContainer.TotalItems = context.Datas.Count;

            showAni.PlayFromStart();
        }

        /// <summary>
        /// Positions the menu at the specified position as best possible within boundary.
        /// </summary>
        public void PositionMenu(Vector3 position, Space space)
        {
            // We must work in local position.
            if (space == Space.World)
                position = transform.InverseTransformPoint(position);
            // Set processed position.
            holder.Position = ConstrainMenuBounds(position);
        }

        /// <summary>
        /// Closes this dropdown menu.
        /// </summary>
        public void CloseMenu()
        {
            if(!Active || IsAnimating)
                return;

            listContainer.TotalItems = 0;
            context = null;

            hideAni.PlayFromStart();
        }

        void IRecyclable.OnRecycleNew() {}

        void IRecyclable.OnRecycleDestroy() {}

        /// <summary>
        /// Calculates the max height of the menu holder.
        /// </summary>
        private float GetMaxHolderHeight() => Root != null ? Root.Resolution.y / 2f : float.MaxValue;

        /// <summary>
        /// Makes sure the menu does not go outside of the view boundary.
        /// </summary>
        private Vector2 ConstrainMenuBounds(Vector2 position)
        {
            // Get corners of the menu holder.
            Vector2 topLeft = new Vector2(
                position.x - ContainerWidth * 0.5f,
                position.y + holder.Height * 0.5f
            );
            Vector2 bottomRight = new Vector2(
                position.x + ContainerWidth * 0.5f,
                position.y - holder.Height * 0.5f
            );

            // Get boundary of the menu position domain.
            Vector2 topLeftBound = new Vector2(
                -Width * 0.5f + ConstraintPadding,
                Height * 0.5f - ConstraintPadding
            );
            Vector2 bottomRightBound = new Vector2(
                Width * 0.5f - ConstraintPadding,
                -Height * 0.5f + ConstraintPadding
            );

            position.x += Mathf.Max(topLeftBound.x - topLeft.x, 0f);
            position.y -= Mathf.Max(topLeft.y - topLeftBound.y, 0f);
            position.x -= Mathf.Max(bottomRight.x - bottomRightBound.x, 0f);
            position.y += Mathf.Max(bottomRightBound.y - bottomRight.y, 0f);
            return position;
        }

        /// <summary>
        /// Event called from menu item when triggered.
        /// </summary>
        private void OnSelectedItem(DropdownMenuItem item)
        {
            if(item == null || item.Data == null || IsAnimating)
                return;

            // Make this item's data selected on the dropdown context.
            if (context != null)
                context.SelectData(item.Data);

            // Hide menu.
            CloseMenu();
        }

        /// <summary>
        /// Event called when a new menu item needs to be created.
        /// </summary>
        private IListItem OnCreateMenuItem()
        {
            var item = listContainer.Container.CreateChild<DropdownMenuItem>("item", 0);
            item.Size = ItemSize;
            item.OnTriggered += () => OnSelectedItem(item);
            return item;
        }

        /// <summary>
        /// Event called on menu item update due to listview.
        /// </summary>
        private void OnUpdateMenuItem(IListItem item)
        {
            DropdownMenuItem menuItem = item as DropdownMenuItem;
            var itemData = context.Datas[item.ItemIndex];
            menuItem.Setup(itemData, context.Selection == itemData);
        }
    }
}