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
using FantasticFourKinect.Persistency;

namespace FantasticFourKinect.Forms
{
    class FreeDetectionForm : AbstractForm , LoadableForm
    {
        #region Declaración de variables

        // Variables Propias
        private HumanSkeleton mySkeletonUser1;
        private HumanSkeleton mySkeletonUser2;
        private AbstractMiddle myMiddle;
        private int gestosCompletadosUser1 = 0;
        private int gestosCompletadosUser2 = 0;
        private int gestosCompletadosTwoUsers = 0;
        private WFDetectionGestures formGesture;

        // Estados
        public List<AbstractState> myStates;

        // Gestos
        public List<AbstractGesture> myGestures;

        // Variables para renderizar
        private Avatar myAvatarUser1;
        private Avatar myAvatarUser2;
        private Camera camara;
        private Object3D suelo;

        // Variables de logueo
        private String logBefore1;
        private String logBefore2;
        private String logBefore3;

        #endregion

        #region Constructor
        public FreeDetectionForm(Principal pM, AbstractMiddle am) : base(pM)
        {
            mySkeletonUser1 = new HumanSkeleton();
            mySkeletonUser2 = new HumanSkeleton();
            myMiddle = am;
            myMiddle.reset();
            myMiddle.addUser(mySkeletonUser1);
            myMiddle.addUser(mySkeletonUser2);

            // Render
            myAvatarUser1 = new Avatar(mySkeletonUser1);
            myAvatarUser2 = new Avatar(mySkeletonUser2);
            camara = new Camera(pM.graphics);

            // Estados
            myStates = DB4ObjectState.loadStates(Environment.CurrentDirectory + "\\Data\\States\\default.sta");

            // Gestos
            myGestures = DB4ObjectGesture.loadGestures(Environment.CurrentDirectory + "\\Data\\Gestures\\default.ges");

            // Crear formulario de edición de gestos
            formGesture = new WFDetectionGestures(this);
            formGesture.Show();

            // Variables de logueo
            logBefore1 = "";
            logBefore2 = "";
            logBefore3 = "";

            this.LoadContent();
        }
        #endregion

        #region Cargando estructuras
        public void LoadContent()
        {
            Model modeloSuelo = pM.Content.Load<Model>("fbx/suelo");
            suelo = new Object3D("Suelo", new Vector3(0.0f, -1.2f, 0), modeloSuelo);
            suelo.setEscala(new Vector3(0.025f, 0.01f, 0.025f));

            //Carga las 15 partes del cuerpo para ambos esqueletos
            mySkeletonUser1.init();
            mySkeletonUser2.init();

            //Cargo las esferas de cada parte del cuerpo para ambos Avatar
            Model esferaUser1 = pM.Content.Load<Model>("fbx/esferaSelected1");
            myAvatarUser1.init(esferaUser1);

            Model esferaUser2 = pM.Content.Load<Model>("fbx/esferaSelected2");
            myAvatarUser2.init(esferaUser2);
        }
        #endregion

        #region Destruyendo estructuras
        public override void UnloadContent()
        {
            formGesture.Visible = false;
        }
        #endregion

        #region Actualizando el modelo
        public override void Update(GameTime gameTime)
        {
            escucharTeclado();

            myMiddle.update();
            myAvatarUser1.update();
            myAvatarUser2.update();
            
            comprobarGestos();
        }
        #endregion

