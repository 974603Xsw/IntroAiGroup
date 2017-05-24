using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2
{
    class Program
    {
        static void Main(string[] args)
        {
            FileIO Fio = new FileIO(args[1]);
            Inference_Engine JSfio = new Inference_Engine(args[1]);
            TT TruthTable = new TT(Fio.getKB());
            BC2 BC = new BC2(JSfio.getAsk(), TruthTable.getTell());
            BC2.initialise(TruthTable.getTell());
            BC.execute();

            if (args[0] == "FIO")
            {
                //Fio.OrganiseAT();
                Fio.ExpelStoredLines();
                Console.WriteLine("\n\n");
            }
        }
    }
}
