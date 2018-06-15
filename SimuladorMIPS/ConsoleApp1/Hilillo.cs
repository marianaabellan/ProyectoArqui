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
            this.Nombre = nombre;
        }

        // TODO: Implementar función.
        public string PrettyPrintRegistrosYCiclos()
        {
            throw new NotImplementedException();
        }

        public int PC { get; set; }
        public string Nombre { get; }
    }
}
