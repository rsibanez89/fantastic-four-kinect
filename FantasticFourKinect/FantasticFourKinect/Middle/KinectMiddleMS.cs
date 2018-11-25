using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;
using Microsoft.Research.Kinect.Nui;

using FantasticFourKinect.Skeleton;


namespace FantasticFourKinect.Middle
{
    class KinectMiddleMS : AbstractMiddle
    {
        //variables de MS-SDK
        Runtime nui;
        SkeletonFrame skeletonFrame;

        private int[] skeletonsPosition;
        private DateTime timeLastFrame;
        private readonly TimeSpan interval = new TimeSpan(0,0,1);// si pasa 1 segundo y no se actualizó el frame, significa que se perdió el usuario.

        public KinectMiddleMS() : base()
        {            
            try
            {
                nui = new Runtime();
                //nui.Initialize(RuntimeOptions.UseDepthAndPlayerIndex | RuntimeOptions.UseSkeletalTracking | RuntimeOptions.UseColor);
                nui.Initialize(RuntimeOptions.UseSkeletalTracking);
                nui.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(nui_SkeletonFrameReady);
                timeLastFrame = new DateTime();
                inicializarSkeletonPosition();
                kinectPluggedIn = true;
            }
            catch (InvalidOperationException)
            {
                System.Console.WriteLine("Runtime initialization failed. Please make sure Kinect device is plugged in.");
                kinectPluggedIn = false;
            }
        }

        private void inicializarSkeletonPosition()
        {
            skeletonsPosition = new int[6];
            for (int i = 0; i <= 5; i++)
                skeletonsPosition[i] = -1;
        }

        void nui_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            skeletonFrame = e.SkeletonFrame;
            timeLastFrame = DateTime.Now;

            for (int i = 0; i < skeletons.Count; i++)
            {
                if ((skeletonsPosition[i] == -1) || (SkeletonTrackingState.Tracked != skeletonFrame.Skeletons[skeletonsPosition[i]].TrackingState))
                {
                    skeletonsPosition[i] = -1;
                    asignarUsuario(i);
                }
                else
                    this.setState(i, this.DETECTADO);
            }
        }

        private void asignarUsuario(int user)
        {
            bool localTraked = false;
            //Queda feo, pero hay veces que te trakea en la posición 0 y otras en la posición 5.
            for (int i = 0; (i < skeletonFrame.Skeletons.Length) && (!localTraked); i++)
            {
                if (SkeletonTrackingState.Tracked == skeletonFrame.Skeletons[i].TrackingState)
                    if (!skeletonsPosition.Contains(i)) //ese usuario ya está asignado a otro
                    {
                        skeletonsPosition[user] = i;
                        localTraked = true;
                        this.setState(user, this.DETECTADO);
                    }
            }
            if (!localTraked)
                this.setState(user, this.BUSCANDO); 
        }

        public override void update()
        {
            if ((DateTime.Now - timeLastFrame) < this.interval)
            {
                for (int i = 0; i < skeletons.Count; i++)
                {
                    if (this.getState(i).Equals(this.DETECTADO))
                    {
                        SkeletonData data = skeletonFrame.Skeletons[skeletonsPosition[i]];
                        if (SkeletonTrackingState.Tracked == data.TrackingState)
                        {
                            HumanSkeleton skeleton = skeletons.ElementAt(i);
                            foreach (DictionaryEntry dE in skeleton.getBodyParts())
                            {
                                Joint j = data.Joints[getSkeletonJoint((String)dE.Key)];
                                ((HumanBodyPart)dE.Value).setPosition(j.Position.X, j.Position.Y, j.Position.Z);
                            }
                        }
                        else
                        {
                            skeletonsPosition[i] = -1;
                            this.setState(i, this.BUSCANDO);
                        }
                    }
                }
            }
            else
                loseUsers();
        }

        private void loseUsers()
        {
            inicializarSkeletonPosition();
            for (int user = 0; user < skeletons.Count; user++)
            {
                this.setState(user, this.BUSCANDO);
            }
        }

        private JointID getSkeletonJoint(String joint)
        {
            switch (joint)
            {
                case "Head": return JointID.Head;
                case "Neck": return JointID.ShoulderCenter;
                case "LeftShoulder": return JointID.ShoulderLeft;
                case "LeftElbow": return JointID.ElbowLeft;
                case "LeftHand": return JointID.HandLeft;
                case "RightShoulder": return JointID.ShoulderRight;
                case "RightElbow": return JointID.ElbowRight;
                case "RightHand": return JointID.HandRight;
                case "Torso": return JointID.Spine;
                case "LeftHip": return JointID.HipLeft;
                case "LeftKnee": return JointID.KneeLeft;
                case "LeftFoot": return JointID.FootLeft;
                case "RightHip": return JointID.HipRight;
                case "RightKnee": return JointID.KneeRight;
                case "RightFoot": return JointID.FootRight;
            }
            return JointID.Count;
        }

        public override void closeConection()
        {
            if (kinectPluggedIn)
                try
                {
                    nui.Uninitialize();
                }
                catch (InvalidOperationException)
                {
                    System.Console.WriteLine("Runtime uninitialization failed.");
                }
        }
    }
}
