using System;
using System.IO;

namespace CalculatorAPI.Logging
{
    // Simple helper to write calculation logs to a date-named text file.
    public static class TextCalculationLogger
    {
        private static readonly object fileLock = new object();
        private static string? configuredLogsDirectory;
       
        // Call this at startup with the application's content root to ensure logs are written next to the app
        public static void Initialize(string contentRootPath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(contentRootPath))
                    return;

                configuredLogsDirectory = Path.Combine(contentRootPath, "Logs");
                if (!Directory.Exists(configuredLogsDirectory))
                    Directory.CreateDirectory(configuredLogsDirectory);

                var todayFile = Path.Combine(configuredLogsDirectory, DateTime.Now.ToString("yyyy-MM-dd") + ".log");
                if (!File.Exists(todayFile))
                {
                    File.AppendAllText(todayFile, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} | Startup log created (Initialize)" + Environment.NewLine);
                }
            }
            catch
            {
                // ignore
            }
        }

        private static string GetLogsDirectory()
        {
            if (!string.IsNullOrEmpty(configuredLogsDirectory))
                return configuredLogsDirectory!;

            // Use the application's base directory as a fallback
            var baseDir = AppContext.BaseDirectory ?? Directory.GetCurrentDirectory();
            var logsDir = Path.Combine(baseDir, "Logs");
            if (!Directory.Exists(logsDir))
                Directory.CreateDirectory(logsDir);
            return logsDir;
        }

        private static string GetLogFilePathForDate(DateTime dt)
        {
            var fileName = dt.ToString("yyyy-MM-dd") + ".log";
            return Path.Combine(GetLogsDirectory(), fileName);
        }

        public static void LogCalculation(string operation, double pa, double pb, double result)
        {
            try
            {
                var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                var line = $"{timestamp} | Operation: {operation} | PA={pa} | PB={pb} | Result={result}";
                var filePath = GetLogFilePathForDate(DateTime.Now);

                lock (fileLock)
                {
                    File.AppendAllText(filePath, line + Environment.NewLine);
                }
            }
            catch
            {
                // swallow exceptions to avoid breaking the application
            }
        }
    }
}
