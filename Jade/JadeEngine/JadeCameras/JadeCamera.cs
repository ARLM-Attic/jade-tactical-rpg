using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JadeEngine.JadeCameras
{
    public class JadeCamera
    {
        public Vector3 Target { get; private set; }
        public Vector3 Position { get; private set; }
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
            Target = new Vector3(0, 0, 0);
            Rotation = new Quaternion(0, 0, 0, 1);

            SetViewport(viewport);
        }

        public void SetTarget(Vector3 target)
        {
            Target = target;
        }

        public void SetViewport(Viewport viewport)
        {
            _viewport = viewport;

            _viewport.MinDepth = 1.0f;
            _viewport.MaxDepth = 1000.0f;
        }

        public void Rotate(Vector3 axis, float angle)
        {
            axis = Vector3.Transform(axis, Matrix.CreateFromQuaternion(Rotation));
            Rotation = Quaternion.Normalize(Quaternion.CreateFromAxisAngle(axis, angle)*Rotation);
            Update();
        }

        public void Translate(Vector3 distance)
        {
            Position += Vector3.Transform(distance, Matrix.CreateFromQuaternion(Rotation));
            Update();
        }

        public void Revolve(Vector3 axis, float angle)
        {
            Vector3 revolveAxis = Vector3.Transform(axis, Matrix.CreateFromQuaternion(Rotation));
            Quaternion rotate = Quaternion.CreateFromAxisAngle(revolveAxis, angle);
            Position = Vector3.Transform(Position - Target, Matrix.CreateFromQuaternion(rotate)) + Target;
            Rotate(axis, angle);
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
