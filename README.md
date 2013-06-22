**Pathoschild.NaturalTimeParser** implements part of the
[GNU date input format](http://www.gnu.org/software/tar/manual/html_node/Date-input-formats.html).
It lets you use date math with natural date strings (like
`DateTime.Now.Offset("+5 days 14 hours -2 minutes")`), and will eventually support creating dates
from natural time formats (like `"last month +2 days"`). The parser can be used by itself, or
integrated with a templating engine like DotLiquid or SmartFormat.NET (see details below).

This is used in at least one production system, so it's reasonably robust. Contributions to further
develop the library are welcome.

## Usage
Download the [`Pathoschild.NaturalTimeParser` NuGet package](https://nuget.org/packages/Pathoschild.NaturalTimeParser/)
and reference the `Pathoschild.NaturalTimeParser` namespace. This lets you apply a natural offset
to a date:
```c#
   // both lines are equivalent
   DateTime result = DateTime.Now.Offset("2 years ago");
   DateTime result = TimeParser.Default.Parse("2 years ago");
```

### Relative time units (date arithmetic)
The parser has full support for [relative time units](http://www.gnu.org/software/tar/manual/html_node/Relative-items-in-date-strings.html#SEC125).
For example, the following formats are supported:

* `1 year ago`
* `-2 years`
* `16 fortnights`
* `-1 year ago` (next year)
  
You can also chain relative units:

* `1 year 2 months` (14 months from now)
* `1 year -2 fortnights` (almost 11 months from now)
* `1 year ago 1 year` (today; equivalent to `-1 year +1 year`)

### Integrated with template engines
##### DotLiquid
The parser is available as a plugin for [DotLiquid](http://dotliquidmarkup.org/) through the
`Pathoschild.NaturalTimeParser.DotLiquid` NuGet package. DotLiquid is a safe templating library
that lets you format strings with token replacement, basic logic, and text transforms. For example,
this lets us format messages like this:
```c#
   Template.RegisterFilter(typeof(NaturalDateFilter));
   string message = "Your trial will expire in 30 days (on {{ 'today' | as_date | date_offset:'30 days' | date:'yyyy-MM-dd' }}).";
   message = Template.Parse(message).Render(); // "Your trial will expire in 30 days (on 2013-06-01)."
```

The plugin adds four custom tokens (`{Today}`/`{TodayUTC}` for the current local/UTC date, and
`{Now}`/`{NowUTC}` for the current local/UTC date & time) and adds support for applying relative
time units to any date. For example, you can format an arbitrary date token:
```c#
   Template.RegisterFilter(typeof(NaturalDateFilter));
   string message = "Your trial will expire in a long time (on {{ expiry_date | date_offset:'30 days' | date:'yyyy-MM-dd' }}).";
   message = Template.Parse(message).Render(Hash.FromAnonymousObject(new { ExpiryDate = new DateTime(2050, 01, 01) } }); // "Your trial will expire in a long time (on 2050-01-31)."
```

##### SmartFormat
The parser is available as a plugin for [SmartFormat.NET](https://github.com/scottrippey/SmartFormat.NET)
through the `Pathoschild.NaturalTimeParser.SmartFormat` NuGet package. SmartFormat is a string
composition library that enables advanced token replacement. For example, this lets us format
messages like this:
```c#
   SmartFormatter formatter = Smart.CreateDefaultSmartFormat().AddExtensionsForNaturalTime();
   string message = "Your trial will expire in 30 days (on {Today:yyyy-MM-dd|+30 days}).";
   message = formatter.Format(message); // "Your trial will expire in 30 days (on 2013-06-01).";
```

The plugin adds four custom tokens (`{Today}`/`{TodayUTC}` for the current local/UTC date, and
`{Now}`/`{NowUTC}` for the current local/UTC date & time) and adds support for applying relative
time units to any date. For example, you can format an arbitrary date token:
```c#
   SmartFormatter formatter = Smart.CreateDefaultSmartFormat().AddExtensionsForNaturalTime();
   string message = "Your trial will expire in a long time (on {ExpiryDate:yyyy-MM-dd|+30 days}).";
   message = formatter.Format(message, new { ExpiryDate = new DateTime(2050, 01, 01) }); // "Your trial will expire in a long time (on 2050-01-31).";
```

## Extending the parser
### Localization
The default implementation is English but can support other languages. For example, you can enable
relative time units in French:
```c#
   // configure French units
   ArithmeticTimePlugin plugin = TimeParser.Default.Parsers.OfType<ArithmeticTimePlugin>().First();
   plugin.SupportedUnits["jour"] = ArithmeticTimePlugin.RelativeTimeUnit.Days;
   plugin.SupportedUnits["heure"] = ArithmeticTimePlugin.RelativeTimeUnit.Hours;

   // now you can use French
   DateTime.Now.Offset("3 jours 4 heures");
```

### Plugins
This is implemented as a simple plugin-based lexer, which breaks down an input string into
its constituent tokens. For example, the string "`yesterday +1 day`" can be broken down into two
tokens:
```js
   [
      ["yesterday"],
      ["days", 1]
   ]
```

The parsing is provided by a set of plugins which implement `IParseTimeStrings` or
`IApplyTimeTokens`:

* `IParseTimeStrings` plugins are called to tokenize the input string. Each plugin scans the
  front of the string for recognized tokens, and stops at the first unrecognized value. Each
  matched token is stripped, and this is repeated until the entire string has been tokenized, or a
  portion is not recognized by any of the plugins (in which case a `TimeParseFormatException` is
  thrown).
* `IApplyTimeTokens` plugins are called to apply a token to a date. For example, the
  `ArithmeticTimePlugin` applies a token like `+3 days` by returning `date.AddDays(3)`.
  
New plugins can be added easily:
```c#
  TimeParser parser = new TimeParser(); // or TimeParser.Default
  parser.Parsers.Add(new SomePlugin());
  parser.Applicators.Add(new SomePlugin());
```