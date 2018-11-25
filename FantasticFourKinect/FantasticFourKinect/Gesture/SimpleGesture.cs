using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FantasticFourKinect.Skeleton;
using FantasticFourKinect.State;

namespace FantasticFourKinect.Gesture
{
    class SimpleGesture : AbstractGesture
    {
        public SimpleGesture(String name) : base(name) { }

        public override bool cumple(HumanSkeleton hS)
        {
            if (index < this.states.Count)
            {
                if ((DateTime.Now - this.initGesture) < this.intervalAt(index))
                {//está dentro del periodo de tiempo permitido para ese estado
                    if (this.stateAt(index).cumple(hS))
                        this.move();
                }
                else//se terminó el tiempo permitido, entonces se reinicia el gesto.
                    reset();
                return false;
            }
            else
            {
                this.reset();
                return true;
            }
        }
    }
}
