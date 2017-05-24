﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2
{
    class TT
    {
        private int[,] TruthTable;
        private List<string> KnowledgeBase;
        private List<string> possibleConditions;
        private List<string> Variables;
        private List<string> Ask;
        private List<string> Tell;
        private Dictionary<string, int> VariableIndex;
        private Dictionary<int, string> TellIndex;
        private int MaxRows;
        private int MaxCols;
        private string FirstVar;
        private string SecondVar;
        private string conditionUsed;
        private string Implication;
        private string Biconditional;
        private string And;
        private string Negation;
        private string Conjunction;
        private string Disjunction;
        private string EOS;             //end of statement

        public TT(List<string> KB)
        {
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

            KnowledgeBase = new List<string>(KB);
            possibleConditions = new List<string>();
            Variables = new List<string>();
            Ask = new List<string>();
            Tell = new List<string>();
            VariableIndex = new Dictionary<string, int>();
            TellIndex = new Dictionary<int, string>();

            possibleConditions.Add(Implication);
            //possibleConditions.Add(Biconditional);
            possibleConditions.Add(And);
            possibleConditions.Add(Negation);
            possibleConditions.Add(Conjunction);
            possibleConditions.Add(Disjunction);
            possibleConditions.Add(EOS);

            GetVariables();
            MaxRows = Convert.ToInt32(Math.Pow(2, Variables.Count));
            SepAskTell();
            MaxCols = Tell.Count + Variables.Count;

            TruthTable = new int[Convert.ToInt32(MaxRows), MaxCols];
            for(int i = 0; i < MaxRows; i++)
            {
                for (int j = 0; j < MaxCols; j++)
                {
                    TruthTable[i, j] = 0;
                }
            }

            PairVariablesToTable();
            PairTelltoTable();

            PopulateTruthTable();
            PrintTable();
        }

        private void PopulateTruthTable()
        {
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

        private void PrintTable()
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

        private int AssessStatement(string statement, int row)
        {
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
                        else if(conditionUsed != cond)
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
                else if(extraStatementPresent)
                    extraStatement += a;
            }

            SecondVar = SecondVar.Trim();
            Console.WriteLine("First:" + FirstVar);
            Console.WriteLine("Second:" + SecondVar);
            Console.WriteLine("ConditionUsed:" + conditionUsed);

            if (extraCondition != "")
            {
                FirstStatementValue = StatementValue(TruthTable[row, VariableIndex[FirstVar]], TruthTable[row, VariableIndex[SecondVar]], conditionUsed);

                if (FirstStatementValue)
                    result = StatementValue(1, TruthTable[row, VariableIndex[extraStatement.Trim()]], extraCondition.Trim());
                else
                    result = StatementValue(0, TruthTable[row, VariableIndex[extraStatement.Trim()]], extraCondition.Trim());
            }
            else
                result = StatementValue(TruthTable[row, VariableIndex[FirstVar]], TruthTable[row, VariableIndex[SecondVar]], conditionUsed);

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


            return result;
        }

        private void PairVariablesToTable()
        {
            for(int i = 0; i < Variables.Count; i++)
            {
                VariableIndex.Add(Variables[i], i);
            }
        }

        private void PairTelltoTable()
        {
            for(int i = Variables.Count; i < MaxCols; i++)
            {
                TellIndex.Add(i, Tell[i - Variables.Count]);
            }
        }

        private void SepAskTell()
        {
            bool Telling = false;
            bool Asking = false;

            foreach(string line in KnowledgeBase)
            {
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

            if (Tell.Contains(""))
                Tell.Remove("");
            if (Tell.Contains(";"))
                Tell.Remove(";");
            if (Tell.Contains("ASK;"))
                Tell.Remove("ASK;");
        }
        private void GetVariables()
        {
            foreach (string line in KnowledgeBase)
            {
                string possibleVar = "";

                foreach (char a in line)
                {
                    bool add = true;
                    foreach (string cond in possibleConditions)
                    {
                        if (cond.Contains<Char>(a))
                        {
                            add = false;
                        }
                    }

                    if (add)
                        possibleVar += a;
                    else if (!add)
                    {
                        possibleVar = possibleVar.Trim();
                        if (!Variables.Contains(possibleVar) && possibleVar != "" && possibleVar.ToUpper() != "TELL" && possibleVar.ToUpper() != "ASK")
                            Variables.Add(possibleVar);
                        possibleVar = "";
                    }
                }
            }
        }
    }
}