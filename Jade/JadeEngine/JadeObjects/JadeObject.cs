using JadeEngine.JadeShaders;
using Microsoft.Xna.Framework;
using JadeEngine.JadeObjects.JadeObjectComponents;
using Microsoft.Xna.Framework.Graphics;

namespace JadeEngine.JadeObjects
{
	public class JadeObject
	{
	    protected string _shaderLabel;

	    private Vector3 _position;
        private Vector3 _scale;
        private Quaternion _rotation;

        public Vector3 Position
	    {
	        get { return _position; }
	    }
        public Vector3 Scale
	    {
	        get { return _scale; }
	    }
	    public Quaternion Rotation
	    {
	        get { return _rotation; }
	    }

        public void SetPosition(Vector3 newPosition)
        {
            _position = newPosition; 
        }
        public void SetScale(Vector3 scale)
        {
            _scale = scale;
        }
        public void SetRotation(Quaternion rotation)
        {
            _rotation = rotation;
        }

	    public string ShaderLabel
		{
			protected get { return _shaderLabel; }
			set { _shaderLabel = value; }
		}

		public JadeObject()
		{
            SetPosition(new Vector3(0));
			SetScale(new Vector3(1));
			SetRotation(new Quaternion(0,0,0,1));
		}

		public void Draw(GraphicsDevice gd)
		{
            if(this is IJadeHasMaterial) ((IJadeHasMaterial)this).SetMaterialProperties();

			if (this is IJadeRenderable)
			{
				JadeEffect shader = JadeShaderManager.GetShader(ShaderLabel);
                shader.SetParameters(this);

				shader.Effect.Begin();
				foreach(EffectPass pass in shader.Effect.CurrentTechnique.Passes)
				{
					pass.Begin();
					((IJadeRenderable) this).Render(gd);
					pass.End();
				}
				shader.Effect.End();
			}

            if(this is IJadeChildRenderer)((IJadeChildRenderer)this).RenderChildren(gd);
		}

        public void Rotate(Vector3 axis, float angle)
        {
            axis = Vector3.Transform(axis, Matrix.CreateFromQuaternion(Rotation));
            SetRotation(Quaternion.Normalize(Quaternion.CreateFromAxisAngle(axis, angle) * Rotation));
        }
	}
}
