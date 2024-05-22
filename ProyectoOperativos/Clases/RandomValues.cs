using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
}
