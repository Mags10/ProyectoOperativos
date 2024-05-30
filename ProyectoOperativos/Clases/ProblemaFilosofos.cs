using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoOperativos
{
    [Obsolete]
    public class ProblemaFilosofos
    {
        private VistaFilosofos vista;
        private List<Palillo> palillos = null;
        private List<Filosofo> filosofos = null;
        private bool initialized = false;
        private bool running = false;
        private bool paused = false;
        private int behaviour = 3;

        private int minSleepFilosofo = 2000;
        private int maxSleepFilosofo = 5000;
        private int minSleepPalillo = 1000;
        private int maxSleepPalillo = 2000;

        // Getters and Setters
        public bool Running { get => running; }
        public bool Paused { get => paused; }

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
                filosofos[i].SetBehaviour(behaviour);
            }
        }

        public void SetSleepFilosofo(int min, int max)
        {
            minSleepFilosofo = min;
            maxSleepFilosofo = max;
            if (!initialized) return;
            foreach (Filosofo f in filosofos)
            {
                f.minSleep = minSleepFilosofo;
                f.maxSleep = maxSleepFilosofo;
            }
        }

        public void SetSleepPalillo(int min, int max)
        {
            minSleepPalillo = min;
            maxSleepPalillo = max;
            foreach (Palillo p in palillos)
            {
                p.minSleep = minSleepPalillo;
                p.maxSleep = maxSleepPalillo;
            }
        }

        public void Start()
        {
            if (!initialized) Initialize();
            foreach (Filosofo f in filosofos) f.Start();
            this.initialized = true;
            this.running = true;
            this.paused = false;
        }

        [Obsolete]
        public void Suspend()
        {
            foreach (Filosofo f in filosofos) f.Suspend();
            this.paused = true;
            this.running = false;
        }

        [Obsolete]
        public void Resume()
        {
            foreach (Filosofo f in filosofos) f.Resume();
            this.paused = false;
            this.running = true;
        }

        [Obsolete]
        public void Abort()
        {
            if (!initialized) return;
            foreach (Filosofo f in filosofos) f.Abort();
            palillos = null;
            filosofos = null;
            initialized = false;
            running = false;
            paused = false;
        }

        [Obsolete]
        public void ChangeBehaviour(int behaviour)
        {
            this.behaviour = behaviour;
            if (!initialized) return;
            foreach (Filosofo f in filosofos) f.SetBehaviour(behaviour);
        }
    }
}
