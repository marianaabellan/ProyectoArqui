namespace SimuladorMIPS
{
    struct Instruccion
    {
        CodOp CodigoDeOperacion;
        int[] Operando;
    }

    struct CacheInstrucciones
    {
        public Instruccion[,] Cache;
        public int[] NumBloque;

        public CacheInstrucciones(int tamano)
        {
            Cache = new Instruccion[4, tamano];
            NumBloque = new int[tamano];
        }
    }
}