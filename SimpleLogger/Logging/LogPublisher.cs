using System.Collections.Generic;
﻿using Newtonsoft.Json;
using System;
using System.Linq;
using System.IO;
using SimpleLogger.Logging.Handlers;

namespace SimpleLogger.Logging
{
    internal class LogPublisher : ILoggerHandlerManager
    {
        private readonly IList<ILoggerHandler> _loggerHandlers;
        private IList<LogMessage> _messages;

        public LogPublisher()
        {
            _loggerHandlers = new List<ILoggerHandler>();
            _messages = new List<LogMessage>();
        }

        public void Publish(LogMessage logMessage)
        {
            _messages.Add(logMessage);
            foreach (var loggerHandler in _loggerHandlers)
                loggerHandler.Publish(logMessage);
        }

		public IList<LogMessage> GetPublishedLogs()
		{
			return GetHandler<FileLoggerHandler>().Read();
		}

		public T GetHandler<T>() where T : ILoggerHandler
		{
			var handler = _loggerHandlers.OfType<T>().First();
			return handler;
		}

		public ILoggerHandlerManager AddHandler(ILoggerHandler loggerHandler)
        {
            if (loggerHandler != null)
                _loggerHandlers.Add(loggerHandler);
            return this;
        }

        public bool RemoveHandler(ILoggerHandler loggerHandler)
        {
            return _loggerHandlers.Remove(loggerHandler);
        }

        public IEnumerable<LogMessage> Messages
        {
            get { return _messages; }
        }
    }
}
