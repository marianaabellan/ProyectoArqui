using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

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

        public Queue ColaHilillos { get; set; }

        public bool Terminado { get; set; }

        private NucleoMultihilillo()
        {
            // TODO: Inicializar cachés con ceros.
        }

        // TODO: Carga un hilillo en H0 y ejecuta run() en un ciclo infinito.
        public void start()
        {

        }
    }
}
