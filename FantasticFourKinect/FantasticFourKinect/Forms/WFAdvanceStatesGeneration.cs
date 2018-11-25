using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FantasticFourKinect.Skeleton;
using FantasticFourKinect.State;

namespace FantasticFourKinect.Forms
{
    partial class WFAdvanceStatesGeneration : Form
    {
        private WFEditGestures wFEditGestures;
        private EditRecordedForm myErf;
        private List<String> bodyParts;
        private List<String> stateTypes;
        private List<AbstractState> myStates;
        private AbstractState currentStateEditing;

        public WFAdvanceStatesGeneration(WFEditGestures wFEditGestures, EditRecordedForm myErf)
        {
            this.wFEditGestures = wFEditGestures;
            this.wFEditGestures.Enabled = false;
            this.myErf = myErf;
            bodyParts = new List<String>();
            stateTypes = new List<String>();
            myStates = new List<AbstractState>();
            InitializeComponent();
        }

        // Algoritmo para generar automáticamente estados a partir de una posición del esqueleto
        private void button1_Click(object sender, EventArgs e)
        {
            this.dataGridView1.Rows.Clear();

            for (int parte1 = 0; parte1 < bodyParts.Count; parte1++)
                for (int parte2 = 0; parte2 < bodyParts.Count; parte2++)
                    for (int index = 0; index < stateTypes.Count; index++)
                    {
                        // Intenta generar un estado específico para un par de partes del cuerpo, ej. [Head,LeftHand,OVER]. 
                        verificarEstado(bodyParts[parte1] + "_" + stateTypes[index] + "_" + bodyParts[parte2], bodyParts[parte1], bodyParts[parte2], stateTypes[index]);
                    }

            this.label1.Text = "Cantidad de estados: " + myStates.Count;
        }

        private void verificarEstado(string stateName, string p1, string p2, string stateType)
        {
            // Sólo se agrega un estado generado automáticamente si se cumple en la posición del esqueleto actual
            if (myErf.cumpleState(stateName, p1, p2, null, 0, -1, stateType, -1, -1))
            {
                AbstractState aS = myErf.makeState(stateName, p1, p2, null, 0, -1, stateType, -1, -1);
                myStates.Add(aS);
                this.dataGridView1.Rows.Add(aS.getStateName(), 0);
            }
        }

        #region Agregar o remover un tipo de estado a la lista
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                stateTypes.Add("CENTER_X");
            else stateTypes.Remove("CENTER_X");
        }

