using System;
using System.Linq;
using System.Collections.Generic;
using PBGame.Rulesets.Difficulty;
using PBGame.Rulesets.Objects;

namespace PBGame.Rulesets.Maps
{
	public abstract class MapConverter<T> : IMapConverter
        where T : BaseHitObject
    {
		public IOriginalMap Map { get; private set; }

		public bool IsConvertible => RequiredTypes.All(t => Map.HitObjects.Any(t.IsInstanceOfType));

		public abstract GameModeType TargetMode { get; }

		/// <summary>
		/// Types of interfaces which all hit objects should be implemented with.
		/// </summary>
		protected abstract IEnumerable<Type> RequiredTypes { get; }



		protected MapConverter(IOriginalMap map)
		{
            this.Map = map;
        }

        public IPlayableMap Convert() => Convert(Map);

		/// <summary>
		/// Converts the specified beatmap in to a game-specific variant version of beatmap.
		/// </summary>
		protected IPlayableMap Convert(IOriginalMap original)
		{
			var newBeatmap = CreateMap(original);
			newBeatmap.HitObjects = ConvertHitObjects(original.HitObjects);
			return newBeatmap;
		}

        /// <summary>
        /// Creates a new beatmap for current game mode.
        /// </summary>
        protected virtual PlayableMap<T> CreateMap(IOriginalMap map) => new PlayableMap<T>(Map);

        /// <summary>
        /// Converts specified hit object in to game-specific variant of the object.
        /// </summary>
        protected abstract IEnumerable<T> ConvertHitObjects(BaseHitObject hitObject);

		/// <summary>
		/// Converts the specified hit objects in to game-specific variant of hit objects.
		/// </summary>
		private List<T> ConvertHitObjects(IEnumerable<BaseHitObject> objects)
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

