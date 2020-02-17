using System.Collections.Generic;
using PBGame.Rulesets.Maps.Timing;
using PBGame.Rulesets.Maps.ControlPoints;
using PBGame.Rulesets.Objects;
using PBGame.Rulesets.Difficulty;
using PBFramework.Data.Queries;

namespace PBGame.Rulesets.Maps
{
    public interface IMap : IQueryableData, IComboColorable {
        
		/// <summary>
		/// Returns the details of this map.
		/// </summary>
		MapDetail Detail { get; }

		/// <summary>
		/// Returns the control points collection holder.
		/// </summary>
		ControlPointGroup ControlPoints { get; }

		/// <summary>
		/// Returns the actual playable game mode of this map.
		/// </summary>
		GameModes PlayableMode { get; }

		/// <summary>
		/// Returns the list of break points in the map.
		/// </summary>
		List<BreakPoint> BreakPoints { get; }

		/// <summary>
		/// Returns the list of hit objects in the map.
		/// </summary>
		IEnumerable<HitObject> HitObjects { get; }

		/// <summary>
		/// Returns the metadata of the map.
		/// </summary>
		MapMetadata Metadata { get; }

		/// <summary>
		/// Returns the difficulty information of this map.
		/// Is non-null only if this map is a playable version.
		/// </summary>
		DifficultyInfo Difficulty { get; }

		/// <summary>
		/// Returns the number of hit objects.
		/// </summary>
		int ObjectCount { get; }

        /// <summary>
        /// Duration of the map in milliseconds.
        /// </summary>
        int Duration { get; }

        /// <summary>
        /// Returns the total duration of breaks.
        /// </summary>
        double BreakDuration { get; }

		/// <summary>
		/// Returns whether this map is playable.
		/// </summary>
		bool IsPlayable { get; }


		/// <summary>
		/// Creates playable variants of this maps for modes included in specified manager.
		/// </summary>
		void CreatePlayable(IModeManager modeManager);

		/// <summary>
		/// Returns the playable map variant for specified game mode.
		/// If specified mode is not supported, it will returns the variant of the game mode the map was created for.
		/// </summary>
        IMap GetPlayable(GameModes gamemode);

        /// <summary>
        /// Returns the shallow clone of this map.
        /// </summary>
        IMap Clone();
    }
}