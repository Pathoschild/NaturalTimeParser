using System;

namespace Pathoschild.NaturalTimeParser.Parser.Tokenization
{
	/// <summary>Applies time tokens to <see cref="DateTime"/> values.</summary>
	public interface IApplyTimeTokens
	{
		/// <summary>Apply a natural time token to a date value.</summary>
		/// <param name="token">The natural time token to apply.</param>
		/// <param name="date">The date value to apply the token to.</param>
		/// <returns>Returns the modified date, or <c>null</c> if the token is not supported.</returns>
		DateTime? TryApply(TimeToken token, DateTime date);
	}
}