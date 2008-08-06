using Microsoft.Xna.Framework.Graphics;

namespace JadeEngine.JadeObjects.JadeObjectComponents
{
    public interface IJadeChildRenderer : IJadeObjectComponent
    {
        void RenderChildren(GraphicsDevice gd);
    }
}
