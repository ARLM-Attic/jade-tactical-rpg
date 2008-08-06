using JadeEngine.JadeObjects.JadeObjectComponents;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using JadeEngine.JadeShaders;

namespace JadeEngine.JadeObjects
{
    public class JadeModel : JadeObject, IJadeLoadable, IJadeChildRenderer
    {
        private string Asset { get; set; }
        private Model Model { get; set; }

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
            JadeShader shader = JadeShaderManager.GetShader(ShaderLabel);
            shader.SetParameters(this);
            foreach (ModelMesh mesh in Model.Meshes)
            {
                gd.Indices = mesh.IndexBuffer;
                shader.MyEffect.Begin();

                foreach(EffectPass pass in shader.MyEffect.CurrentTechnique.Passes)
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
                shader.MyEffect.End();
            }
        }
    }
}
