using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2
{
    class TT
    {
        private int[,] TruthTable;                          //2D array for truth table
        private int[] askedTable;                           //arry which stores truthtable values for the query. 
        private List<string> KnowledgeBase;                 //stores the knowledgebase which is passed from FileIO
        private List<string> possibleConditions;            //stores a list of possibleConditions, e.g. Implication, BiConditional, And, Negation, etc...
        private List<string> Variables;                     //stores a list of the variables. e.g. p1, p2, p3, a, b, c, etc...
        private List<string> Ask;                           //stores a list of the ask section of the knowledge base. 
        private List<string> Tell;                          //stores a list of the tell section of the knowledge base
        private Dictionary<string, int> VariableIndex;      //a dictionary which stores the index value of a variable corresponding to the column in the TruthTable array, whose key is the variable.
        private Dictionary<int, string> TellIndex;          //a dictionary which stores the statements/clauses from the tellsection/Knowledgebase which corresponds to the column in the TruthTable array, whose key is the index itself. 
        private int MaxRows;                                //Max Rows for the truth table
        private int MaxCols;                                //Max Columns for the truth table
        private int TruthCount;                             //Stores the count for how many statements in the Knowledge base are true when the query is true.
        private string FirstVar;                            //Stores the first variable detected in a statement/clause that is currently being processed
        private string SecondVar;                           //Stores the second variable detected in a statement/clause that is currently being processed
        private string conditionUsed;                       //Stores the condition used in the statement being currently processed
        private string Implication;                         //used to store a string which represents the implication symbol.
        private string Biconditional;                       //used to store a string which represents the Bi-Conditional symbol.
        private string And;                                 //used to store a string which represents the And symbol.
        private string Negation;                            //used to store a string which represents the Negation symbol.
        private string Conjunction;                         //used to store a string which represents the Conjunction symbol.
        private string Disjunction;                         //used to store a string which represents the Disjunction symbol.
        private string EOS;                                 //end of statement symbol (;)

        public TT(List<string> KB)
        {
            //Initialising strings and ints. 
            FirstVar = "";                                  
            SecondVar = "";
            conditionUsed = "";
            Implication = "=>";
            Biconditional = "<=>";
            And = "&";
            Negation = "!";
            Conjunction = "^";
            Disjunction = "!^";
            EOS = ";";
            TruthCount = 0;

            //initialising data structures. 
            KnowledgeBase = new List<string>(KB);
            possibleConditions = new List<string>();
            Variables = new List<string>();
            Ask = new List<string>();
            Tell = new List<string>();
            VariableIndex = new Dictionary<string, int>();
            TellIndex = new Dictionary<int, string>();

            //adding conditions/symbols to the possibleconditions list. 
            possibleConditions.Add(Implication);
            //possibleConditions.Add(Biconditional);
            possibleConditions.Add(And);
            possibleConditions.Add(Negation);
            possibleConditions.Add(Conjunction);
            possibleConditions.Add(Disjunction);
            possibleConditions.Add(EOS);

            //Method call which adds the variables to their own list. 
            GetVariables();
            //seperates the knowledge base into a lists comprising the Ask and Tell sections. 
            SepAskTell();

            //Calculating the maximum number of rows required in the truth table. 2^(number of variables)
            MaxRows = Convert.ToInt32(Math.Pow(2, Variables.Count));
            //Calculating the maximum number of columns required in the truth table. 
            MaxCols = Tell.Count + Variables.Count;
        }

        private void GetVariables()
        {
            foreach (string line in KnowledgeBase)                  //cycling through each line in the knowledge base
            {
                string possibleVar = "";

                foreach (char a in line)                            //cycling through each character in the current line. 
                {
                    bool add = true;
                    foreach (string cond in possibleConditions)     //detecting whether we have come across a condition. 
                    {
                        if (cond.Contains<Char>(a))
                        {
                            add = false;
                        }
                    }

                    if (add) //if we havent come across a condition, keep adding to the possible variable. 
                        possibleVar += a;
                    else if (!add)
                    {
                        possibleVar = possibleVar.Trim();   //once weve detected a condition/symbol then we know we have a variable stored so far. so trim any spaces and store. 
                        if (!Variables.Contains(possibleVar) && possibleVar != "" && possibleVar.ToUpper() != "TELL" && possibleVar.ToUpper() != "ASK")
                            Variables.Add(possibleVar);     //check whether we already have the variable stored and that we arent reading an empty space or strings TELL or ASK
                        possibleVar = "";                   //reset possiblevar since weve stored something already. re-peat cycle. 
                    }
                }
            }
        }


        private void SepAskTell()           //seperates Ask and Tell form the Knowledge base. 
        {
            bool Telling = false;
            bool Asking = false;

            foreach (string line in KnowledgeBase)
            {   
                //since we know the first line is TELL we dont worry about adding it, instead we set the bools after attempting to add the lines to list. 
                if (Telling)
                    Tell.Add(line);

                if (Asking)
                    Ask.Add(line);

                if (line.ToUpper() == ("TELL;"))
                {
                    Telling = true;
                    Asking = false;
                }
                else if (line.ToUpper() == "ASK;")
                {
                    Telling = false;
                    Asking = true;
                }
            }
            //cleaning up the list of any unwanted characters.
            if (Tell.Contains(""))
                Tell.Remove("");
            if (Tell.Contains(";"))
                Tell.Remove(";");
            if (Tell.Contains("ASK;"))
                Tell.Remove("ASK;");
        }

        private int AssessStatement(string statement, int row)  //main logic of TT method. assess the statement to assign the respective value in the truthtable. 
        {
            FirstVar = "";
            SecondVar = "";
            bool result = false;
            conditionUsed = "";
            string extraStatement = "";
            string extraCondition = "";
            bool extraStatementPresent = false;
            bool gettingCondition = false;
            bool EOSreached = false;
            bool FirstStatementValue = false;

            foreach (char a in statement)
            {
                gettingCondition = false;
                foreach (string cond in possibleConditions)
                {
                    if (cond.Contains<Char>(a))
                    {
                        gettingCondition = true;
                        if (conditionUsed == "")
                        {
                            conditionUsed = cond;
                            break;
                        }
                        else if (conditionUsed != cond)
                        {
                            if (a == ';')
                                EOSreached = true;
                            else
                            {
                                extraStatementPresent = true;
                                extraCondition += a;
                            }
                        }
                    }
                }

                if (!EOSreached && !gettingCondition && !extraStatementPresent)
                {
                    if (conditionUsed == "")
                        FirstVar += a;
                    else if (conditionUsed != "")
                        SecondVar += a;
                }
                else if (extraStatementPresent && !gettingCondition)
                    extraStatement += a;
            }

            FirstVar = FirstVar.Trim();
            SecondVar = SecondVar.Trim();

            if (extraCondition != "")
            {
                FirstStatementValue = StatementValue(TruthTable[row, VariableIndex[FirstVar]], TruthTable[row, VariableIndex[SecondVar]], conditionUsed);

                if (FirstStatementValue)
                    result = StatementValue(1, TruthTable[row, VariableIndex[extraStatement.Trim()]], extraCondition.Trim());
                else
                    result = StatementValue(0, TruthTable[row, VariableIndex[extraStatement.Trim()]], extraCondition.Trim());
            }
            else if (SecondVar != "")
                result = StatementValue(TruthTable[row, VariableIndex[FirstVar]], TruthTable[row, VariableIndex[SecondVar]], conditionUsed);
            else
                result = StatementValue(TruthTable[row, VariableIndex[FirstVar]], 1, "NONE");

            if (result)
                return 1;
            else
                return 0;
        }

        private bool StatementValue(int a, int b, string condition)
        {
            bool result = false;

            if (condition == Implication)
            {
                if (a == 0 || b == 1)
                    result = true;
            }
            else if (condition == And)
            {
                if (a == 1 && b == 1)
                    result = true;
            }
            else if (condition == Biconditional)
            {
                if (a == b)
                    result = true;
            }
            else if (condition == "NONE")
            {
                if (a == 1)
                    result = true;
            }


            return result;
        }

        public void CheckTable()
        {
            TruthCount = 0;
            bool Matched = true;

            for(int i = 0; i < MaxRows; i++)
            {
                Matched = true;

                for(int j = Variables.Count; j < MaxCols; j++)
                {
                    if (TruthTable[i, j] != askedTable[i])
                        Matched = false;
                }

                if (Matched)
                    TruthCount++;
            }

            if (TruthCount > 0)
                Console.WriteLine("YES: " + TruthCount);
            else
                Console.WriteLine("NO");
        }

        public void PopulateAskedTable()
        {
            askedTable = new int[MaxRows];

            for (int i = 0; i < MaxRows; i++)
            {
                askedTable[i] = AssessStatement(Ask[0], i);
            }
        }

        public void PopulateTruthTable()
        {
            TruthTable = new int[MaxRows, MaxCols];

            for (int i = 0; i < MaxRows; i++)
            {
                for (int j = 0; j < MaxCols; j++)
                {
                    TruthTable[i, j] = 0;
                }
            }

            string binary = "";
            int padding = 0;

            for(int i = 0; i < MaxRows; i++)
            {
                binary = Convert.ToString(i, 2);
                padding = Variables.Count - binary.Length;
                
                for (int x = 0; x < padding; x++)
                    binary = "0" + binary;
           
                for (int j = 0; j < binary.Length; j++)
                {
                    TruthTable[i, j] = Convert.ToInt32(binary[j].ToString());
                }

                for(int j = binary.Length; j < MaxCols; j++)
                {
                    TruthTable[i, j] = AssessStatement(TellIndex[j], i);
                }
            }
        }

        public void PrintTable()
        {
            for (int i = 0; i < MaxRows; i++)
            {
                for (int j = 0; j < Variables.Count; j++)
                {
                    Console.Write(TruthTable[i, j]);
                }

                for (int j = Variables.Count; j < MaxCols; j++)
                {
                    Console.Write(" " + TruthTable[i, j] + " ");
                }

                Console.WriteLine();
            }
        }

        public void PairVariablesToTable()
        {
            for(int i = 0; i < Variables.Count; i++)
            {
                VariableIndex.Add(Variables[i], i);
            }
        }

        public void PairTelltoTable()
        {
            for(int i = Variables.Count; i < MaxCols; i++)
            {
                TellIndex.Add(i, Tell[i - Variables.Count]);
            }
        }
    }
}
