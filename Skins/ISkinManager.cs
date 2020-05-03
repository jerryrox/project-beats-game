using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using PBGame.IO;
using PBGame.Audio;
using PBFramework;
using PBFramework.Threading;

namespace PBGame.Skins
{
    public interface ISkinManager : IImportsFile {

        /// <summary>
        /// Event called when a skin has been imported.
        /// </summary>
        event Action<ISkin> OnImportSkin;


        /// <summary>
        /// Returns the default skin of the game.
        /// </summary>
        ISkin DefaultSkin { get; }

        /// <summary>
        /// Returns the skin currently selected.
        /// </summary>
        ISkin CurrentSkin { get; }

        /// <summary>
        /// Returns the list of all skins loaded in the game.
        /// </summary>
        IReadOnlyList<ISkin> Skins { get; }


        /// <summary>
        /// Clears loaded skins from memory and reloads from disk.
        /// </summary>
        Task Reload(IEventProgress progress);

        /// <summary>
        /// Selects the specified skin for usage.
        /// </summary>
        IExplicitPromise SelectSkin(ISkin skin, ISoundPooler soundPooler);
    }
}