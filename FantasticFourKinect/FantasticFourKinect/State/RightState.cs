using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FantasticFourKinect.Skeleton;

namespace FantasticFourKinect.State
{
    class RightState: AbstractState
    {
        private String part1;
        private String part2;
        private float presicion;

        public RightState(String p1, String p2, float presicion, string nameState)
            : base(nameState)
        {
            this.part1 = p1;
            this.part2 = p2;
            this.presicion = presicion;
        }

        // TRUE: Si parte 1 esta a la derecha de parte 2
        // Presicion, refiere a datos reales enviados por el dispositivo
        public override bool cumple(HumanSkeleton hS)
        {
            HumanBodyPart p1 = hS.getBodyPart(part1);
            HumanBodyPart p2 = hS.getBodyPart(part2);
            return (p1.X - p2.X) > presicion;
        }

        public override bool cumple(HumanSkeleton hS1, HumanSkeleton hS2)
        {
            HumanBodyPart p1 = hS1.getBodyPart(part1);
            HumanBodyPart p2 = hS2.getBodyPart(part2);
            return (p1.X - p2.X) > presicion;
        }
    }
}