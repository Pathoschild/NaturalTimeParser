using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Pathoschild.NaturalTimeParser.Parser;
using Pathoschild.NaturalTimeParser.Parser.Plugins;
using Pathoschild.NaturalTimeParser.Parser.Tokenization;
using Pathoschild.NaturalTimeParser.Tests.Plugins;

namespace Pathoschild.NaturalTimeParser.Tests
{
	/// <summary>Asserts that the default <see cref="TimeParser"/> supports all valid scenarios.</summary>
	/// <remarks>This tests the parser framework. More detailed input format assertions should be added for the plugins that implement them (e.g., <see cref="ArithmeticTimePluginTests"/>).</remarks>
	[TestFixture]
	public class TimeParserTests
	{
		/*********
		** Unit tests
		*********/
		[Test(Description = "Assert that the parser has the default parser and applicator plugins registered.")]
		public void HasDefaultPlugins()
		{
			TimeParser parser = new TimeParser();
			Assert.That(parser.Parsers.OfType<ArithmeticTimePlugin>().Any(), "The arithmetic time plugin isn't registered as a parser.");
			Assert.That(parser.Applicators.OfType<ArithmeticTimePlugin>().Any(), "The arithmetic time plugin isn't registered as an applicator.");
		}

		[Test(Description = "Assert that the parser can tokenize various representative input formats. More detailed format tokens are in the plugin tests.")]
		[TestCase("3 days ago", Result = "[Days:-3]")]
		[TestCase("14 months -3 days 2 hours", Result = "[Months:14][Days:-3][Hours:2]")]
		public string CanTokenize(string format)
		{
			IEnumerable<TimeToken> tokens = new TimeParser().Tokenize(format);
			return TestHelpers.GetRepresentation(tokens);
		}

		[Test(Description = "Assert that the parser can apply tokens in various formats to a date.")]
		[TestCase(ArithmeticTimePlugin.Key, "42", RelativeTimeUnit.Days, Result = "2000-02-12T00:00:00")]
		[TestCase(ArithmeticTimePlugin.Key, "42", RelativeTimeUnit.Fortnights, Result ="2001-08-11T00:00:00")]
		public string CanApply(string type, string value, object context)
		{
			TimeToken token = new TimeToken(type, null, value, context);
			DateTime? date = new TimeParser().Apply(new DateTime(2000, 1, 1), new[] { token });
			return TestHelpers.GetRepresentation(date);
		}

		[Test(Description = "Assert that the parser can parse a date format and apply it to a date.")]
		[TestCase("42 days", Result = "2000-02-12T00:00:00")]
		[TestCase("42 fortnights", Result = "2001-08-11T00:00:00")]
		public string CanParse(string format)
		{
			DateTime? date = new TimeParser().Parse(format, new DateTime(2000, 1, 1));
			return TestHelpers.GetRepresentation(date);
		}

		[Test(Description = "Assert that valid names are supported for the ParseName method.")]
		public void CanParseName()
		{
			TimeParser parser = new TimeParser();
			Assert.AreEqual(DateTime.Today, parser.ParseName("today"), "The 'today' name returned an unexpected value.");
			Assert.AreEqual(DateTime.UtcNow.Date, parser.ParseName("todayUTC"), "The 'todayUTC' name returned an unexpected value.");
			Assert.AreEqual(DateTime.Now, parser.ParseName("now"), "The 'now' name returned an unexpected value.");
			Assert.AreEqual(DateTime.UtcNow, parser.ParseName("nowUTC"), "The 'nowUTC' name returned an unexpected value.");
		}


	}
}
