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
        private static int contador = 0;
        public static VistaFilosofos vista;
        private Panel palilloView;
        private Semaphore semaforo = new Semaphore(1, 1);

        public Palillo()
        {
            this.Id = contador++;
            palilloView = vista.Controls.Find("palillo" + Id, true).FirstOrDefault() as Panel;
            palilloView.BackColor = System.Drawing.Color.FromArgb(255, 0, 0, 0);
            //semaforo.Release();
        }

        public void Tomar()
        {
            semaforo.WaitOne();
            randomSleep();
            palilloView.BackColor = System.Drawing.Color.FromArgb(255, 255, 224, 192);
        }

        public void Soltar()
        {
            palilloView.BackColor = System.Drawing.Color.FromArgb(255, 0, 0, 0);
            randomSleep();
            semaforo.Release();
        }

        private void randomSleep()
        {
            Thread.Sleep(randomInt(1000, 2000));
        }
        private int randomInt(int min, int max)
        {
            Random r = new Random();
            return r.Next(min, max);
        }
    }
}