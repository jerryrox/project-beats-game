namespace PBGame.Rulesets.Beats.Standard.UI.Components
{
    /// <summary>
    /// A general interface of an inner component of dragger.
    /// </summary>
    public interface IDraggerComponent {

        /// <summary>
        /// Returns the parent dragger view.
        /// </summary>
        DraggerView Dragger { get; }


        /// <summary>
        /// Sets the parent dragger view.
        /// Specifying a null dragger should be equivalent to calling RemoveDragger.
        /// </summary>
        void SetDragger(DraggerView dragger);

        /// <summary>
        /// Disassociates this object from parent dragger.
        /// </summary>
        void RemoveDragger();
    }
}