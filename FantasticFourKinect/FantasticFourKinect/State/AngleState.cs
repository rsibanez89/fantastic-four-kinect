using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FantasticFourKinect.Skeleton;

namespace FantasticFourKinect.State
{
    class AngleState : AbstractState
    {
        private String part1;
        private String part2;
        private String part3;
        private Vector3D v1;
        private Vector3D v2;
        private float angulo;
        private float precision;

        public AngleState(String p1, String p2, String p3, float angulo, float precision, string nameState)
            : base(nameState)
        {
            this.part1 = p1;
            this.part2 = p2;
            this.part3 = p3;
            this.angulo = angulo;
            this.precision = precision;
            v1 = new Vector3D();
            v2 = new Vector3D();
        }

        private void update(HumanSkeleton hS)
        {
            HumanBodyPart p1 = hS.getBodyPart(part1);
            HumanBodyPart p2 = hS.getBodyPart(part2);
            HumanBodyPart p3 = hS.getBodyPart(part3);
            v1.set(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);
            v2.set(p3.X - p2.X, p3.Y - p2.Y, p3.Z - p2.Z);
        }

        private double productoEscalar()
        {
            return (v1.X * v2.X) + (v1.Y * v2.Y) + (v1.Z * v2.Z);
        }

        private double getCosAngulo()
        {
            double moduloV1 = Math.Sqrt(v1.X * v1.X + v1.Y * v1.Y + v1.Z * v1.Z);
            double moduloV2 = Math.Sqrt(v2.X * v2.X + v2.Y * v2.Y + v2.Z * v2.Z);

            return (productoEscalar() / (moduloV1 * moduloV2));
        }

        //esta funcion es para ver que tira despues hay que eliminarla
        public double getAngulo(HumanSkeleton hS)
        {
            this.update(hS);
            return Math.Acos(getCosAngulo());
        }

        public override bool cumple(HumanSkeleton hS)
        {
            
            this.update(hS);
            double angulo = Math.Acos(getCosAngulo());
            if (angulo > 0 && angulo < 90)
                return true;
            return false;
        }

        public override bool cumple(HumanSkeleton hS1, HumanSkeleton hS2)
        {
            return false;//QUE APLICACIÓN TENDRIA?
        }
    }

    class Vector3D
    {
        public float X; 
        public float Y; 
        public float Z;

        public Vector3D()
        {
            this.set(0, 0, 0);
        }

        public Vector3D(float x, float y, float z)
        {
            this.set(x, y, z);
        }

        public void set(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

    }
}
