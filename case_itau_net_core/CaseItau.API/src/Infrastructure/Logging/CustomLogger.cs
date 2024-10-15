using System;
using System.IO;
using CaseItau.API.Infrastructure.Logging;
using Microsoft.Extensions.Logging;

public class CustomLogger : ILogger
{
    private readonly string _categoryName;
    private readonly CustomLoggerProviderConfiguration _config;

    public CustomLogger(string categoryName, CustomLoggerProviderConfiguration config)
    {
        _categoryName = categoryName;
        _config = config;
    }

    public IDisposable BeginScope<TState>(TState state) => null;

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel >= _config.LogLevel;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        if (formatter == null)
        {
            throw new ArgumentNullException(nameof(formatter));
        }

        string message = formatter(state, exception);
        if (string.IsNullOrEmpty(message))
        {
            return;
        }

        string logFilePath = "logs/logs_customizados.txt";

        var logDirectory = Path.GetDirectoryName(logFilePath);
        if (!Directory.Exists(logDirectory))
        {
            Directory.CreateDirectory(logDirectory);
        }

        string logRecord = $"{logLevel}: {eventId.Id} - {_categoryName} - {message}";

        if (exception != null)
        {
            logRecord += Environment.NewLine + exception;
        }

        lock (this)
        {
            using (var writer = new StreamWriter(logFilePath, append: true))
            {
                writer.WriteLine(logRecord);
                writer.Close();
            }
        }
    }
}
