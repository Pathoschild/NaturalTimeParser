using System.Collections.Generic;

namespace Pathoschild.NaturalTimeParser.Parser.Tokenization
{
	/// <summary>Parses a natural time format string into a set of tokens.</summary>
	/// <remarks>This plugin is called to tokenize the input string. It should scan the front of the string for recognized tokens, and stop at the first unrecognized value.</remarks>
	public interface IParseTimeStrings
	{
		/// <summary>Scan the front of an input string to read a set of matching tokens.</summary>
		/// <param name="input">The natural time format string.</param>
		/// <returns>Returns a set of matching tokens, or an empty collection if no supported token was found.</returns>
		IEnumerable<TimeToken> Tokenize(string input);
	}
}