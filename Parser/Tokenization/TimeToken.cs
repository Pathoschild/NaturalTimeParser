namespace Pathoschild.NaturalTimeParser.Parser.Tokenization
{
	/// <summary>Represents a natural time token (such as "today" or "-2 days").</summary>
	public struct TimeToken
	{
		/*********
		** Accessors
		*********/
		/// <summary>The arbitrary parser plugin key, intended to help match parsed values.</summary>
		public string Parser { get; set; }

		/// <summary>The substring that was matched. This value will be stripped from the input.</summary>
		public string Match { get; set; }

		/// <summary>The value associated with the token.</summary>
		public string Value { get; set; }

		/// <summary>The arbitrary context data associated with the token. This is used by the plugin.</summary>
		public object Context { get; set; }


		/*********
		** Public methods
		*********/
		/// <summary>Construct an instance.</summary>
		/// <param name="type">The arbitrary parser plugin key, intended to help match parsed values.</param>
		/// <param name="match">The substring that was matched. This value will be stripped from the input.</param>
		/// <param name="value">The value associated with the token.</param>
		/// <param name="context">The arbitrary context data associated with the token. This is used by the plugin.</param>
		public TimeToken(string type, string match, string value, object context = null)
			: this()
		{
			this.Parser = type;
			this.Match = match;
			this.Value = value;
			this.Context = context;
		}
	}
}