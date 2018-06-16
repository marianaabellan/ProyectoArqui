using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Diagnostics;
using System.Threading;

namespace SimuladorMIPS
{
    class NucleoMultihilillo
    {
        // Patrón singleton.
        private static NucleoMultihilillo instance = null;

        // TIP: Esto es un "Property" de C#.
        public static NucleoMultihilillo Instance
        {
            get
            {
                if (instance == null)
                    instance = new NucleoMultihilillo();
                return instance;
            }
        }


        private NucleoMultihilillo()
        {
            Terminado = false;
            CacheD = new CacheDatos(tamanoCache);
            CacheI = new CacheInstrucciones(tamanoCache);
            Debug.Print("Núcleo 0 creado.");
        }

        // Carga un hilillo en H0 y ejecuta Run() en un ciclo infinito.
        public void Start()
        {
            lock (ColaHilillos)
            {
                if (ColaHilillos.Count > 0)
                {
                    h[0] = ColaHilillos.Dequeue();
                    // TIP: Es útil usar asserts de Debug cuando pensamos un caso que "nunca pasa".
                    Debug.Assert(h[0].Fase == Hilillo.FaseDeHilillo.L); // Creo que debería estar listo, pues es el inicio de la simulación.
                }
                else
                {
                    h[0] = Hilillo.HililloVacio;
                }
            }
            h[1] = Hilillo.HililloVacio;

            while (true)
            {
                Run();
            }
        }

        // Aquí va la lógica general: fetch, execute, missI, missD.
        private void Run()
        {
            if (h[0].Fase == Hilillo.FaseDeHilillo.L)
            {
                Fetch(0);
            }
            else if (h[0].Fase == Hilillo.FaseDeHilillo.IR)
            {
                Execute(0);
            }
            else if (h[1].Fase == Hilillo.FaseDeHilillo.L)
            {
                Fetch(1);
            }
            else if(h[1].Fase == Hilillo.FaseDeHilillo.IR)
            {
                Execute(1);
            }

            Debug.Assert(!(h[0].Fase == Hilillo.FaseDeHilillo.FI && h[1].Fase == Hilillo.FaseDeHilillo.FI));
            if (h[0].Fase == Hilillo.FaseDeHilillo.FI)
            {
                MissI(0);
            }
            else if (h[1].Fase == Hilillo.FaseDeHilillo.FI)
            {
                MissI(1);
            }

            Debug.Assert(!(h[0].Fase == Hilillo.FaseDeHilillo.FD && h[1].Fase == Hilillo.FaseDeHilillo.FD));
            if (h[0].Fase == Hilillo.FaseDeHilillo.FD)
            {
                MissD(0);
            }
            else if (h[1].Fase == Hilillo.FaseDeHilillo.FD)
            {
                MissD(1);
            }

            Tick();
        }

        // i: número del hilillo que va a hacer el fetch.
        private void Fetch(int i)
        {
            throw new NotImplementedException();
        }

        // i: número del hilillo que se va a ejecutar.
        private void Execute(int i)
        {
            throw new NotImplementedException();
        }

        // i: número del hilillo del cual se va a manejar el fallo.
        private void MissI(int i)
        {
            throw new NotImplementedException();
        }

        // i: número del hilillo del cual se va a manejar el fallo.
        private void MissD(int i)
        {
            throw new NotImplementedException();
        }

        private void Tick()
        {
            throw new NotImplementedException();
        }

        // Retorna información general de los hilillos que están corriendo para desplegarla en pantalla durante la ejecución.
        public string PrettyPrintHilillos()
        {
            string output = "\t\tHilillo 0: " + h[0].Nombre + "\n" // YOLO.
                    + "\t\tHilillo 1: " + h[1].Nombre;

            return output;
        }

        // Retorna los contenidos de los registros y las cachés, de forma legible en consola.
        public string PrettyPrintRegistrosYCaches()
        {
            string output = "";

            output += "\t\tRegistros: \n"
                + "\t\t\tHilillo 0:\n"
                + h[0].PrettyPrintRegistrosYCiclos()
                + "\t\t\tHilillo 1:\n"
                + h[1].PrettyPrintRegistrosYCiclos()
                + "\t\tCachés:\n"
                + "\t\t\tCaché de instrucciones:\n\n";

            for (int i = 0; i < tamanoCache; i++)
            {
                output += "Posición " + i + "\t";
            }
            output += "\n";

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < tamanoCache; j++)
                {
                    output += CacheI.Cache[i, j].CodigoDeOperacion + " ";
                    for (int k = 0; k < 3; k++)
                    {
                        output += CacheI.Cache[i, j].Operando[k] + " ";
                    }
                    output += "\t";
                }
                output += "\n";
            }

            for (int i = 0; i < tamanoCache; i++)
            {
                output += CacheI.NumBloque[i] + "\t";
            }

            output += "\n\n";

            output += "\t\t\tCaché de datos:\n\n";

            for (int i = 0; i < tamanoCache; i++)
            {
                output += "Posición " + i + "\t";
            }
            output += "\n";

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < tamanoCache; j++)
                {
                    output += CacheD.Cache[i, j] + "\t";
                }
                output += "\n";
            }

            for (int i = 0; i < tamanoCache; i++)
            {
                output += CacheD.NumBloque[i] + "\t";
            }

            for (int i = 0; i < tamanoCache; i++)
            {
                output += CacheD.Estado[i] + "\t";
            }

            return output;
        }

        public Queue<Hilillo> ColaHilillos { get; set; }
        public bool Terminado { get; set; }
        public int Quantum { get; set; }
        public Barrier Barrera { get; set; }
        public List<Hilillo> HilillosFinalizados { get; set; }

        private Hilillo[] h;

        public CacheDatos CacheD { get; set; } 
        private CacheInstrucciones CacheI; // Miembro privado, porque nadie va a acceder a ella desde fuera.
        private const int tamanoCache = 8;
    }
}
