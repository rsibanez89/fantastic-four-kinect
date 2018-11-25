using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FantasticFourKinect.Skeleton;

namespace FantasticFourKinect.State
{
    class AndState : AbstractState
    {
        private AbstractState state1;
        private AbstractState state2;

        public AndState(AbstractState state1, AbstractState state2, string nameState) : base ( nameState )
        {
            this.state1 = state1;
            this.state2 = state2;
        }

        public override bool cumple(HumanSkeleton hS)
        {
            return (state1.cumple(hS) && state2.cumple(hS));
        }

        public override bool cumple(HumanSkeleton hS1, HumanSkeleton hS2)
        {
            return (state1.cumple(hS1,hS2) && state2.cumple(hS1,hS2));
        }
    }
}
