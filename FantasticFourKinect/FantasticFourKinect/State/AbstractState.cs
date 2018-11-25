using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FantasticFourKinect.Skeleton;

namespace FantasticFourKinect.State
{
    abstract class AbstractState
    {
        private string stateName;

        public AbstractState(string stateName)
        {
            this.stateName = stateName;
        }

        public string getStateName()
        {
            return this.stateName;
        }
        
        public abstract bool cumple(HumanSkeleton hS);

        public abstract bool cumple(HumanSkeleton hS1, HumanSkeleton hS2);

        internal void setPrecision(Double p)
        {
            Console.WriteLine("me cambiaron la presición a: " + p);
        }
    }
}
