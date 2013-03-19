using System;

namespace Pathoschild.NaturalTimeParser.Parser
{
	/// <summary>The exception that is thrown when the format of a string cannot be understood as part of a natural time format. The input format and the invalid token can be accessed through the <see cref="Input"/> and <see cref="InvalidToken"/> properties.</summary>
	public class TimeParseFormatException : FormatException
	{
		/*********
		** Accessors
		*********/
		/// <summary>The string that could not be parsed.</summary>
		public string Input { get; set; }

		/// <summary>The portion of the string that could not be understood as a natural time token.</summary>
		public string InvalidToken { get; set; }


		/*********
		** Public methods
		*********/
		/// <summary>Construct an instance.</summary>
		/// <param name="input">The string that could not be parsed.</param>
		/// <param name="invalidToken">The portion of the string that could not be understood as a natural time token.</param>
		public TimeParseFormatException(string input, string invalidToken)
			: base(String.Format("Could not parse natural time format '{0}'. The following portion could not be understood: '{1}'.", input, invalidToken))
		{
			this.Input = input;
			this.InvalidToken = invalidToken;
		}
	}
}