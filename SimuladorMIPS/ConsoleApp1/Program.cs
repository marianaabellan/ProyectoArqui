using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Threading;
using System.Diagnostics;
using System.IO;

// <>

namespace SimuladorMIPS
{
    class Program
    {
        static void Main(string[] args)
        {
            Memoria mem = Memoria.Instance;
            NucleoMultihilillo N0 = NucleoMultihilillo.Instance;
            NucleoMonohilillo N1 = NucleoMonohilillo.Instance;
            Queue<Hilillo> colaHilillos = new Queue<Hilillo>();
            int reloj = 0;

            // TIP: La clase Debug permite imprimir mensajes de debug en una consola distinta de la principal.
            Debug.Print("Asignando cola de hilillos a núcleos...");
            N0.ColaHilillos = colaHilillos;
            N1.ColaHilillos = colaHilillos;

            // Solicitar al usuario hilillos a correr y cargarlos en memoria.
            int direccionDeInicioDeHilillo = 384; // Indica dónde comienza las instrucciones de cada hilillo.
            Console.WriteLine("Usted se encuentra en la carpeta " + Directory.GetCurrentDirectory());

            Console.WriteLine("Inserte el nombre de un archivo hilillo o 'c' para continuar.");
            string nombreDeArchivo = Console.ReadLine();

            // TODO: Revisar posibles excepciones.
            while (nombreDeArchivo != "c")
            {
                Hilillo h = new Hilillo(direccionDeInicioDeHilillo);
                colaHilillos.Enqueue(h);

                StreamReader archivo = new StreamReader(nombreDeArchivo);
                while (!archivo.EndOfStream)
                {
                    string instruccion = "";
                    try
                    {
                        instruccion = archivo.ReadLine();
                    }
                    catch (IOException e)
                    {
                        // TODO: Revisar qué hacer en este caso.
                        Console.WriteLine("Error al leer el archivo.");
                        break;
                    }
                    string[] temp = instruccion.Split(' ');
                    for (int i = 0; i < 4; i++)
                        mem.Mem[direccionDeInicioDeHilillo + i] = Convert.ToInt32(temp[i]);
                    direccionDeInicioDeHilillo += 4;
                }

                Console.WriteLine("Inserte el nombre de un archivo hilillo o 'c' para continuar.");
                nombreDeArchivo = Console.ReadLine();
            }

            // Solicitar al usuario el quantum.
            Console.WriteLine("Inserte el quantum:");
            int quantum = Convert.ToInt32(Console.ReadLine());
            N0.Quantum = N1.Quantum = quantum;

            // Solicitar al usuario modalidad de ejecución (lenta/rápida).
            bool ejecucionLentaActivada = false;
            Console.WriteLine("¿Desea activar la modalidad de ejecución lenta? (s/n)");
            if (Console.ReadLine() == "s")
            {
                ejecucionLentaActivada = true;
                Console.WriteLine("Ejecución lenta activada.");
            }

            Debug.Print("Creando hilos de simulación...");
            Thread nucleo0 = new Thread(N0.start);
            Thread nucleo1 = new Thread(N1.start);

            // Esto echa a andar los hilos.
            nucleo0.Start();
            nucleo1.Start();

            Debug.Print("Hilo principal: Entrando a sección crítica: revisando si los núcleos terminaron...");
            Monitor.Enter(N0.Terminado);
            Monitor.Enter(N1.Terminado);
            while(!N0.Terminado || !N1.Terminado) // Sección crítica.
            {
                Monitor.Exit(N0.Terminado);
                Monitor.Exit(N1.Terminado);
                Debug.Print("Hilo principal: fin de sección crítica. Los núcleos no han terminado.");

                reloj++;

                // TODO: Imprimir reloj.
                // TODO: Imprimir identificación de hilillos en ejecución.

                if (ejecucionLentaActivada && reloj % 20 == 0)
                {
                    // TODO: Imprimir memoria, cachés y registros.

                    Console.ReadKey();
                }

                Debug.Print("Hilo principal: Entrando a sección crítica: revisando si los núcleos terminaron...");
                Monitor.Enter(N0.Terminado);
                Monitor.Enter(N1.Terminado);
            }
            Monitor.Exit(N0.Terminado);
            Monitor.Exit(N1.Terminado);
            Debug.Print("Hilo principal: fin de sección crítica. Los núcleos terminaron.");

            // TODO: Imprimir contenido de memoria y cachés.

            // TODO: Para cada hilillo que corrió, imprimir registros y ciclos que duró.
            // WARNING: Revisar diseño para lograr lo anterior.
        }
    }
}
