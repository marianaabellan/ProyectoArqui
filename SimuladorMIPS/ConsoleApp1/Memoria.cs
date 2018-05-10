using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimuladorMIPS
{
    class Memoria
    {
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

        }
    }
}
