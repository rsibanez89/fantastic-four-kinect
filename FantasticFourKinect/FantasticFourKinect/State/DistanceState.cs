using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FantasticFourKinect.Skeleton;

namespace FantasticFourKinect.State
{
    class DistanceState : AbstractState
    {
        private String part1;
        private String part2;
        private float presicion;

        public DistanceState(String p1, String p2, float presicion, string nameState)
            : base(nameState)
        {
            this.part1 = p1;
            this.part2 = p2;
            this.presicion = presicion;
        }

        public float getDistance(HumanSkeleton hS)
        {
            return getDistance(hS, hS);
        }

        public override bool cumple(HumanSkeleton hS)
        {
            return (getDistance(hS) < presicion);
        }

        public float getDistance(HumanSkeleton hS1, HumanSkeleton hS2)
        {
            HumanBodyPart p1 = hS1.getBodyPart(part1);
            HumanBodyPart p2 = hS2.getBodyPart(part2);
            double partialX = Math.Pow(p1.X - p2.X, 2);
            double partialY = Math.Pow(p1.Y - p2.Y, 2);
            double partialZ = Math.Pow(p1.Z - p2.Z, 2);

            return (float)Math.Sqrt(partialX + partialY + partialZ);
        }

        public override bool cumple(HumanSkeleton hS1, HumanSkeleton hS2)
        {
            return (getDistance(hS1,hS2) < presicion);
        }
    }
}
