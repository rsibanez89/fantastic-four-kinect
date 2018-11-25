using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using FantasticFourKinect.Skeleton;
using FantasticFourKinect.State;
using FantasticFourKinect.Gesture;

namespace FantasticFourKinect.Forms
{
    partial class WFEditGestures : Form
    {
        public EditRecordedForm myErf;
        public WFAdvanceStatesGeneration myAdvance;

        public WFEditGestures(EditRecordedForm erf)
        {
            InitializeComponent();
            
            // Se dibujan los botones            
            this.button5.Image = new Bitmap(Environment.CurrentDirectory + "\\Data\\Resources\\Save.png");
            this.button8.Image = new Bitmap(Environment.CurrentDirectory + "\\Data\\Resources\\Load.png");
            this.button7.Image = new Bitmap(Environment.CurrentDirectory + "\\Data\\Resources\\Save.png");
            this.button9.Image = new Bitmap(Environment.CurrentDirectory + "\\Data\\Resources\\Load.png");
            this.button12.Image = new Bitmap(Environment.CurrentDirectory + "\\Data\\Resources\\Save.png");
            this.myErf = erf;

            cargarEstados();
            cargarGestos();
            resetBar(myErf.getCountPosition());
        }

        private void cargarEstados()
        {
            listBox1.Items.Clear();
            comboBox3.Items.Clear();
            comboBox6.Items.Clear();
            comboBox7.Items.Clear();
            foreach (AbstractState aS in myErf.myStates)
            {
                listBox1.Items.Add(aS.getStateName());
                comboBox3.Items.Add(aS.getStateName());
                comboBox6.Items.Add(aS.getStateName());
                comboBox7.Items.Add(aS.getStateName());
            }
        }

        private void cargarGestos()
        {
            listBox2.Items.Clear();
            foreach (AbstractGesture aG in myErf.myGestures)
                listBox2.Items.Add(aG.getGestureName());
        }

