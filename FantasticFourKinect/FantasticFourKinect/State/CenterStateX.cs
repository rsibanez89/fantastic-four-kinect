using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FantasticFourKinect.Skeleton;

namespace FantasticFourKinect.State
{
    class CenterStateX : AbstractState
    {
        private String part1;
        private String part2;
        private float precision;

        public CenterStateX(String p1, String p2, float precision, string nameState)
            : base(nameState)
        {
            this.part1 = p1;
            this.part2 = p2;
            this.precision = precision;
        }

        // TRUE: Si parte 1 esta en el mismo valor de x que la parte 2
        // Presicion, refiere a datos reales enviados por el dispositivo y aca a valores chicos de presicion mayor es la precision.
        public override bool cumple(HumanSkeleton hS)
        {
            HumanBodyPart p1 = hS.getBodyPart(part1);
            HumanBodyPart p2 = hS.getBodyPart(part2);
            return (Math.Abs(p1.X - p2.X) < precision);
        }

        public override bool cumple(HumanSkeleton hS1, HumanSkeleton hS2)
        {
            HumanBodyPart p1 = hS1.getBodyPart(part1);
            HumanBodyPart p2 = hS2.getBodyPart(part2);
            return (Math.Abs(p1.X - p2.X) < precision);
        }
    }
}
