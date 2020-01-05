using System.Collections.Generic;
using UnityEngine;

namespace PBGame.Skins
{
	/// <summary>
	/// Indicates that the default IComboColorable can be overridden with the colors defined in this interface.
	/// </summary>
	public interface ICustomColorable {

		/// <summary>
		/// Table of colors to be used for custom element coloring.
		/// </summary>
		Dictionary<string, Color> CustomColors { get; set; }
	}
}