        //Boton que genera estados
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                String stateName = textBox1.Text;
                String bp1 = String.IsNullOrEmpty((String)comboBox1.SelectedItem) ? "" : (String)comboBox1.SelectedItem;
                String bp2 = String.IsNullOrEmpty((String)comboBox2.SelectedItem) ? "" : (String)comboBox2.SelectedItem;
                String bp3 = String.IsNullOrEmpty((String)comboBox5.SelectedItem) ? "" : (String)comboBox5.SelectedItem;
                float grados = String.IsNullOrEmpty(textBox4.Text) ? 0 : System.Convert.ToSingle(textBox4.Text);
                float precision = String.IsNullOrEmpty(textBox3.Text) ? 0 : System.Convert.ToSingle(textBox3.Text);
                Console.WriteLine("Pre: " + precision);
                String stateType = String.IsNullOrEmpty((String)comboBox4.SelectedItem) ? "" : (String)comboBox4.SelectedItem;
                int state1 = comboBox6.SelectedIndex;
                int state2 = comboBox7.SelectedIndex;
                bool addState = false;
                if (myErf.cumpleState(stateName, bp1, bp2, bp3, precision, grados, stateType, state1, state2))
                    addState = true;
                else
                    if (MessageBox.Show("El estado no se cumple. ¿Agegar de todas formas?", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                        addState = true;
                if (addState)
                {
                    listBox1.Items.Add(textBox1.Text);
                    comboBox3.Items.Add(textBox1.Text);
                    comboBox6.Items.Add(textBox1.Text);
                    comboBox7.Items.Add(textBox1.Text);
                    myErf.addState(stateName, bp1, bp2, bp3, precision, grados, stateType, state1, state2);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Faltan datos para completar el estado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //Se cargan estados con tiempos asociados que en conjunto formarán un gesto
        private void button10_Click(object sender, EventArgs e)
        {
            if ((textBox2.Text != "" || checkBox1.Checked) && comboBox3.SelectedIndex != -1)
            {
                if (checkBox1.Checked)
                    listBox3.Items.Add(comboBox3.SelectedItem + "_TimeUndefined");
                else listBox3.Items.Add(comboBox3.SelectedItem + "_" + textBox2.Text);
                myErf.makePartialGesture(textBox2.Text, checkBox1.Checked, (String)comboBox3.SelectedItem);
            }
            else MessageBox.Show("Faltan datos para completar el gesto parcial", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        //Se crea un gesto a partir de un conjunto de estados
        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox3.Items.Count > 0)
            {
                if (textBox5.Text != "")
                {
                    comboBox3.SelectedIndex = -1;
                    textBox2.Text = "";
                    checkBox1.Checked = false;
                    listBox3.Items.Clear();
                    myErf.makeGesture(textBox5.Text);
                    listBox2.Items.Add(textBox5.Text);
                }
                else MessageBox.Show("Debe especificar un nombre para el gesto", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else MessageBox.Show("Para crear un gesto debe haber al menos un estado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        public void UpdateGrid()
        {
            if (checkBox2.Checked)
            {
                this.dataGridView1.Rows.Clear();

                foreach (DictionaryEntry dE in myErf.mySkeleton.getBodyParts())
                {
                    float X = ((HumanBodyPart)dE.Value).X;
                    float Y = ((HumanBodyPart)dE.Value).Y;
                    float Z = ((HumanBodyPart)dE.Value).Z;
                    String nameJoint = (String)dE.Key;

                    this.dataGridView1.Rows.Add(nameJoint, X, Y, Z);
                }
            }
        }

        private void makeNameState()
        {
            if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1)
            {//estan las 2 partes del cuerpo seleccionadas
                String p1 = (String)comboBox1.SelectedItem;
                String p2 = (String)comboBox2.SelectedItem;
                myErf.crearDistance(p1, p2);
                if (comboBox4.SelectedIndex != -1)//está seleccionado el estado
                    textBox1.Text = (String)comboBox1.SelectedItem + "_" + (String)comboBox4.SelectedItem + "_" + (String)comboBox2.SelectedItem;
            }
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            myErf.repaintJointRed(comboBox1.Text);
            makeNameState();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            myErf.repaintJointGreen(comboBox2.Text);
            makeNameState();
        }

        private void comboBox4_SelectedValueChanged(object sender, EventArgs e)
        {
            makeNameState();
            switch ((String)comboBox4.SelectedItem)
            {
                case "ANGLE": 
                    comboBox5.Enabled = true;
                    textBox4.Enabled = true;
                    especialStateDefault();
                    break;
                case "AND":
                    comboBox5.Enabled = false;
                    textBox4.Enabled = false;
                    especialStateANDOR();
                    break;
                case "OR":
                    comboBox5.Enabled = false;
                    textBox4.Enabled = false;
                    especialStateANDOR();
                    break;
                default:
                    comboBox5.Enabled = false;
                    textBox4.Enabled = false;
                    especialStateDefault();
                    break;
            }
        }

        private void especialStateDefault()
        {
            comboBox6.Visible = false;
            comboBox7.Visible = false;
            label13.Visible = false;
            label14.Visible = false;
            label1.Visible = true;
            label2.Visible = true;
            textBox3.Enabled = true;
        }

        private void especialStateANDOR()
        {
            comboBox5.Enabled = false;
            comboBox6.Visible = true;
            comboBox7.Visible = true;
            label13.Visible = true;
            label14.Visible = true;
            label1.Visible = false;
            label2.Visible = false;
            textBox3.Enabled = false;
        }

        private void GestureEditionForm_Activated(object sender, EventArgs e)
        {
            myErf.disableKeyboard();
        }

        private void GestureEditionForm_Deactivate(object sender, EventArgs e)
        {
            myErf.enableKeyboard();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Retrocede la animación una posición
            myErf.retrocederAnimacion();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //Avanza la animación una posición
            myErf.avanzarAnimacion();
        }

        private void button6_MouseDown(object sender, MouseEventArgs e)
        {
            //Avanza la animación mientras el botón se mantenga presionado
            myErf.forward(true);
        }

        private void button6_MouseUp(object sender, MouseEventArgs e)
        {
            //Detiene el avance de la animación cuando se suelta el botón
            myErf.forward(false);
        }

        private void button4_MouseDown(object sender, MouseEventArgs e)
        {
            //Retrocede la animación mientras el botón se mantenga presionado
            myErf.backward(true);
        }

        private void button4_MouseUp(object sender, MouseEventArgs e)
        {
            //Detiene el retroceso de la animación cuando se suelta el botón
            myErf.backward(false);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            myErf.reloadAnimation();
            resetBar(myErf.getCountPosition());
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                textBox2.Enabled = false;
            else textBox2.Enabled = true;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                this.groupBox3.Visible = true;
                this.Width = 869;
            }
            else
            {
                this.groupBox3.Visible = false;
                this.Width = 474;
            }
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void abrirAnimacionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.DefaultExt = ".ani";
            this.openFileDialog1.Filter = "Archivos de animación (*.ani) | *.ani";
            this.openFileDialog1.InitialDirectory = Environment.CurrentDirectory + "\\Data\\Animation";
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                myErf.path = this.openFileDialog1.FileName;
                myErf.reloadAnimation();
                resetBar(myErf.getCountPosition());
                this.label21.Text = Convert.ToString(1);
            }
        }

        //Boton para guardar estados persistentemente.
        private void button5_Click(object sender, EventArgs e)
        {
            this.saveFileDialog1.DefaultExt = ".sta";
            this.saveFileDialog1.Filter = "Archivos de estado (*.sta) | *.sta";
            this.saveFileDialog1.InitialDirectory = Environment.CurrentDirectory + "\\Data\\States";
            if (this.saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (this.saveFileDialog1.FileName.EndsWith(".sta"))
                    myErf.saveStates(this.saveFileDialog1.FileName);
                else
                    myErf.saveStates(this.saveFileDialog1.FileName + ".sta");
            }
        }

        //Boton para cargar estados desde persistencia.
        private void button8_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.DefaultExt = ".sta";
            this.openFileDialog1.Filter = "Archivos de estado (*.sta) | *.sta";
            this.openFileDialog1.InitialDirectory = Environment.CurrentDirectory + "\\Data\\States";
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                myErf.loadStates(this.openFileDialog1.FileName);
                cargarEstados();
            }
        }

        //Boton para guardar gestos persistentemente.
        private void button7_Click(object sender, EventArgs e)
        {
            this.saveFileDialog1.DefaultExt = ".ges";
            this.saveFileDialog1.Filter = "Archivos de gesto (*.ges) | *.ges";
            this.saveFileDialog1.InitialDirectory = Environment.CurrentDirectory + "\\Data\\Gestures";
            if (this.saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (this.saveFileDialog1.FileName.EndsWith(".ges"))
                    myErf.saveGestures(this.saveFileDialog1.FileName);
                else
                    myErf.saveGestures(this.saveFileDialog1.FileName + ".ges");
            }
        }

        //Boton para cargar gestos desde persistencia.
        private void button9_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.DefaultExt = ".ges";
            this.openFileDialog1.Filter = "Archivos de gesto (*.ges) | *.ges";
            this.openFileDialog1.InitialDirectory = Environment.CurrentDirectory + "\\Data\\Gestures";
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                myErf.loadGestures(this.openFileDialog1.FileName);
                cargarGestos();
            }
        }

