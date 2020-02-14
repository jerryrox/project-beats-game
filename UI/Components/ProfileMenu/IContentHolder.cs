using PBFramework.UI;
using PBFramework.Graphics;

namespace PBGame.UI.Components.ProfileMenu
{
    public interface IContentHolder : IGraphicObject
    {
        ISprite GlowSprite { get; }
    }
}