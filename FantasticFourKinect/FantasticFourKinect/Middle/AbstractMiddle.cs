using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FantasticFourKinect.Skeleton;

namespace FantasticFourKinect.Middle
{
    abstract class AbstractMiddle
    {
        protected bool kinectPluggedIn;

        protected List<HumanSkeleton> skeletons;
        protected List<String> skeletonsState;

        public readonly String DETECTADO = "DETECTADO";
        public readonly String BUSCANDO = "BUSCANDO";
        public readonly String CALIBRANDO = "CALIBRANDO";

        public AbstractMiddle()
        {
            this.skeletons = new List<HumanSkeleton>();
            this.skeletonsState = new List<String>();
            kinectPluggedIn = false;
        }

        public virtual void reset()
        {
            this.skeletons.Clear();
            this.skeletonsState.Clear();
        }

        public String getState(int user)
        {
            if (user < skeletonsState.Count)
                return skeletonsState[user];
            return BUSCANDO;
        }

        public HumanSkeleton getSkeleton(int user)
        {
            return this.skeletons[user];
        }

        protected void setState(int user, String state)
        {
            skeletonsState[user] = state;
        }

        public void addUser(HumanSkeleton skeleton)
        {
            this.skeletons.Add(skeleton);
            this.skeletonsState.Add(BUSCANDO);
        }

        public bool isKinectPluggedIn()
        {
            return kinectPluggedIn;
        }

        abstract public void update();

        abstract public void closeConection();
    }
}
