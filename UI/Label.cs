using PBGame.Assets.Fonts;
using PBFramework.UI;
using PBFramework.Dependencies;

namespace PBGame.UI
{
    /// <summary>
    /// Extension of framework label for default font support.
    /// </summary>
    public class Label : UguiLabel {

        [InitWithDependency]
        private void Init(IFontManager fontManager)
        {
            Font = fontManager.DefaultFont;
        }
    }
}