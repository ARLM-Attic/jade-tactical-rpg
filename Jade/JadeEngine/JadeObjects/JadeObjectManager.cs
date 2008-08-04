using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
namespace JadeEngine.JadeObjects
{
	public class JadeObjectManager
	{
		private static JadeNode _rootNode;

		public static JadeNode RootNode
		{
			get 
			{
				if (_rootNode == null)
					_rootNode = new JadeNode();

				return _rootNode; 
			}
		}

		public static void AddObject(JadeObject obj)
		{
			JadeObjectNode node = new JadeObjectNode(obj);
			RootNode.AddNode(node);
		}

		internal static void Draw(GraphicsDevice graphicsDevice)
		{
			RootNode.Draw(graphicsDevice);
		}

		internal static void LoadContent(GraphicsDevice gd, ContentManager cm)
		{
			RootNode.LoadContent(gd, cm);
		}
	}
}
