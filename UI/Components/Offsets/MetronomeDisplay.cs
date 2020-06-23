using System;
using System.Collections;
using System.Collections.Generic;
using PBGame.Audio;
using PBGame.Graphics;
using PBFramework.UI;
using PBFramework.Graphics;
using PBFramework.Allocation.Recyclers;
using PBFramework.Dependencies;
using UnityEngine;
using UnityEngine.UI;

namespace PBGame.UI.Components.Offsets
{
    public class MetronomeDisplay : UguiGrid {

        /// <summary>
        /// The base horizontal cell spacing value.
        /// The actual spacing will be BaseSpacing / tickCount.
        /// </summary>
        private const float BaseSpacing = 176f;

        private ManagedRecycler<MetronomeTick> tickRecycler;


        /// <summary>
        /// Returns the current metronome in use.
        /// </summary>
        public IMetronome CurMetronome { get; private set; }

        /// <summary>
        /// Returns the number of ticks currently visible.
        /// </summary>
        public int TickCount => tickRecycler.ActiveCount;

        [ReceivesDependency]
        private IColorPreset ColorPreset { get; set; }


        [InitWithDependency]
        private void Init()
        {
            tickRecycler = new ManagedRecycler<MetronomeTick>(CreateTick);

            CellSize = new Vector2(24f, 24f);
            Axis = GridLayoutGroup.Axis.Horizontal;
            Alignment = TextAnchor.MiddleCenter;
        }

        /// <summary>
        /// Sets the metronome to listen ticks from.
        /// </summary>
        public void SetMetronome(IMetronome metronome)
        {
            RemoveMetronome();

            CurMetronome = metronome;
            if (metronome != null)
            {
                metronome.BeatIndex.OnNewValue += OnMetronomeBeatIndex;
                metronome.BeatsInInterval.BindAndTrigger(OnBeatsInIntervalChange);
            }
        }

        /// <summary>
        /// Removes current association with the metonome.
        /// </summary>
        public void RemoveMetronome()
        {
            if (CurMetronome != null)
            {
                CurMetronome.BeatIndex.OnNewValue -= OnMetronomeBeatIndex;
                CurMetronome.BeatsInInterval.OnNewValue -= OnBeatsInIntervalChange;
            }
            CurMetronome = null;
        }

        /// <summary>
        /// Prepares tick displays based on specified count.
        /// </summary>
        public void SetupTicks(int tickCount)
        {
            ClearTicks();

            tickCount = Mathf.Max(tickCount, 0);
            if(tickCount <= 0)
                return;

            for (int i = 0; i < tickCount; i++)
            {
                MetronomeTick tick = tickRecycler.GetNext();
                tick.Depth = i;
                tick.Tint = i == 0 ? ColorPreset.PrimaryFocus : ColorPreset.SecondaryFocus;
            }
            SpaceWidth = BaseSpacing / tickCount;
        }

        /// <summary>
        /// Triggers tick at specified index.
        /// Returns whether the tick was triggred successfully.
        /// </summary>
        public bool TriggerTick(int index)
        {
            if(index < 0 || index >= TickCount)
                return false;
            tickRecycler.ActiveObjects[index].Tick();
            return true;
        }

        /// <summary>
        /// Removes all ticks in the display.
        /// </summary>
        public void ClearTicks() => tickRecycler.ReturnAll();

        /// <summary>
        /// Creates a new metronome tick.
        /// </summary>
        private MetronomeTick CreateTick()
        {
            MetronomeTick tick = CreateChild<MetronomeTick>("tick");
            return tick;
        }

        /// <summary>
        /// Event called on beats in interval value change.
        /// </summary>
        private void OnBeatsInIntervalChange(int beats) => SetupTicks(beats);

        /// <summary>
        /// Event called on metronome beat index change.
        /// </summary>
        private void OnMetronomeBeatIndex(int index) => TriggerTick(index);
    }
}