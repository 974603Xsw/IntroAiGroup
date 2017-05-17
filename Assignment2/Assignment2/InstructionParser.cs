using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2
{
    class InstructionParser
    {
        private List<string> KB;                        //KB => Knowledge base
        private Dictionary<string, string> HCList;      //Dictionary of Horn clauses
        private Dictionary<string, string> HCfromKB;    //Dictionary containing the propositions (p1, p2, a, etc...) as keys and the symbols/conditions as the value (=>, &, !)
        private string temp;                            //used to temporarily store line for manipulation before being stored in HCfromKB. 

        public InstructionParser(List<string> StoredLines)
        {
            KB = new List<string>();
            HCList = new Dictionary<string, string>();
            HCfromKB = new Dictionary<string, string>();
            KB = StoredLines;
            HCList.Add("Implication", "=>");            //a=>b true if (a is false OR b is true). false if (a is true AND b is false)
            HCList.Add("Biconditional", "<=>");         //a<=>b true when a = b. false when a != b
            HCList.Add("And", "&");                     //a&b true when a = b = 1, false when a OR b = 0.
            HCList.Add("Negation", "!");                //if a = 0 then !a = 1
            HCList.Add("Conjunction", "^");             //if a AND B are both true then a^b is true. false for everthing else
            HCList.Add("Disjunction", "!^");            //if a OR b is true then a!^b is true. i.e. only one (a or b) is required to be true for statement to be true. 

            ProcessKB();
        }      

        public void Expel()
        {
            foreach(KeyValuePair<string, string> HC in HCfromKB)
            {
                Console.WriteLine("Key: " + HC.Key + " Value: " + HC.Value + "\n");
            }
        }

        private void ProcessKB()
        {
            foreach(string l in KB)
            {
               foreach(KeyValuePair<string, string> HC in HCList)
                {
                    //Need to come up with solution for when there is more than one condition present. 
                    //This current method is only ideal for one condition. 
                    if (l.IndexOfAny(HC.Value.ToCharArray()) != -1)
                    {
                        temp = l;
                        temp = temp.Replace(HC.Value, ",");
                        if(!HCfromKB.ContainsKey(temp))
                            HCfromKB.Add(temp, HC.Value);
                    }
                }
            }
        }
    }
}
