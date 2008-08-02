using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace JadeEngine.JadeShaders
{
	public class JadeShaderManager
	{
		private static Dictionary<string, JadeShader> _shaders;

		private static Dictionary<string, JadeShader> Shaders
		{
			get
			{
				if (_shaders == null)
					_shaders = new Dictionary<string, JadeShader>();

				return _shaders;
			}
			set { _shaders = value; }
		}

		public static void AddShader(string shaderLabel, JadeShader shader)
		{
			Shaders.Add(shaderLabel, shader);
		}

		internal static JadeShader GetShader(string shaderLabel)
		{
			if(!String.IsNullOrEmpty(shaderLabel) && Shaders.ContainsKey(shaderLabel))
				return Shaders[shaderLabel];

			return Shaders["Microsoft.Xna.Framework.Graphics.BasicEffect"];
		}

		internal static void Initalize()
		{
			JadeShader basicEffect = new JadeShader();
			Shaders.Add("Microsoft.Xna.Framework.Graphics.BasicEffect", basicEffect);
		}

		internal static void LoadContent(GraphicsDevice graphicsDevice, ContentManager contentManager)
		{
			foreach (JadeShader shader in Shaders.Values)
				shader.LoadContent(graphicsDevice, contentManager);
		}
	}
}
