using System;
using PBGame.Maps;
using PBGame.Rulesets.Maps;
using PBGame.Rulesets.Maps.Timing;
using PBGame.Rulesets.Maps.ControlPoints;
using PBFramework.Data.Bindables;
using PBFramework.Audio;
using UnityEngine;

namespace PBGame.Audio
{
	public class Metronome : IMetronome {

		public event Action OnBeat;

        private IAudioController audioController;
        private IPlayableMap map;
		private TimingControlPoint curTimingPoint;
		private TimingControlPoint nextTimingPoint;
        private int timingPointIndex;

        private BeatFrequency frequency = BeatFrequency.Full;

        private float curTime = -1;
		private float nextBeatTime = 0;

        private BindableInt bindableBeatIndex = new BindableInt(0);
        private BindableFloat bindableBeatLength = new BindableFloat(TimingControlPoint.DefaultBeatLength);


        public IReadOnlyBindable<int> BeatIndex => bindableBeatIndex;

        public int BeatsInInterval => GetBeatsInInterval();

        public BeatFrequency Frequency
        {
            get => frequency;
            set
            {
                frequency = value;
                SetCurrentTimingPoint(curTimingPoint, timingPointIndex);
                RecalibrateBeatTime();
            }
        }

        public IReadOnlyBindable<float> BeatLength => bindableBeatLength;


        public Metronome(IMapSelection selection, IAudioController controller)
		{
            if(controller == null)
                throw new ArgumentNullException(nameof(controller));

            audioController = controller;

            if (selection != null)
            {
                selection.Map.OnNewValue += map =>
                {
                    this.map = map;
                    curTime = controller.CurrentTime;
                    FindTimingPoint();
                };
            }
            controller.OnSeek += time =>
            {
                curTime = time;
                FindTimingPoint();
            };
            controller.OnStop += () =>
            {
                curTime = 0f;
                FindTimingPoint();
            };
        }

        public void Update()
		{
			// Get current time
			curTime = audioController.CurrentTime;

			// If beat time reached
			if(curTime >= nextBeatTime)
			{
                OnBeat?.Invoke();
				FindNextBeatTime();
			}
		}

		/// <summary>
		/// Finds the new current and next timing points to calculate the beat timings from.
		/// </summary>
		private void FindTimingPoint()
		{
			if(map == null)
			{
                SetCurrentTimingPoint(null, 0);
				nextTimingPoint = null;

                RecalibrateBeatTime();
			}
			else
			{
				var timingPoints = map.ControlPoints.TimingPoints;
                // There must be one timing point anyway
                SetCurrentTimingPoint(timingPoints[0], 0);
				nextTimingPoint = null;

				// Find the next timing point at the edge.
                FindNextTimingPoint();
            }
		}

		/// <summary>
		/// Finds the next timing point and applies to current and next timing point references.
		/// </summary>
        private void FindNextTimingPoint()
        {
            var timingPoints = map.ControlPoints.TimingPoints;
            for (int i = timingPointIndex + 1; i < timingPoints.Count; i++)
            {
                // Store next timing point
                nextTimingPoint = timingPoints[i];
                // If this timing point not reached, preserve next timing point and stop check.
                if (curTime < timingPoints[i].Time)
                    break;

                // Set new current timing point and remove next timing point, in case this is the last loop.
                SetCurrentTimingPoint(timingPoints[i], i);
                nextTimingPoint = null;
            }

            RecalibrateBeatTime();
        }

        /// <summary>
        /// Finds the next beating time from current time.
        /// </summary>
        private void FindNextBeatTime()
		{
			if(map != null)
			{
				// Check if current time has reached the next timing point.
				if(nextTimingPoint != null && curTime >= nextTimingPoint.Time)
				{
                    // Find the next timing point.
                    FindNextTimingPoint();
				}
			}

			// Set next beat time.
			nextBeatTime += bindableBeatLength.Value;

            // Find new beat index.
            FindBeatIndex();
        }

		/// <summary>
		/// Resets the next beat time to the latest previous beat for current timing point.
		/// </summary>
		private void RecalibrateBeatTime()
		{
			if(curTimingPoint == null)
			{
				nextBeatTime = (int)(curTime / TimingControlPoint.DefaultBeatLength) * TimingControlPoint.DefaultBeatLength;
			}
			else
			{
                float beatLength = bindableBeatLength.Value;
                nextBeatTime = (int)((curTime - curTimingPoint.Time) / beatLength) * beatLength + curTimingPoint.Time;
				if(curTime < curTimingPoint.Time)
					nextBeatTime -= beatLength;
			}

            // Find beat index from the start of the timing point.
            FindBeatIndex();
        }

		/// <summary>
		/// Sets new timing point.
		/// </summary>
        private void SetCurrentTimingPoint(TimingControlPoint timingPoint, int index)
        {
            curTimingPoint = timingPoint;
            timingPointIndex = index;

            bindableBeatLength.Value = (
                curTimingPoint == null ?
                TimingControlPoint.DefaultBeatLength :
                curTimingPoint.BeatLength
            ) / (int)frequency;
        }

        /// <summary>
        /// Finds new beat index at current time.
        /// </summary>
        private void FindBeatIndex()
        {
            float startTime = 0f;
            if (curTimingPoint != null)
            {
                startTime = curTimingPoint.Time;
            }
            // Adding 1 in calculation to prevent precision point error.
            bindableBeatIndex.Value = (int)((curTime + 1f - startTime) / bindableBeatLength.Value) % GetBeatsInInterval();
        }

        /// <summary>
        /// Returns the number of beats in an interval of current time signature.
        /// </summary>
        private int GetBeatsInInterval()
        {
            return (int)frequency * (
                curTimingPoint == null ?
                (int)TimeSignatureType.Quadruple :
                (int)curTimingPoint.TimeSignature
            );
        }
    }
}

