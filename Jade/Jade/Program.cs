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
        public static readonly float ROTATION_SPEED = 0.05f;
        public static readonly float CAMERA_SPEED = 0.001f;

        static JadeGame game = new JadeGame();
        static JadeEffect shader = new JadeEffect(@"Content\Shaders\TransformTexture");
        static JadeEffect basicShader = new JadeEffect(@"Content\Shaders\BasicShader");
        static JadeKeyboardDevice keyboard = new JadeKeyboardDevice();
        static JadeMouseDevice mouse = new JadeMouseDevice();
        static bool Clicked { get; set; }

        static string[] sky = {
                                    @"Content/Textures/Skybox/Stars-01", 
                                    @"Content/Textures/Skybox/Stars-01", 
                                    @"Content/Textures/Skybox/Stars-01", 
                                    @"Content/Textures/Skybox/Stars-01", 
                                    @"Content/Textures/Skybox/Stars-01", 
                                    @"Content/Textures/Skybox/Stars-01"
                              };

        static JadeSkyBox skybox = new JadeSkyBox(sky);
        static JadeModel sword = new JadeModel(@"Content/Models/TestCube");

        static void Main(string[] args)
        {
            sword.SetScale(new Vector3(0.25f));
            sword.SetAmbientLightColor(new Vector3(0.2f));
            sword.SetDiffuseLightColor(new Vector3(0.2f));
            sword.SetSpecularLightColor(new Vector3(0.0f));
            sword.SetPosition(new Vector3(sword.Position.X, sword.Position.Y, sword.Position.Z - 2.5f));

            game.IsMouseVisible = true;

            JadeInputManager.AddDevice(keyboard);
            JadeInputManager.AddDevice(mouse);
            keyboard.OnKeyRelease += keyboard_OnKeyRelease;
            keyboard.OnKeyHeld += keyboard_OnKeyHeld;
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

        static void keyboard_OnKeyHeld(Collection<Keys> keys)
        {
            if (keys.Contains(Keys.Left))
                sword.Rotate(Vector3.Up, -ROTATION_SPEED);

            if (keys.Contains(Keys.Right))
                sword.Rotate(Vector3.Up, ROTATION_SPEED);

            if(keys.Contains(Keys.Up))
                sword.Rotate(Vector3.Right, -ROTATION_SPEED);

            if (keys.Contains(Keys.Down))
                sword.Rotate(Vector3.Right, ROTATION_SPEED);
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
            float distance = ticks / 1000.0f;
            JadeCameraManager.ActiveCamera.Translate(new Vector3(0, 0, distance));
        }

        static void mouse_OnMove(Vector2 move)
        {
            if (Clicked)
            {
				if(JadeCameraManager.ActiveCamera.Target != sword.Position)
					JadeCameraManager.ActiveCamera.SetTarget(sword.Position);

                JadeCameraManager.ActiveCamera.Revolve(Vector3.Right, move.Y * CAMERA_SPEED);
				JadeCameraManager.ActiveCamera.Revolve(Vector3.Up, move.X * CAMERA_SPEED);
            }
        }

        static void keyboard_OnKeyRelease(Collection<Keys> keys)
        {
            if(keys.Contains(Keys.F))  game.ToggleFullScreen();
        }
    }
}

