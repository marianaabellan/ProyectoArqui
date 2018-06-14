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
        public Hilillo(int direccionDeInicio, string nombre)
        {
            PC = direccionDeInicio;
            this.nombre = nombre;
        }

        public int PC { get; set; }
        public readonly string Nombre { get; set; }
    }
}
