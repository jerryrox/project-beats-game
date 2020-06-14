using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Configurations.Settings;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.SettingsMenu.Contents
{
    public class ContentGroup : UguiObject {

        private const float InitialHeight = 96f;
        private const float EntryPosStart = -60f;

        private ISprite iconSprite;
        private ILabel titleLabel;
        private List<BaseContentEntry> entries = new List<BaseContentEntry>();

        private SettingsTab tabData;
        private float entriesSize = 0f;


        /// <summary>
        /// Returns the tab data represented by this group.
        /// </summary>
        public SettingsTab TabData => tabData;

        /// <summary>
        /// Value assigned from content holder used to cache the ratio of this object's position relative to the scrollview.
        /// </summary>
        public float PositionProgress { get; set; }


        [InitWithDependency]
        private void Init()
        {
            // Initial height. Should change as new entries are added.
            Height = InitialHeight;

            var titleBox = CreateChild("title-box", 0);
            {
                titleBox.Anchor = AnchorType.TopStretch;
                titleBox.Pivot = PivotType.Top;
                titleBox.RawWidth = -32f;
                titleBox.Height = 32f;
                titleBox.Y = -16f;

                iconSprite = titleBox.CreateChild<UguiSprite>("icon", 0);
                {
                    iconSprite.Anchor = AnchorType.LeftStretch;
                    iconSprite.Pivot = PivotType.Left;
                    iconSprite.Width = titleBox.Height;
                    iconSprite.X = 0f;
                    iconSprite.SetOffsetVertical(0f);
                }
                titleLabel = titleBox.CreateChild<Label>("title", 1);
                {
                    titleLabel.Anchor = AnchorType.Fill;
                    titleLabel.Offset = new Offset(40f, 0f, 0f, 0f);
                    titleLabel.Alignment = TextAnchor.MiddleLeft;
                    titleLabel.WrapText = true;
                    titleLabel.FontSize = 20;
                    titleLabel.IsBold = true;
                }
            }
            var line = CreateChild<UguiSprite>("line", 1);
            {
                line.Anchor = AnchorType.BottomStretch;
                line.Pivot = PivotType.Bottom;
                line.Y = 0f;
                line.Height = 21.5f;
                line.SpriteName = "glow-bar";
                line.ImageType = Image.Type.Sliced;
                line.SetOffsetHorizontal(24);
            }
        }

        /// <summary>
        /// Sets the settings tab data to render the content entries for.
        /// </summary>
        public void SetTabData(SettingsTab tabData)
        {
            this.tabData = tabData;

            iconSprite.SpriteName = tabData.IconName;
            titleLabel.Text = tabData.Name;

            foreach (var entryData in tabData.GetEntries())
            {
                var entry = CreateEntryObject(entryData);
                entry.SetEntryData(entryData);

                // Set transformation properties.
                entry.Anchor = AnchorType.TopStretch;
                entry.Pivot = PivotType.Top;
                entry.SetOffsetHorizontal(32f);
                entry.Y = EntryPosStart - entriesSize;

                // Add to entries list and mutate some internal states
                entries.Add(entry);
                entriesSize += entry.Height;
            }

            // Set new height of the group.
            Height = InitialHeight + entriesSize;
        }

        /// <summary>
        /// Creates ane returns a new content entry for specified entry data.
        /// </summary>
        private BaseContentEntry CreateEntryObject(SettingsEntryBase entryData)
        {
            if(entryData == null)
                throw new NullReferenceException("entryData mustn't be null!");

            if (entryData is SettingsEntryAction)
                return CreateChild<ContentButtonEntry>("action");
            else if (entryData is SettingsEntryBool)
                return CreateChild<ContentToggleEntry>("bool");
            else if (entryData is SettingsEntryEnum)
                return CreateChild<ContentSelectionEntry>("enum");
            else if (entryData is SettingsEntryFloat)
                return CreateChild<ContentFloatEntry>("float");
            else if (entryData is SettingsEntryInt)
                return CreateChild<ContentIntEntry>("int");
            else if (entryData is SettingsEntryString)
                return CreateChild<ContentInputEntry>("string");
            throw new Exception("Unsupported entry type: " + entryData.GetType().Name);
        }
    }
}