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
        private Vector3 _ambientColor = new Vector3(0.25f);
        private Vector3 _diffuesColor = new Vector3(0.50f);
        private Vector3 _specularColor = new Vector3(0.1f);
        private float _specularPower = 32;

        private string Asset { get; set; }
        private Model Model { get; set; }

        private Vector3 AmbientLightColor { get { return _ambientColor; } }
        private Vector3 DiffuseLightColor { get { return _diffuesColor; } }
        private Vector3 SpecularLightColor { get { return _specularColor; } }
        private float SpecularPower { get { return _specularPower; } }

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

        public void SetAmbientLightColor(Vector3 color)
        {
            _ambientColor = color;
        }

        public void SetDiffuseLightColor(Vector3 color)
        {
            _diffuesColor = color;
        }

        public void SetSpecularLightColor(Vector3 color)
        {
            _specularColor = color;
        }
        
        public void SetSpecularPower(float power)
        {
            _specularPower = power;
        }

        public void SetMaterialProperties()
        {
            Effect effect = JadeShaderManager.GetShader(ShaderLabel).Effect;

            if(effect.Parameters["AmbientLightColor"] != null)
                effect.Parameters["AmbientLightColor"].SetValue(AmbientLightColor);

            if (effect.Parameters["EyePosition"] != null) 
                effect.Parameters["EyePosition"].SetValue(JadeCameraManager.ActiveCamera.Position);

            if (effect.Parameters["LightDirection"] != null)
                effect.Parameters["LightDirection"].SetValue(new Vector3(1, -1, -1));

            if (effect.Parameters["LightDiffuseColor"] != null)
                effect.Parameters["LightDiffuseColor"].SetValue(DiffuseLightColor);

            if (effect.Parameters["DiffuseColor"] != null) 
                effect.Parameters["DiffuseColor"].SetValue(DiffuseLightColor);

            if (effect.Parameters["LightSpecularColor"] != null)
                effect.Parameters["LightSpecularColor"].SetValue(SpecularLightColor);

            if (effect.Parameters["SpecularPower"] != null) 
                effect.Parameters["SpecularPower"].SetValue(SpecularPower);
        }
    }
}
