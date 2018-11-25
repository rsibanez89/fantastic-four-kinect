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
using FantasticFourKinect.Middle;
using FantasticFourKinect.Skeleton;

namespace FantasticFourKinect.Forms
{
    class MenuForm : AbstractForm
    {
        #region Declaración de variables

        //Variables Propias
        private AbstractMiddle myMiddle;
        private HumanSkeleton mySkeleton;

        private int isSelecting;
        private int isSelectingBefore;
        private DateTime intervalSelection;
        private TimeSpan interval1;
        private TimeSpan interval2;
        private TimeSpan interval3;

        #endregion

        #region Constructor, Inicialización y Carga de estructuras
        public MenuForm(Principal pM, AbstractMiddle am) : base(pM)
        {
            mySkeleton = new HumanSkeleton();
            mySkeleton.init();
            myMiddle = am;
            myMiddle.reset();
            myMiddle.addUser(mySkeleton);
            isSelecting = -1;
            isSelectingBefore = -1;
            interval1 = new TimeSpan(10000000);
            interval2 = new TimeSpan(20000000);
            interval3 = new TimeSpan(30000000);
        }
        #endregion

        #region Actualizando el modelo
        public override void Update(GameTime gameTime)
        {
            myMiddle.update();
            if (myMiddle.getState(0).Equals(myMiddle.DETECTADO))
                Mouse.SetPosition(Convert.ToInt32(myMiddle.getSkeleton(0).getBodyPart("RightHand").X * 1920 + 500), Convert.ToInt32(1080 - myMiddle.getSkeleton(0).getBodyPart("RightHand").Y * 1080 - 540));

            escucharTeclado();
            verificarSeleccion();
        }
        #endregion

        #region Destruyendo estructuras
        public override void UnloadContent()
        {

        }
        #endregion

        #region Dibujando el modelo
        public override void Draw(GameTime gameTime)
        {
            pM.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
            pM.spriteBatch.Draw(pM.background, new Rectangle(0, 0, 1000, 600), Color.White);

            if (myMiddle.isKinectPluggedIn()) 
                pM.spriteBatch.Draw(pM.kinect_ok, new Rectangle(10, 85, 133, 45), Color.White);
            else pM.spriteBatch.Draw(pM.kinect_fail, new Rectangle(10, 85, 133, 45), Color.White);

            switch (isSelecting)
            {
                case 1: drawOptions(pM.op1s, pM.op2, pM.op3, pM.op4); break;
                case 2: drawOptions(pM.op1, pM.op2s, pM.op3, pM.op4); break;
                case 3: drawOptions(pM.op1, pM.op2, pM.op3s, pM.op4); break;
                case 4: drawOptions(pM.op1, pM.op2, pM.op3, pM.op4s); break;
                default: drawOptions(pM.op1, pM.op2, pM.op3, pM.op4); break;
            }

            if (isSelecting != -1)
            {
                if(Mouse.GetState().LeftButton.Equals(ButtonState.Pressed))
                    executeOption();
                else
                    checkTime();

            }
            else pM.spriteBatch.Draw(pM.hand, new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 48, 76), Color.White);

            pM.spriteBatch.End();
        }
        #endregion

        #region Verifica la posición del cursor
        private void verificarSeleccion()
        {
            int x = Mouse.GetState().X;
            int y = Mouse.GetState().Y;

            if (y > 450)
            {
                if (x <= 250)
                    isSelecting = 1;

                if (x > 250 && x <= 500)
                    isSelecting = 2;

                if (x > 500 && x <= 750)
                    isSelecting = 3;

                if (x > 750)
                    isSelecting = 4;
            }
            else isSelecting = -1;

            notify();
        }
        #endregion

        #region Escuchar teclado
        private void escucharTeclado()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.F1))
            {
                pM.selectedOption = 5;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D1) || Keyboard.GetState().IsKeyDown(Keys.D3) || Keyboard.GetState().IsKeyDown(Keys.D4))
            {
                if (myMiddle.isKinectPluggedIn())
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.D1))
                    {
                        pM.selectedOption = 1;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.D3))
                    {
                        pM.selectedOption = 3;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.D4))
                    {
                        pM.selectedOption = 4;
                    }
                }
                else System.Windows.Forms.MessageBox.Show("Asegúrese de que el dispositivo Kinect está conectado", "Kinect unplugged...", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D2))
            {
                pM.selectedOption = 2;
            }
        }
        #endregion

        #region Otros métodos privados
        // Notifica cuando cambia la selección de alguna opción
        private void notify()
        {
            if (isSelecting != isSelectingBefore)
            {
                isSelectingBefore = isSelecting;
                intervalSelection = DateTime.Now;
            }
        }

        private void checkTime()
        {
            if ((DateTime.Now - intervalSelection) < interval1)
                pM.spriteBatch.Draw(pM.hand3, new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 48, 76), Color.White);
            else if ((DateTime.Now - intervalSelection) < interval2)
                pM.spriteBatch.Draw(pM.hand2, new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 48, 76), Color.White);
            else if ((DateTime.Now - intervalSelection) < interval3)
                pM.spriteBatch.Draw(pM.hand1, new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 48, 76), Color.White);
            else executeOption();
        }

        private void executeOption()
        {
            if (isSelecting != 2)
                if (myMiddle.isKinectPluggedIn())
                    pM.selectedOption = isSelecting;
                else System.Windows.Forms.MessageBox.Show("Asegúrese de que el dispositivo Kinect está conectado", "Kinect unplugged...", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
            else pM.selectedOption = isSelecting;
        }

        private void drawOptions(Texture2D op1, Texture2D op2, Texture2D op3, Texture2D op4)
        {
            pM.spriteBatch.Draw(op1, new Rectangle(85, 450, 149, 136), Color.White);
            pM.spriteBatch.Draw(op2, new Rectangle(327, 441, 148, 148), Color.White);
            pM.spriteBatch.Draw(op3, new Rectangle(558, 446, 165, 143), Color.White);
            pM.spriteBatch.Draw(op4, new Rectangle(776, 453, 157, 133), Color.White);
        }
        #endregion
    }
}