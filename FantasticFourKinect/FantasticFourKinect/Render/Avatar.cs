using System;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections;

using FantasticFourKinect.Skeleton;

namespace FantasticFourKinect.Render
{
    class Avatar
    {
        private Hashtable esferas;
        private HumanSkeleton skeleton;
        private Matrix translation;

        public Avatar(HumanSkeleton skeleton)
        {
            this.esferas = new Hashtable();
            this.skeleton = skeleton;
            translation = Matrix.Identity;
        }

        public void add(Object3D o3D)
        {
            esferas.Add(o3D.name, o3D);
        }

        public void update()
        {
            foreach (DictionaryEntry dE in skeleton.getBodyParts())
            {
                Object3D esfera = (Object3D)esferas[(String)dE.Key];
                esfera.traslacion = Matrix.CreateTranslation(((HumanBodyPart)dE.Value).X, ((HumanBodyPart)dE.Value).Y, ((HumanBodyPart)dE.Value).Z) * translation;
            }
        }

        public void draw(GameTime gameTime, Matrix vista, Matrix proyeccion)
        {
            foreach (DictionaryEntry dE in esferas)
            {
                Object3D esfera = (Object3D)dE.Value;
                esfera.draw(gameTime, vista, proyeccion);
            }
        }

        public void init(Model esfera)
        {
            foreach (DictionaryEntry dE in skeleton.getBodyParts())
                add(new Object3D((String)dE.Key, new Vector3(0.0f, 0.0f, 0), esfera));
        }

        public void setTranslation(Vector3 t)
        {
            this.translation *= Matrix.CreateTranslation(t);
        }

        internal Object3D get(string nameJoint)
        {
            return (Object3D)esferas[nameJoint];
        }

        public HumanSkeleton getHumanSkeleton() 
        {
            return this.skeleton;
        }
    }
}