using JadeEngine.JadeShaders;
using Microsoft.Xna.Framework;
using JadeEngine.JadeObjects.JadeObjectComponents;
using Microsoft.Xna.Framework.Graphics;

namespace JadeEngine.JadeObjects
{
	public class JadeObject
	{
	    protected string _shaderLabel;

	    public Vector3 Position { get; set; }
	    public Vector3 Scale { get; set; }
	    public Quaternion Rotation { get; set; }

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
                shader.SetParameters(this);

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
