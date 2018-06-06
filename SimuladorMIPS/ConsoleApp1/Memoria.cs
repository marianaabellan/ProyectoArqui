using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimuladorMIPS
{
    class Memoria
    {
        //TODO: No estoy seguro del tamaño de la memoria.
        private static readonly int size = 1020;

        // Patrón singleton.
        private static Memoria instance = null;

        // Esto es un "Property" de C#.
        public static Memoria Instance
        {
            get
            {
                if (instance == null)
                    instance = new Memoria();
                return instance;
            }
        }

        private Memoria()
        {
            Mem = new int[size];
            for (int i = 0; i < size; i++)
            {
                Mem[i] = 1;
            }
        }

        public bool BusDeDatos { get; set; }
        public bool BusDeInstrucciones { get; set; }
        public int[] Mem { get; set; }

        
    }
}