        private void listBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
            {
                int index = listBox1.SelectedIndex;
                myErf.removeState(index);
                cargarEstados();
            }
        }

        private void generaciónAvanzadaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            myAdvance = new WFAdvanceStatesGeneration(this, myErf);
            myAdvance.Show();
        }

        private void WFEditGestures_VisibleChanged(object sender, EventArgs e)
        {
            if ((myAdvance != null) && (this.Visible == false))
                myAdvance.Visible = false;
        }

        internal void addAllStates(List<AbstractState> myStates)
        {
            for (int index = 0; index < myStates.Count; index++)
            {
                listBox1.Items.Add(myStates[index].getStateName());
                comboBox3.Items.Add(myStates[index].getStateName());
                comboBox6.Items.Add(myStates[index].getStateName());
                comboBox7.Items.Add(myStates[index].getStateName());
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                int inicio = String.IsNullOrEmpty(textBox6.Text) ? 0 : System.Convert.ToInt16(textBox6.Text);
                int fin = String.IsNullOrEmpty(textBox7.Text) ? 0 : System.Convert.ToInt16(textBox7.Text);
                if(inicio<fin)
                    if ( (inicio >= 0) && (fin <= myErf.getCountPosition()) )
                        myErf.cropAnimation(inicio,fin);

            }
            catch (Exception)
            {
                MessageBox.Show("Datos Inválidos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            this.saveFileDialog1.DefaultExt = ".ani";
            this.saveFileDialog1.Filter = "Archivos de animación (*.ani) | *.ani";
            this.saveFileDialog1.InitialDirectory = Environment.CurrentDirectory + "\\Data\\Animation";
            if (this.saveFileDialog1.ShowDialog() == DialogResult.OK)
                myErf.saveAnimation(this.saveFileDialog1.FileName);
        }

        private void resetBar(int maximumValue) 
        {
            this.trackBar1.Maximum = maximumValue;
            this.trackBar1.Value = 0;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            this.label21.Text = this.trackBar1.Value.ToString();
            myErf.goToPosition(this.trackBar1.Value);

        }
    }
}