        private void checkBox16_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox16.Checked)
                stateTypes.Add("CENTER_Y");
            else stateTypes.Remove("CENTER_Y");
        }

        private void checkBox17_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox17.Checked)
                stateTypes.Add("CENTER_Z");
            else stateTypes.Remove("CENTER_Z");
        }

        private void checkBox18_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox18.Checked)
                stateTypes.Add("DISTANCE");
            else stateTypes.Remove("DISTANCE");
        }

        private void checkBox19_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox19.Checked)
                stateTypes.Add("LEFT");
            else stateTypes.Remove("LEFT");
        }

        private void checkBox20_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox20.Checked)
                stateTypes.Add("OVER");
            else stateTypes.Remove("OVER");
        }

        private void checkBox21_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox21.Checked)
                stateTypes.Add("POSITION");
            else stateTypes.Remove("POSITION");
        }

        private void checkBox22_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox22.Checked)
                stateTypes.Add("RIGHT");
            else stateTypes.Remove("RIGHT");
        }

        private void checkBox23_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox23.Checked)
                stateTypes.Add("UNDER");
            else stateTypes.Remove("UNDER");
        }
        #endregion

        #region Agregar o remover partes del cuerpo a la lista
        private void Head_CheckedChanged(object sender, EventArgs e)
        {
            if (Head.Checked)
                bodyParts.Add("Head");
            else bodyParts.Remove("Head");
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
                bodyParts.Add("Neck");
            else bodyParts.Remove("Neck");
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
                bodyParts.Add("LeftShoulder");
            else bodyParts.Remove("LeftShoulder");
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
                bodyParts.Add("LeftElbow");
            else bodyParts.Remove("LeftElbow");
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked)
                bodyParts.Add("LeftHand");
            else bodyParts.Remove("LeftHand");
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked)
                bodyParts.Add("RightShoulder");
            else bodyParts.Remove("RightShoulder");
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox7.Checked)
                bodyParts.Add("RightElbow");
            else bodyParts.Remove("RightElbow");
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox8.Checked)
                bodyParts.Add("RightHand");
            else bodyParts.Remove("RightHand");
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox9.Checked)
                bodyParts.Add("Torso");
            else bodyParts.Remove("Torso");
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox10.Checked)
                bodyParts.Add("LeftHip");
            else bodyParts.Remove("LeftHip");
        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox11.Checked)
                bodyParts.Add("LeftKnee");
            else bodyParts.Remove("LeftKnee");
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox12.Checked)
                bodyParts.Add("LeftFoot");
            else bodyParts.Remove("LeftFoot");
        }

        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox13.Checked)
                bodyParts.Add("RightHip");
            else bodyParts.Remove("RightHip");
        }

        private void checkBox14_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox14.Checked)
                bodyParts.Add("RightKnee");
            else bodyParts.Remove("RightKnee");
        }

        private void checkBox15_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox15.Checked)
                bodyParts.Add("RightFoot");
            else bodyParts.Remove("RightFoot");
        }
        #endregion

        // Agrega todos los estados generados automáticamente
        private void button2_Click(object sender, EventArgs e)
        {
            myErf.addState(myStates);
            wFEditGestures.addAllStates(myStates);
            MessageBox.Show("Se agregaron " + myStates.Count + " estados correctamente", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Habilita el formulario padre cuando este se cierra
        private void WFAdvanceStatesGeneration_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.wFEditGestures.Enabled = true;
        }

        // Agrega sólo los estados seleccionados
        private void agregarSeleccionadosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<AbstractState> selectedStates = getSelectedStates();
            if (selectedStates.Count > 0)
            {
                myErf.addState(selectedStates);
                wFEditGestures.addAllStates(selectedStates);
                MessageBox.Show("Se agregaron " + selectedStates.Count + " estados correctamente", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Evento disparado cuando se comienza a editar una celda
        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            currentStateEditing = myStates[e.RowIndex];
        }

        // Evento disparado cuando se termina de editar una celda
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            currentStateEditing.setPrecision(Convert.ToDouble(dataGridView1[e.ColumnIndex, e.RowIndex].Value));
        }

        // Evento que genera una cadena de estados anidados AND de las filas seleccionadas
        private void crearANDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<AbstractState> selectedStates = getSelectedStates();
            if (selectedStates.Count > 1)
            {
                AbstractState aS = new AndState(selectedStates[0], selectedStates[1], "AND " + selectedStates.Count);
                for (int index = 2; index < selectedStates.Count - 1; index++)
                {
                    aS = new AndState(selectedStates[index], aS, "AND " + selectedStates.Count);
                }
                myStates.Add(aS);
                this.dataGridView1.Rows.Add(aS.getStateName(), "---");
                this.label1.Text = "Cantidad de estados: " + myStates.Count.ToString();
            }
            else MessageBox.Show("Deben seleccionar al menos 2 estados", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Retorna una lista con los estados seleccionados del DataGridView
        private List<AbstractState> getSelectedStates()
        {
            DataGridViewSelectedRowCollection dg = dataGridView1.SelectedRows;
            List<AbstractState> selectedStates = new List<AbstractState>();
            for (int index = 0; index < dg.Count; index++)
            {
                int position = dg[index].Cells["Column1"].RowIndex;
                selectedStates.Add(myStates[position]);
            }

            return selectedStates;
        }

        // Evento que genera una cadena de estados anidados OR de las filas seleccionadas
        private void crearORToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<AbstractState> selectedStates = getSelectedStates();
            if (selectedStates.Count > 1)
            {
                AbstractState aS = new OrState(selectedStates[0], selectedStates[1], "OR " + selectedStates.Count);
                for (int index = 2; index < selectedStates.Count - 1; index++)
                {
                    aS = new OrState(selectedStates[index], aS, "OR " + selectedStates.Count);
                }
                myStates.Add(aS);
                this.dataGridView1.Rows.Add(aS.getStateName(), "---");
                this.label1.Text = "Cantidad de estados: " + myStates.Count.ToString();
            }
            else MessageBox.Show("Deben seleccionar al menos 2 estados", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
