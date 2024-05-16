using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoOperativos
{
    public partial class VistaFilosofos : Form
    {
        private ProblemaFilosofos problema;

        public VistaFilosofos()
        {
            InitializeComponent();
            problema = new ProblemaFilosofos(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            problema.Start();
            this.button1.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Pause
            problema.Suspend();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Resume
            problema.Resume();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Stop
            problema.Abort();
            this.button1.Enabled = true;
        }
    }
}
