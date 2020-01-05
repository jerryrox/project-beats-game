using System;
using System.Linq;
using System.Collections.Generic;
using PBGame.Audio;
using PBGame.Rulesets.Beats.Standard.Judgements;
using PBGame.Rulesets.Maps;
using PBGame.Rulesets.Maps.ControlPoints;
using PBGame.Rulesets.Objects;

namespace PBGame.Rulesets.Beats.Standard.Objects
{
	/// <summary>
	/// Representation of a dragger.
	/// </summary>
	public class Dragger : HitObject, IHasCurve {

		/// <summary>
		/// The hit object located at the starting position of the dragger.
		/// </summary>
		public HitCircle StartCircle { get; set; }

        /// <summary>
        /// The hit object located at the ending position of the dragger.
        /// </summary>
        public DraggerEndCircle EndCircle { get; set; }

        public SliderPath Path { get; set; }

		public int RepeatCount { get; set; }

        public List<List<SoundInfo>> NodeSamples { get; set; } = new List<List<SoundInfo>>();

        public double Distance => 0;

        public double EndTime { get; set; }

		public double Duration => EndTime - StartTime;

		/// <summary>
		/// Interval which a dragger tick on the path body should be generated at.
		/// </summary>
		public double TickInterval { get; set; }

		/// <summary>
		/// Returns the ending position x of the dragger.
		/// </summary>
		public double EndX { get { return EndCircle.X; } }


		protected override void ApplyMapPropertiesSelf (ControlPointGroup controlPoints, MapDifficulty difficulty)
		{
			base.ApplyMapPropertiesSelf (controlPoints, difficulty);

			// Calculate tick generation interval.
			TimingControlPoint timingPoint = controlPoints.TimingPointAt(StartTime);
			TickInterval = timingPoint.BeatLength / difficulty.SliderTickRate;
		}

		protected override void CreateNestedObjects ()
		{
			base.CreateNestedObjects();

			// Find the sample which contains a normal hit sound.
			var baseSample = Samples.Where(s => s.Sound == SoundInfo.HitNormal).FirstOrDefault() ?? Samples.FirstOrDefault();

			// Create start circle
			AddNestedObject(StartCircle = new DraggerStartCircle() {
				StartTime = StartTime,
				Samples = NodeSamples[0],
				SamplePoint = SamplePoint,
				X = 0
			});

			// Create ticks
			for(double t=StartTime+TickInterval; t<EndTime-ControlPointOffset; t+=TickInterval)
			{
				// Create sample for tick.
				var tickSampleList = new List<SoundInfo>();
				if(baseSample != null)
				{
					var tickSample = baseSample.Clone();
					tickSample.Sound = SoundInfo.Slidertick;
					tickSampleList.Add(tickSample);
				}

				// Create tick object.
				double progress = (t-StartTime) / Duration;
				AddNestedObject(new DraggerTick() {
					StartTime = t,
					Samples = tickSampleList,
					SamplePoint = SamplePoint,
					X = this.GetPosition(progress).x
				});
			}

			// Create end circle
			AddNestedObject(EndCircle = new DraggerEndCircle() {
				StartTime = EndTime,
				Samples = NodeSamples[NodeSamples.Count-1],
				SamplePoint = SamplePoint,
				X = this.GetPosition(1).x
			});
		}

		public override Rulesets.Judgements.JudgementInfo CreateJudgementInfo() { return new JudgementInfo(); }
	}
}

