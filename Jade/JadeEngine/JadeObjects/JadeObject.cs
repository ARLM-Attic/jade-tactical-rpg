using JadeEngine.JadeShaders;
using Microsoft.Xna.Framework;
using JadeEngine.JadeObjects.JadeObjectComponents;
using Microsoft.Xna.Framework.Graphics;

namespace JadeEngine.JadeObjects
{
	public class JadeObject
	{
		private Vector3 _position;
		private Vector3 _scale;
		private Quaternion _rotation;
		protected string _shaderLabel;

		public Vector3 Position
		{
			get { return _position; }
			set { _position = value; }
		}

		public Vector3 Scale
		{
			get { return _scale; }
			set { _scale = value; }
		}

		public Quaternion Rotation
		{
			get { return _rotation; }
			set { _rotation = value; }
		}

		public string ShaderLabel
		{
			protected get { return _shaderLabel; }
			set { _shaderLabel = value; }
		}

		public JadeObject()
		{
			Position = new Vector3(0);
			Scale = new Vector3(1);
			Rotation = new Quaternion(0,0,0,1);
		}

		public void Draw(GraphicsDevice graphicsDevice)
		{
			if (this is IJadeRenderable)
			{
				JadeShader shader = JadeShaderManager.GetShader(ShaderLabel);
                shader.SetParameters();

				shader.MyEffect.Begin();
				foreach(EffectPass pass in shader.MyEffect.CurrentTechnique.Passes)
				{
					pass.Begin();
					((IJadeRenderable) this).Render(graphicsDevice);
					pass.End();
				}
				shader.MyEffect.End();
			}
		}
	}
}
