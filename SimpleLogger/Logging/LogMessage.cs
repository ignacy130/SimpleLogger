using System;
using SimpleLogger.Logging.Formatters;
using Newtonsoft.Json;

namespace SimpleLogger.Logging
{
	public class LogMessage
	{
		public DateTime DateTime { get; set; }
		public Logger.Level Level { get; set; }
		public string Text { get; set; }
		public string CallingClass { get; set; }
		public string CallingMethod { get; set; }
		public int LineNumber { get; set; }
		private ILoggerFormatter formatter;

		[JsonConstructor]
		private LogMessage()
		{

		}

		public LogMessage(ILoggerFormatter formatter) {
			this.formatter = formatter; }

		public LogMessage(ILoggerFormatter formatter, Logger.Level level, string text, DateTime dateTime, string callingClass, string callingMethod, int lineNumber) : this(formatter)
		{
			Level = level;
			Text = text;
			DateTime = dateTime;
			CallingClass = callingClass;
			CallingMethod = callingMethod;
			LineNumber = lineNumber;
		}

		public override string ToString()
		{
			return formatter.ApplyFormat(this);
		}
	}
}
