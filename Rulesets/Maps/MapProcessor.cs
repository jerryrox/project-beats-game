using System;
using PBGame.Rulesets.Objects;
using PBGame.Audio;

namespace PBGame.Rulesets.Maps
{
	public class MapProcessor : IMapProcessor {
		
		public IPlayableMap Map { get; set; }


		public MapProcessor(IPlayableMap map)
		{
            this.Map = map;
        }

		public virtual void PreProcess()
		{
		}

		public virtual void PostProcess()
		{
			ApplySamplePoint(Map);
		}

		/// <summary>
		/// Applies sample points to the hitobjects.
		/// </summary>
		private void ApplySamplePoint(IPlayableMap map)
		{
			Action<BaseHitObject> applyPoint = (hitObj) => {
				var samplePoint = hitObj.SamplePoint;
				if(samplePoint != null)
				{
					if(hitObj.Samples != null)
					{
						for(int i=0; i<hitObj.Samples.Count; i++)
							samplePoint.ApplySample(hitObj.Samples[i]);
					}
					var curve = hitObj as IHasCurve;
					if(curve != null)
					{
						foreach(var samples in curve.NodeSamples)
						{
							for(int i=0; i<samples.Count; i++)
								samplePoint.ApplySample(samples[i]);
						}
					}
				}
			};

			foreach(var hitObject in map.HitObjects)
			{
				applyPoint(hitObject);
				foreach(var nested in hitObject.NestedObjects)
					applyPoint(nested);
			}
		}
	}
}

