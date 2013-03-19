using System;
using System.Linq;
using Pathoschild.NaturalTimeParser.Parser;
using Pathoschild.NaturalTimeParser.Parser.Tokenization;

namespace Pathoschild.NaturalTimeParser.Demo
{
	class Program
	{
		static void Main()
		{
			TimeParser parser = new TimeParser();
			TimeToken[] tokens = parser.Tokenize("-1 month +5 minutes").ToArray();
			DateTime resultA = parser.Apply(DateTime.Now, tokens);
			Console.WriteLine(resultA);

			DateTime resultB = DateTime.Now.Offset("-1 month +5 minutes");
			Console.WriteLine(resultB);
		}
	}
}
