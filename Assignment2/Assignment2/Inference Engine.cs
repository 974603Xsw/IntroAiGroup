using System;
namespace Assignment2
{
	public class Inference_Engine
	{

        private string q;
        private string[] s;

		public Inference_Engine(string location)
		{
			char[] illegalcharater = { ';' };
			int a = -1;
			int b = -1;
			string[] DATA = System.IO.File.ReadAllLines(System.IO.Path.Combine(Environment.CurrentDirectory, @"", location));
			for (int i = 0; i < DATA.Length; i++)
			{
				if (DATA[i] == "TELL")
				{
					a = i;
				}
				if (DATA[i] == "ASK")
				{
					b = i;
				}
			}
			if ((a == -1) || (b == -1))
			{
				Console.WriteLine("Text document not in correct format");
				Console.Read();
				Environment.Exit(0);
			}
			s = DATA[a+1].Split(illegalcharater);
			q = DATA[b + 1];
		}

        public string getAsk()
        {
            return q;
        }
	}
}
