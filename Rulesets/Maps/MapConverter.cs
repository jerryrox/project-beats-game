using System;
using System.Linq;
using System.Collections.Generic;
using PBGame.Rulesets.Difficulty;
using PBGame.Rulesets.Objects;

namespace PBGame.Rulesets.Maps
{
	public abstract class MapConverter<T> : IMapConverter
        where T : HitObject
    {
		public IMap Map { get; private set; }

		public bool IsConvertible => RequiredTypes.All(t => Map.HitObjects.Any(t.IsInstanceOfType));

		public abstract GameModes TargetMode { get; }

		/// <summary>
		/// Types of interfaces which all hit objects should be implemented with.
		/// </summary>
		protected abstract IEnumerable<Type> RequiredTypes { get; }



		protected MapConverter(IMap map)
		{
            this.Map = map;
        }

        public IMap Convert() => Convert(Map);

		/// <summary>
		/// Converts the specified beatmap in to a game-specific variant version of beatmap.
		/// </summary>
		protected IMap Convert(IMap original)
		{
			var newBeatmap = CreateMap();

			newBeatmap.PlayableMode = TargetMode;
			newBeatmap.Detail = original.Detail;
			newBeatmap.ControlPoints = original.ControlPoints;
			newBeatmap.HitObjects = ConvertHitObjects(original.HitObjects);
			newBeatmap.BreakPoints = original.BreakPoints;
			newBeatmap.ComboColors = original.ComboColors;
			newBeatmap.Difficulties = new List<DifficultyInfo>() { original.GetDifficulty(TargetMode) };

			return newBeatmap;
		}

        /// <summary>
        /// Creates a new beatmap for current game mode.
        /// </summary>
        protected virtual Map<T> CreateMap() => new Map<T>();

        /// <summary>
        /// Converts specified hit object in to game-specific variant of the object.
        /// </summary>
        protected abstract IEnumerable<T> ConvertHitObjects(HitObject hitObject);

		/// <summary>
		/// Converts the specified hit objects in to game-specific variant of hit objects.
		/// </summary>
		private List<T> ConvertHitObjects(IEnumerable<HitObject> objects)
		{
			List<T> newObjects = new List<T>();

			foreach(var obj in objects)
			{
				// If this object is already converted
				var tObj = obj as T;
				if(tObj != null)
				{
					newObjects.Add(tObj);
					continue;
				}

				// Convert the hit object
				var converted = ConvertHitObjects(obj);

				// Add converted object
				foreach(var c in converted)
				{
					if(c != null)
						newObjects.Add(c);
				}
			}

			return newObjects;
		}
	}
}

