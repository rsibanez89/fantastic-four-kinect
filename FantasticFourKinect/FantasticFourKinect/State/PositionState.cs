using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FantasticFourKinect.Skeleton;

namespace FantasticFourKinect.State
{
    class PositionState : AbstractState
    {
        private String part1;
        private float presicion;

        public PositionState(String p1, float presicion, string nameState)
            : base(nameState)
        {
            this.part1 = p1;
            this.presicion = presicion;
        }

        //Se calcula la posición relativa al torso.
        public override bool cumple(HumanSkeleton hS)
        {
            HumanBodyPart p1 = hS.getBodyPart(part1);
            HumanBodyPart torso = hS.getBodyPart("Torso");
            return true;
        }

        public override bool cumple(HumanSkeleton hS1, HumanSkeleton hS2)
        {
            return false;//QUE APLICACIÓN TENDRIA?
        }
    }
}
