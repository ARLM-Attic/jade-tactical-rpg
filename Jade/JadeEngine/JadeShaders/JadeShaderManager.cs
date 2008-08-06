using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace JadeEngine.JadeShaders
{
	public class JadeShaderManager
	{
		private static Dictionary<string, JadeEffect> _effects;
	    private static Dictionary<string, JadeShader> _postProcessors;

		private static Dictionary<string, JadeEffect> Effects
		{
			get
			{
				if (_effects == null)
					_effects = new Dictionary<string, JadeEffect>();

				return _effects;
			}
			set { _effects = value; }
		}
        private static Dictionary<string, JadeShader> PostProcessors
        {
            get
            {
                if (_postProcessors == null)
                    _postProcessors = new Dictionary<string, JadeShader>();

                return _postProcessors;
            }
            set { _postProcessors = value; }
        }

		public static void AddEffect(string shaderLabel, JadeEffect shader)
		{
			Effects.Add(shaderLabel, shader);
		}

        public static void AddPostProcessor(string shaderLabel, JadePostProcessor postProcessor)
        {
            PostProcessors.Add(shaderLabel, postProcessor);
        }

		internal static JadeEffect GetShader(string shaderLabel)
		{
			if(!String.IsNullOrEmpty(shaderLabel) && Effects.ContainsKey(shaderLabel))
				return Effects[shaderLabel];

			return Effects["Microsoft.Xna.Framework.Graphics.BasicEffect"];
		}

		internal static void Initalize()
		{
			JadeEffect basicEffect = new JadeEffect();
			Effects.Add("Microsoft.Xna.Framework.Graphics.BasicEffect", basicEffect);
		}

		internal static void LoadContent(GraphicsDevice gd, ContentManager cm)
		{
			foreach (JadeShader shader in Effects.Values)
				shader.LoadContent(gd, cm);

            foreach (JadeShader shader in PostProcessors.Values)
                shader.LoadContent(gd, cm);
		}

        internal static void PostProcess(GraphicsDevice gd)
        {
            foreach(JadePostProcessor post in PostProcessors.Values)
                post.Draw(gd);
        }
	}
}
