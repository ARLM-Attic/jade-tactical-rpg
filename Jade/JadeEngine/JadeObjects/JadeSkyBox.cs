using JadeEngine.JadeObjects.JadeObjectComponents;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using JadeEngine.JadeShaders;
using JadeEngine.JadeCameras;

namespace JadeEngine.JadeObjects
{
    public class JadeSkyBox : JadeObject, IJadeChildRenderer, IJadeLoadable
    {
        private JadeTexturedQuad[] Sides { get; set; }
        private string[] Files { get; set; }
        private Vector3[] Offsets { get; set; }

        public JadeSkyBox(string[] textures)
        {
            Files = textures;
            Scale = new Vector3(500);
            Sides = new JadeTexturedQuad[6];

            CreateSides();
            CalculateOffsets();
        }

        private void CreateSides()
        {
            for(int i = 0; i < 6; i++)
            {
                Sides[i] = new JadeTexturedQuad(Files[i]);
                Sides[i].Scale = Scale;
            }

            Sides[0].Rotation = Quaternion.CreateFromAxisAngle(new Vector3(1, 0, 0), MathHelper.PiOver2);
            Sides[1].Rotation = Quaternion.CreateFromAxisAngle(new Vector3(1, 0, 0), -MathHelper.PiOver2);
            Sides[2].Rotation = Quaternion.CreateFromAxisAngle(new Vector3(0, 1, 0), MathHelper.PiOver2);
            Sides[3].Rotation = Quaternion.CreateFromAxisAngle(new Vector3(0, 1, 0), -MathHelper.PiOver2);
            Sides[5].Rotation = Quaternion.CreateFromAxisAngle(new Vector3(0, 1, 0), MathHelper.Pi);
        }

        private void CalculateOffsets()
        {
            Offsets = new Vector3[6];

            Offsets[0] = new Vector3(0, 0.5f, 0) * Scale;
            Offsets[1] = new Vector3(0, -0.5f, 0) * Scale;
            Offsets[2] = new Vector3(-0.5f, 0, 0) * Scale;
            Offsets[3] = new Vector3(0.5f, 0, 0) * Scale;
            Offsets[4] = new Vector3(0, 0, -0.5f) * Scale;
            Offsets[5] = new Vector3(0, 0, 0.5f) * Scale;
        }

        public void LoadContent(GraphicsDevice gd, ContentManager cm)
        {
            foreach(JadeTexturedQuad quad in Sides)
                quad.LoadContent(gd, cm);
        }

        public void RenderChildren(GraphicsDevice gd)
        {
            JadeShader shader = JadeShaderManager.GetShader(ShaderLabel);

            shader.MyEffect.Begin();
            foreach(EffectPass pass in shader.MyEffect.CurrentTechnique.Passes)
            {
                for(int i = 0; i < 6; i++)
                {
                    Sides[i].Position = JadeCameraManager.ActiveCamera.Position + Offsets[i];

                    shader.SetParameters(Sides[i]);

                    pass.Begin();
                    Sides[i].Render(gd);
                    pass.End();
                }
            }
            shader.MyEffect.End();
        }
    }
}
