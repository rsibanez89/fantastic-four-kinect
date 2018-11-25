using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

using System.IO;
using FantasticFourKinect.Skeleton;

namespace FantasticFourKinect.Middle
{
    class SkeletonReaderMiddle : AbstractMiddle
    {
        private readonly char SEPARADOR = ';';
        private StreamReader mySR;

        private List<List<HumanSkeleton>> myRecorder;
        public List<int> cursor;

        public SkeletonReaderMiddle() : base()
        {
            myRecorder = new List<List<HumanSkeleton>>();
            cursor = new List<int>();
        }

        public void addUser(HumanSkeleton skeleton, String path)
        {
            try
            {
                mySR = new StreamReader(path);
                this.addUser(skeleton);
                cursor.Add(0);
                crearEstructura();
            }
            catch (Exception e) { Console.WriteLine("Problemas al abrir el archivo " + e.Message); }
        }

        private void crearEstructura()
        {
            try
            {
                String line;
                List<HumanSkeleton> animacion = new List<HumanSkeleton>();
                while ((line = mySR.ReadLine()) != null)
                {
                    HumanSkeleton nuevo = new HumanSkeleton();
                    nuevo.init();//lo inicializa con las 15 partes del cuerpo 
                    char[] delimiterChars = { SEPARADOR };
                    string[] words = line.Split(delimiterChars);
                    for (int i = 0; i < nuevo.getBodyParts().Count * 4; i += 4)
                    {
                        setSkeletonBodyPart(nuevo, words[i], words[i + 1], words[i + 2], words[i + 3]);
                    }
                    animacion.Add(nuevo);
                }
                myRecorder.Add(animacion);
                this.closeConection();
                this.setState(this.skeletons.Count - 1, this.DETECTADO);
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
        }

        private void setSkeletonBodyPart(HumanSkeleton nuevo, string p, string X, string Y, string Z)
        {
            nuevo.getBodyPart(p).setPosition(System.Convert.ToSingle(X), System.Convert.ToSingle(Y), System.Convert.ToSingle(Z));
        }

        public override void update()
        {
            for (int user = 0; user < skeletons.Count; user++)
                update(user);
        }

        public void update(int user)
        {
            if (cursor[user] < myRecorder[user].Count)
                {
                    HumanSkeleton.copy(myRecorder[user].ElementAt(cursor[user]), skeletons[user]);
                    cursor[user]++;
                }
        }

        public void undoUpdate()
        {
            for (int user = 0; user < skeletons.Count; user++)
                undoUpdate(user);
        }

        public void undoUpdate(int user)
        {
            if (cursor[user] > 0)
            {
                cursor[user]--;
                HumanSkeleton.copy(myRecorder[user].ElementAt(cursor[user]), skeletons[user]);
            }
        }

        public override void closeConection()
        {
            if (mySR != null)
            {
                mySR.Close();
                mySR = null;
            }
        }

        public int getCountPosition(int user)
        {
            return myRecorder[user].Count;
        }

        public bool EOF(int user)
        {
            return (cursor[user] == myRecorder[user].Count);
        }

        public override void reset()
        {
            for (int i = 0; i < cursor.Count; i++)
                cursor[i] = 0;
        }

        public void cropAnimation(int i, int f, int user)
        {
            if (cursor[user] > i)
                cursor[user] = 0;
            myRecorder[user].RemoveRange(i, f - i);
        }

        public void saveChange(List<String> myPaths)
        {
            SkeletonSaveMiddle temp = new SkeletonSaveMiddle(myPaths, myRecorder, cursor);
            temp.saveAll();
            temp.closeConection();
        }

        internal void updateToPosition(int p, int user)
        {
            if (cursor[user] < myRecorder[user].Count)
            {
                HumanSkeleton.copy(myRecorder[user].ElementAt(cursor[user]), skeletons[user]);
                cursor[user] = p;
            }
        }
    }
}
