namespace Pathoschild.NaturalTimeParser.Parser.Plugins
{
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
}