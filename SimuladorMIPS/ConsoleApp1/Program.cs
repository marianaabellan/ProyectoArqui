using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace SimuladorMIPS
{
    class Program
    {
        static void Main(string[] args)
        {
            Memoria mem = Memoria.Instance;
            NucleoMultihilillo N0 = NucleoMultihilillo.Instance;
            NucleoMonohilillo N1 = NucleoMonohilillo.Instance;
            Queue colaHilillos = new Queue(); // Hay que settear los núcleos con esta cola.
        }
    }
}
