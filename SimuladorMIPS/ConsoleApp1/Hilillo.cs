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
            Registro = new int[32];
            Ciclos = 0;
        }

        // Esta función se llama al final para imprimir datos de un hilillo finalizado.
        public string PrettyPrintRegistrosYCiclos()
        {
            string output = "";

            output += Nombre + ":\n"
                + "Ciclos: " + Ciclos + "\n"
                + "Registros: ";

            for (int i = 0; i < 32; i++)
            {
                output += Registro[i] + " ";
            }

            return output;
        }

        public int PC { get; set; }
        public string Nombre { get; }
        public int[] Registro { get; set; }
        public int Ciclos;
    }
}
