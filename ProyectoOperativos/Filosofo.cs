using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace ProyectoOperativos
{
    public static class RandomValues
    {
        private static Random r = new Random();
        public static int RandomInt(int min, int max)
        {
            return r.Next(min, max);
        }
        public static void RandomSleep(int min, int max)
        {
            Thread.Sleep(RandomInt(min, max));
        }
    }

    public class ProblemaFilosofos
    {
        private VistaFilosofos vista;
        private List<Palillo> palillos = null;
        private List<Filosofo> filosofos = null;
        private bool initialized = false;

        private int minSleepFilosofo { get; set; } = 2000;
        private int maxSleepFilosofo { get; set; } = 5000;
        private int minSleepPalillo { get; set; } = 1000;
        private int maxSleepPalillo { get; set; } = 2000;

        public ProblemaFilosofos(VistaFilosofos vista)
        {
            this.vista = vista;
            Initialize();
        }

        private void Initialize()
        {
            if (initialized) return;
            initialized = true;
            palillos = new List<Palillo>();
            filosofos = new List<Filosofo>();
            for (int i = 0; i < 5; i++)
            {
                palillos.Add(new Palillo(i, vista));
                filosofos.Add(new Filosofo(palillos, i, vista));
                palillos[i].minSleep = minSleepPalillo;
                palillos[i].maxSleep = maxSleepPalillo;
                filosofos[i].minSleep = minSleepFilosofo;
                filosofos[i].maxSleep = maxSleepFilosofo;
            }
        }

        public void Start()
        {
            if (!initialized) Initialize();
            foreach (Filosofo f in filosofos) f.Start();
        }

        public void Suspend()
        {
            foreach (Filosofo f in filosofos) f.Suspend();
        }

        public void Resume()
        {
            foreach (Filosofo f in filosofos) f.Resume();
        }

        public void Abort()
        {
            foreach (Filosofo f in filosofos) f.Abort();
            palillos = null;
            filosofos = null;
            initialized = false;
        }
    }

    public class Filosofo
    {
        private int Id { get; set; }
        public int minSleep { get; set; } = 2000;
        public int maxSleep { get; set; } = 5000;

        private bool running = false;
        private Panel filosofoView, izqView, derView;
        private List<Palillo> palillos = null;
        Thread hilo;

        public Filosofo(List<Palillo> palillos, int id, VistaFilosofos vista)
        {
            this.palillos = palillos;
            this.Id = id;

            // Get panel 'filosofoId' from the view and set it to the current filosofo
            filosofoView = vista.Controls.Find("filosofo" + Id, true).FirstOrDefault() as Panel;
            izqView = vista.Controls.Find("palillo" + Id + Id, true).FirstOrDefault() as Panel;
            derView = vista.Controls.Find("palillo" + Id + (Id + 1) % 5, true).FirstOrDefault() as Panel;
            Console.WriteLine("Filosofo " + Id + " creado");
            Console.WriteLine("Palillo izq " + Id + Id);
            Console.WriteLine("Palillo der " + Id + (Id + 1) % 5);
        }

        public void Start()
        {
            if (hilo != null) return;
            hilo = new Thread(ciclo); // Se crea un hilo
            this.running = true;
            hilo.Start(); // Se inicia el hilo
        }

        public void Abort()
        {
            if (hilo == null) return;
            hilo.Abort(); // Se detiene el hilo
            this.running = false;
        }

        public void Suspend()
        {
            if (hilo == null) return;
            this.running = false;
        }

        public void Resume()
        {
            if (hilo == null) return;
            this.running = true;
        }

        private void ciclo()
        {
            while (true)
            {
                // Zona critica por si se suspende el hilo
                while (!running) Thread.Sleep(100);
                int value = RandomValues.RandomInt(0, 6);
                if (value <= 1)
                {
                    filosofoView.BackColor = System.Drawing.Color.LightCoral;
                    // Hambriento
                    Console.WriteLine("Filosofo " + Id + " hambriento");
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

                    filosofoView.BackColor = System.Drawing.Color.Red;
                    Console.WriteLine("Filosofo " + Id + " comiendo");
                    RandomValues.RandomSleep(minSleep, maxSleep);
                    filosofoView.BackColor = System.Drawing.Color.LightGray;
                    Console.WriteLine("Filosofo " + Id + " termino de comer");

                    dejaIzquierdo();
                    dejaDerecho();
                }
                else
                {
                    filosofoView.BackColor = System.Drawing.Color.LightBlue;
                    Console.WriteLine("Filosofo " + Id + " pensando");
                    RandomValues.RandomSleep(minSleep, maxSleep);
                }
            }
        }

        private void tomaIzquierdo()
        {
            palillos[Id].Tomar();
            Console.WriteLine("Filosofo " + Id + " tomo palillo izquierdo " + palillos[Id].Id);
            izqView.BackColor = System.Drawing.Color.Black;
        }

        private void tomaDerecho()
        {
            palillos[(Id + 1) % 5].Tomar();
            Console.WriteLine("Filosofo " + Id + " tomo palillo derecho " + palillos[(Id + 1) % 5].Id);
            derView.BackColor = System.Drawing.Color.Black;
        }

        private void dejaIzquierdo()
        {
            Console.WriteLine("Filosofo " + Id + " solto palillo izquierdo " + palillos[Id].Id);
            izqView.BackColor = System.Drawing.Color.Transparent;
            palillos[Id].Soltar();
        }

        private void dejaDerecho()
        {
            Console.WriteLine("Filosofo " + Id + " solto palillo derecho " + palillos[(Id + 1) % 5].Id);
            derView.BackColor = System.Drawing.Color.Transparent;
            palillos[(Id + 1) % 5].Soltar();
        }

    }
}
