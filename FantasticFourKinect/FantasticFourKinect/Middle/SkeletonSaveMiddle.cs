using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;
using System.IO;
using FantasticFourKinect.Skeleton;

namespace FantasticFourKinect.Middle
{
    class SkeletonSaveMiddle : AbstractMiddle
    {
        private readonly char SEPARADOR = ';';
        private StreamWriter mySW;
        private List<String> myPaths;

        private List<List<HumanSkeleton>> myRecorder;
        public List<int> cursor;

        public SkeletonSaveMiddle() : base()
        {
            myPaths = new List<String>();
            myRecorder = new List<List<HumanSkeleton>>();
            cursor = new List<int>();
        }

        public SkeletonSaveMiddle(List<String> myPaths, List<List<HumanSkeleton>> myRecorder, List<int> cursor)
        {
            
            this.myPaths = myPaths;
            this.myRecorder = myRecorder;
            this.cursor = cursor;
        }

        public void addUser(HumanSkeleton skeleton, String path)
        {
            this.addUser(skeleton);
            this.myPaths.Add(path);
            List<HumanSkeleton> animation = new List<HumanSkeleton>();
            myRecorder.Add(animation);
            cursor.Add(0);
        }

        public override void update()
        {
            for (int user = 0; user < skeletons.Count; user++)
            {
                HumanSkeleton nuevo = new HumanSkeleton();
                nuevo.init();//lo inicializa con las 15 partes del cuerpo 
                HumanSkeleton.copy(skeletons[user], nuevo);
                myRecorder[user].Add(nuevo);
                cursor[user] += 1;
            }
        }

        public void saveAll()
        {
            for (int user = 0; user < this.skeletons.Count; user++)
                saveUser(user);
        }

        public void saveUser(int user)
        {
            try
            {
                mySW = new StreamWriter(myPaths[user]);
                List<HumanSkeleton> animation = myRecorder[user];
                for (int i = 0; i < animation.Count; i++)
                {
                    String linea = "";
                    foreach (DictionaryEntry dE in animation[i].getBodyParts())
                    {
                        linea += (String)dE.Key + SEPARADOR + ((HumanBodyPart)dE.Value).X + SEPARADOR + ((HumanBodyPart)dE.Value).Y + SEPARADOR + ((HumanBodyPart)dE.Value).Z + SEPARADOR; // ver si se puede guardar la linea entera
                    }
                    mySW.WriteLine(linea);
                }
                closeConection();
            }
            catch (Exception e) { Console.WriteLine("Problemas al abrir el archivo " + e.Message); }
        }

        public override void closeConection()
        {
            if (mySW != null)
                try
                {
                    mySW.Close();
                }
                catch (Exception e) { Console.WriteLine("Problemas al cerrar el archivo " + e.Message); }
        }
    }
}
