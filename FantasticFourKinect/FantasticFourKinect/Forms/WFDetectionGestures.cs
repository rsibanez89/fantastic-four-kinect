using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FantasticFourKinect.Forms
{
    partial class WFDetectionGestures : Form
    {
        #region Variables privadas
        private LoadableForm myForm;
        #endregion

        #region Constructor
        public WFDetectionGestures(LoadableForm DF)
        {
            InitializeComponent();
            this.myForm = DF;
        }
        #endregion

        #region Log de eventos en los list box
        public void addLogList1(String log)
        {
            this.listBox1.Items.Add(log);
            this.listBox1.SelectedIndex = listBox1.Items.Count - 1;
        }

        public void addLogList2(String log)
        {
            this.listBox2.Items.Add(log);
            this.listBox2.SelectedIndex = listBox2.Items.Count - 1;
        }

        public void addLogList3(String log)
        {
            this.listBox3.Items.Add(log);
            this.listBox3.SelectedIndex = listBox3.Items.Count - 1;
        }
        #endregion

        #region Métodos para la carga de datos (animaciones, estados y gestos)
        private void estadosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.DefaultExt = ".sta";
            this.openFileDialog1.Filter = "Archivos de estado (*.sta) | *.sta";
            this.openFileDialog1.InitialDirectory = Environment.CurrentDirectory + "\\Data\\States";
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
                myForm.loadStates(this.openFileDialog1.FileName);
        }

        private void gestosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.DefaultExt = ".ges";
            this.openFileDialog1.Filter = "Archivos de gesto (*.ges) | *.ges";
            this.openFileDialog1.InitialDirectory = Environment.CurrentDirectory + "\\Data\\Gestures";
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
                myForm.loadGestures(this.openFileDialog1.FileName);
        }

        private void animacionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.DefaultExt = ".ani";
            this.openFileDialog1.Filter = "Archivos de animación (*.ani) | *.ani";
            this.openFileDialog1.InitialDirectory = Environment.CurrentDirectory + "\\Data\\Animation";
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
                myForm.loadAnimation(this.openFileDialog1.FileName);
        }
        #endregion
    }
}
