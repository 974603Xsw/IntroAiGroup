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
            
            if (args[0] == "TT")
            {
                TT TruthTable = new TT(Fio.getKB());
                TruthTable.PairVariablesToTable();
                TruthTable.PairTelltoTable();
                TruthTable.PopulateTruthTable();
                TruthTable.PopulateAskedTable();
                TruthTable.CheckTable();
            }
            else if (args[0] == "FC")
            {
                BC2 BC = new BC2(Fio.getAsk(), Fio.getTell());
                BC2.initialise(Fio.getTell());
                Console.WriteLine(BC.execute());
            }
            else if (args[0] == "BC")
            {

            }
        }
    }
}
