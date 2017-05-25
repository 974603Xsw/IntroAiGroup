using System;
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
        private List<string> KB2;
        private string Ask;
        private string Tell;
        private string Ask2;
        private string Tell2;
        private bool StoreAsk;
        private bool StoreTell;

        public FileIO(string Fname)
        {
            KnowledgeBase = new List<string>();
            KB2 = new List<string>();
            Ask = "";
            Tell = "";
            StoreAsk = false;
            StoreTell = false;
            Open(Fname);
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
                            TrimLine = l;
                            KB2.Add(TrimLine + ";");
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
                if (l == "TELL;")
                {
                    StoreTell = true;
                    StoreAsk = false;
                }
                else if(l == "ASK;")
                {
                    StoreTell = false;
                    StoreAsk = true;
                }
 
                if (StoreTell && (l != "TELL;" && l!= "ASK;"))
                {
                    if (l != "" && l != " " && l != ";")
                        Tell += l.Trim();
                }

                if(StoreAsk && (l != "TELL;" && l != "ASK;"))
                {
                    if (l != "" && l != " " && l != ";")
                        Ask += l.Trim();
                }
            }

            foreach (string l in KB2)
            {
                if (l == "TELL;")
                {
                    StoreTell = true;
                    StoreAsk = false;
                }
                else if (l == "ASK;")
                {
                    StoreTell = false;
                    StoreAsk = true;
                }

                if (StoreTell && (l != "TELL;" && l != "ASK;"))
                {
                    if (l != "" && l != " " && l != ";")
                        Tell2 += l;
                }

                if (StoreAsk && (l != "TELL;" && l != "ASK;"))
                {
                    if (l != "" && l != " " && l != ";")
                        Ask2 += l;
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

        public string getTell()
        {
            return Tell;
        }

        public string getAsk()
        {
            return Ask;
        }

        public string getTell2()
        {
            return Tell2;
        }

        public string getAsk2()
        {
            return Ask2;
        }

        public void ExpelStoredLines()
        {
            for(int i = 0; i < KnowledgeBase.Count; i++)
            {
                Console.WriteLine(KnowledgeBase[i]);
            }

            Console.WriteLine("\n\nTELL:\n" + Tell + "\n\nASK:\n" + Ask);
        }
    }
}
