using System;
using Pathoschild.NaturalTimeParser.Parser;
using SmartFormat.Core.Extensions;
using SmartFormat.Core.Output;
using SmartFormat.Core.Parsing;

namespace Pathoschild.NaturalTimeParser.Extensions.SmartFormatting
{
	/// <summary>Extends <see cref="SmartFormat"/> to support natural time offsets on <see cref="DateTime"/> values.</summary>
	public class NaturalDateFormatter : IFormatter
	{
		/// <summary>Process a format token and write the resulting output if it can be handled.</summary>
		/// <param name="current">The current token value.</param>
		/// <param name="format">The format token to apply.</param>
		/// <param name="handled">Whether this formatter plugin can handle the format token.</param>
		/// <param name="output">The result output to which to write the formatted value.</param>
		/// <param name="formatDetails">The format metadata.</param>
		public void EvaluateFormat(object current, Format format, ref bool handled, IOutput output, FormatDetails formatDetails)
		{
			// validate
			if (format == null || !(current is DateTime))
				return;

			// parse token
			FormatItem item = format.Items[0];
			string[] formatSpec = item.Text.Split('|');
			if (formatSpec.Length < 2)
				return;
			string dateFormat = formatSpec[0];
			string offset = formatSpec[1];

			// write offset date
			DateTime date = (DateTime)current;
			try
			{
				date = date.Offset(offset);
			}
			catch (Exception ex)
			{
				throw new SmartFormat.Core.FormatException(format, ex, item.endIndex);
			}
			output.Write(date.ToString(dateFormat), formatDetails);
			handled = true;
		}
	}
}