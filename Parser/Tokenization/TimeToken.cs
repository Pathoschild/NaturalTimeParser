namespace Pathoschild.NaturalTimeParser.Parser.Tokenization
{
	/// <summary>Represents a relative date token (such as "-1 days").</summary>
	public struct TimeToken
	{
		/*********
		** Accessors
		*********/
		/// <summary>The token type matched. This is a unique value intended to help match parsed values.</summary>
		public string Type { get; set; }

		/// <summary>The substring that was matched. This value will be stripped from the input.</summary>
		public string Match { get; set; }

		/// <summary>The value associated with the token.</summary>
		public string Value { get; set; }

		/// <summary>The arbitrary value associated with the token. This is used by the time parser.</summary>
		public object Context { get; set; }


		/*********
		** Public methods
		*********/
		/// <summary>Construct an instance.</summary>
		/// <param name="type">The token type matched. This is a unique value intended to help match parsed values.</param>
		/// <param name="match">The substring that was matched. This value will be stripped from the input.</param>
		/// <param name="value">The value associated with the token.</param>
		/// <param name="context">The arbitrary value associated with the token. This is used by the time parser.</param>
		public TimeToken(string type, string match, string value, object context = null)
			: this()
		{
			this.Type = type;
			this.Match = match;
			this.Value = value;
			this.Context = context;
		}
	}
}