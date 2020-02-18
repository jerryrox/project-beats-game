using System.Collections.Generic;
using PBGame.Rulesets.Maps.Timing;
using PBGame.Rulesets.Maps.ControlPoints;
using PBGame.Rulesets.Objects;
using PBFramework.Data.Queries;

namespace PBGame.Rulesets.Maps
{
    public interface IMap : IQueryableData, IComboColorable {
        
		/// <summary>
		/// Returns the details of this map.
		/// </summary>
		MapDetail Detail { get; }

        /// <summary>
        /// Returns the metadata of the map.
        /// </summary>
        MapMetadata Metadata { get; }

        /// <summary>
        /// Returns the control points collection holder.
        /// </summary>
        ControlPointGroup ControlPoints { get; }

		/// <summary>
		/// Returns the list of break points in the map.
		/// </summary>
		List<BreakPoint> BreakPoints { get; }

		/// <summary>
		/// Returns the list of hit objects in the map.
		/// </summary>
		IEnumerable<HitObject> HitObjects { get; }

        /// <summary>
        /// Returns the number of hit objects.
        /// </summary>
        int ObjectCount { get; }

        /// <summary>
        /// Duration of the map in milliseconds.
        /// </summary>
        int Duration { get; }

        /// <summary>
        /// Returns the total duration of breaks in milliseconds.
        /// </summary>
        float BreakDuration { get; }

		/// <summary>
		/// Returns whether this map is a playable map.
		/// </summary>
		bool IsPlayable { get; }
    }
}