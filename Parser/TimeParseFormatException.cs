using System;

namespace Pathoschild.NaturalTimeParser.Parser
{
	/// <summary>The exception that is thrown when the format of a string cannot be understood as part of a relative date. The input and invalid token can be accessed through the <see cref="Input"/> and <see cref="InvalidToken"/> properties.</summary>
	public class TimeParseFormatException : FormatException
	{
		/*********
		** Accessors
		*********/
		/// <summary>The string that could not be parsed.</summary>
		public string Input { get; set; }

		/// <summary>The portion of the string that could not be understood.</summary>
		public string InvalidToken { get; set; }


		/*********
		** Public methods
		*********/
		/// <summary>Construct an instance.</summary>
		/// <param name="input">The string that could not be parsed.</param>
		/// <param name="invalidToken">The portion of the string that could not be understood.</param>
		public TimeParseFormatException(string input, string invalidToken)
			: base(String.Format("Could not parse date offset expression '{0}'. The following portion could not be understood: '{1}'.", input, invalidToken))
		{
			this.Input = input;
			this.InvalidToken = invalidToken;
		}
	}
}