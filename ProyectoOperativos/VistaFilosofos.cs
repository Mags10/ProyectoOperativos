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
        public VistaFilosofos()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Filosofo filosofo = new Filosofo();
            Filosofo filosofo2 = new Filosofo();
            Filosofo filosofo3 = new Filosofo();
            Filosofo filosofo4 = new Filosofo();
            Filosofo filosofo5 = new Filosofo();

            filosofo.Start();
            filosofo2.Start();
            filosofo3.Start();
            filosofo4.Start();
            filosofo5.Start();
        }
    }
}
