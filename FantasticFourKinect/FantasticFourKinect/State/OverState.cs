using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FantasticFourKinect.Skeleton;

namespace FantasticFourKinect.State
{
    class UnderState : AbstractState
    {
        private String part1;
        private String part2;
        private float presicion;

        public UnderState(String p1, String p2, float presicion, string nameState)
            : base(nameState)
        {
            this.part1 = p1;
            this.part2 = p2;
            this.presicion = presicion;
        }

        // TRUE: Si parte 1 esta bajo parte 2 en altura
        // Presicion, refiere a datos reales enviados por el dispositivo
        public override bool cumple(HumanSkeleton hS)
        {
            HumanBodyPart p1 = hS.getBodyPart(part1);
            HumanBodyPart p2 = hS.getBodyPart(part2);
            return (p2.Y - p1.Y) > presicion;
        }

        public override bool cumple(HumanSkeleton hS1, HumanSkeleton hS2)
        {
            HumanBodyPart p1 = hS1.getBodyPart(part1);
            HumanBodyPart p2 = hS2.getBodyPart(part2);
            return (p2.Y - p1.Y) > presicion;
        }
    }
}
