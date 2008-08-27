using JadeEngine.JadeObjects.JadeObjectComponents;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using JadeEngine.JadeShaders;
using Microsoft.Xna.Framework;
using JadeEngine.JadeCameras;

namespace JadeEngine.JadeObjects
{
    public class JadeModel : JadeObject, IJadeLoadable, IJadeChildRenderer, IJadeHasMaterial
    {
        private Vector4 _materialDiffuesColor = new Vector4(0, 0.15f, 0, 1);
		private Vector4 _materialSpecularColor = new Vector4(1);
        private float _materialSpecularPower = 3000;

        private string Asset { get; set; }
        private Model Model { get; set; }

        private Vector4 MaterialDiffuseColor { get { return _materialDiffuesColor; } }
        private Vector4 MaterialSpecularColor { get { return _materialSpecularColor; } }
        private float MaterialSpecularPower { get { return _materialSpecularPower; } }

        public JadeModel(string asset)
        {
            Asset = asset;
        }

        public void LoadContent(GraphicsDevice gd, ContentManager cm)
        {
            Model = cm.Load<Model>(Asset);
        }

        public void RenderChildren(GraphicsDevice gd)
        {
            JadeEffect shader = JadeShaderManager.GetShader(ShaderLabel);
            shader.SetParameters(this);
            foreach (ModelMesh mesh in Model.Meshes)
            {
                gd.Indices = mesh.IndexBuffer;
                shader.Effect.Begin();

                foreach(EffectPass pass in shader.Effect.CurrentTechnique.Passes)
                {
                    pass.Begin();
                    foreach(ModelMeshPart part in mesh.MeshParts)
                    {
                        gd.VertexDeclaration = part.VertexDeclaration;
                        gd.Vertices[0].SetSource(mesh.VertexBuffer, part.StreamOffset, part.VertexStride);

                        gd.Textures[0] = ((BasicEffect) part.Effect).Texture;
                        gd.DrawIndexedPrimitives(PrimitiveType.TriangleList, part.BaseVertex, 0, part.NumVertices, part.StartIndex, part.PrimitiveCount);
                    }
                    pass.End();
                }
                shader.Effect.End();
            }
        }

        public void SetMaterialDiffuseColor(Vector4 color)
        {
			_materialDiffuesColor = color;
        }

        public void SetMaterialSpecularColor(Vector4 color)
        {
			_materialSpecularColor = color;
        }
        
        public void SetMaterialSpecularPower(float power)
        {
			_materialSpecularPower = power;
        }

        public void SetMaterialProperties()
        {
            Effect effect = JadeShaderManager.GetShader(ShaderLabel).Effect;

			if (effect.Parameters["MaterialDiffuseColor"] != null)
				effect.Parameters["MaterialDiffuseColor"].SetValue(MaterialDiffuseColor);

			if (effect.Parameters["MaterialSpecularColor"] != null)
				effect.Parameters["MaterialSpecularColor"].SetValue(MaterialSpecularColor);

			if (effect.Parameters["MaterialSpecularPower"] != null)
				effect.Parameters["MaterialSpecularPower"].SetValue(MaterialSpecularPower);
        }
    }
}
