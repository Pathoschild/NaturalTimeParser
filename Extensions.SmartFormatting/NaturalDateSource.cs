using System;
using SmartFormat.Core.Extensions;
using SmartFormat.Core.Parsing;

namespace Pathoschild.NaturalTimeParser.Extensions.SmartFormatting
{
	/// <summary>Enables basic date tokens like <c>{Today}</c> (current date without time) and <c>{Now}</c> (current date and time).</summary>
	public class NaturalDateSource : ISource
	{
		/// <summary>Process a selector token and get the represented value if it can be handled.</summary>
		/// <param name="current">The current token value.</param>
		/// <param name="selector">The selector token to apply.</param>
		/// <param name="handled">Whether this selector plugin can handle the format token.</param>
		/// <param name="result">The selected value.</param>
		/// <param name="formatDetails">The format metadata.</param>
		public void EvaluateSelector(object current, Selector selector, ref bool handled, ref object result, FormatDetails formatDetails)
		{
			string token = selector.Text.ToLower();
			switch (token)
			{
				case "now":
					result = DateTime.Now;
					handled = true;
					return;

				case "today":
					result = DateTime.Now.Date;
					handled = true;
					return;

				case "nowutc":
					result = DateTime.UtcNow;
					handled = true;
					return;

				case "todayutc":
					result = DateTime.UtcNow.Date;
					handled = true;
					return;
			}
		}
	}
}