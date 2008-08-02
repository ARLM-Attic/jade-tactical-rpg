using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace JadeEngine.JadeObjects.JadeObjectComponents
{
	public interface IJadeLoadable: IJadeObjectComponent
	{
		void LoadContent(GraphicsDevice graphicsDevice, ContentManager contentManager); 
	}
}
