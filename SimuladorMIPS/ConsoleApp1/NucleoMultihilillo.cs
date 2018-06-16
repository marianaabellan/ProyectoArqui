using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Diagnostics;
using System.Threading;

namespace SimuladorMIPS
{
    class NucleoMultihilillo
    {
        // Patrón singleton.
        private static NucleoMultihilillo instance = null;

        // TIP: Esto es un "Property" de C#.
        public static NucleoMultihilillo Instance
        {
            get
            {
                if (instance == null)
                    instance = new NucleoMultihilillo();
                return instance;
            }
        }


        private NucleoMultihilillo()
        {
            Terminado = false;
            CacheD = new CacheDatos(tamanoCache);
            CacheI = new CacheInstrucciones(tamanoCache);
            Debug.Print("Núcleo 0 creado.");
        }

        // Carga un hilillo en H0 y ejecuta Run() en un ciclo infinito.
        public void Start()
        {
            lock (ColaHilillos)
            {
                if (ColaHilillos.Count > 0)
                {
                    h[0] = ColaHilillos.Dequeue();
                    // TIP: Es útil usar asserts de Debug cuando pensamos un caso que "nunca pasa".
                    Debug.Assert(h[0].Fase == Hilillo.FaseDeHilillo.L); // Creo que debería estar listo, pues es el inicio de la simulación.
                }
                else
                {
                    h[0] = Hilillo.HililloVacio;
                }
            }
            h[1] = Hilillo.HililloVacio;

            while (true)
            {
                Run();
            }
        }

        // Aquí va la lógica general: fetch, execute, missI, missD.
        private void Run()
        {
            if (h[0].Fase == Hilillo.FaseDeHilillo.L)
            {
                Fetch(0);
            }
            else if (h[0].Fase == Hilillo.FaseDeHilillo.IR)
            {
                Execute(0);
            }
            else if (h[1].Fase == Hilillo.FaseDeHilillo.L)
            {
                Fetch(1);
            }
            else if(h[1].Fase == Hilillo.FaseDeHilillo.IR)
            {
                Execute(1);
            }

            Debug.Assert(!(h[0].Fase == Hilillo.FaseDeHilillo.FI && h[1].Fase == Hilillo.FaseDeHilillo.FI));
            if (h[0].Fase == Hilillo.FaseDeHilillo.FI)
            {
                MissI(0);
            }
            else if (h[1].Fase == Hilillo.FaseDeHilillo.FI)
            {
                MissI(1);
            }

            Debug.Assert(!(h[0].Fase == Hilillo.FaseDeHilillo.FD && h[1].Fase == Hilillo.FaseDeHilillo.FD));
            if (h[0].Fase == Hilillo.FaseDeHilillo.FD)
            {
                MissD(0);
            }
            else if (h[1].Fase == Hilillo.FaseDeHilillo.FD)
            {
                MissD(1);
            }

            Tick();
        }

        // i: número del hilillo que va a hacer el fetch.
        private void Fetch(int i)
        {
            throw new NotImplementedException();
        }

        // i: número del hilillo que se va a ejecutar.
        private void Execute(int i)
        {
            throw new NotImplementedException();
        }

        // i: número del hilillo del cual se va a manejar el fallo.
        private void MissI(int i)
        {
            throw new NotImplementedException();
        }

