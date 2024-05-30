using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoOperativos
{
    public class Palillo
    {
        public int Id { get; }
        public int minSleep { get; set; } = 500;
        public int maxSleep { get; set; } = 1500;

        private Panel palilloView;
        private Semaphore semaforo = new Semaphore(1, 1);

        public Palillo(int id, VistaFilosofos vista)
        {
            this.Id = id;
            palilloView = vista.Controls.Find("palillo" + Id, true).FirstOrDefault() as Panel;
            palilloView.BackColor = System.Drawing.Color.Black;
        }

        public void Tomar()
        {
            semaforo.WaitOne();
            RandomValues.RandomSleep(minSleep, maxSleep);
            palilloView.BackColor = System.Drawing.Color.Bisque;
        }

        public void Soltar()
        {
            palilloView.BackColor = System.Drawing.Color.Black;
            RandomValues.RandomSleep(minSleep, maxSleep);
            semaforo.Release();
        }
    }
}