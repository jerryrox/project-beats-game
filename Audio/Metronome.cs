using System;
using PBGame.Maps;
using PBGame.Rulesets.Maps;
using PBGame.Rulesets.Maps.ControlPoints;
using PBFramework.Audio;

namespace PBGame.Audio
{
	public class Metronome : IMetronome {

		public event Action OnBeat;

        public event Action<double> OnBeatLengthChange;

        private IMusicController musicController;

        private IMap map;
		private TimingControlPoint curTimingPoint;
		private TimingControlPoint nextTimingPoint;
        private int timingPointIndex;

        private double curTime = -1;
		private double nextBeatTime = 0;


		public double BeatLength => curTimingPoint == null ? TimingControlPoint.DefaultBeatLength : curTimingPoint.BeatLength;


		public Metronome(IMapSelection selection, IMusicController controller)
		{
            musicController = controller;

            selection.OnMapChange += map =>
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
			nextBeatTime += BeatLength;
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
				nextBeatTime = (int)((curTime - curTimingPoint.Time) / BeatLength) * BeatLength + curTimingPoint.Time;
				if(curTime < curTimingPoint.Time)
					nextBeatTime -= BeatLength;
			}
		}

		/// <summary>
		/// Sets new timing point.
		/// </summary>
        private void SetCurrentTimingPoint(TimingControlPoint timingPoint, int index)
        {
            curTimingPoint = timingPoint;
            timingPointIndex = index;

            OnBeatLengthChange?.Invoke(BeatLength);
        }
    }
}

