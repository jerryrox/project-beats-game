using System;
using PBGame.Audio;
using PBGame.Rulesets.Maps;
using PBFramework.Threading.Futures;

namespace PBGame.Stores
{
    public interface IMapAssetStore : IDisposable {

        /// <summary>
        /// Whether the hitsounds are loaded for current map.
        /// </summary>
        bool IsHitsoundLoaded { get; }

        /// <summary>
        /// The map currently being used in context.
        /// </summary>
        IPlayableMap Map { get; }

        /// <summary>
        /// The alternative map sound table for map hitsounds.
        /// </summary>
        MapSoundTable SoundTable { get; }

        /// <summary>
        /// Returns a future which loads the hit sounds from the map.
        /// </summary>
        IControlledFuture LoadHitsounds();
    }
}