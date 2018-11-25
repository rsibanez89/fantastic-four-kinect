using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FantasticFourKinect.Gesture;

using System.IO;
using Db4objects.Db4o;
using Db4objects.Db4o.Query;

namespace FantasticFourKinect.Persistency
{
    class DB4ObjectGesture
    {
        public static void saveGestures(List<AbstractGesture> myGestures, String path)
        {
            File.Delete(path);//Borro el archivo si es que existe
            IObjectContainer db = Db4oEmbedded.OpenFile(path);
            db.Store(myGestures);
            db.Close();            
        }

        public static List<AbstractGesture> loadGestures(String path)
        {
            try
            {
                IObjectContainer db = Db4oEmbedded.OpenFile(path);
                IObjectSet result = db.QueryByExample(typeof(List<AbstractGesture>));
                List<AbstractGesture> retorno;
                if (result.Count > 0)
                    retorno = (List<AbstractGesture>)result[0];
                else
                    retorno = new List<AbstractGesture>();
                db.Close();
                return retorno;
            }
            catch (IOException e) 
            {
                System.Console.WriteLine("Problemas en la apertura de gestos..." + e);
            }
            return new List<AbstractGesture>();
        }
    }
}
