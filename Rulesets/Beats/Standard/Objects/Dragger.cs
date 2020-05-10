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

        public SliderPath Path { get; set; }

		public int RepeatCount { get; set; }

        public List<List<SoundInfo>> NodeSamples { get; set; } = new List<List<SoundInfo>>();

        public float Distance => 0;

        public float EndTime { get; set; }

		public float Duration => EndTime - StartTime;

		/// <summary>
		/// Interval which a dragger tick on the path body should be generated at.
		/// </summary>
		public float TickInterval { get; set; }

		/// <summary>
		/// Returns the ending position x of the dragger.
		/// </summary>
		public float EndX { get; set; }


		protected override void ApplyMapPropertiesSelf (ControlPointGroup controlPoints, MapDifficulty difficulty)
		{
			base.ApplyMapPropertiesSelf (controlPoints, difficulty);

            // Set samples to be played on dragger end.
            Samples = NodeSamples[NodeSamples.Count - 1];

            // Set ending position
            EndX = this.GetPosition(1f).x;

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
			for(float t=StartTime+TickInterval; t<EndTime-ControlPointOffset; t+=TickInterval)
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
				float progress = (t-StartTime) / Duration;
				AddNestedObject(new DraggerTick() {
					StartTime = t,
					Samples = tickSampleList,
					SamplePoint = SamplePoint,
					X = this.GetPosition(progress).x
				});
			}
		}

		public override Rulesets.Judgements.JudgementInfo CreateJudgementInfo() { return new JudgementInfo(); }
	}
}

