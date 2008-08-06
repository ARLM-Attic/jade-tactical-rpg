using System;
using JadeEngine;
using JadeEngine.JadeObjects;
using JadeEngine.JadeShaders;
using JadeEngine.JadeInputs;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using JadeEngine.JadeCameras;

namespace Jade
{
    static class Program
    {
        static JadeGame game = new JadeGame();
        static JadeEffect shader = new JadeEffect(@"Content\Shaders\TransformTexture");
        static JadeEffect basicShader = new JadeEffect(@"Content\Shaders\BasicShader");
        static JadeKeyboardDevice keyboard = new JadeKeyboardDevice();
        static JadeMouseDevice mouse = new JadeMouseDevice();
        static bool Clicked { get; set; }

        static string[] sky = {
                                    @"Content/Textures/Skybox/top", 
                                    @"Content/Textures/Skybox/bottom", 
                                    @"Content/Textures/Skybox/left", 
                                    @"Content/Textures/Skybox/right", 
                                    @"Content/Textures/Skybox/front", 
                                    @"Content/Textures/Skybox/back"
                              };

        static JadeSkyBox skybox = new JadeSkyBox(sky);
        static JadeModel sword = new JadeModel(@"Content/Models/KunaiBlack");

        static void Main(string[] args)
        {
            sword.SetScale(new Vector3(0.02f));
            sword.SetPosition(new Vector3(sword.Position.X, sword.Position.Y, sword.Position.Z + 2.5f));
            game.IsMouseVisible = true;

            JadeInputManager.AddDevice(keyboard);
            JadeInputManager.AddDevice(mouse);
            keyboard.OnKeyRelease += keyboard_OnKeyRelease;
            mouse.OnMove += mouse_OnMove;
            mouse.OnScroll += mouse_OnScroll;
            mouse.OnClick += mouse_OnClick;
            mouse.OnRelease += mouse_OnRelease;

            JadeShaderManager.AddEffect("TT", shader);
            JadeShaderManager.AddEffect("BS", basicShader);
            skybox.ShaderLabel = "TT";
            sword.ShaderLabel = "BS";

            JadeObjectManager.AddObject(skybox);
            JadeObjectManager.AddObject(sword);
            game.Run();
        }

        static void mouse_OnRelease(Point position, Collection<JadeMouseButton> buttons)
        {
            if (buttons.Contains(JadeMouseButton.LeftButton))
                Clicked = false;
        }

        static void mouse_OnClick(Point position, Collection<JadeMouseButton> buttons)
        {
            if (buttons.Contains(JadeMouseButton.LeftButton))
                Clicked = true;
        }

        static void mouse_OnScroll(int ticks)
        {
            float distance = ticks / 100.0f;
            JadeCameraManager.ActiveCamera.Translate(new Vector3(0, 0, distance));
        }

        static void mouse_OnMove(Vector2 move)
        {
            if (Clicked)
            {
                JadeCameraManager.ActiveCamera.Revolve(new Vector3(1, 0, 0), move.Y*0.001f);
                JadeCameraManager.ActiveCamera.Revolve(new Vector3(0, 1, 0), move.X*0.001f);
            }
        }

        static void keyboard_OnKeyRelease(Collection<Keys> keys)
        {
            if(keys.Contains(Keys.F))  game.ToggleFullScreen();
        }
    }
}

