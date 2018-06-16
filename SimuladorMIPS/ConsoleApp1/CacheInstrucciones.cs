﻿namespace SimuladorMIPS
{
    struct Instruccion
    {
        public CodOp CodigoDeOperacion;
        public int[] Operando;
    }

    struct CacheInstrucciones
    {
        // TIP: Ver diferencia entre "jagged array" y "multidimensional array".
        public Instruccion[,] Cache;
        public int[] NumBloque;

        public CacheInstrucciones(int tamano)
        {
            Cache = new Instruccion[4, tamano];
            NumBloque = new int[tamano];

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < tamano; j++)
                {
                    Cache[i, j].CodigoDeOperacion = 0;
                    Cache[i, j].Operando = new int[] { 0, 0, 0 };
                }
            }

            for (int i = 0; i < tamano; i++)
                NumBloque[i] = 0;
        }
    }
}