using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoOperativos
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());

            // Se crea un hilo para formulario de vista y mostrarlo
            Thread hilo = new Thread(() =>
            {
                VistaFilosofos vista = new VistaFilosofos();
                Application.Run(vista);
            });
            hilo.Start();

        }
    }
}
