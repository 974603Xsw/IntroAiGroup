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

        public FileIO(string Fname)
        {
            KnowledgeBase = new List<string>();
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
                            TrimLine = l.Trim();
                            KnowledgeBase.Add(TrimLine);
                        }
                    }
                    Result = true;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("File not found");
                Console.WriteLine(e.Message);
                Result = false;
            }

            return Result;
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
        }
    }
}
