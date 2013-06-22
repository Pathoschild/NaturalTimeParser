using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using DotLiquid;
using NUnit.Framework;
using Pathoschild.NaturalTimeParser.Extensions.DotLiquid;

namespace Pathoschild.NaturalTimeParser.Tests.Extensions
{
	/// <summary>Asserts that the DotLiquid <see cref="NaturalDateFilter"/> plugin support all valid scenarios.</summary>
	[TestFixture]
	public class DotLiquidTests
	{
		/*********
		** Unit tests
		*********/
		[Test(Description = "Assert that the datasource plugin correctly provides the {Today} token.")]
		public void Source_Today_TokenReplacedWithExpectedValue()
		{
			string withSource = this.GetTemplate("{{ 'today' | as_date | date_offset:'1 month ago' | date:'yyyy-MM-dd' }}").Render();
			string withoutSource = this.GetTemplate("{{ date | date_offset:'1 month ago' | date:'yyyy-MM-dd' }}").Render(Hash.FromAnonymousObject(new { Date = DateTime.UtcNow.Date }));
			Assert.AreEqual(withoutSource, withSource);
		}

		[Test(Description = "Assert that the datasource plugin correctly provides the {Now} token.")]
		public void Source_Now_TokenReplacedWithExpectedValue()
		{
			string withSource = this.GetTemplate("{{ 'now' | as_date | date_offset:'1 hour ago' | date:'yyyy-MM-dd' }}").Render();
			string withoutSource = this.GetTemplate("{{ date | date_offset:'1 hour ago' | date:'yyyy-MM-dd' }}").Render(Hash.FromAnonymousObject(new { Date = DateTime.Now }));

			Assert.AreEqual(withoutSource, withSource);
		}

		[Test(Description = "Assert that the formatter plugin returns the correct output for an offset date token.")]
		[TestCase("{{ date | date:'yyyy-MM-dd' }}", Result = "2000-01-01")]
		[TestCase("{{ date | date_offset:'10 years 2 months 3 days' |date:'yyyy-MM-dd' }}", Result = "2010-03-04")]
		[TestCase("{{ date | date_offset:'-1 year' | date:'yyyy' }}", Result = "1999")]
		[TestCase("{{ date | date_offset:'1 day ago' | date:'yyyy-MM-dd' }}", Result = "1999-12-31")]
		public string Formatter_BuildsExpectedOutput(string format)
		{
			return this.GetTemplate(format).Render(Hash.FromAnonymousObject(new { Date = new DateTime(2000, 1, 1) }));
		}


		/*********
		** Protected methods
		*********/
		/// <summary>Construct a formatter instance with the natural time filter registered.</summary>
		/// <param name="message">The message to format.</param>
		public Template GetTemplate(string message)
		{
			Template.RegisterFilter(typeof(NaturalDateFilter));
			return Template.Parse(message);
		}
	}
}