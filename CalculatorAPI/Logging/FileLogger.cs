using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.Logging;

namespace CalculatorAPI.Logging
{
    // Simple file logger that writes messages to a specified file.
    // Not a production-grade logger, but sufficient for local logging to a text file.
    public class FileLogger : ILogger
    {
        private readonly string categoryName;
        private readonly FileLoggerProvider provider;

        public FileLogger(string categoryName, FileLoggerProvider provider)
        {
            this.categoryName = categoryName;
            this.provider = provider;
        }

        public IDisposable BeginScope<TState>(TState state) => null!;

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;

            var message = formatter(state, exception);
            var sb = new StringBuilder();
            sb.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            sb.Append(" [");
            sb.Append(logLevel.ToString());
            sb.Append("] ");
            sb.Append(categoryName);
            sb.Append(" - ");
            sb.Append(message);
            if (exception != null)
            {
                sb.Append("\n");
                sb.Append(exception.ToString());
            }

            provider.WriteLine(sb.ToString());
        }
    }

    public class FileLoggerProvider : ILoggerProvider
    {
        private readonly string logFilePath;
        private readonly object fileLock = new object();

        public FileLoggerProvider(string path)
        {
            logFilePath = path ?? throw new ArgumentNullException(nameof(path));

            var dir = Path.GetDirectoryName(logFilePath) ?? "";
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(categoryName, this);
        }

        internal void WriteLine(string message)
        {
            try
            {
                lock (fileLock)
                {
                    File.AppendAllText(logFilePath, message + Environment.NewLine);
                }
            }
            catch
            {
                // Swallow any exceptions to avoid crashing the app because of logging failures.
            }
        }

        public void Dispose()
        {
            // nothing to dispose
        }
    }
}
