using System;
using Microsoft.Xna.Framework;

namespace FantasticFourKinect.Render
{
    public class Camera
    {
        public Matrix view;
        public Vector3 cameraPosition;
        public Vector3 cameraLookAt;
        public Vector3 CapUpVector;

        public Vector3 cameraRot;
        

        public Matrix projection;

        public Camera(GraphicsDeviceManager graphics)
        {
            initView();
            initProjection(graphics);
        }

        public void initView()
        {
            cameraPosition =  new Vector3(0.0f, 0.0f, 5.0f);

            cameraLookAt = new Vector3(0.0f, 0.0f, 0.0f);

            CapUpVector = new Vector3(0.0f, 1.0f, 0.0f);

            cameraRot = new Vector3(0.0f, 0.0f, 0.0f);

            view = Matrix.CreateLookAt(cameraPosition, cameraLookAt, CapUpVector);
        }

        public void initProjection(GraphicsDeviceManager graphics)
        {
            float nearClip = 1.0f;
            float farClip = 10000.0f;
            
            float aspectRatio; 
            aspectRatio = (float)graphics.GraphicsDevice.Viewport.Width / (float)graphics.GraphicsDevice.Viewport.Height;
                       
            float fov;
            fov  = MathHelper.PiOver4;

            projection = Matrix.CreatePerspectiveFieldOfView(fov, aspectRatio, nearClip, farClip);

        }

        public void updateCam()
        {
            view = Matrix.CreateLookAt(cameraPosition, cameraLookAt, CapUpVector) * 
                   Matrix.CreateRotationX(MathHelper.ToRadians(cameraRot.X)) *
                   Matrix.CreateRotationY(MathHelper.ToRadians(cameraRot.Y)) *
                   Matrix.CreateRotationZ(MathHelper.ToRadians(cameraRot.Z));
        }

    }
}
