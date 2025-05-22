using Core.CrossCuttingCorcerns.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingCorcerns.Logging
{
    public class FileLogger : ILogger
    {
        private readonly string _logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", "log.txt");

        public void Log(string message)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_logFilePath)!);
                using var writer = new StreamWriter(_logFilePath, append: true);
                writer.WriteLine($"[{DateTime.Now}] {message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FileLogger HATASI: {ex.Message}");
            }
        }
    }
}
