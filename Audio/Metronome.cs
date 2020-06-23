using System;
using PBGame.Maps;
using PBGame.Rulesets.Maps;
using PBGame.Rulesets.Maps.Timing;
using PBGame.Rulesets.Maps.ControlPoints;
using PBFramework.Data;
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
        private BindableInt bindableBeatsInInterval = new BindableInt(0);


        public IPlayableMap CurrentMap
        {
            get => map;
            set
            {
                map = value;
                FindNewTimingPoint();
            }
        }

        public IAudioController AudioController
        {
            get => audioController;
            set
            {
                if (audioController != null)
                {
                    audioController.OnSeek -= OnAudioSeek;
                    audioController.OnStop -= OnAudioStop;
                }

                audioController = value;

                if (audioController != null)
                {
                    audioController.OnSeek += OnAudioSeek;
                    audioController.OnStop += OnAudioStop;
                    FindNewTimingPoint();
                }
            }
        }

        public IReadOnlyBindable<int> BeatIndex => bindableBeatIndex;

        public IReadOnlyBindable<int> BeatsInInterval => bindableBeatsInInterval;

        public BeatFrequency Frequency
        {
            get => frequency;
            set
            {
                frequency = value;
                ResetBeatsInInterval();
                ResetBeatLength();
                ResetNextBeatTime();
                FindCurBeatIndex();
            }
        }

        public IReadOnlyBindable<float> BeatLength => bindableBeatLength;

        /// <summary>
        /// Returns the current playback time of the audio controller.
        /// </summary>
        private float CurrentTime => audioController == null ? 0f : audioController.CurrentTime;


        public void Update()
		{
			// Get current time
			curTime = CurrentTime;

            // Check if current time has reached the next timing point.
            if(nextTimingPoint != null && curTime >= nextTimingPoint.Time)
            {
                // Find the next timing point.
                FindNextTimingPointFrom(timingPointIndex);
            }

			// If beat time reached
			if(curTime >= nextBeatTime)
			{
                OnBeat?.Invoke();
                
                // Set next beat time.
                nextBeatTime += bindableBeatLength.Value;

                // Find new beat index.
                FindCurBeatIndex();
			}
		}

		/// <summary>
		/// Finds the new current and next timing points to calculate the beat timings from.
		/// </summary>
		private void FindNewTimingPoint()
		{
            curTime = CurrentTime;

            // Set default timing point.
			if(map == null)
                curTimingPoint = null;
			else
			{
				var timingPoints = map.ControlPoints.TimingPoints;
                curTimingPoint = timingPoints[0];
            }

            // The next timing point and cur timing point index must be reassigned.
            nextTimingPoint = null;
            timingPointIndex = 0;

            // Find the next timing point at the edge.
            FindNextTimingPointFrom(0);
            FindCurBeatIndex();
		}

		/// <summary>
		/// Incrementally finds the next timing point from specified timing point index.
		/// </summary>
        private void FindNextTimingPointFrom(int index)
        {
            if (map != null)
            {
                var timingPoints = map.ControlPoints.TimingPoints;
                for (int i = index + 1; i < timingPoints.Count; i++)
                {
                    // Store next timing point
                    nextTimingPoint = timingPoints[i];
                    // If this timing point not reached, preserve next timing point and stop check.
                    if (curTime < timingPoints[i].Time)
                        break;

                    // Set new current timing point and remove next timing point, in case this is the last loop.
                    curTimingPoint = timingPoints[i];
                    nextTimingPoint = null;
                    timingPointIndex = i;
                }
            }
            
            ResetBeatsInInterval();
            ResetBeatLength();
            ResetNextBeatTime();
        }

        /// <summary>
        /// Finds the next beat time.
        /// </summary>
        private void ResetNextBeatTime()
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
        }

        /// <summary>
        /// Calculates beat length again and assigns to local variable.
        /// </summary>
        private void ResetBeatLength()
        {
            bindableBeatLength.Value = (
                curTimingPoint == null ?
                TimingControlPoint.DefaultBeatLength :
                curTimingPoint.BeatLength
            ) / (int)frequency;
        }

        /// <summary>
        /// Finds new beat index at current time.
        /// </summary>
        private void FindCurBeatIndex()
        {
            float startTime = 0f;
            if (curTimingPoint != null)
            {
                startTime = curTimingPoint.Time;
            }
            // Adding 1 in calculation to prevent precision point error.
            bindableBeatIndex.Value = (int)((curTime + 1f - startTime) / bindableBeatLength.Value) % bindableBeatsInInterval.Value;
        }

        /// <summary>
        /// Resets the value of beats in interval.
        /// </summary>
        private void ResetBeatsInInterval()
        {
            bindableBeatsInInterval.Value = (int)frequency * (int)(
                curTimingPoint == null ?
                TimeSignatureType.Quadruple :
                curTimingPoint.TimeSignature
            );
        }

        /// <summary>
        /// Event called from audio controller when the audio playback time has been sought.
        /// </summary>
        private void OnAudioSeek(float time) => FindNewTimingPoint();

        /// <summary>
        /// Event called from audio controller when the audio playback has stopped.
        /// </summary>
        private void OnAudioStop() => FindNewTimingPoint();
    }
}

