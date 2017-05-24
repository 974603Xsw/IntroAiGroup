﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Assignment2
{
    class FileIO
    {
        private StreamReader sr;
        private string line;
        private string TrimLine;
        private List<string> KnowledgeBase;
        private string Ask;
        private string Tell;
        private bool StoreAsk;
        private bool StoreTell;

        public FileIO(string Fname)
        {
            KnowledgeBase = new List<string>();
            Open(Fname);
            Ask = "";
            Tell = "";
            StoreAsk = false;
            StoreTell = false;
        }

        private bool Open(string Fname)
        {
            bool Result = false;

            try
            {
                using (sr = new StreamReader(Fname))
                {
                    while((line = sr.ReadLine()) != null)
                    {
                        string[] seperatedText = line.Split(';');
                        foreach (string l in seperatedText)
                        {
                            TrimLine = l.Trim();
                            KnowledgeBase.Add(TrimLine + ";");
                        }
                    }
                    Result = true;
                }

                OrganiseAT();
            }
            catch(Exception e)
            {
                Console.WriteLine("File not found");
                Console.WriteLine(e.Message);
                Result = false;
            }

            return Result;
        }

        public void OrganiseAT()
        {
            foreach(string l in KnowledgeBase)
            {
                if (l == "TELL")
                {
                    StoreTell = true;
                    StoreAsk = false;
                }
                else if(l == "ASK")
                {
                    StoreTell = false;
                    StoreAsk = true;
                }

                if(StoreTell && (l != "TELL" && l!= "ASK"))
                {
                    Tell += l;
                }

                if(StoreAsk && (l != "TELL" && l != "ASK"))
                {
                    Ask += l;
                }
            }
        }

        public List<string> getKB()
        {
            return KnowledgeBase;
        }

        public List<string> getStoredLines()
        {
            return KnowledgeBase;
        }

        public void ExpelStoredLines()
        {
            for(int i = 0; i < KnowledgeBase.Count; i++)
            {
                Console.WriteLine(KnowledgeBase[i]);
            }

            //Console.WriteLine("\n\nTELL:\n" + Tell + "\n\nASK:\n" + Ask);
        }
    }
}
