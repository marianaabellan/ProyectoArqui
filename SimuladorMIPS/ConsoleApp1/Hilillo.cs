using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SimuladorMIPS
{
    class Hilillo
    {
        public Hilillo(int direccionDeInicio)
        {
            PC = direccionDeInicio;
        }

        public int PC { get; set; }

        // TODO: Nombre e identificación.
    }
}
