**Pathoschild.NaturalTimeParser** provides a partial C# implementation of natural time formats in the
[GNU date input format](http://www.gnu.org/software/tar/manual/html_node/Date-input-formats.html).
This currently lets you perform arithmetic with natural date strings (like
`DateTime.Now.Offset("+5 days 14 hours -2 minutes")`), and will eventually support creating
dates from natural time formats (like `"last month +2 days"`).

This is mainly a proof of concept so far. Contributions to make it more usable in production are
welcome.

## Usage
Download the `Pathoschild.NaturalTimeParser` NuGet package (TODO) and reference the
`Pathoschild.NaturalTimeParser` namespace. This lets you apply a natural offset to a date:
```c#
   DateTime.Now.Offset("-15 minutes 4 hours 16 years");
   TimeParser.Default.Parse("-2 hours");
```

## Implementation
This is implemented as a simple plugin-based lexer, which breaks down an input string into
its constituent tokens. For example, the string "`yesterday +1 day`" can be broken down into two
tokens:
```js
    [
       {"unit": "yesterday"},
       {"unit": "day", "value": 1}
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