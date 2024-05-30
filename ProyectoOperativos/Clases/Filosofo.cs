using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace ProyectoOperativos
{
    [Obsolete]
    public class Filosofo
    {
        private int Id { get; set; }
        public int minSleep { get; set; } = 1500;
        public int maxSleep { get; set; } = 4500;

        private bool running = false;
        private Panel filosofoView, izqView, derView;
        private VistaFilosofos vista;
        private List<Palillo> palillos = null;
        private int behaviour = 3;
        Thread hilo;

        public Filosofo(List<Palillo> palillos, int id, VistaFilosofos vista)
        {
            this.vista = vista;
            this.palillos = palillos;
            this.Id = id;

            // Get panel 'filosofoId' from the view and set it to the current filosofo
            filosofoView = vista.Controls.Find("filosofo" + Id, true).FirstOrDefault() as Panel;
            izqView = vista.Controls.Find("palillo" + Id + Id, true).FirstOrDefault() as Panel;
            derView = vista.Controls.Find("palillo" + Id + (Id + 1) % 5, true).FirstOrDefault() as Panel;
        }

        public void Start()
        {
            if (hilo != null) return;
            hilo = new Thread(ciclo); // Se crea un hilo
            this.running = true;
            hilo.Start(); // Se inicia el hilo
        }

        [Obsolete]
        public void Abort()
        {
            if (hilo == null) return;
            if (!this.running) hilo.Resume();
            hilo.Abort(); // Se detiene el hilo
            this.running = false;
        }

        [Obsolete]
        public void Suspend()
        {
            if (hilo == null) return;
            hilo.Suspend(); // Se suspende el hilo
            this.running = false;
        }

        [Obsolete]
        public void Resume()
        {
            if (hilo == null) return;
            hilo.Resume(); // Se reanuda el hilo
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
                    vista.logUpdate("Filosofo " + Id + " está hambriento");
                    // Se revisa el Id del filosofo para tomar los palillos en orden
                    // Si el Id es par, toma el palillo izquierdo primero
                    // Si el Id es impar, toma el palillo derecho primero
                    // De esta forma se evita un bloqueo mutuo cuando
                    // todos los filosofos toman el palillo izquierdo
                    switch (behaviour)
                    {
                        case 0:
                            tomaDerecho();
                            tomaIzquierdo();
                            break;
                        case 1:
                            tomaIzquierdo();
                            tomaDerecho();
                            break;
                        case 2:
                            if (RandomValues.RandomInt(0, 2) == 0)
                            {
                                tomaIzquierdo();
                                tomaDerecho();
                            }
                            else
                            {
                                tomaDerecho();
                                tomaIzquierdo();
                            }
                            break;
                        case 3:
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
                            break;
                    }
                    filosofoView.BackColor = System.Drawing.Color.Red;
                    vista.logUpdate("Filósofo " + Id + " empezó a comer");
                    RandomValues.RandomSleep(minSleep, maxSleep);
                    filosofoView.BackColor = System.Drawing.Color.LightGray;
                    vista.logUpdate("Filósofo " + Id + " terminó de comer");

                    dejaIzquierdo();
                    dejaDerecho();
                }
                else
                {
                    filosofoView.BackColor = System.Drawing.Color.LightBlue;
                    vista.logUpdate("Filósofo " + Id + " pensando");
                    RandomValues.RandomSleep(minSleep, maxSleep);
                }
            }
        }

        private void tomaIzquierdo()
        {
            palillos[Id].Tomar();
            vista.logUpdate("Filósofo " + Id + " tomó palillo izquierdo " + palillos[Id].Id);
            izqView.BackColor = System.Drawing.Color.Black;
        }

        private void tomaDerecho()
        {
            palillos[(Id + 1) % 5].Tomar();
            vista.logUpdate("Filósofo " + Id + " tomó palillo derecho " + palillos[(Id + 1) % 5].Id);
            derView.BackColor = System.Drawing.Color.Black;
        }

        private void dejaIzquierdo()
        {
            vista.logUpdate("Filósofo " + Id + " soltó palillo izquierdo " + palillos[Id].Id);
            izqView.BackColor = System.Drawing.Color.Transparent;
            palillos[Id].Soltar();
        }

        private void dejaDerecho()
        {
            vista.logUpdate("Filósofo " + Id + " soltó palillo derecho " + palillos[(Id + 1) % 5].Id);
            derView.BackColor = System.Drawing.Color.Transparent;
            palillos[(Id + 1) % 5].Soltar();
        }

        public void SetBehaviour(int behaviour)
        {
            if (behaviour < 0 || behaviour > 3) return;
            this.behaviour = behaviour;
        }

    }
}
