using System;
using DotLiquid;
using Pathoschild.NaturalTimeParser.Parser;

namespace Pathoschild.NaturalTimeParser.Extensions.DotLiquid
{
	/// <summary>Extends the DotLiquid <see cref="Template"/> to support natural time offsets on <see cref="DateTime"/> values. This class should be registered with <see cref="Template.RegisterFilter"/>.</summary>
	/// <example>
	/// This filter can be applied to date tokens. For example:
	/// <code>{{ blog.date | date_offset:'30 days' | date:'yyyy-MM-dd' }}</code>
	/// </example>
	public static class NaturalDateFilter
	{
		/// <summary>Apply a natural time offset to the date.</summary>
		/// <param name="date">The initial date to offset.</param>
		/// <param name="offset">A natural time offset (like "<c>+5 months 2 days</c>").</param>
		public static DateTime DateOffset(DateTime date, string offset)
		{
			return date.Offset(offset);
		}

		/// <summary>Convert a date token like 'today' to a date.</summary>
		/// <param name="token">The token value. Supported values are today/todayutc (date) and now/nowutc (datetime).</param>
		public static DateTime AsDate(string token)
		{
			DateTime? result = new TimeParser().ParseName(token);
			return result ?? DateTime.MinValue;
		}
	}
}