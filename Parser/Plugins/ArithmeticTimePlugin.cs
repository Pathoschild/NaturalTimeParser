using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Pathoschild.NaturalTimeParser.Parser.Tokenization;

namespace Pathoschild.NaturalTimeParser.Parser.Plugins
{
	/// <summary>Parses a natural time format string containing time arithmetic (like "+2 days") into a set of tokens.</summary>
	/// <remarks>This is an implementation of GNU relative items: http://www.gnu.org/software/tar/manual/html_node/Relative-items-in-date-strings.html </remarks>
	public class ArithmeticTimePlugin : IParseTimeStrings, IApplyTimeTokens
	{
		/*********
		** Properties
		*********/
		/// <summary>The regular expression that matches the date tokens in the input expression.</summary>
		protected readonly Regex ParsePattern = new Regex(@"^(?<expression>\s*((?<sign>[\+\-]{0,1})\s*(?<value>\d*))?\s*\b(?<unit>[a-zA-Z]+)\b(?<negate>(\s*\bago\b)?)\s*)+", RegexOptions.Compiled | RegexOptions.ExplicitCapture);

		/// <summary>The arbitrary key which identifies this plugin.</summary>
		protected const string Key = "Arithmetic";


		/*********
		** Accessors
		*********/
		/// <summary>A supported relative time unit.</summary>
		public enum RelativeTimeUnit
		{
			/// <summary>A unit of one second.</summary>
			Seconds,

			/// <summary>A unit of one minute.</summary>
			Minutes,

			/// <summary>A unit of one hour.</summary>
			Hours,

			/// <summary>A unit of one day.</summary>
			Days,

			/// <summary>A unit of seven days.</summary>
			Weeks,

			/// <summary>A unit of fourteen days.</summary>
			Fortnights,

			/// <summary>A unit of one month.</summary>
			Months,

			/// <summary>A unit of one year.</summary>
			Years,

			/// <summary>An unknown unit of time.</summary>
			Unknown
		};

		/// <summary>The supported time units.</summary>
		/// <remarks>This provides a case-insensitive unit lookup when parsing relative time items. The optional -s suffix is stripped before this lookup.</remarks>
		public readonly IDictionary<string, RelativeTimeUnit> SupportedUnits = new Dictionary<string, RelativeTimeUnit>(StringComparer.InvariantCultureIgnoreCase)
		{
			{ "sec", RelativeTimeUnit.Seconds },
			{ "second", RelativeTimeUnit.Seconds },
			{ "min", RelativeTimeUnit.Minutes },
			{ "minute", RelativeTimeUnit.Minutes },
			{ "hour", RelativeTimeUnit.Hours },
			{ "day", RelativeTimeUnit.Days },
			{ "week", RelativeTimeUnit.Weeks },
			{ "month", RelativeTimeUnit.Months },
			{ "year", RelativeTimeUnit.Years }
		};


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
				// extract parts
				string expression = match.Groups["expression"].Captures[i].Value;
				string sign = match.Groups["sign"].Captures[i].Value;
				string rawValue = match.Groups["value"].Captures[i].Value;
				string rawUnit = match.Groups["unit"].Captures[i].Value;
				bool negate = match.Groups["negate"].Captures[i].Value.Length > 0;

				// parse value
				int value = string.IsNullOrWhiteSpace(rawValue) ? 1 : int.Parse(rawValue);
				if (sign == "-")
					value *= -1;
				if (negate)
					value *= -1; // note: double-negation (like -1 year ago) is valid

				// parse unit
				RelativeTimeUnit unit = this.ParseUnit(rawUnit);
				if (unit == RelativeTimeUnit.Unknown)
					yield break; // unsupported unit

				// return token
				yield return new TimeToken(ArithmeticTimePlugin.Key, expression, unit.ToString(), value);
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
			RelativeTimeUnit unit = (RelativeTimeUnit)Enum.Parse(typeof(RelativeTimeUnit), token.Value);
			int value = (int)token.Context;

			// apply
			switch (unit)
			{
				case RelativeTimeUnit.Seconds:
					return date.AddSeconds(value);

				case RelativeTimeUnit.Minutes:
					return date.AddMinutes(value);

				case RelativeTimeUnit.Hours:
					return date.AddHours(value);

				case RelativeTimeUnit.Days:
					return date.AddDays(value);

				case RelativeTimeUnit.Weeks:
					return date.AddDays(value * 7);

				case RelativeTimeUnit.Fortnights:
					return date.AddDays(value * 14);

				case RelativeTimeUnit.Months:
					return date.AddMonths(value);

				case RelativeTimeUnit.Years:
					return date.AddYears(value);

				default:
					throw new FormatException(String.Format("Invalid arithmetic time unit: {0}", unit));
			}
		}


		/*********
		** Protected methods
		*********/
		/// <summary>Parse a localized time unit (like "secs") into a <see cref="RelativeTimeUnit"/>.</summary>
		/// <param name="unit">The localized time unit.</param>
		protected RelativeTimeUnit ParseUnit(string unit)
		{
			return this.SupportedUnits.ContainsKey(unit)
				? this.SupportedUnits[unit]
				: RelativeTimeUnit.Unknown;
		}
	}
}