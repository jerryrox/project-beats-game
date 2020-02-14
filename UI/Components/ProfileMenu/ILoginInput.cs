namespace PBGame.UI.Components.ProfileMenu
{
    public interface ILoginInput : IInputBox {

        /// <summary>
        /// Simulates invalid input value feedback to user.
        /// </summary>
        void ShowInvalid();
    }
}