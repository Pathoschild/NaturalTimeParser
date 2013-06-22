using System;
using System.Collections.Generic;
using System.Linq;
using Pathoschild.NaturalTimeParser.Parser.Plugins;
using Pathoschild.NaturalTimeParser.Parser.Tokenization;

namespace Pathoschild.NaturalTimeParser.Parser
{
	/// <summary>Parses date input strings matching the GNU input date format.</summary>
	public class TimeParser
	{
		/*********
		** Accessors
		*********/
		/// <summary>The default instance.</summary>
		public static TimeParser Default = new TimeParser();

		/// <summary>Plugins which parse tokens from the input string.</summary>
		public IList<IParseTimeStrings> Parsers { get; protected set; }

		/// <summary>Plugins which apply time tokens to a date.</summary>
		public IList<IApplyTimeTokens> Applicators { get; set; }


		/*********
		** Public methods
		*********/
		/// <summary>Construct an instance with the default plugins.</summary>
		public TimeParser()
		{
			this.Parsers = new List<IParseTimeStrings>(new[] { new ArithmeticTimePlugin() });
			this.Applicators = new List<IApplyTimeTokens>(new[] { new ArithmeticTimePlugin() });
		}

		/// <summary>Parse a date input string matching the GNU input date format.</summary>
		/// <param name="input">The date input string.</param>
		public DateTime Parse(string input)
		{
			return this.Parse(input, DateTime.UtcNow);
		}

		/// <summary>Parse a date input string matching a natural name like 'today'.</summary>
		/// <param name="token">The date name. Accepted values are today/todayUTC (current date) and now/nowUTC (current datetime).</param>
		/// <returns>The generated date, or <c>null</c> if the token name is not supported.</returns>
		public DateTime? ParseName(string token)
		{
			if (token == null)
				return null;
			token = token.Trim().ToLower();
			switch (token)
			{
				case "now":
					return DateTime.Now;

				case "today":
					return DateTime.Now.Date;

				case "nowutc":
					return DateTime.UtcNow;

				case "todayutc":
					return DateTime.UtcNow.Date;

				default:
					return null;
			}
		}

		/// <summary>Parse a date input string matching the GNU input date format.</summary>
		/// <param name="input">The date input string.</param>
		/// <param name="initial">The initial date to which to apply relative formats.</param>
		public DateTime Parse(string input, DateTime initial)
		{
			TimeToken[] tokens = this.Tokenize(input).ToArray();
			return this.Apply(initial, tokens);
		}

		/// <summary>Converts an input string into a sequence of time tokens.</summary>
		/// <param name="input">The date input string.</param>
		/// <exception cref="TimeParseFormatException">A portion of the input string could not be understood as a time token.</exception>
		/// <exception cref="InvalidOperationException">A parse plugin behaved in an unexpected way.</exception>
		public IEnumerable<TimeToken> Tokenize(string input)
		{
			string remaining = input;
			while (true)
			{
				// input parsing complete
				bool matched = false;
				remaining = remaining.Trim();
				if (remaining.Length == 0)
					yield break;

				// call each parser
				foreach (IParseTimeStrings parser in this.Parsers)
				{
					// parse tokens
					TimeToken[] tokens = parser.Tokenize(remaining).ToArray();
					if (!tokens.Any())
						continue;

					// handle matched tokens
					matched = true;
					foreach (TimeToken token in tokens)
					{
						// strip token from input
						int tokenIndex = remaining.IndexOf(token.Match, StringComparison.InvariantCulture);
						if (tokenIndex == -1)
							throw new InvalidOperationException(String.Format("The matched time token '{0}' was not found in the input string.", token.Match));
						if (tokenIndex != 0)
							throw new InvalidOperationException(String.Format("The matched time token '{0}' did not match the next segment of the input string.", token.Match));
						remaining = remaining.Substring(token.Match.Length);

						// return token
						yield return token;
					}

					// start over with new string
					break;
				}

				// no parser matched
				if (!matched)
					throw new TimeParseFormatException(input, remaining);
			}
		}

		/// <summary>Apply a sequence of time tokens to a date.</summary>
		/// <param name="date">The initial date.</param>
		/// <param name="tokens">The tokens which modify the date.</param>
		public DateTime Apply(DateTime date, IEnumerable<TimeToken> tokens)
		{
			foreach (TimeToken token in tokens)
			{
				bool matched = false;
				foreach (IApplyTimeTokens applicator in this.Applicators)
				{
					DateTime? result = applicator.TryApply(token, date);
					if (result != null)
					{
						matched = true;
						date = result.Value;
						break;
					}
				}
				if (!matched)
					throw new InvalidOperationException(String.Format("There is no time applicator plugin which recognizes the token '{0}'. The parsed type is '{1}' with a value of '{2}'.", token.Match, token.Parser, token.Value));
			}

			return date;
		}
	}
}
