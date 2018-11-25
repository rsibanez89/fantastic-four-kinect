using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace FantasticFourKinect.Skeleton
{
    class HumanSkeleton
    {
        private Hashtable skeleton;
        public float minX;
        public float minY;
        public float minZ;
        public float maxX;
        public float maxY;
        public float maxZ;
        public readonly float MASINF = 1000000;
        public readonly float MENOSINF = -1000000;

        public HumanSkeleton()
        {
            skeleton = new Hashtable();
            minX = MASINF;
            minY = MASINF;
            minZ = MASINF;
            maxX = MENOSINF;
            maxY = MENOSINF;
            maxZ = MENOSINF;
        }

        #region Se cargan las partes del cuerpo sin especificar la posición.
        public void init()
        {
            this.addBodyPart(new HumanBodyPart("Head")); // Cabeza
            this.addBodyPart(new HumanBodyPart("Neck")); // Cuello

            this.addBodyPart(new HumanBodyPart("LeftShoulder")); // Hombro Izquierdo
            this.addBodyPart(new HumanBodyPart("LeftElbow")); // Codo Izquierdo
            this.addBodyPart(new HumanBodyPart("LeftHand")); // Mano Izquierda

            this.addBodyPart(new HumanBodyPart("RightShoulder")); // Hombro Derecho
            this.addBodyPart(new HumanBodyPart("RightElbow")); // Codo Derecho
            this.addBodyPart(new HumanBodyPart("RightHand")); // Mano Derecha

            this.addBodyPart(new HumanBodyPart("Torso")); // Torso

            this.addBodyPart(new HumanBodyPart("LeftHip")); // Cadera Izquierda
            this.addBodyPart(new HumanBodyPart("LeftKnee")); // Rodilla Izquierda
            this.addBodyPart(new HumanBodyPart("LeftFoot")); // Pie Izquierdo

            this.addBodyPart(new HumanBodyPart("RightHip")); // Cadera Derecha
            this.addBodyPart(new HumanBodyPart("RightKnee")); // Rodilla Derecha
            this.addBodyPart(new HumanBodyPart("RightFoot")); // Pie Derecho
        }
        #endregion


        public void addBodyPart(HumanBodyPart bodyPart)
        {
            this.skeleton.Add(bodyPart.getName(), bodyPart);
        }

        public HumanBodyPart getBodyPart(String name)
        {
            if (skeleton.ContainsKey(name))
                return (HumanBodyPart)skeleton[name];
            return null;
        }

        public Hashtable getBodyParts()
        {
            return skeleton;
        }

        // Obtiene los puntos máximos y mínimos detectados por la cámara.
        public void update()
        {
            bool change = false;
            foreach (DictionaryEntry dE in skeleton)
            {
                float x = ((HumanBodyPart)dE.Value).X;
                float y = ((HumanBodyPart)dE.Value).Y;
                float z = ((HumanBodyPart)dE.Value).Z;
                if (x > maxX)
                {
                    maxX = x;
                    change = true;
                }
                if (x < minX)
                {
                    minX = x;
                    change = true;
                }
                if (y > maxY)
                {
                    maxY = y;
                    change = true;
                }
                if (y < minY)
                {
                    minY = y;
                    change = true;
                }
                if (z > maxZ)
                {
                    maxZ = z;
                    change = true;
                }
                if (z < minZ)
                {
                    minZ = z;
                    change = true;
                }
            }
            if (change)
                System.Console.WriteLine("[" + minX + "," + maxX + "]" + " ; " + "[" + minY + "," + maxY + "]" + " ; " + "[" + minZ + "--" + maxZ + "]");
        }

        public static void copy(HumanSkeleton sOrigin, HumanSkeleton sDest)
        {
            foreach (DictionaryEntry dE in sOrigin.getBodyParts())
            {
                sDest.getBodyPart((String)dE.Key).setPosition( ((HumanBodyPart)dE.Value).X, ((HumanBodyPart)dE.Value).Y, ((HumanBodyPart)dE.Value).Z );
            }
        }
    }
}