        // i: número del hilillo del cual se va a manejar el fallo.
        private void MissD(int i)
        {
            Debug.Assert(h[i].Fase == Hilillo.FaseDeHilillo.FD);

            int Y = h[i].IR.Operando[0];
            int X = h[i].IR.Operando[1];
            int n = h[i].IR.Operando[2];
            int direccionDeMemoria = h[i].Registro[Y] + n;
            int bloqueDeMemoria = direccionDeMemoria / 16;
            int posicionEnCache = bloqueDeMemoria % tamanoCache;
            int palabra = (direccionDeMemoria - bloqueDeMemoria * 16) / 4;

            Debug.Print("Núcleo 0: Fallo de datos. Revisando bloque " + bloqueDeMemoria
                + " en posición de caché " + posicionEnCache + " para hilillo " + i + ".");

            if (!h[i].Recursos)
            {
                Debug.Print("Núcleo 0: Recursos no disponibles.");

                if (!Monitor.TryEnter(CacheD.NumBloque[posicionEnCache]))
                {
                    Debug.Print("Núcleo 0: No se pudo bloquear posición en caché. Fin de MissD().");
                    return;
                }
                if (!Monitor.TryEnter(Memoria.Instance.BusDeDatos))
                {
                    Monitor.Exit(CacheD.NumBloque[posicionEnCache]);
                    Debug.Print("Núcleo 0: No se pudo bloquear bus de datos. Fin de MissD().");
                    return;
                }

                Debug.Print("Núcleo 0: Se bloqueó la posición de caché y bus de datos.");

                h[i].Recursos = true;

                if (CacheD.Estado[posicionEnCache] == EstadoDeBloque.M)
                {
                    Debug.Print("Núcleo 0: Bloque modificado. Es necesario copiar en memoria.");
                    h[i].Ticks = 40;
                }
                else
                {
                    goto RevisarEtapaSnooping;
                }
            }
            else
            {
                Debug.Print("Núcleo 0: Recursos disponibles.");

                if (CacheD.Estado[posicionEnCache] != EstadoDeBloque.M)
                {
                    goto RevisarEtapaSnooping;
                }
            }

            h[i].Ticks--;
            if (h[i].Ticks > 0)
            {
                Debug.Print("Núcleo 0: \"Copiando\" bloque modificado en memoria. Ticks restantes: " + h[i].Ticks);
                return;
            }
            else
            {
                Debug.Assert(h[i].Ticks == 0);

                // Se copia a memoria el bloque modificado.
                Debug.Print("Núcleo 0: Copiando bloque de la posición de caché " + posicionEnCache
                    + " a dirección de memoria " + direccionDeMemoria + "(posición de memoria simulada: "
                    + (direccionDeMemoria / 4) + ").");
                for (int j = 0; j < 4; j++)
                {
                    Memoria.Instance.Mem[direccionDeMemoria / 4 + i] = CacheD.Cache[i, posicionEnCache];
                }

                CacheD.Estado[posicionEnCache] = EstadoDeBloque.I;
            }

            RevisarEtapaSnooping:
            Debug.Print("Núcleo 0: Revisando etapa de snooping...");
            switch(h[i].EtapaDeSnooping)
            {
                case Hilillo.EtapaSnooping.ANTES:
                    Debug.Print("Núcleo 0: Etapa de snooping: ANTES.");
                    if (!Monitor.TryEnter(N1.CacheD.NumBloque[posicionEnCache]))
                    {
                        Debug.Print("Núcleo 0: No se pudo reservar la posición de caché en N1. Fin de missD().");
                        return;
                    }
                    if (N1.CacheD.NumBloque[posicionEnCache] != bloqueDeMemoria) // ¿Es la que queremos?
                    {
                        // No.
                        if (h[i].IR.CodigoDeOperacion == CodOp.SW && CacheD.Estado[posicionEnCache] == EstadoDeBloque.C)
                        {
                            throw new NotImplementedException();
                        }
                    }
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
            }
        }

        private void Tick()
        {
            throw new NotImplementedException();
        }

        // Retorna información general de los hilillos que están corriendo para desplegarla en pantalla durante la ejecución.
        public string PrettyPrintHilillos()
        {
            string output = "\t\tHilillo 0: " + h[0].Nombre + "\n" // YOLO.
                    + "\t\tHilillo 1: " + h[1].Nombre;

            return output;
        }

        // Retorna los contenidos de los registros y las cachés, de forma legible en consola.
        public string PrettyPrintRegistrosYCaches()
        {
            string output = "";

            output += "\t\tRegistros: \n"
                + "\t\t\tHilillo 0:\n"
                + h[0].PrettyPrintRegistrosYCiclos()
                + "\t\t\tHilillo 1:\n"
                + h[1].PrettyPrintRegistrosYCiclos()
                + "\t\tCachés:\n"
                + "\t\t\tCaché de instrucciones:\n\n";

            for (int i = 0; i < tamanoCache; i++)
            {
                output += "Posición " + i + "\t";
            }
            output += "\n";

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < tamanoCache; j++)
                {
                    output += CacheI.Cache[i, j].CodigoDeOperacion + " ";
                    for (int k = 0; k < 3; k++)
                    {
                        output += CacheI.Cache[i, j].Operando[k] + " ";
                    }
                    output += "\t";
                }
                output += "\n";
            }

            for (int i = 0; i < tamanoCache; i++)
            {
                output += CacheI.NumBloque[i] + "\t";
            }

            output += "\n\n";

            output += "\t\t\tCaché de datos:\n\n";

            for (int i = 0; i < tamanoCache; i++)
            {
                output += "Posición " + i + "\t";
            }
            output += "\n";

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < tamanoCache; j++)
                {
                    output += CacheD.Cache[i, j] + "\t";
                }
                output += "\n";
            }

            for (int i = 0; i < tamanoCache; i++)
            {
                output += CacheD.NumBloque[i] + "\t";
            }

            for (int i = 0; i < tamanoCache; i++)
            {
                output += CacheD.Estado[i] + "\t";
            }

            return output;
        }

        public Queue<Hilillo> ColaHilillos { get; set; }
        public bool Terminado { get; set; }
        public int Quantum { get; set; }
        public Barrier Barrera { get; set; }
        public List<Hilillo> HilillosFinalizados { get; set; }

        private Hilillo[] h;

        public CacheDatos CacheD { get; set; } 
        private CacheInstrucciones CacheI; // Miembro privado, porque nadie va a acceder a ella desde fuera.
        private const int tamanoCache = 8;

        public NucleoMonohilillo N1 { get; set; }
    }
}
