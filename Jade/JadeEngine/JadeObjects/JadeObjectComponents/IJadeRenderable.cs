using Microsoft.Xna.Framework.Graphics;

namespace JadeEngine.JadeObjects.JadeObjectComponents
{
	interface IJadeRenderable : IJadeObjectComponent
	{
		void Render(GraphicsDevice graphicsDevice);
	}
}