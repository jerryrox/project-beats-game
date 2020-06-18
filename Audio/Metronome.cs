using System;
using PBGame.Maps;
using PBGame.Rulesets.Maps;
using PBGame.Rulesets.Maps.ControlPoints;
using PBFramework.Data.Bindables;
using PBFramework.Audio;

namespace PBGame.Audio
{
	public class Metronome : IMetronome {

		public event Action OnBeat;

        private IMusicController musicController;
        private IPlayableMap map;
		private TimingControlPoint curTimingPoint;
		private TimingControlPoint nextTimingPoint;
        private int timingPointIndex;

        private float curTime = -1;
		private float nextBeatTime = 0;

        private BindableFloat bindableBeatLength = new BindableFloat(TimingControlPoint.DefaultBeatLength);


        public IReadOnlyBindable<float> BeatLength => bindableBeatLength;//curTimingPoint == null ? TimingControlPoint.DefaultBeatLength : curTimingPoint.BeatLength;


		public Metronome(IMapSelection selection, IMusicController controller)
		{
            musicController = controller;

            selection.Map.OnNewValue += map =>
            {
                this.map = map;
                curTime = controller.CurrentTime;
                FindTimingPoint();
            };
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
			curTime = musicController.Clock.CurrentTime;

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
                SetCurrentTimingPoint(new TimingControlPoint(), -1);
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
        }

		/// <summary>
		/// Resets the next beat time to the latest previous beat for current timing point.
		/// </summary>
		private void RecalibrateBeatTime()
		{
			if(curTimingPoint == null)
			{
				nextBeatTime = (int)(curTime / 1000) * 1000;
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
		/// Sets new timing point.
		/// </summary>
        private void SetCurrentTimingPoint(TimingControlPoint timingPoint, int index)
        {
            curTimingPoint = timingPoint;
            timingPointIndex = index;

            bindableBeatLength.Value = curTimingPoint == null ? TimingControlPoint.DefaultBeatLength : curTimingPoint.BeatLength;
        }
    }
}