        #region Verifica que estados y gestos se cumplen en ambos avatares
        private void comprobarGestos()
        {
            for (int i = 0; i < myStates.Count; i++)
            {
                // Reconocimiento de estados para usuario 1
                if (myMiddle.getState(0).Equals(myMiddle.DETECTADO) && myStates[i].cumple(myAvatarUser1.getHumanSkeleton()))
                {
                    gestosCompletadosUser1++;
                    if (logBefore1 != myStates[i].getStateName())
                    {
                        formGesture.addLogList1("[STATE] " + myStates[i].getStateName());
                        logBefore1 = myStates[i].getStateName();
                    }
                }
                // Reconocimiento de estados para usuario 2
                if (myMiddle.getState(1).Equals(myMiddle.DETECTADO) && myStates[i].cumple(myAvatarUser2.getHumanSkeleton()))
                {
                    gestosCompletadosUser2++;
                    if (logBefore2 != myStates[i].getStateName())
                    {
                        formGesture.addLogList2("[STATE] " + myStates[i].getStateName());
                        logBefore2 = myStates[i].getStateName();
                    }
                }
                // Reconocimiento de estados para ambos usuarios
                if (myMiddle.getState(0).Equals(myMiddle.DETECTADO) && myMiddle.getState(1).Equals(myMiddle.DETECTADO) && myStates[i].cumple(myAvatarUser1.getHumanSkeleton(), myAvatarUser2.getHumanSkeleton()))
                {
                    gestosCompletadosTwoUsers++;
                    if (logBefore3 != myStates[i].getStateName())
                    {
                        formGesture.addLogList3("[STATE] " + myStates[i].getStateName());
                        logBefore3 = myStates[i].getStateName();
                    }
                }
            }

            for (int i = 0; i < myGestures.Count; i++)
            {
                // Reconocimiento de gestos para usuario 1
                if (myMiddle.getState(0).Equals(myMiddle.DETECTADO) && myGestures[i].cumple(myAvatarUser1.getHumanSkeleton()))
                {
                    gestosCompletadosUser1++;
                    if (logBefore1 != myGestures[i].getGestureName())
                    {
                        formGesture.addLogList1("[GESTURE] " + myGestures[i].getGestureName());
                        logBefore1 = myGestures[i].getGestureName();
                    }
                }
                // Reconocimiento de gestos para usuario 2
                if (myMiddle.getState(1).Equals(myMiddle.DETECTADO) && myGestures[i].cumple(myAvatarUser2.getHumanSkeleton()))
                {
                    gestosCompletadosUser2++;
                    if (logBefore2 != myGestures[i].getGestureName())
                    {
                        formGesture.addLogList2("[GESTURE] " + myGestures[i].getGestureName());
                        logBefore2 = myGestures[i].getGestureName();
                    }
                }
            }
        }
        #endregion

        #region Dibujando el modelo
        public override void Draw(GameTime gameTime)
        {
            suelo.draw(gameTime, camara.view, camara.projection);
            if (myMiddle.getState(0).Equals(myMiddle.DETECTADO))
            {
                myAvatarUser1.draw(gameTime, camara.view, camara.projection);
            }
            if (myMiddle.getState(1).Equals(myMiddle.DETECTADO))
            {
                myAvatarUser2.draw(gameTime, camara.view, camara.projection);
            }

            pM.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
            pM.spriteBatch.Draw(pM.esc, new Rectangle(890, 10, 95, 107), Color.White);
            pM.spriteBatch.Draw(pM.camera, new Rectangle(650, 10, 196, 119), Color.White);
            pM.spriteBatch.DrawString(pM.font, "User 0: " + myMiddle.getState(0), new Vector2(10, 0), Color.Red);
            pM.spriteBatch.DrawString(pM.font, "User 1: " + myMiddle.getState(1), new Vector2(10, 20), Color.LightGreen);
            pM.spriteBatch.End();
        }
        #endregion

        #region Se cargan todos lo estados y gestos que pueden ser detectadas por la aplicación
        private void cargarEstados()
        {
            //--Estados
            myStates = DB4ObjectState.loadStates(Environment.CurrentDirectory + "\\Data\\States\\default.sta");

            //--Gestos
            myGestures = DB4ObjectGesture.loadGestures(Environment.CurrentDirectory + "\\Data\\Gestures\\default.ges");
        }
        #endregion

        #region Se cargan animaciones, estados y gestos que pueden ser detectadas por la aplicación
        public void loadStates(string path)
        {
            myStates = DB4ObjectState.loadStates(path);
        }

        public void loadGestures(string path)
        {
            myGestures = DB4ObjectGesture.loadGestures(path);
        }

        public void loadAnimation(string path)
        {
            // Sin implementación ya que no requiere una animación esta opción
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
        }
        #endregion
    }
}

