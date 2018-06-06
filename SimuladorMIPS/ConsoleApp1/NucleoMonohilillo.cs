using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Diagnostics;

namespace SimuladorMIPS
{
    class NucleoMonohilillo
    {
        // Patrón singleton.
        private static NucleoMonohilillo instance = null;

        // TIP: Esto es un "Property" de C#.
        public static NucleoMonohilillo Instance
        {
            get
            {
                if (instance == null)
                    instance = new NucleoMonohilillo();
                return instance;
            }
        }

        public Queue<Hilillo> ColaHilillos { get; set; }

        public bool Terminado { get; set; }

        private NucleoMonohilillo()
        {
            // TODO: Inicializar cachés con ceros.
            Debug.Print("Núcleo 1 creado.");
        }

        // TODO: Carga un hilillo y ejecuta run() en un ciclo infinito.
        public void start()
        {

        }
    }
}
