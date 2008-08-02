using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
namespace JadeEngine.JadeObjects
{
	public class JadeNode
	{
		private JadeNodeCollection _childNodes;

		private JadeNodeCollection ChildNodes
		{
			get { return _childNodes; }
			set { _childNodes = value; }
		}

		internal JadeNode()
		{
			ChildNodes = new JadeNodeCollection();
		}

		internal void AddNode(JadeNode newNode)
		{
			ChildNodes.Add(newNode);
		}

		internal virtual void Update(GameTime gameTime)
		{
			foreach (JadeNode child in ChildNodes)
				child.Update(gameTime);
		}

		internal virtual void Draw(GraphicsDevice graphicsDevice)
		{
			foreach (JadeNode child in ChildNodes)
				child.Draw(graphicsDevice);
		}

		internal virtual void LoadContent(GraphicsDevice gd, ContentManager cm)
		{
			foreach (JadeNode node in ChildNodes)
				node.LoadContent(gd, cm);
		}
	}
}
