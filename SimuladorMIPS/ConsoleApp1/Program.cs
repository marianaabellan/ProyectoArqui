using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Threading;

namespace SimuladorMIPS
{
    class Program
    {
        static void Main(string[] args)
        {
            Memoria mem = Memoria.Instance;
            NucleoMultihilillo N0 = NucleoMultihilillo.Instance;
            NucleoMonohilillo N1 = NucleoMonohilillo.Instance;
            Queue colaHilillos = new Queue();
            int reloj = 0;

            N0.ColaHilillos = colaHilillos;
            N1.ColaHilillos = colaHilillos;

            // TODO: Solicitar al usuario hilillos a correr.

            // TODO: Cargar hilillos en memoria.

            // TODO: Solicitar al usuario el quantum.

            // TODO: Solicitar al usuario modalidad de ejecución (lenta/rápida).
            bool ejecucionLentaActivada = false;

            Thread nucleo0 = new Thread(N0.start);
            Thread nucleo1 = new Thread(N1.start);

            // Esto echa a andar los hilos.
            nucleo0.Start();
            nucleo1.Start();

            Monitor.Enter(N0.Terminado);
            Monitor.Enter(N1.Terminado);
            while(!N0.Terminado || !N1.Terminado) // Sección crítica.
            {
                Monitor.Exit(N0.Terminado);
                Monitor.Exit(N1.Terminado);

                reloj++;

                // TODO: Imprimir reloj.
                // TODO: Imprimir identificación de hilillos en ejecución.

                if (ejecucionLentaActivada && reloj % 20 == 0)
                {
                    // TODO: Imprimir memoria cachés y registros.

                    Console.ReadKey();
                }

                Monitor.Enter(N0.Terminado);
                Monitor.Enter(N1.Terminado);
            }
            Monitor.Exit(N0.Terminado);
            Monitor.Exit(N1.Terminado);

            // TODO: Imprimir contenido de memoria y cachés.

            // TODO: Para cada hilillo que corrió, imprimir registros y ciclos que duró.
            // WARNING: Revisar diseño para lograr lo anterior.
        }
    }
}
