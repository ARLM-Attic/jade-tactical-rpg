using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using JadeEngine.JadeObjects.JadeObjectComponents;

namespace JadeEngine.JadeObjects
{
	public class JadeObjectNode : JadeNode
	{
		private JadeObject _myObject;

		private JadeObject MyObject
		{
			get { return _myObject; }
			set { _myObject = value; }
		}

		internal JadeObjectNode(JadeObject obj)
		{
			MyObject = obj;
		}

		internal override void Draw(GraphicsDevice graphicsDevice)
		{
			MyObject.Draw(graphicsDevice);
		}

		internal override void LoadContent(GraphicsDevice gd, ContentManager cm)
		{
			if(MyObject is IJadeLoadable)
			{
				((IJadeLoadable) MyObject).LoadContent(gd, cm);
			}
		}
	}
}
