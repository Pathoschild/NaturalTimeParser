using System;
using NUnit.Framework;
using Pathoschild.NaturalTimeParser.Extensions.SmartFormatting;
using SmartFormat;
using SmartFormat.Core;

namespace Pathoschild.NaturalTimeParser.Tests.Extensions
{
	/// <summary>Asserts that the SmartFormat <see cref="NaturalDateSource"/> and <see cref="NaturalDateFormatter"/> plugins support all valid scenarios.</summary>
	[TestFixture]
	public class SmartFormattingTests
	{
		/*********
		** Unit tests
		*********/
		[Test(Description = "Assert that the datasource plugin correctly provides the {Today} token.")]
		public void Source_Today_TokenReplacedWithExpectedValue()
		{
			string withSource = this.GetFormatter().Format("{Today:yyyy-MM-dd|1 month ago}");
			string withoutSource = this.GetFormatter().Format("{Date:yyyy-MM-dd|1 month ago}", new { Date = DateTime.UtcNow.Date });

			Assert.AreEqual(withoutSource, withSource);
		}

		[Test(Description = "Assert that the datasource plugin correctly provides the {Now} token.")]
		public void Source_Now_TokenReplacedWithExpectedValue()
		{
			string withSource = this.GetFormatter().Format("{Now:yyyy-MM-dd HH:mm|1 hour ago}");
			string withoutSource = this.GetFormatter().Format("{Date:yyyy-MM-dd HH:mm|1 hour ago}", new { Date = DateTime.UtcNow });

			Assert.AreEqual(withoutSource, withSource);
		}

		[Test(Description = "Assert that the formatter plugin returns the correct output for an offset date token.")]
		[TestCase("{Date}", Result = "2000-01-01 12:00:00 AM")]
		[TestCase("{Date:yyyy-MM-dd}", Result = "2000-01-01")]
		[TestCase("{Date:|10 years 2 months 3 days}", Result = "2010-03-04 12:00:00 AM")]
		[TestCase("{Date:yyyy|-1 year}", Result = "1999")]
		[TestCase("{Date:yyyy-MM-dd|1 day ago}", Result = "1999-12-31")]
		[TestCase("{Date:yyyy-MM-dd|1 day ago|extra|values|ignored}", Result = "1999-12-31")]
		public string Formatter_BuildsExpectedOutput(string format)
		{
			return this.GetFormatter().Format(format, new { Date = new DateTime(2000, 1, 1) });
		}

		[Test(Description = "Assert that the formatter plugin correctly handles errors and respects the configured error action.")]
		[TestCase("{Date:yyyy-MM-dd|invalid}", ErrorAction.Ignore, Result = "")]
		[TestCase("{Date:yyyy-MM-dd|invalid}", ErrorAction.MaintainTokens, Result = "{Date:yyyy-MM-dd|invalid}")]
		[TestCase("{Date:yyyy-MM-dd|invalid}", ErrorAction.ThrowError, ExpectedException = typeof(SmartFormat.Core.FormatException))]
		public string Formatter_RespectsErrorAction(string format, ErrorAction errorAction)
		{
			return this.GetFormatter(errorAction).Format(format, new { Date = new DateTime(2000, 1, 1) });
		}


		/*********
		** Protected methods
		*********/
		/// <summary>Construct a formatter instance with the natural time plugins registered.</summary>
		/// <param name="errorAction">How to handle format errors.</param>
		public SmartFormatter GetFormatter(ErrorAction? errorAction = null)
		{
			SmartFormatter formatter = Smart.CreateDefaultSmartFormat().AddExtensionsForNaturalTime();
			if (errorAction.HasValue)
				formatter.ErrorAction = errorAction.Value;
			return formatter;
		}
	}
}