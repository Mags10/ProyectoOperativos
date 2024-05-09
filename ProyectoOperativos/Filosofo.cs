using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace ProyectoOperativos
{
    public class Filosofo
    {
        // Recurso compartido
        // Cada palillo es un semaforo binario
        private static List<Palillo> palillos = null;

        private int Id { get; set; }
        private Panel filosofoView, izqView, derView;
        private static int contador = 0;
        public static VistaFilosofos vista;
        Thread hilo;

        public Filosofo()
        {
            this.Id = contador++;

            // Get panel 'filosofoId' from the view and set it to the current filosofo
            filosofoView = vista.Controls.Find("filosofo" + Id, true).FirstOrDefault() as Panel;
            izqView = vista.Controls.Find("palillo" + Id + Id, true).FirstOrDefault() as Panel;
            derView = vista.Controls.Find("palillo" + Id + (Id + 1) % 5, true).FirstOrDefault() as Panel;
            Console.WriteLine("Filosofo " + Id + " creado");
            Console.WriteLine("Palillo izq " + Id + Id);
            Console.WriteLine("Palillo der " + Id + (Id + 1) % 5);

            if (palillos == null)
            {
                Palillo.vista = vista;
                palillos = new List<Palillo>();
                for (int i = 0; i < 5; i++)
                {
                    palillos.Add(new Palillo());
                }
            }
        }

        public void Start()
        {
            hilo = new Thread(ciclo); // Se crea un hilo
            hilo.Start(); // Se inicia el hilo
        }

        public void Abort()
        {
            hilo.Abort(); // Se detiene el hilo
        }

        public void ciclo()
        {
            while (true)
            {
                int value = randomInt(0, 3);
                if (value == 1)
                {
                    filosofoView.BackColor = System.Drawing.Color.FromArgb(64, 255, 0, 0);
                    // Hambriento
                    // Se revisa el Id del filosofo para tomar los palillos en orden
                    // Si el Id es par, toma el palillo izquierdo primero
                    // Si el Id es impar, toma el palillo derecho primero
                    // De esta forma se evita un bloqueo mutuo cuando
                    //      todos los filosofos toman el palillo izquierdo
                    if (this.Id % 2 == 0)
                    {
                        tomaIzquierdo();
                        tomaDerecho();
                    }
                    else
                    {
                        tomaDerecho();
                        tomaIzquierdo();
                    }

                    filosofoView.BackColor = System.Drawing.Color.FromArgb(255, 255, 0, 0);
                    Console.WriteLine("Filosofo " + Id + " comiendo");
                    randomSleep();
                    randomSleep();
                    filosofoView.BackColor = System.Drawing.Color.FromArgb(255, 224, 224, 224);
                    Console.WriteLine("Filosofo " + Id + " termino de comer");
                
                    if (this.Id % 2 == 0)
                    {
                        dejaIzquierdo();
                        dejaDerecho();
                    }
                    else
                    {
                        dejaDerecho();
                        dejaIzquierdo();
                    }
                }
                else
                {
                    filosofoView.BackColor = System.Drawing.Color.LightBlue;
                    // Pensando
                    Console.WriteLine("Filosofo " + Id + " pensando");
                    randomSleep();
                    randomSleep();
                    //filosofoView.BackColor = System.Drawing.Color.FromArgb(255, 224, 224, 224);
                }
            }
        }

        private void tomaIzquierdo()
        {
            palillos[Id].Tomar();
            Console.WriteLine("Filosofo " + Id + " tomo palillo izquierdo " + palillos[Id].Id);
            izqView.BackColor = System.Drawing.Color.FromArgb(255, 0, 0, 0);
        }

        private void tomaDerecho()
        {
            palillos[(Id + 1) % 5].Tomar();
            Console.WriteLine("Filosofo " + Id + " tomo palillo derecho " + palillos[(Id + 1) % 5].Id);
            derView.BackColor = System.Drawing.Color.FromArgb(255, 0, 0, 0);
        }

        private void dejaIzquierdo()
        {
            izqView.BackColor = System.Drawing.Color.FromArgb(255, 255, 255, 255);
            palillos[Id].Soltar();
            Console.WriteLine("Filosofo " + Id + " solto palillo izquierdo " + palillos[Id].Id);
        }

        private void dejaDerecho()
        {
            derView.BackColor = System.Drawing.Color.FromArgb(255, 255, 255, 255);
            palillos[(Id + 1) % 5].Soltar();
            Console.WriteLine("Filosofo " + Id + " solto palillo derecho " + palillos[(Id + 1) % 5].Id);
        }


        private void randomSleep()
        {
            Thread.Sleep(randomInt(2000, 5000));
        }

        private int randomInt(int min, int max)
        {
            // Random number generator with seed based on current time
            Random r = new Random();
            return (r.Next(min, max * 22*45*r.Next(1,100)) % (max - min)) + min;
        }

    }
}
