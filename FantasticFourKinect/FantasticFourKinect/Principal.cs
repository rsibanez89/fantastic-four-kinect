using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using FantasticFourKinect.Middle;
using FantasticFourKinect.Skeleton;
using FantasticFourKinect.State;
using FantasticFourKinect.Gesture;
using FantasticFourKinect.Render;
using FantasticFourKinect.Forms;

namespace FantasticFourKinect
{
    // Clase principal que delega el control a los distintos formularios
    public class Principal : Microsoft.Xna.Framework.Game
    {
        #region Declaración de variables

        // Variables XNA
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public SpriteFont font;
        
        // Texturas 2D
        public Texture2D background;
        public Texture2D esc;
        public Texture2D camera;
        public Texture2D op1;
        public Texture2D op2;
        public Texture2D op3;
        public Texture2D op4;
        public Texture2D op1s;
        public Texture2D op2s;
        public Texture2D op3s;
        public Texture2D op4s;
        public Texture2D kinect_ok;
        public Texture2D kinect_fail;
        public Texture2D hand;
        public Texture2D hand1;
        public Texture2D hand2;
        public Texture2D hand3;

        // Otras variables
        private AbstractForm myForm;
        public int selectedOption;
        private AboutF4K abt;
        private AbstractMiddle myMiddle;

        #endregion

        #region Constructor
        public Principal()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        #endregion

        #region Inicialización
        protected override void Initialize()
        {
            IsMouseVisible = false;
            
            // Para iniciar el dispositivo con los drivers de Microsoft.
            myMiddle = new KinectMiddleMS();

            // Formulario XNA de opción
            myForm = new MenuForm(this, myMiddle);

            base.Initialize();
        }
        #endregion

        #region Cargando estructuras
        protected override void LoadContent()
        {
            // Render
            spriteBatch = new SpriteBatch(GraphicsDevice);
            graphics.PreferredBackBufferWidth = 1000;
            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();            
            abt = new AboutF4K();
            selectedOption = -1;
            loadExternalResources();
        }

        // Método de carga de recursos externos
        private void loadExternalResources()
        {
            font = Content.Load<SpriteFont>("Calibri");
            esc = Content.Load<Texture2D>("images/volver");
            camera = Content.Load<Texture2D>("images/camera");
            background = Content.Load<Texture2D>("images/FondoPrograma");
            op1 = Content.Load<Texture2D>("images/op1");
            op2 = Content.Load<Texture2D>("images/op2");
            op3 = Content.Load<Texture2D>("images/op3");
            op4 = Content.Load<Texture2D>("images/op4");
            op1s = Content.Load<Texture2D>("images/op1s");
            op2s = Content.Load<Texture2D>("images/op2s");
            op3s = Content.Load<Texture2D>("images/op3s");
            op4s = Content.Load<Texture2D>("images/op4s");
            kinect_ok = Content.Load<Texture2D>("images/kinect_ok");
            kinect_fail = Content.Load<Texture2D>("images/kinect_fail");
            hand = Content.Load<Texture2D>("images/hand");
            hand1 = Content.Load<Texture2D>("images/hand1");
            hand2 = Content.Load<Texture2D>("images/hand2");
            hand3 = Content.Load<Texture2D>("images/hand3");
        }
        #endregion

        #region Destruyendo estructuras
        protected override void UnloadContent()
        {            
            myMiddle.closeConection();
        }
        #endregion

        #region Actualizando el modelo
        protected override void Update(GameTime gameTime)
        {
            escucharTeclado();

            myForm.Update(gameTime);

            base.Update(gameTime);
        }
        #endregion

        #region Dibujando el modelo
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            
            myForm.Draw(gameTime);

            base.Draw(gameTime);
        }
        #endregion

        #region Escuchar teclado
        private void escucharTeclado()
        {
            // Seleccionando opciones de menú
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                selectedOption = 0;
            }

            switch (selectedOption)
            {
                case 0:
                    {
                        myForm.UnloadContent();
                        myForm = new MenuForm(this, myMiddle);
                        break;
                    }
                case 1:
                    {
                        myForm = new RecordForm(this, myMiddle);
                        break;
                    }
                case 2:
                    {
                        myForm = new EditRecordedForm(this);
                        break;
                    }
                case 3:
                    {
                        myForm = new TeacherForm(this, myMiddle);
                        break;
                    }
                case 4:
                    {
                        myForm = new FreeDetectionForm(this, myMiddle);
                        break;
                    }
                case 5:
                    {
                        if (!abt.Visible)
                            abt.Show();
                        break;
                    }
            }

            selectedOption = -1;
        }
        #endregion
    }
}
