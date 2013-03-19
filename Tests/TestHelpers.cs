
using System;
using System.Collections.Generic;
using System.Globalization;
using Pathoschild.NaturalTimeParser.Parser.Plugins;
using Pathoschild.NaturalTimeParser.Parser.Tokenization;

namespace Pathoschild.NaturalTimeParser.Tests
{
	/// <summary>Provide convenience methods for testing time formats.</summary>
	public class TestHelpers
	{
		/// <summary>Get a string representation of a sequence of tokens for assertions.</summary>
		/// <param name="tokens">The tokens to represent.</param>
		public static string GetRepresentation(IEnumerable<TimeToken> tokens)
		{
			string result = "";
			foreach (TimeToken token in tokens)
				result += String.Format("[{0}:{1}]", token.Context, token.Value);
			return result;
		}

		/// <summary>Get a string representation of a date for assertions.</summary>
		/// <param name="date">The date to represent.</param>
		public static string GetRepresentation(DateTime? date)
		{
			return date.HasValue
				? date.Value.ToString("s")
				: "";
		}
	}
}
