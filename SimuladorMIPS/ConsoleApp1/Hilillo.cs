﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SimuladorMIPS
{
    class Hilillo
    {
        // WARNING: Para evitar problemas de inconsistencia, usar este hilillo siempre que se necesite un hilillo vacío.
        private static Hilillo hililloVacio;
        public static Hilillo HililloVacio
        {
            get
            {
                if (hililloVacio == null)
                {
                    hililloVacio = new Hilillo(0, "vacío");
                    hililloVacio.Fase = FaseDeHilillo.V;
                }
                return hililloVacio;
            }
        }

        public Hilillo(int direccionDeInicio, string nombre)
        {
            PC = direccionDeInicio;
            this.Nombre = nombre;
            Registro = new int[32];
            Ciclos = 0;
            Fase = FaseDeHilillo.L;
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

        public enum FaseDeHilillo { V, L, FI, IR, FD, Exec, Fin }

        public FaseDeHilillo Fase;
    }
}
