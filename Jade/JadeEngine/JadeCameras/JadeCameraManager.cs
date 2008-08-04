using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace JadeEngine.JadeCameras
{
    public class JadeCameraManager
    {
        private static Dictionary<string, JadeCamera> _cameras;
        private static JadeCamera _activeCamera;

        private static Dictionary<string, JadeCamera> Cameras
        {
            get
            {
                if(_cameras == null)
                    _cameras = new Dictionary<string, JadeCamera>();
                return _cameras; 
            }
        }

        public static JadeCamera ActiveCamera
        {
            get { return _activeCamera; }
            private set { _activeCamera = value; }
        }

        public static void SetActiveCamera(string cameraLabel)
        {
            if(Cameras.ContainsKey(cameraLabel))
                ActiveCamera = Cameras[cameraLabel];
        }

        public static void AddCamera(string cameraLabel, JadeCamera camera)
        {
            Cameras.Add(cameraLabel, camera);
        }

        internal static void LoadContent(GraphicsDevice gd)
        {
            JadeCamera defaultCamera = new JadeCamera(gd.Viewport);
            Cameras.Add("JadeEngine.JadeCameras.Default", defaultCamera);
            SetActiveCamera("JadeEngine.JadeCameras.Default");
        }

        internal static void UpdateViewports(Viewport viewport)
        {
            foreach(JadeCamera camera in Cameras.Values)
                camera.SetViewport(viewport);
        }
    }
}
