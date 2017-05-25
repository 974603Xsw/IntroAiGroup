using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Sean Morris
//974603X

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
            FirstVar = "";                      //Stores the first variables of the statement e.g. p1, p2, p3, a, b, etc...
            SecondVar = "";                     //stores the second variable of the statement e.g. p1, p2, p3, a, b, etc...
            bool result = false;                //stores whether the statement has been detected to be true or false.
            conditionUsed = "";                 //stores the condition which was used in the statement. e.g. =>, &, etc...
            string extraStatement = "";         //detects if another statement is in current statement. e.g. a&b => c, one statement is a&b, next is => c
            string extraCondition = "";         //stores the extra condition if any
            bool extraStatementPresent = false; //bool used to detect if another stateme1nt is present. 
            bool gettingCondition = false;      //bool used to determine if condition is being read in from statement, to prevent storage of first/second variable.
            bool EOSreached = false;            //detects if end of statement reached. 
            bool FirstStatementValue = false;   //stores the value of the first statement if another statement is present. 

            foreach (char a in statement)       //cycling through each char of the statement. 
            {
                gettingCondition = false;       //setting getting condition to false at the start of each cycle. 
                foreach (string cond in possibleConditions)  //running through each condition in the conditions list and seeing if the current character is present in list. 
                {
                    if (cond.Contains<Char>(a))
                    {
                        gettingCondition = true;            //if there is a character present from the conditions list we set gettingcondition to be true. 
                        if (conditionUsed == "")            //checking if we have already detected a condition to be used. 
                        {
                            conditionUsed = cond;           //if not we store the current detected condition. 
                            break;
                        }
                        else if (conditionUsed != cond)     //if there already is a condition stored we check if this is the eos symbol. although it is not really a condition we store it to make use of detecitng eos. 
                        {
                            if (a == ';')
                                EOSreached = true;
                            else
                            {
                                extraStatementPresent = true;       //if the extra condition is not the eos we start storing the extra condition and set the extrastatement bool to true. 
                                extraCondition += a;
                            }
                        }
                    }
                }

                if (!EOSreached && !gettingCondition && !extraStatementPresent)     //checking to make sure that we can start storing the first/second statement then storing. 
                {
                    if (conditionUsed == "")
                        FirstVar += a;
                    else if (conditionUsed != "")   //if we have stored our condition already then we can start storing the second variable. 
                        SecondVar += a;
                }
                else if (extraStatementPresent && !gettingCondition)      //if we do ave an extra statement and are not getting the condition we start getting the extra statement. 
                    extraStatement += a;
            }

            FirstVar = FirstVar.Trim();         //trimming the first and second variables so no spaces. 
            SecondVar = SecondVar.Trim();

            if (extraCondition != "")   //if there is an extra condition detected we calculate then start calculating what the correct value for the first statement should be
            {
                FirstStatementValue = StatementValue(TruthTable[row, VariableIndex[FirstVar]], TruthTable[row, VariableIndex[SecondVar]], conditionUsed);

                if (FirstStatementValue)        //then we take the first statement and plug it into our method for calculating the correct value to determine the value for the whole statement. 
                    result = StatementValue(1, TruthTable[row, VariableIndex[extraStatement.Trim()]], extraCondition.Trim());
                else
                    result = StatementValue(0, TruthTable[row, VariableIndex[extraStatement.Trim()]], extraCondition.Trim());
            }
            else if (SecondVar != "")   //if there is no second statement and there is a second variable detected then we use our method to determine the value using the truth table. 
                result = StatementValue(TruthTable[row, VariableIndex[FirstVar]], TruthTable[row, VariableIndex[SecondVar]], conditionUsed);
            else
                result = StatementValue(TruthTable[row, VariableIndex[FirstVar]], 1, "NONE");   //if there is no second statement we input these values for the our statement calculations. 

            if (result)      //returning the value to be stored in our truth table for the statement. 
                return 1;
            else
                return 0;
        }

        private bool StatementValue(int a, int b, string condition) //calculating the result of a statement. 
        {
            bool result = false;

            if (condition == Implication)       //basic logic for if implication is the condition
            {
                if (a == 0 || b == 1)
                    result = true;
            }
            else if (condition == And)          //basic logic for if & is the condition
            {
                if (a == 1 && b == 1)
                    result = true;
            }
            else if (condition == Biconditional)      //basic logic for if biconditional is the statement
            {
                if (a == b)
                    result = true;
            }
            else if (condition == "NONE")           //basic statement for if there is only one variable. if our statement is a fact. 
            {
                if (a == 1)
                    result = true;
            }


            return result;
        }

        public void CheckTable()        //finding what statements are true comparing the knowledgebase values against the query/asked. 
        {
            TruthCount = 0;
            bool Matched = true;

            for(int i = 0; i < MaxRows; i++)    //cycling through all rows of the truth table. 
            {
                Matched = true;                 //reseting matched to be true for each cycle. 

                for(int j = Variables.Count; j < MaxCols; j++)  //cycling through the Knowledge base values within the truth table
                {
                    if (TruthTable[i, j] != askedTable[i])      //checking if the current col and row element matches or not with the respective query statement. 
                        Matched = false;
                }

                if (Matched)                                    //if there is a match we increment the count. 
                    TruthCount++;
            }

            if (TruthCount > 0)                                 //if there were matches we print out the result and counted matches. 
                Console.WriteLine("YES: " + TruthCount);
            else
                Console.WriteLine("NO");                        //else we print our result no. 
        }

        public void PopulateAskedTable()                //populating the table for the asked/query. 
        {
            askedTable = new int[MaxRows];

            for (int i = 0; i < MaxRows; i++)           //cycle through rows. 
            {
                askedTable[i] = AssessStatement(Ask[0], i);     //since there is only one element in ask list. we pass the first element, we use assess statement as it uses values from truth table to determine 
                                                                //the value asked for depending on the statement. 
            }
        }

        public void PopulateTruthTable()                    //populating the truth table. 
        {
            TruthTable = new int[MaxRows, MaxCols];

            for (int i = 0; i < MaxRows; i++)
            {
                for (int j = 0; j < MaxCols; j++)
                {
                    TruthTable[i, j] = 0;
                }
            }                                               //initialising the truth table with 0. 

            string binary = "";                             //setting up storage for a binary value. whose number of bits is the number of variables found in the knowledge base. 
            int padding = 0;                                //setting up the number of bits which need to be padded infront of the binary value

            for(int i = 0; i < MaxRows; i++)
            {
                binary = Convert.ToString(i, 2);            //storing the binary value for the row we are using to place in the truth table for variables side. 
                padding = Variables.Count - binary.Length;  //calculating the passing required. 
                
                for (int x = 0; x < padding; x++)
                    binary = "0" + binary;                  //adding the padding to the actual binary storage. 
           
                for (int j = 0; j < binary.Length; j++)
                {
                    TruthTable[i, j] = Convert.ToInt32(binary[j].ToString());       //for the binary length we store it in the truth table for each bit respectively. since the truth table is already organised
                                                                                    //for each col to correspond with the variable and then KB in sequence they are detected/stored. 
                }

                for(int j = binary.Length; j < MaxCols; j++)                        //once the line of columns are done for the variables, we asses the neccesary statements. 
                {                                                                   //binary length is used to skip the cols already registered for variables, but we can also use variables.count. 
                    TruthTable[i, j] = AssessStatement(TellIndex[j], i);
                }
            }
        }

        public void PrintTable()                                        //simple method for printing the truth table. used for early debugging purposes. 
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

        public void PairVariablesToTable()                  //pairing the variables in a dictionairy to pair the variables to their respective thruth table column. essentially storing the index 
        {
            for(int i = 0; i < Variables.Count; i++)
            {
                VariableIndex.Add(Variables[i], i);
            }
        }

        public void PairTelltoTable()                       //pairing the Knowledge base statements to their own respective columns. like pairvariables to table, we are just storing the index. 
        {
            for(int i = Variables.Count; i < MaxCols; i++)
            {
                TellIndex.Add(i, Tell[i - Variables.Count]);
            }
        }
    }
}
