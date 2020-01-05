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
		/// Returns the list of all difficulty informations of the map.
		/// Converted maps will only have one difficulty info which has a context of the mode this map is converted for.
		/// </summary>
		List<DifficultyInfo> Difficulties { get; }

		/// <summary>
		/// Returns the total duration of breaks.
		/// </summary>
		double BreakDuration { get; }


		/// <summary>
		/// Returns the difficulty info for specified game mode.
		/// If not found, the difficulty info for the map's original game mode will be returned.
		/// </summary>
		DifficultyInfo GetDifficulty(GameModes gameMode);

		/// <summary>
		/// Creates a playable version of the map by converting the map using specified game mode servicer.
		/// </summary>
		IMap CreatePlayable(IModeService service);

		/// <summary>
		/// Returns the shallow clone of this map.
		/// </summary>
		IMap Clone();
    }
}