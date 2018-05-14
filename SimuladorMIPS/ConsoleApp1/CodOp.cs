namespace SimuladorMIPS
{
    enum CodOp
    {
        JR = 2,
        JAL,
        BEQZ,
        BNEZ,
        DADDI = 8,
        DMUL = 12,
        DDIV = 14,
        DADD = 32,
        DSUB = 34,
        LW,
        SW = 43,
        FIN = 63
    }
}