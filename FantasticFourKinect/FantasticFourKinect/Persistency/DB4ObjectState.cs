using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FantasticFourKinect.State;

using System.IO;
using Db4objects.Db4o;
using Db4objects.Db4o.Query;

namespace FantasticFourKinect.Persistency
{
    class DB4ObjectState
    {
        public static void saveStates(List<AbstractState> myStates, String path)
        {
            File.Delete(path);//Borro el archivo si es que existe
            IObjectContainer db = Db4oEmbedded.OpenFile(path);
            db.Store(myStates);
            db.Close();            
        }

        public static List<AbstractState> loadStates(String path)
        {
            try
            {
                IObjectContainer db = Db4oEmbedded.OpenFile(path);
                IObjectSet result = db.QueryByExample(typeof(List<AbstractState>));
                List<AbstractState> retorno;
                if (result.Count > 0)
                    retorno = (List<AbstractState>)result[0];
                else
                    retorno = new List<AbstractState>();
                db.Close();
                return retorno;
            }
            catch (IOException e) 
            {
                System.Console.WriteLine("Problemas en la apertura de estados..." + e);
            }
            return new List<AbstractState>();
        }
    }
}
