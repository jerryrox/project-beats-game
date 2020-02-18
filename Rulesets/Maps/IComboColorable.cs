using System.Collections.Generic;
using UnityEngine;

namespace PBGame.Rulesets.Maps
{
    /// <summary>
    /// Indicates that the inner objects can be colored with the color list.
    /// </summary>
	public interface IComboColorable {

		/// <summary>
		/// Returns the list of colors to be used for hit object coloring.
		/// </summary>
		List<Color> ComboColors { get; }
	}
}

