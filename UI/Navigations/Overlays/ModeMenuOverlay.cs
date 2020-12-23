using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using PBGame.UI.Models;
using PBGame.UI.Components.ModeMenu;
using PBGame.Rulesets;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Navigations.Overlays
{
    public class ModeMenuOverlay : BaseSubMenuOverlay<ModeMenuModel>
    {
        private readonly static Vector2 ModeCellSize = new Vector2(240f, 48f);

        protected override int ViewDepth => ViewDepths.ModeMenuOverlay;

        [ReceivesDependency]
        private IModeManager ModeManager { get; set; }

        [InitWithDependency]
        private void Init()
        {
            List<IModeService> modeServices = ModeManager.AllServices().ToList();

            container.Anchor = AnchorType.TopLeft;
            container.Pivot = PivotType.TopLeft;
            container.X = 300;
            container.Y = -16;
            container.Width = ModeCellSize.x;
            container.Height = modeServices.Count * ModeCellSize.y;

            var background = container.CreateChild<UguiSprite>("bg");
            {
                background.Anchor = AnchorType.Fill;
                background.Offset = Offset.Zero;
                background.Color = new Color(0f, 0f, 0f, 0.75f);

                var grid = background.CreateChild<UguiGrid>("grid");
                {
                    grid.Anchor = AnchorType.Fill;
                    grid.Offset = Offset.Zero;
                    grid.CellSize = ModeCellSize;
                    grid.Axis = GridLayoutGroup.Axis.Vertical;
                    grid.Limit = 0;

                    foreach (var service in modeServices)
                    {
                        var cell = grid.CreateChild<ModeMenuCell>($"cell-{service.Name}");
                        cell.SetModeService(service);
                        cell.OnTriggered += () => Model.SelectMode(service);
                    }
                }
            }
        }
    }
}