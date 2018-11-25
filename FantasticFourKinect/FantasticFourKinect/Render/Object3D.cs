using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace FantasticFourKinect.Render
{
    public class Object3D
    {
        public Matrix[] transformaciones;
        
        public Matrix rotacion;
        public Matrix traslacion;
        public Matrix escala;

        private Matrix world;
        private Model model;
        public string name;
        

        public Object3D(string name, Vector3 initialPos, Model model) 
        {
            this.model = model;
            this.name = name;

            transformaciones = new Matrix[this.model.Bones.Count];
                        
            world = Matrix.Identity;
            traslacion = Matrix.Identity;
            escala = Matrix.Identity;
            escala = Matrix.CreateScale(0.0005f);

            traslacion *= Matrix.CreateTranslation(initialPos);
        }

        public void draw(GameTime gameTime, Matrix vista, Matrix proyeccion) 
        {
            model.CopyAbsoluteBoneTransformsTo(transformaciones);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect efecto in mesh.Effects) 
                {
                    efecto.EnableDefaultLighting();
                    efecto.PreferPerPixelLighting = true;
                    efecto.World = transformaciones[mesh.ParentBone.Index] * obtenerWorld();
                    efecto.View = vista;
                    efecto.Projection = proyeccion;
                }
                mesh.Draw();
            }
        }

        private Matrix obtenerWorld() 
        {
            return world * escala * traslacion;
        }

        public void setEscala(Vector3 escala)
        {
            this.escala = Matrix.CreateScale(escala);
        }

        internal void setModel(Model esferaSelected)
        {
            this.model = esferaSelected;
        }
    }
}
