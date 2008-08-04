using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using JadeEngine.JadeObjects;
using JadeEngine.JadeCameras;

namespace JadeEngine.JadeShaders
{
	public class JadeShader
	{
		private string _asset;
		private Effect _effect;

		private string Asset
		{
			get { return _asset; }
			set { _asset = value; }
		}

		internal Effect MyEffect
		{
			get { return _effect; }
			private set { _effect = value; }
		}

		internal JadeShader()
		{
			Asset = "Microsoft.Xna.Framework.Graphics.BasicEffect";
		}

		public JadeShader(string asset)
		{
			Asset = asset;
		}

		internal void LoadContent(GraphicsDevice graphicsDevice, ContentManager contentManager)
		{
			MyEffect = (Asset.Equals("Microsoft.Xna.Framework.Graphics.BasicEffect"))
				? new BasicEffect(graphicsDevice, null) 
				: contentManager.Load<Effect>(Asset);
		}

        internal void SetParameters(JadeObject obj)
        {
            Matrix world =
                JadeCameraManager.ActiveCamera.World*
                Matrix.CreateScale(obj.Scale)*
                Matrix.CreateFromQuaternion(obj.Rotation)*
                Matrix.CreateTranslation(obj.Position);

            if (null != MyEffect.Parameters["World"]) MyEffect.Parameters["World"].SetValue(world);
            if (null != MyEffect.Parameters["View"]) MyEffect.Parameters["View"].SetValue(JadeCameraManager.ActiveCamera.View);
            if (null != MyEffect.Parameters["Project"]) MyEffect.Parameters["Project"].SetValue(JadeCameraManager.ActiveCamera.Projection);
        }
	}
}
