using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace Utilities
{
    class GhostCamera
    {
        Vector4 normal;
        Vector4 lookat;
        public Vector4 position;
        float angle;

        public GhostCamera()
        {

            this.normal = new Vector4(0, 0, 1f, 1f);
            this.position = new Vector4(3f, 0f, 1f,1f);
            this.lookat = new Vector4(-0.1f, 0, 0, 1f);
            angle = 0.1f;
        }

        public Matrix4 lookAt()
        {
            return Matrix4.LookAt(position.X, position.Y, position.Z, position.X + lookat.X, position.Y +lookat.Y, position.Z + lookat.Z, normal.X, normal.Y, normal.Z);
        }


        // rotate the vector (vx, vy, vz) around (ax, ay, az) by an angle "angle"

        
        void rotate(ref Vector4 rotated, Vector4 around, float angle)
        {
            Vector3 rot = new Vector3(rotated.X, rotated.Y, rotated.Z);
            Vector3 aro = new Vector3(around.X, around.Y, around.Z);
            double cosin = Math.Cos(angle);
            double sin = Math.Sin(angle);
            Vector3 cross = Vector3.Cross(rot, aro);
            double dot = Vector3.Dot(aro, rot);
            rotated.X *= (float)(cosin + cross.X * sin + dot * aro.X * (1 - cosin));//vx*ca + crossx*sa + dot*ax*(1-ca);
            rotated.Y *= (float)(cosin + cross.Y * sin + dot * aro.Y * (1 - cosin));//vy*ca + crossy*sa + dot*ay*(1-ca);
            rotated.Z *= (float)(cosin + cross.Z * sin + dot * aro.Z * (1 - cosin));//vz*ca + crossz*sa + dot*az*(1-ca);
        }

        Vector4 calculateRightVector()
        {

            Vector3 nor, look, cross;
            nor = new Vector3(normal.X, normal.Y, normal.Z);
            look = new Vector3(lookat.X, lookat.Y, lookat.Z);
            cross = Vector3.Cross(nor, look);

            return new Vector4(cross, 1f);
        }

        public void moveForward()
        {
            position = Vector4.Transform(position, Matrix4.CreateTranslation(lookat.X, lookat.Y, lookat.Z));
            //position += lookat;
        }

        public void moveBack()
        {
            position = Vector4.Transform(position, Matrix4.CreateTranslation(-lookat.X, -lookat.Y, -lookat.Z));
            //position -= lookat;
        }

        public void rotateUp()
        {
            lookat = Vector4.Transform(lookat, Matrix4.CreateRotationX(angle));
            //normal = Vector4.Transform(normal, Matrix4.CreateRotationX(angle));
            //normal = Vector4.Transform(normal, Matrix4.Invert(Matrix4.Transpose(Matrix4.CreateRotationX(angle))));

            /*
            //calculate right vector
            Vector4 rightVector = calculateRightVector();

            //rotate up
            rotate(ref normal, rightVector, angle);
            rotate(ref lookat,rightVector, angle);*/
             
        }

        public void rotateDown()
        {
            lookat = Vector4.Transform(lookat, Matrix4.CreateRotationX(-angle));
            //normal = Vector4.Transform(normal, Matrix4.CreateRotationX(-angle));
            //normal = Vector4.Transform(normal, Matrix4.Invert(Matrix4.Transpose(Matrix4.CreateRotationX(-angle))));
            /*
            //calculate right vector
            Vector4 rightVector = calculateRightVector();

            //rotate down
            rotate(ref normal, rightVector, -angle);
            rotate(ref lookat, rightVector, -angle);*/
        }

        public void rotateLeft()
        {
            lookat = Vector4.Transform(lookat, Matrix4.CreateRotationZ(angle));
            //rotate(ref lookat, normal, -angle); 
        }

        public void rotateRight()
        {
            lookat = Vector4.Transform(lookat, Matrix4.CreateRotationZ(-angle));
            //rotate(ref lookat, normal, angle);
        }


    }
}
