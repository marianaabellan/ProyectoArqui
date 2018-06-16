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


        private NucleoMonohilillo()
        {
            Terminado = false;
            CacheD = new CacheDatos(4);
            CacheI = new CacheInstrucciones(4);
            Debug.Print("Núcleo 1 creado.");
        }

        // TODO: Carga un hilillo y ejecuta run() en un ciclo infinito.
        public void Start()
        {
            throw new NotImplementedException();
        }

        // Retorna información general de los hilillos que están corriendo para desplegarla en pantalla durante la ejecución.
        public string PrettyPrintHilillos()
        {
            string output = "\t\tHilillo 0: " + h.Nombre; // YOLO.

            return output;
        }

        // TODO: Retorna los contenidos de los registros y las cachés, de forma legible en consola.
        public string PrettyPrintRegistrosYCaches()
        {
            throw new NotImplementedException();
        }

        public Queue<Hilillo> ColaHilillos { get; set; }
        public bool Terminado { get; set; }
        public int Quantum { get; set; }
        public Barrier Barrera { get; set; }
        public List<Hilillo> HilillosFinalizados { get; set; }
        public bool Cancelado { get; set; }

        private Hilillo h;

        public CacheDatos CacheD { get; set; }
        private CacheInstrucciones CacheI; // Miembro privado, porque nadie va a acceder a ella desde fuera.
    }
}
