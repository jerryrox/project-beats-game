namespace PBGame.Rulesets.Maps
{
    /// <summary>
    /// Interface of a processor which makes the map playable after conversion to a specific game mode.
    /// </summary>
	public interface IMapProcessor {

		/// <summary>
		/// Returns the map being processed.
		/// </summary>
		IMap Map { get; }


		/// <summary>
		/// Preprocesses map before map property application.
		/// </summary>
		void PreProcess();

		/// <summary>
		/// Postprocesses map after map property application.
		/// </summary>
		void PostProcess();
	}
}

