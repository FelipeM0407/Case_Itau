using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace CaseItau.API.Infrastructure.Logging
{
    public class CustomLoggerProvider : ILoggerProvider
    {
        private readonly CustomLoggerProviderConfiguration _config;
        private readonly Dictionary<string, CustomLogger> _loggers = new Dictionary<string, CustomLogger>();

        public CustomLoggerProvider(CustomLoggerProviderConfiguration config)
        {
            _config = config;
        }

        public ILogger CreateLogger(string categoryName)
        {
            if (!_loggers.ContainsKey(categoryName))
            {
                _loggers[categoryName] = new CustomLogger(categoryName, _config);
            }
            return _loggers[categoryName];
        }

        public void Dispose()
        {
            _loggers.Clear();
        }
    }
}
