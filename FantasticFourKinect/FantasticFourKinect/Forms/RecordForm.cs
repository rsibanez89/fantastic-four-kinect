using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using System.Collections;
using FantasticFourKinect.Middle;
using FantasticFourKinect.Skeleton;
using FantasticFourKinect.State;
using FantasticFourKinect.Gesture;
using FantasticFourKinect.Render;

namespace FantasticFourKinect.Forms
{

    class RecordForm : AbstractForm
    {
        #region Declaración de variables

        // Variables Propias
        private HumanSkeleton mySkeleton;
        private AbstractMiddle myMiddle;
        private SkeletonSaveMiddle mySkeletonSave;
        private String path;
        private bool saved;

        // Variables para renderizar
        private Avatar myAvatar;
        private Camera camara;
        private Object3D suelo;
        
        #endregion

        #region Constructor
        public RecordForm(Principal pM, AbstractMiddle am) : base (pM)
        {
            mySkeleton = new HumanSkeleton();

            pathSelection();

            myMiddle = am;
            myMiddle.reset();
            myMiddle.addUser(mySkeleton);

            // Para iniciar el grabador del HumanSkeleton
            mySkeletonSave = new SkeletonSaveMiddle();
            mySkeletonSave.addUser(mySkeleton, path);

            saved = false;

            // Render
            myAvatar = new Avatar(mySkeleton);
            camara = new Camera(pM.graphics);

            this.LoadContent();
        }
        #endregion

        #region Seleccion de la ruta
        private void pathSelection()
        {
            using (System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog())
            {
                saveFileDialog1.DefaultExt = ".ani";
                saveFileDialog1.Filter = "Archivos de animación (*.ani) | *.ani";
                saveFileDialog1.InitialDirectory = Environment.CurrentDirectory + "\\Data\\Animation";
                if (saveFileDialog1.ShowDialog() != System.Windows.Forms.DialogResult.None)
                {
                        if (saveFileDialog1.FileName.EndsWith(".ani"))
                            path = saveFileDialog1.FileName;
                        else
                            path = saveFileDialog1.FileName + ".ani";
                }
                
            }
        }
        #endregion

        #region Cargando estructuras
        public void LoadContent()
        {
            Model modeloSuelo = pM.Content.Load<Model>("fbx/suelo");
            suelo = new Object3D("Suelo", new Vector3(0.0f, -1.2f, 0), modeloSuelo);
            suelo.setEscala(new Vector3(0.025f, 0.01f, 0.025f));

            //Carga las 15 partes del cuerpo
            mySkeleton.init();

            //Cargo las esferas para cada parte del cuerpo para el Avatar
            Model esfera = pM.Content.Load<Model>("fbx/esfera");
            myAvatar.init(esfera);
        }
        #endregion

        #region Destruyendo estructuras
        public override void UnloadContent()
        {
            mySkeletonSave.saveAll();
            mySkeletonSave.closeConection();
        }
        #endregion

        #region Actualizando el modelo
        public override void Update(GameTime gameTime)
        {

            escucharTeclado();

            myMiddle.update();

            if (myMiddle.getState(0).Equals(myMiddle.DETECTADO))
            {
                myAvatar.update();
                mySkeletonSave.update();
            }
        }
        #endregion

        #region Dibujando el modelo
        public override void Draw(GameTime gameTime)
        {
            suelo.draw(gameTime, camara.view, camara.projection);

            if (myMiddle.getState(0).Equals(myMiddle.DETECTADO))
            {                
                myAvatar.draw(gameTime, camara.view, camara.projection);
            }

            pM.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
            pM.spriteBatch.Draw(pM.esc, new Rectangle(890, 10, 95, 107), Color.White);
            pM.spriteBatch.Draw(pM.camera, new Rectangle(650, 10, 196, 119), Color.White);
            pM.spriteBatch.End();

        }
        #endregion

        #region Escuchar teclado
        private void escucharTeclado()
        {
            Vector3 cambioCamara = Vector3.Zero;
            Vector3 rotCamara = Vector3.Zero;

            float camSpeed = 0.10f;

            //Movimiento de cámara
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                cambioCamara.Z += camSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                cambioCamara.Z -= camSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                cambioCamara.X -= camSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                cambioCamara.X += camSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                rotCamara.Y += camSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                rotCamara.Y -= camSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                rotCamara.X += camSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                rotCamara.X -= camSpeed;
            }

            this.camara.cameraPosition += cambioCamara + rotCamara;
            this.camara.cameraLookAt += cambioCamara;
            this.camara.updateCam();

            if (Keyboard.GetState().IsKeyDown(Keys.F) && !saved)
            {
                saved = true;
                mySkeletonSave.saveAll();
            }
        }
        #endregion
    }
}
