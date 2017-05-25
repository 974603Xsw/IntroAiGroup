using System;
using System.Collections.Generic;

// to run simply do a: new FC(ask,tell) and then fc.execute()
// ask is a propositional symbol
// and tell is a knowledge base 
// ask : r
// tell : p=>q;q=>r;p;q;

namespace Assignment2
{
    class FC
    {
        // create variables
        public static String tell;
        public static String ask;
        public static List<String> agenda;

        public static List<String> facts;
        public static List<String> clauses;
        public static List<int> count;
        public static List<String> entailed;


        public FC(String a, String t)
        {
            // initialize variables
            agenda = new List<String>();
            clauses = new List<String>();
            entailed = new List<String>();
            facts = new List<String>();
            count = new List<int>();
            tell = t;
            ask = a;
            ask = ask.Replace(";", "");
            init(tell);
        }

        // method which calls the main fcentails() method and returns output back to iengine
        public String execute()
        {
            String output = "";
            if (!fcentails())
            {
                // the method returned true so it entails
                output = "YES: ";
                // for each entailed symbol
                for (int i = 0; i < entailed.Count; i++)
                {
                    output += entailed[i] + ", ";
                }
                if(!output.Contains(ask))
                    output += ask;
            }
            else {
                output = "NO";
            }
            return output;
        }

        // FC algorithm
        public bool fcentails()
        {
            // loop through while there are unprocessed facts
            if (agenda.Contains("")) 
            agenda.Remove("");

            while (agenda.Count > 0)
            {
                // take the first item and process it

                string p = agenda[0];
                agenda.RemoveAt(0);
                // add to entailed
                entailed.Add(p);
                // for each of the clauses....
                for (int i = 0; i < clauses.Count; i++)
                {
                    // .... that contain p in its premise
                    if (premiseContains(clauses[i], p))
                    {
                        int j = count[i];
                        // reduce count : unknown elements in each premise
                        count[i] = --j;
                        // all the elements in the premise are now known
                        if (count[i] == 0)
                        {
                            // the conclusion has been proven so put into agenda
                            string head = clauses[i].Split('=')[1];
                            // have we just proven the 'ask'?
                            head = head.Trim();
                            if (head.Equals(ask))
                                return true;
                            if (head.Contains("> "))
                                head = head.Replace("> ", "");
                            if (head.Contains(">"))
                                head = head.Replace(">", "");
                            agenda.Add(head);
                        }
                    }
                }
            }
            // if we arrive here then ask cannot be entailed
            return false;
        }

        // method which sets up initial values for forward chaining
        // takes in string representing KB and seperates symbols and clauses, calculates count etc..
        public static void init(String tell)
        {
            String[] sentences = tell.Split(';');
            for (int i = 0; i < sentences.Length; i++)
            {

                if (!sentences[i].Contains("=>"))
                    // add facts to be processed
                    agenda.Add(sentences[i]);
                else {
                    // add sentences
                    clauses.Add(sentences[i]);
            		count.Add(sentences[i].Split('&').Length);
                }
            }
        }

        // method which checks if p appears in the premise of a given clause	
        // input : clause, p
        // output : true if p is in the premise of clause
        public static bool premiseContains(String clause, String p)
        {
            String premise = clause.Split('=')[0];
            premise = premise.Trim();
            String[] conjuncts = premise.Split('&');
            // check if p is in the premise
            if (conjuncts.Length == 1)
            {
                return premise.Equals(p);
            }
            else {
                return conjuncts[0].Equals(p);
            }
        }
    }
}
