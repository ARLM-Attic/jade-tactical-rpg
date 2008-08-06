using System;
using System.Collections.Generic;
using System.Text;
using JadeEngine.JadeObjects.JadeObjectComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace JadeEngine.JadeObjects
{
	public class JadeTexturedQuad : JadeObject, IJadeRenderable, IJadeLoadable
	{
		private string _asset;
		private Texture2D _texture;
		private VertexPositionTexture[] _vertices;
		private int[] _inds;

		private string Asset
		{
			get { return _asset; }
			set { _asset = value; }
		}

		private Texture2D MyTexture
		{
			get { return _texture; }
			set { _texture = value; }
		}

		private VertexPositionTexture[] Vertices
		{
			get { return _vertices; }
			set { _vertices = value; }
		}

		private int[] Inds
		{
			get { return _inds; }
			set { _inds = value; }
		}

		public JadeTexturedQuad(string asset)
		{
			Asset = asset;
		}

		public void Render(GraphicsDevice graphicsDevice)
		{
			using(VertexDeclaration declaration = new VertexDeclaration(graphicsDevice, VertexPositionTexture.VertexElements))
			{
				graphicsDevice.VertexDeclaration = declaration;
				graphicsDevice.Textures[0] = MyTexture;
				graphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, Vertices, 0, 4, Inds, 0, 2);
			}
		}

		public void LoadContent(GraphicsDevice graphicsDevice, ContentManager contentManager)
		{
			MyTexture = contentManager.Load<Texture2D>(Asset);

			Vertices = new VertexPositionTexture[]
			           	{
			           		new VertexPositionTexture(new Vector3(-0.5f, 0.5f, 0), new Vector2(0, 0)),
			           		new VertexPositionTexture(new Vector3(0.5f, 0.5f, 0), new Vector2(1, 0)),
			           		new VertexPositionTexture(new Vector3(-0.5f, -0.5f, 0), new Vector2(0, 1)),
			           		new VertexPositionTexture(new Vector3(0.5f, -0.5f, 0), new Vector2(1, 1))
			           	};

			Inds = new int[] {0, 1, 2, 1, 3, 2};
		}

	}
}
