﻿using System;
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
                TT TruthTable = new TT(Fio.getKB());    //construtor for Truth Table Method.
                TruthTable.PairVariablesToTable();      //Pairing the variables found in KnowledgeBase to truth table. (storing index)
                TruthTable.PairTelltoTable();           //Pairing the statements found in KnowledgeBase to truth table. (storing index)
                TruthTable.PopulateTruthTable();        //populating the truthtable per knowledgebase statements, facts.
                TruthTable.PopulateAskedTable();        //populating the query truth table using the truthtable already made.
                TruthTable.CheckTable();                //cross referencing matches within the query truth table and knowledgebase truthtable. 
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
