using PBFramework.Assets.Fonts;

namespace PBGame.Assets.Fonts
{
    public class FontManager : IFontManager {
    
        public IFont DefaultFont { get; private set; }


        public FontManager()
        {
            DefaultFont = new ResourceFont("Fonts/Montserrat");
        }
    }
}