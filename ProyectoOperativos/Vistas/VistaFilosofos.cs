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

    [Obsolete]
    public partial class VistaFilosofos : Form
    {
        private ProblemaFilosofos problema;

        public VistaFilosofos()
        {
            InitializeComponent();
            problema = new ProblemaFilosofos(this);
            rebootSreen();
            toolStripButton2.Text = "Detener simulación";
            this.splitContainer1_Panel1_Resize(null, null);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (problema.Running)
            {
                toolStripButton1.Image = Properties.Resources.Play;
                toolStripButton1.Text = "Reanudar simulación";
                iniciarToolStripMenuItem.Image = Properties.Resources.Play;
                statusLabel.Text = "Simulación pausada";
                iniciarToolStripMenuItem.Text = "Reanudar";
                problema.Suspend();
            }
            else if (problema.Paused)
            {
                toolStripButton1.Image = Properties.Resources.Pause;
                toolStripButton1.Text = "Pausar simulación";
                iniciarToolStripMenuItem.Image = Properties.Resources.Pause;
                iniciarToolStripMenuItem.Text = "Pausar";
                statusLabel.Text = "Simulación en curso...";
                problema.Resume();
            }
            else
            {
                toolStripButton1.Image = Properties.Resources.Pause;
                toolStripButton1.Text = "Pausar simulación";
                iniciarToolStripMenuItem.Image = Properties.Resources.Pause;
                iniciarToolStripMenuItem.Text = "Pausar";
                statusLabel.Text = "Simulación en curso...";
                problema.Start();
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            problema.Abort();
            rebootSreen();
        }

        private void rebootSreen()
        {
            richTextBox1.Clear();
            toolStripButton1.Image = Properties.Resources.Play;
            toolStripButton1.Text = "Iniciar simulación";
            iniciarToolStripMenuItem.Image = Properties.Resources.Play;
            iniciarToolStripMenuItem.Text = "Iniciar";
            statusLabel.Text = "Aplicación lista para iniciar simulación";
            foreach (Panel plt in panel13.Controls)
            {
                if (plt.Name.Contains("palillo")) plt.BackColor = SystemColors.Control;
                else if (plt.Name.Contains("filosofo")) plt.BackColor = Color.LightGray;
            }
            foreach (Panel plt in panel1.Controls)
            {
                if (plt.Name.Contains("palillo")) plt.BackColor = Color.Black;
            }

        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            this.toolStripButton2_Click(sender, e);
            this.toolStripButton1_Click(sender, e);
        }

        private void reiniciarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.toolStripButton3_Click(sender, e);
        }

        private void pararToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this .toolStripButton2_Click(sender, e);
        }

        private void iniciarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.toolStripButton1_Click(sender, e);
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Salir
            this.Close();
        }

        private void splitContainer1_Panel1_Resize(object sender, EventArgs e)
        {
            // Posicionar panel13 al centro de splitContainer1.Panel1
            panel13.Location = new Point((splitContainer1.Panel1.Width - panel13.Width) / 2, (splitContainer1.Panel1.Height - panel13.Height) / 2);
        }

        private bool showingTools = true;

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (showingTools)
            {
                toolStripButton4.Image = Properties.Resources.CheckBoxUnchecked;
                toolStripButton4.ToolTipText = "Mostrar controles";
                splitContainer1.Panel2Collapsed = true;
                showingTools = false;
            }
            else
            {
                toolStripButton4.Image = Properties.Resources.CheckBoxChecked;
                toolStripButton4.ToolTipText = "Ocultar controles";
                splitContainer1.Panel2Collapsed = false;
                showingTools = true;
            }
        }

        private void etiquetasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!showingTools)
            {
                toolStripButton4_Click(sender, e);
            }
            tabControl1.SelectedTab = tabPage1;
        }

        private void velocidadToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (!showingTools)
            {
                toolStripButton4_Click(sender, e);
            }
            tabControl1.SelectedTab = tabPage2;
        }

        private void terminalToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (!showingTools)
            {
                toolStripButton4_Click(sender, e);
            }
            tabControl1.SelectedTab = tabPage3;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            changeFilosofoSleep();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            changeFilosofoSleep();
        }

        private void changeFilosofoSleep()
        {
            float value = (float)trackBar1.Value;
            float range = (float)numericUpDown1.Value;
            float min = value - range;
            float max = value + range;
            if (max < 0.5f)
            {
                richTextBox1.AppendText(" -> No se puede actualizar el log con velocidades menores a 0.5s\n");
                canUpdateLog = false;
            }
            else canUpdateLog = true;
            if (min < 0) min = 0;
            problema.SetSleepFilosofo((int)(min * 1000), (int)(max * 1000));
            // formato : Entre 1.5s y 4.5s
            label11.Text = "Entre " + min.ToString("0.0") + "s y " + max.ToString("0.0") + "s";
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            changePalilloSleep();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            changePalilloSleep();
        }

        private void changePalilloSleep()
        {
            float value = (float)trackBar2.Value;
            float range = (float)numericUpDown2.Value;
            float min = value - range;
            float max = value + range;
            if (max < 0.5f)
            {
                richTextBox1.AppendText(" -> No se puede actualizar el log con velocidades menores a 0.5s\n");
                canUpdateLog = false;
            }
            else canUpdateLog = true;
            if (min < 0) min = 0;
            problema.SetSleepPalillo((int)(min * 1000), (int)(max * 1000));
            // formato : Entre 1.5s y 4.5s
            label12.Text = "Entre " + min.ToString("0.0") + "s y " + max.ToString("0.0") + "s";
        }


        private bool canUpdateLog = true;
        public void logUpdate(string text)
        {
            if (!canUpdateLog)
            {
                return;
            }
            if (this.richTextBox1.InvokeRequired)
            {
                this.richTextBox1.Invoke(new Action(() => richTextBox1.AppendText(" -> " + text + "\n")));
                this.richTextBox1.Invoke(new Action(() => richTextBox1.ScrollToCaret()));
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
            {
                problema.ChangeBehaviour(3);
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
            {
                problema.ChangeBehaviour(2);
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                problema.ChangeBehaviour(1);
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                problema.ChangeBehaviour(0);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            trackBar1.Value = 3;
            trackBar2.Value = 1;
            numericUpDown1.Value = 1.5M;
            numericUpDown2.Value = 0.5M;
            changeFilosofoSleep();
            changePalilloSleep();
        }
    }
}
