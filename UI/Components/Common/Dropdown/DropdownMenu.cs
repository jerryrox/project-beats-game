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
    public class DropdownMenu : UguiObject {

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

        private IGraphicObject holder;
        private IGraphicObject aniHolder;
        private CanvasGroup canvasGroup;
        private ISprite shadow;
        private ISprite container;
        private IGrid grid;

        private IAnime showAni;
        private IAnime hideAni;

        private Recycler<DropdownMenuItem> itemRecycler;
        private List<DropdownMenuItem> activeItems = new List<DropdownMenuItem>();

        private DropdownContext context;


        /// <summary>
        /// Returns the size of the menu items holder.
        /// </summary>
        public Vector2 HolderSize => holder.Size;

        /// <summary>
        /// Returns whether the show or hide animation is currently playing.
        /// </summary>
        private bool IsAnimating => showAni.IsPlaying || hideAni.IsPlaying;


        [InitWithDependency]
        private void Init()
        {
            itemRecycler = new Recycler<DropdownMenuItem>(CreateMenuItem);

            // Make the menu fit within the parent's object.
            Anchor = Anchors.Fill;
            Offset = Offset.Zero;

            var blocker = CreateChild<BasicTrigger>("blocker", 0);
            {
                blocker.Anchor = Anchors.Fill;
                blocker.Offset = Offset.Zero;

                blocker.OnTriggered += () => CloseMenu();
            }
            holder = CreateChild("holder", 1);
            {
                holder.Size = new Vector2(ContainerWidth, 0f);

                aniHolder = holder.CreateChild("ani-holder", 0);
                {
                    aniHolder.Anchor = Anchors.Fill;
                    aniHolder.Offset = Offset.Zero;

                    canvasGroup = aniHolder.RawObject.AddComponent<CanvasGroup>();
                    canvasGroup.alpha = 0f;

                    shadow = aniHolder.CreateChild<UguiSprite>("shadow", 0);
                    {
                        shadow.Anchor = Anchors.Fill;
                        shadow.Offset = new Offset(-7f);
                        shadow.SpriteName = "glow-circle-16";
                        shadow.ImageType = Image.Type.Sliced;
                        shadow.Color = Color.black;
                    }
                    container = aniHolder.CreateChild<UguiSprite>("container", 1);
                    {
                        container.Anchor = Anchors.Fill;
                        container.Offset = Offset.Zero;
                        container.SpriteName = "circle-16";
                        container.ImageType = Image.Type.Sliced;
                        container.Color = new Color(0f, 0f, 0f, 0.75f);

                        grid = container.CreateChild<UguiGrid>("grid", 0);
                        {
                            grid.Anchor = Anchors.Fill;
                            grid.Offset = Offset.Zero;
                            grid.CellSize = ItemSize;
                            grid.Axis = GridLayoutGroup.Axis.Vertical;
                        }
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
            hideAni.AddEvent(hideAni.Duration, () => Active = false);
        }

        /// <summary>
        /// Sets up the dropdown menu for specified context and makes the menu visible.
        /// </summary>
        public void OpenMenu(DropdownContext context)
        {
            if(Active || IsAnimating)
                return;

            this.context = context;

            // Show menu items and resize menu.
            foreach (var data in context.Datas)
            {
                var item = itemRecycler.GetNext();
                item.Setup(data, data == context.Selection);
                item.Depth = activeItems.Count;
                activeItems.Add(item);
            }
            holder.Height = ItemSize.y * activeItems.Count;

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

            // Remove all menu items.
            for (int i = 0; i < activeItems.Count; i++)
                itemRecycler.Return(activeItems[i]);
            activeItems.Clear();

            context = null;

            hideAni.PlayFromStart();
        }

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

            // Change focus so the user can see a glimpse of focus change animation on the items.
            for (int i = 0; i < activeItems.Count; i++)
                activeItems[i].IsFocused = activeItems[i] == item;

            // Hide menu.
            CloseMenu();
        }

        /// <summary>
        /// Creates and returna new dropdown menu item.
        /// </summary>
        private DropdownMenuItem CreateMenuItem()
        {
            var item = grid.CreateChild<DropdownMenuItem>("item", 0);
            item.OnTriggered += () => OnSelectedItem(item);
            return item;
        }
    }
}