using System;

namespace Pathoschild.NaturalTimeParser.Parser
{
	/// <summary>Extends <see cref="DateTime"/> to support natural time offsets.</summary>
	public static class DateTimeExtensions
	{
		/// <summary>Apply a natural date offset to the date.</summary>
		/// <param name="date">The initial date to offset.</param>
		/// <param name="offset">A natural date offset (like "<c>+5 months 2 days</c>").</param>
		public static DateTime Offset(this DateTime date, string offset)
		{
			return TimeParser.Default.Parse(offset, date);
		}
	}
}
