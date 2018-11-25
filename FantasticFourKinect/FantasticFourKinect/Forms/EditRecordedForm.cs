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
    class EditRecordedForm : AbstractForm
    {
        #region Declaración de variables

        //Variables Propias
        public HumanSkeleton mySkeleton;
        private SkeletonReaderMiddle myMiddle;
        private bool ready;
        public String path;
        private DistanceState ds;

        //--Estados
        public List<AbstractState> myStates;

        //--Gestos
        public List<AbstractGesture> myGestures;
        private List<Pair> myPartialGestures;

        //Variables para renderizar
        private Avatar myAvatar;
        private Camera camara;
        private Object3D suelo;
        
        //Variables de formulario de edición
        private WFEditGestures formEditor;
        private String jointBeforeRed = "";
        private String jointBeforeGreen = "";
        private bool keyboardEnabled = true;
        private bool avanzar = false;
        private bool retroceder = false;
        #endregion

        #region Constructor
        public EditRecordedForm(Principal pM)
            : base(pM)
        {

            mySkeleton = new HumanSkeleton();
            path = Environment.CurrentDirectory + "\\Data\\Animation\\default.ani";

            //Para simular el inicio del dispositivo desde un archivo.
            myMiddle = new SkeletonReaderMiddle();
            myMiddle.addUser(mySkeleton, path);

            ready = false;

            //--Render
            myAvatar = new Avatar(mySkeleton);
            camara = new Camera(pM.graphics);

            //--Estados
            myStates = DB4ObjectState.loadStates(Environment.CurrentDirectory + "\\Data\\States\\default.sta");

            //--Gestos
            myGestures = DB4ObjectGesture.loadGestures(Environment.CurrentDirectory + "\\Data\\Gestures\\default.ges");
            myPartialGestures = new List<Pair>();

            //Crear formulario de edición de gestos
            formEditor = new WFEditGestures(this);
            formEditor.Show();

            this.LoadContent();
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

            avanzarAnimacion();
        }
        #endregion

        #region Destruyendo estructuras
        public override void UnloadContent()
        {
            myMiddle.closeConection();
            formEditor.Visible = false;
        }
        #endregion

        #region Actualizando el modelo
        public override void Update(GameTime gameTime)
        {
            if (keyboardEnabled)
            {
                escucharTeclado();
            }

            if (myMiddle.getState(0).Equals(myMiddle.DETECTADO))
            {
                myAvatar.update();
                ready = true;
            }

            if (avanzar)
            {
                myMiddle.update();
                formEditor.UpdateGrid();
            }

            if (retroceder)
            {
                myMiddle.undoUpdate();
                formEditor.UpdateGrid();
            }
        }
        #endregion

        #region Dibujando el modelo
        public override void Draw(GameTime gameTime)
        {
            if (ready)
            {
                suelo.draw(gameTime, camara.view, camara.projection);
                myAvatar.draw(gameTime, camara.view, camara.projection);
            }

            pM.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
            pM.spriteBatch.Draw(pM.esc, new Rectangle(890, 10, 106, 112), Color.White);
            pM.spriteBatch.Draw(pM.camera, new Rectangle(650, 10, 196, 119), Color.White);
            pM.spriteBatch.DrawString(pM.font, "Progreso: " + System.Convert.ToString((myMiddle.cursor[0] * 100) / myMiddle.getCountPosition(0)) + " %", new Vector2(10, 0), Color.White);
            pM.spriteBatch.DrawString(pM.font, "Nro. de línea: " + System.Convert.ToString(myMiddle.cursor[0]), new Vector2(10, 30), Color.White);
            if (ds != null)
                pM.spriteBatch.DrawString(pM.font, "Distancia entre partes: " + ds.getDistance(mySkeleton), new Vector2(10, 60), Color.Red);
            pM.spriteBatch.End();
        }
        #endregion

        #region Escuchar teclado
        private void escucharTeclado()
        {
            Vector3 cambioCamara = Vector3.Zero;
            Vector3 rotCamara = Vector3.Zero;
            float camSpeed = 0.10f;
            float rotSpeed = 1.0f;

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
                rotCamara.X += rotSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                rotCamara.X -= rotSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                rotCamara.Y -= rotSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                rotCamara.Y += rotSpeed;
            }

            //Recarga la animación
            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                myMiddle = new SkeletonReaderMiddle();
                myMiddle.addUser(mySkeleton, path);
            }


            this.camara.cameraRot += rotCamara;
            this.camara.cameraPosition += cambioCamara;
            this.camara.cameraLookAt += cambioCamara;
            this.camara.updateCam();

            /*this.camara.cameraPosition += cambioCamara + rotCamara;
            this.camara.cameraLookAt += cambioCamara;
            this.camara.updateCam();*/
        }
        #endregion

        #region Se dibujan de distinto color las partes del cuerpo seleccionadas
        //Dada una parte del cuerpo, la redibuja usando la textura en la ruta "path"
        private void setModel(string nameJoint, string path)
        {
            Model esferaSelected = pM.Content.Load<Model>(path);
            Object3D obj = myAvatar.get(nameJoint);
            obj.setModel(esferaSelected);
        }

        //Redibuja de color rojo la parte 1 del cuerpo
        internal void repaintJointRed(string nameJoint)
        {
            if (jointBeforeRed != "")
                setModel(jointBeforeRed, "fbx/esfera");
            setModel(nameJoint, "fbx/esferaSelected1");
            jointBeforeRed = nameJoint;
        }

        //Redibuja de color verde la parte 2 del cuerpo
        internal void repaintJointGreen(string nameJoint)
        {
            if (jointBeforeGreen != "")
                setModel(jointBeforeGreen, "fbx/esfera");
            setModel(nameJoint, "fbx/esferaSelected2");
            jointBeforeGreen = nameJoint;
        }
        #endregion

        #region Bloqueo y desbloqueo de teclado
        internal void disableKeyboard()
        {
            this.keyboardEnabled = false;
        }

        internal void enableKeyboard()
        {
            this.keyboardEnabled = true;
        }
        #endregion

        #region Métodos de avance y retroceso de la animación
        internal void retrocederAnimacion()
        {
            myMiddle.undoUpdate();
            formEditor.UpdateGrid();
        }

        internal void avanzarAnimacion()
        {
            myMiddle.update();
            formEditor.UpdateGrid();
        }

        internal void forward(bool value)
        {
            avanzar = value;
        }

        internal void backward(bool value)
        {
            retroceder = value;
        }
        #endregion

        #region Recarga asincrónicamente la animación guardada
        internal void reloadAnimation()
        {
            myMiddle = new SkeletonReaderMiddle();
            myMiddle.addUser(mySkeleton, path);
            myMiddle.update();
            formEditor.UpdateGrid();
        }
        #endregion

        public bool cumpleState(string stateName, string p1, string p2, string p3, float precision, float grados, string stateType, int posState1, int posState2)
        {
            AbstractState aS = makeState(stateName, p1, p2, p3, precision, grados, stateType, posState1, posState2);

            if (aS.cumple(mySkeleton))
                return true;

            return false;
        }

        public void addState(string stateName, string p1, string p2, string p3, float precision, float grados, string stateType, int posState1, int posState2)
        {
            AbstractState aS = makeState(stateName, p1, p2, p3, precision, grados, stateType, posState1, posState2);
            myStates.Add(aS);
        }

        public void addState(List<AbstractState> newStates)
        {
            for (int i = 0; i < newStates.Count; i++)
                myStates.Add(newStates[i]);
        }

        public AbstractState makeState(string stateName, string p1, string p2, string p3, float precision, float grados, string stateType, int posState1, int posState2)
        {
            AbstractState aS;
            AbstractState aS1 = getState(posState1);
            AbstractState aS2 = getState(posState2);

            switch (stateType)
            {
                case "AND": aS = new AndState(aS1, aS2, stateName); break;
                case "ANGLE": aS = new AngleState(p1, p3, p2, grados, precision, stateName); break;
                case "CENTER_X": aS = new CenterStateX(p1, p2, precision, stateName); break;
                case "CENTER_Y": aS = new CenterStateY(p1, p2, precision, stateName); break;
                case "CENTER_Z": aS = new CenterStateZ(p1, p2, precision, stateName); break;
                case "DISTANCE": aS = new DistanceState(p1, p2, precision, stateName); break;
                case "LEFT": aS = new LeftState(p1, p2, precision, stateName); break;
                case "OR": aS = new OrState(aS1, aS2, stateName); break;
                case "OVER": aS = new OverState(p1, p2, precision, stateName); break;
                case "POSITION": aS = new PositionState(p1, precision, stateName); break;
                case "RIGHT": aS = new RightState(p1, p2, precision, stateName); break;
                case "UNDER": aS = new UnderState(p1, p2, precision, stateName); break;
                default: aS = new InvalidState(stateName); break;
            }
            return aS;
        }

        public void removeState(int index)
        {
            try
            {
                myStates.RemoveAt(index);
            }
            catch (Exception) { }
        }

        private AbstractState getState(int index)
        {
            try
            {
                return myStates.ElementAt(index);
            }
            catch (Exception) { }
            return null;
        }

        private AbstractState findState(String stateName)
        {
            for (int i = 0; i < myStates.Count; i++)
                if (myStates[i].getStateName().Equals(stateName))
                    return myStates[i];
            return null;
        }

        // Crea el par (estado,tiempo)
        internal void makePartialGesture(string timeGesture, bool isTimeUndefined, string stateName)
        {
            AbstractState aS = findState(stateName);

            long ticks; // 10.000.000 de ticks es 1 segundo
            if (isTimeUndefined)
                ticks = 1000000000000; // por poner algo, después hay que mirar
            else
                ticks = System.Convert.ToInt16(timeGesture) * 10000000;

            Pair p = new Pair(aS, new TimeSpan(ticks));

            myPartialGestures.Add(p);
        }

        // Crea un gesto y le carga los estados y tiempos que lo componen
        internal void makeGesture(String gestureName)
        {
            SimpleGesture sA = new SimpleGesture(gestureName);

            for (int i = 0; i < myPartialGestures.Count; i++)
                sA.addPairStateTime(myPartialGestures[i]);

            myPartialGestures = new List<Pair>();

            myGestures.Add(sA);
        }

        internal void saveStates(string path)
        {
            DB4ObjectState.saveStates(myStates, path); //Guardo persistentemente.
        }

        internal void saveGestures(string path)
        {
            DB4ObjectGesture.saveGestures(myGestures, path); //Guardo persistentemente.
        }

        internal void loadStates(string path)
        {
            myStates = DB4ObjectState.loadStates(path);
        }

        internal void loadGestures(string path)
        {
            myGestures = DB4ObjectGesture.loadGestures(path);
        }

        internal void crearDistance(String p1, String p2)
        {
            ds = new DistanceState(p1, p2, 0, "DISTANCE");
        }

        public int getCountPosition() 
        {
            return myMiddle.getCountPosition(0);
        }

        public void cropAnimation(int i, int f)
        {
            myMiddle.cropAnimation(i, f, 0);
        }

        public void saveAnimation(String path)
        {
            List<String> myPaths = new List<String>();
            myPaths.Add(path);
            myMiddle.saveChange(myPaths);
        }

        internal void goToPosition(int p)
        {
            myMiddle.updateToPosition(p,0);
        }
    }
}

