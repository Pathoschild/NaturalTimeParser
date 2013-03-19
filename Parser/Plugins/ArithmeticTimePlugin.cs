using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Pathoschild.NaturalTimeParser.Parser.Tokenization;

namespace Pathoschild.NaturalTimeParser.Parser.Plugins
{
	/// <summary>Parses a natural time format string containing time arithmetic (like "+2 days") into a set of tokens.</summary>
	public class ArithmeticTimePlugin : IParseTimeStrings, IApplyTimeTokens
	{
		/*********
		** Accessors
		*********/
		/// <summary>The regular expression that matches the date tokens in the input expression.</summary>
		protected Regex ParsePattern = new Regex(@"^(?<expression>\s*(?<sign>[\+\-]{0,1})\s*(?<value>\d+)\s*\b(?<unit>[a-zA-Z]+)\b)+$", RegexOptions.Compiled);

		/// <summary>The arbitrary key which identifies this plugin.</summary>
		protected const string Key = "Arithmetic";


		/*********
		** Public methods
		*********/
		/// <summary>Scan the front of an input string to read a set of matching tokens.</summary>
		/// <param name="input">The natural time format string.</param>
		/// <returns>Returns a set of matching tokens, or an empty collection if no supported token was found.</returns>
		public IEnumerable<TimeToken> Tokenize(string input)
		{
			// parse input
			Match match = this.ParsePattern.Match(input);
			if (!match.Success)
				yield break;

			// extract tokens
			int patternCount = match.Groups["expression"].Captures.Count;
			for (int i = 0; i < patternCount; i++)
			{
				string expression = match.Groups["expression"].Captures[i].Value;
				int value = int.Parse(match.Groups["value"].Captures[i].Value);
				if (match.Groups["sign"].Captures[i].Value == "-")
					value *= -1;
				string unit = match.Groups["unit"].Captures[i].Value;
				yield return new TimeToken(ArithmeticTimePlugin.Key, expression, unit, value);
			}
		}

		/// <summary>Apply a natural time token to a date value.</summary>
		/// <param name="token">The natural time token to apply.</param>
		/// <param name="date">The date value to apply the token to.</param>
		/// <returns>Returns the modified date, or <c>null</c> if the token is not supported.</returns>
		public DateTime? TryApply(TimeToken token, DateTime date)
		{
			// parse token
			if (token.Parser != ArithmeticTimePlugin.Key || !(token.Context is int))
				return null;
			string unit = token.Value.ToLower();
			int value = (int)token.Context;

			// apply
			switch (unit)
			{
				case "second":
				case "seconds":
					return date.AddSeconds(value);

				case "minute":
				case "minutes":
					return date.AddMinutes(value);

				case "hour":
				case "hours":
					return date.AddHours(value);

				case "day":
				case "days":
					return date.AddDays(value);

				case "month":
				case "months":
					return date.AddMonths(value);

				case "year":
				case "years":
					return date.AddYears(value);

				default:
					throw new FormatException(String.Format("Invalid arithmetic time unit: {0}", unit));
			}
		}
	}
}