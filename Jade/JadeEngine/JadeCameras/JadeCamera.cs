using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JadeEngine.JadeCameras
{
    public class JadeCamera
    {
        private Vector3 Position { get; set; }
        private Quaternion Rotation { get; set; }
        public Matrix World { get; private set; }
        public Matrix View { get; private set; }
        public Matrix Projection { get; private set; }

        private Viewport _viewport;
        public Viewport MyViewport
        {
            get { return _viewport; }
        }

        public JadeCamera(Viewport viewport)
        {
            Position = new Vector3(0, 0, 1);
            Rotation = new Quaternion(0, 0, 0, 1);

            SetViewport(viewport);
        }

        public void SetViewport(Viewport viewport)
        {
            _viewport = viewport;

            _viewport.MinDepth = 1.0f;
            _viewport.MaxDepth = 1000.0f;
        }

        public void Update()
        {
            World = Matrix.Identity;
            View = Matrix.Invert(Matrix.CreateFromQuaternion(Rotation) * Matrix.CreateTranslation(Position));
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.Pi/3.0f,
                                                             (float) MyViewport.Width/(float) MyViewport.Height,
                                                             MyViewport.MinDepth, MyViewport.MaxDepth);
        }
    }
}
