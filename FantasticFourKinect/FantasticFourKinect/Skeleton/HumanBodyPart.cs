using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FantasticFourKinect.Skeleton
{
    class HumanBodyPart
    {
        public float X;
        public float Y;
        public float Z;
        protected String name;

        public HumanBodyPart(String name)
        {
            X = 0;
            Y = 0;
            Z = 0;
            this.name = name;
        }

        public HumanBodyPart(float x, float y, float z, String name)
        {
            X = x;
            Y = y;
            Z = z;
            this.name = name;
        }

        public String getName()
        {
            return name;
        }

        public void setPosition(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
