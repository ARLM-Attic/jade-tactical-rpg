using System;
using JadeEngine;
using JadeEngine.JadeObjects;
using JadeEngine.JadeShaders;

namespace Jade
{
    static class Program
    {
        static JadeGame game = new JadeGame();
        static JadeTexturedQuad quad = new JadeTexturedQuad(@"Content\Textures\Vagina");
        static JadeShader shader = new JadeShader(@"Content\Shaders\TransformTexture"); 

        static void Main(string[] args)
        {
            JadeShaderManager.AddShader("TT", shader);
            quad.ShaderLabel = "TT";
            JadeObjectManager.AddObject(quad);

            game.Run();
        }
    }
}

