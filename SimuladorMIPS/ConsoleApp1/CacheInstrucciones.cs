namespace SimuladorMIPS
{
    struct Instruccion
    {
        public CodOp CodigoDeOperacion;
        public int[] Operando;

        public Instruccion (int dummy = 0)
        {
            CodigoDeOperacion = 0;
            Operando = new int[] { 0, 0, 0 };
        }
    }

    struct CacheInstrucciones
    {
        // TIP: Ver diferencia entre "jagged array" y "multidimensional array".
        public Instruccion[,] Cache;
        // WARNING: A la hora de bloquear, usar el lock de la variable NumBloque correspondiente.
        // Por ejemplo, para bloquear la posición 3: TryEnter(CacheI.NumBloque[3]);
        public int[] NumBloque;

        public CacheInstrucciones(int tamano)
        {
            Cache = new Instruccion[4, tamano];
            NumBloque = new int[tamano];

            for (int i = 0; i < tamano; i++)
                NumBloque[i] = 0;
        }
    }
}