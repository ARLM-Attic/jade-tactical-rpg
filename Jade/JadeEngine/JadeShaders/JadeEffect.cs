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

			Matrix worldIT = new Matrix();
        	Matrix.Invert(ref world, out worldIT);
			Matrix.Transpose(ref worldIT, out worldIT);

			if (null != Effect.Parameters["World"]) Effect.Parameters["World"].SetValue(world);
			if (null != Effect.Parameters["WorldIT"]) Effect.Parameters["WorldIT"].SetValue(worldIT);
			if (null != Effect.Parameters["View"]) Effect.Parameters["View"].SetValue(JadeCameraManager.ActiveCamera.View);
            if (null != Effect.Parameters["Project"]) Effect.Parameters["Project"].SetValue(JadeCameraManager.ActiveCamera.Projection);

            if (Effect.Parameters["EyePosition"] != null)
                Effect.Parameters["EyePosition"].SetValue(JadeCameraManager.ActiveCamera.Position);

			if (Effect.Parameters["AmbientLightColor"] != null)
				Effect.Parameters["AmbientLightColor"].SetValue(new Vector3(0.15f));

			if (Effect.Parameters["EmissiveLightColor"] != null)
				Effect.Parameters["EmissiveLightColor"].SetValue(new Vector3(0, 0.15f, 0));

			if (Effect.Parameters["DirLightColor"] != null)
				Effect.Parameters["DirLightColor"].SetValue(new Vector3(0, 0, 0.15f));

			if (Effect.Parameters["AmbientLightPosition"] != null)
				Effect.Parameters["AmbientLightPosition"].SetValue(new Vector3(0, 0, -3));

            base.SetParameters(obj);
        }
    }
}
