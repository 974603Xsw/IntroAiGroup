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
            InstructionParser IP = new InstructionParser(Fio.getStoredLines());

            if (args[0] == "FIO")
            {
                Fio.ExpelStoredLines();
                Console.WriteLine("\n\n");
                IP.Expel();
            }
        }
    }
}
