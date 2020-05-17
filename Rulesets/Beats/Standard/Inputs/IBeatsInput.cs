using System;
using PBFramework.Inputs;

namespace PBGame.Rulesets.Beats.Standard.Inputs
{
    public interface IBeatsInput<T>
        where T : class, IInput
    {

        /// <summary>
        /// Event called when the cursor has been released.
        /// </summary>
        event Action OnRelease;

        /// <summary>
        /// The raw input information backing this game input.
        /// </summary>
        T Input { get; set; }


        /// <summary>
        /// Returns whether the cursor is active and its values are usable.
        /// </summary>
        bool IsActive { get; }
    }
}