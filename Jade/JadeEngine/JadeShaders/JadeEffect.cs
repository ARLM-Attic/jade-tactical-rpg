using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

using JadeEngine.JadeObjects;
using JadeEngine.JadeCameras;

namespace JadeEngine.JadeShaders
{
	public class JadeEffect : JadeShader
	{
		internal JadeEffect() : base("Microsoft.Xna.Framework.Graphics.BasicEffect") { }

		public JadeEffect(string asset) : base(asset) { }

        internal override void LoadContent(GraphicsDevice gd, ContentManager cm)
		{
			if(Asset.Equals("Microsoft.Xna.Framework.Graphics.BasicEffect"))
				Effect = new BasicEffect(gd, null);
            else
				base.LoadContent(gd, cm);
		}

        internal override void SetParameters(JadeObject obj)
        {
            Matrix world =
                JadeCameraManager.ActiveCamera.World*
                Matrix.CreateScale(obj.Scale)*
                Matrix.CreateFromQuaternion(obj.Rotation)*
                Matrix.CreateTranslation(obj.Position);

            if (null != Effect.Parameters["World"]) Effect.Parameters["World"].SetValue(world);
            if (null != Effect.Parameters["View"]) Effect.Parameters["View"].SetValue(JadeCameraManager.ActiveCamera.View);
            if (null != Effect.Parameters["Project"]) Effect.Parameters["Project"].SetValue(JadeCameraManager.ActiveCamera.Projection);

            if (Effect.Parameters["EyePosition"] != null)
                Effect.Parameters["EyePosition"].SetValue(JadeCameraManager.ActiveCamera.Position);

            if (Effect.Parameters["LightDirection"] != null)
                Effect.Parameters["LightDirection"].SetValue(new Vector3(1, -1, -1));

            if (Effect.Parameters["LightDiffuseColor"] != null)
                Effect.Parameters["LightDiffuseColor"].SetValue(new Vector3(0.25f, 0.25f, 1.0f));

            if (Effect.Parameters["LightSpecularColor"] != null)
                Effect.Parameters["LightSpecularColor"].SetValue(new Vector3(0.85f, 0.85f, 1.0f));

            base.SetParameters(obj);
        }
    }
}
