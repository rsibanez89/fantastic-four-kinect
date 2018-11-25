using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FantasticFourKinect.Skeleton;

namespace FantasticFourKinect.State
{
    class InvalidState : AbstractState
    {

        public InvalidState(string nameState) : base(nameState)
        {
        }
        
        public override bool cumple(HumanSkeleton hS)
        {
            return false;
        }

        public override bool cumple(HumanSkeleton hS1, HumanSkeleton hS2)
        {
            return false;
        }
    }
}
