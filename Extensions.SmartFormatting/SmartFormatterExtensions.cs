using SmartFormat;

namespace Pathoschild.NaturalTimeParser.Extensions.SmartFormatting
{
	/// <summary>Extends <see cref="SmartFormatter"/> with plugin registration.</summary>
	public static class SmartFormatterExtensions
	{
		/// <summary>Register the <see cref="NaturalDateSource"/> and <see cref="NaturalDateFormatter"/> plugins for natural time parsing.</summary>
		/// <param name="formatter">The formatter to extend.</param>
		public static SmartFormatter AddExtensionsForNaturalTime(this SmartFormatter formatter)
		{
			formatter.SourceExtensions.Add(new NaturalDateSource());
			formatter.FormatterExtensions.Insert(0, new NaturalDateFormatter());
			return formatter;
		}
	}
}