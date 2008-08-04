using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using JadeEngine.JadeObjects;
using JadeEngine.JadeShaders;
using JadeEngine.JadeCameras;

namespace JadeEngine
{
	public class JadeGame : Game
	{
		private GraphicsDeviceManager _gdm;

		public JadeGame()
		{
			GDM = new GraphicsDeviceManager(this);

			Window.AllowUserResizing = true;
			Window.ClientSizeChanged += Window_ClientSizeChanged;
		}

		void Window_ClientSizeChanged(object sender, System.EventArgs e)
		{
			GDM.PreferredBackBufferWidth = Window.ClientBounds.Width;
			GDM.PreferredBackBufferHeight = Window.ClientBounds.Height;
            JadeCameraManager.UpdateViewports(GraphicsDevice.Viewport);
		}

		private GraphicsDeviceManager GDM
		{
			get
			{
				if (_gdm == null)
					_gdm = new GraphicsDeviceManager(this);
				return _gdm;
			}
			set { _gdm = value; }
		}

		protected override void Initialize()
		{
			JadeShaderManager.Initalize();
			base.Initialize();
		}

		protected override void LoadContent()
		{
            JadeCameraManager.LoadContent(GraphicsDevice);
			JadeObjectManager.RootNode.LoadContent(GraphicsDevice, Content);
			JadeShaderManager.LoadContent(GraphicsDevice, Content);
			base.LoadContent();
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);
			JadeObjectManager.Draw(GraphicsDevice);
			base.Draw(gameTime);
		}

		protected override void Update(GameTime gameTime)
		{
			KeyboardState keyboardState = Keyboard.GetState();

			if(keyboardState.IsKeyDown(Keys.F))
				GDM.ToggleFullScreen();
            JadeCameraManager.ActiveCamera.Update();
			base.Update(gameTime);
		}
	}
}
