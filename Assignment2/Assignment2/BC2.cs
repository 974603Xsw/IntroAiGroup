using System;
using System.Collections.Generic;

namespace Assignment2
{
	public class BC2
	{
		public static string tell;
		public static string ask;
		public static List<String> agenda;
		public static List<String> facts;
		public static List<String> clauses;
		public static List<String> entailed;

		public BC2(String a, string t)
		{
			//Initalise Varibles
			agenda = new List<string>();
			clauses = new List<string>();
			entailed = new List<string>();
			facts = new List<string>();
			tell = t;
			ask = a;
			initialise(tell);
		}

		//Method which sets up inital values for BC, called in creating BC class
		public static void initialise(string _tell)//string tell
		{
			agenda.Add(ask);
			//Split Knowledge base into sentences
			String[] sentences = _tell.Split(';');
			for (int i = 0; i < sentences.Length; i++)
			{
				if (!sentences[i].Contains("=>"))
				{
					facts.Add(sentences[i]);
				}
				else
				{
					clauses.Add(sentences[i]);
				}
			}
		}

		//Method which calls the main bcentails() method and returns output
		public string execute()
		{
			string output = "";
			if (!BCentails())
			{
				//The method returned true so it entails
				output = "YES: ";
				//loop through all entailed symbols in reverse
				for (int i = entailed.Count - 1; i >= 0; i--)
				{
					if (i == 0)
					{
						output = output + entailed[i];
					}
					else {
						//no comma at the end
						output = output + entailed[i] + ", ";
					}
				}
			}
			else {
				output = "NO";
			}
			return output;
		}

		//BC algorithum
		public bool BCentails()
		{
			//while list of symbols are not empty
			while (agenda.Count != 0)
			{
				//get current symbol
				string q = agenda[agenda.Count - 1];
				agenda.RemoveAt(agenda.Count - 1);
				//add the entailed array
				entailed.Add(q);
				//if this element is a fact then we dont need to go futher
				if (!facts.Contains(q))
				{
					//if not then create an array to hold new symbols to be processed 
					List<string> p = new List<string>();
					for (int i = 0; i < clauses.Count; i++)
					{
						//for each clauses that contains the symobl as its conclusion
						if (conclusionContains(clauses[i], q))
						{
							List<string> temp = getPremises(clauses[i]);
							for (int j = 0; j < temp.Count; j++)
							{
								//add the symbol to a temp array
								p.Add(temp[j]);
							}
						}
					}
					if (p.Count == 0)
					{
						return false;
					}
					else
					{
						//there are symbols so check for previously processed ones and add to agenda
						for (int i = 0; i < p.Count; i++)
						{
							
							if (!entailed.Contains(p[i]))
							{
								agenda.Add(p[i]);
							}
						}

					}
				}
			}//while ends
			return true;
		} 

		//method that returns the conjuncts contained in a clause
		public static List<string> getPremises(string clauseTest)//
		{
			//get the premise
			string premise = clauseTest.Split('=', '>')[0];
			List<String> temp = new List<string>();
			string[] conjucts = premise.Split('&');
			for (int i = 0; i < conjucts.Length; i++)
			{
				if (!agenda.Contains(conjucts[i]))
				{ 
					temp.Add(conjucts[i]);
				}

			}
			return temp;
		}

		// method which checks if c appears in the conclusion of a given clause	
		// input : clause, c
		// output : true if c is in the conclusion of clause	
		public static bool conclusionContains(string clause, string c)
		{
			char[] delimiter = { '=', '>' };
			string conclusion = clause.Split(delimiter)[2];
			//Trim conclusion due to white space
			conclusion = conclusion.Trim();
			c = c.Trim();
			if (conclusion.Equals(c))
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}