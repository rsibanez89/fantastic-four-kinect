using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FantasticFourKinect.Skeleton;
using FantasticFourKinect.State;

namespace FantasticFourKinect.Gesture
{
    abstract class AbstractGesture
    {
        protected List<Pair> states;
        protected int index;
        protected String name;
        protected DateTime initGesture;

        public AbstractGesture(String name)
        {
            this.states = new List<Pair>();
            this.index = 0;
            this.name = name;
        }

        public void addState(AbstractState newState, TimeSpan interval)
        {
            this.states.Add(new Pair(newState, interval));
        }

        public void addPairStateTime(Pair p)
        {
            this.states.Add(p);
        }

        public void start()
        {
            this.initGesture = DateTime.Now;
        }

        protected void move()
        {
            this.start();
            this.index++;
        }

        protected void reset()
        {
            this.index = 0;
            this.start();
        }

        public AbstractState stateAt(int index)
        {
            return (this.states.ElementAt(index)).state; 
        }

        public TimeSpan intervalAt(int index)
        {
            return (this.states.ElementAt(index)).interval;
        }

        public String getGestureName()
        {
            return this.name;
        }

        public abstract bool cumple(HumanSkeleton hS);

    }

    class Pair
    {
        public AbstractState state;
        public TimeSpan interval;

        public Pair(AbstractState state, TimeSpan interval)
        {
            this.state = state;
            this.interval = interval;
        }
    }
}
