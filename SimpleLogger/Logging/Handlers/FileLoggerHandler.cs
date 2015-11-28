using System;
using System.IO;
using SimpleLogger.Logging.Formatters;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SimpleLogger.Logging.Handlers
{
    public class FileLoggerHandler : ILoggerHandler
    {
        private readonly string _fileName;
        private readonly string _directory;
        private readonly ILoggerFormatter _loggerFormatter;

        public FileLoggerHandler() : this(CreateFileName()) { }

        public FileLoggerHandler(string fileName) : this(fileName, string.Empty) { }

        public FileLoggerHandler(string fileName, string directory) : this(new JsonFormatter(), fileName, directory) { }

        public FileLoggerHandler(ILoggerFormatter loggerFormatter) : this(loggerFormatter, CreateFileName()) { }

        public FileLoggerHandler(ILoggerFormatter loggerFormatter, string fileName) : this(loggerFormatter, fileName, string.Empty) { }

        public FileLoggerHandler(ILoggerFormatter loggerFormatter, string fileName, string directory)
        {
            _loggerFormatter = loggerFormatter;
            _fileName = fileName;
            _directory = directory;
        }

        public void Publish(LogMessage logMessage)
        {
            if (!string.IsNullOrEmpty(_directory))
            {
                var directoryInfo = new DirectoryInfo(Path.Combine(_directory));
                if (!directoryInfo.Exists)
                    directoryInfo.Create();
            }

			var fs = File.Open(Path.Combine(_directory, _fileName), FileMode.Append);

			using (var writer = new StreamWriter(fs))
			{
				writer.WriteLine(_loggerFormatter.ApplyFormat(logMessage));
				writer.Write(',');
			}
        }
		public IList<LogMessage> Read()
		{
			var pseudojson =string.Empty;

			var fs = File.Open(Path.Combine(_directory, _fileName), FileMode.Open);
			using (var reader = new StreamReader(fs))
			{
				pseudojson = reader.ReadToEnd();
			}
			pseudojson = pseudojson.Insert(0, "[");
			pseudojson = pseudojson.Remove(pseudojson.Length - 1, 1);
			pseudojson += "]";
			var logs = JsonConvert.DeserializeObject<IList<LogMessage>>(pseudojson);
			return logs;
		}

		private static string CreateFileName()
        {
            var currentDate = DateTime.Now;
            var guid = Guid.NewGuid();
            return string.Format("Log_{0:0000}{1:00}{2:00}-{3:00}{4:00}_{5}.log",
                currentDate.Year, currentDate.Month, currentDate.Day, currentDate.Hour, currentDate.Minute, guid);
        }
    }
}
