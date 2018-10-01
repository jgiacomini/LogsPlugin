using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plugin.Logs.Model;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Logs.Test
{
    [TestClass]
    public class LogServiceTest
    {
        private readonly string _file_Prefix = "Test";

        private string GetRandomDirectoryPath(string fileName)
        {
            var path = Path.Combine(Path.GetTempPath(), fileName);
            Directory.CreateDirectory(path);

            return path;
        }

        [TestMethod]
        public async Task Logs_Service_LogInfoAsync()
        {
            var directoryPath = GetRandomDirectoryPath("Infos");

            var filePrefix = _file_Prefix + "LogInfo";
            var logService = new LogService(filePrefix, directoryPath);
            Debug.WriteLine(directoryPath);
            logService.Log("log information test", LogLevel.Information);

            await logService.FlushAsync();
            var today = DateTime.Now;

            var fileName = Path.Combine(directoryPath, $"{today.ToString("yyyy-MM")}{Path.DirectorySeparatorChar}{filePrefix}_log_{today.ToString("yyyy-MM-dd")}.csv");
            Assert.IsTrue(File.Exists(fileName), $"File doesn't exist {fileName}");
        }

        [TestMethod]
        public async Task LogService_FlushAsync()
        {
            var directoryPath = GetRandomDirectoryPath("Flush");
            var filePrefix = _file_Prefix + "_flush";
            var logService = new LogService(filePrefix, directoryPath);
            Debug.WriteLine(directoryPath);

            logService.Log("log information test", LogLevel.Information);
            await logService.FlushAsync();
            var today = DateTime.Today;
            var fileName = Path.Combine(directoryPath, Path.Combine($"{today.ToString("yyyy-MM")}", $"{filePrefix}_log_{today.ToString("yyyy-MM-dd")}.csv"));
            Assert.IsTrue(File.Exists(fileName), $"File doesn't exist {fileName}");
        }

        [TestMethod]
        public async Task LogService_WriteBigString()
        {
            var directoryPath = GetRandomDirectoryPath("BigString");
            var filePrefix = _file_Prefix + "_bigString";
            var logService = new LogService(filePrefix, directoryPath);
            Debug.WriteLine(directoryPath);

            var sb = new StringBuilder();

            await Task.Run(() =>
            {
                for (int i = 0; i < 10000; i++)
                {
                    sb.AppendLine("azertyuipqsdfghjklmwxcvbn,;:");
                    sb.AppendLine("azertyuipqsdfghjklmwxcvbn,;:");
                    sb.AppendLine("azertyuipqsdfghjklmwxcvbn,;:");
                    sb.AppendLine("azertyuipqsdfghjklmwxcvbn,;:");
                    sb.AppendLine("azertyuipqsdfghjklmwxcvbn,;:");
                    sb.AppendLine("azertyuipqsdfghjklmwxcvbn,;:");
                    sb.AppendLine("azertyuipqsdfghjklmwxcvbn,;:");
                    sb.AppendLine("azertyuipqsdfghjklmwxcvbn,;:");
                    sb.AppendLine("azertyuipqsdfghjklmwxcvbn,;:");
                    sb.AppendLine("azertyuipqsdfghjklmwxcvbn,;:");
                }
            }).ConfigureAwait(false);

            logService.Log(sb.ToString(), LogLevel.Information);

            await logService.FlushAsync().ConfigureAwait(false);
            var today = DateTime.Today;
            var fileName = Path.Combine(directoryPath, $"{today.ToString("yyyy-MM")}{Path.DirectorySeparatorChar}{filePrefix}_log_{today.ToString("yyyy-MM-dd")}.csv");
            Assert.IsTrue(File.Exists(fileName), $"File doesn't exist {fileName}");
        }

        [TestMethod]
        public async Task LogService_LogErrorAsync()
        {
            var directoryPath = GetRandomDirectoryPath("LogError");
            var filePrefix = _file_Prefix + "_LogError";
            var logService = new LogService(filePrefix, directoryPath);
            Debug.WriteLine(directoryPath);
            var inner = new ArgumentOutOfRangeException("out of range mother fucker");
            logService.Log(new ArgumentNullException("log error test", inner), LogLevel.Error);
            await logService.FlushAsync();
            var today = DateTime.Now;
            Assert.IsTrue(File.Exists(Path.Combine(directoryPath, $"{today.ToString("yyyy-MM")}{Path.DirectorySeparatorChar}{filePrefix}_log_{today.ToString("yyyy-MM-dd")}.csv")));
            Assert.IsTrue(File.Exists(Path.Combine(directoryPath, $"{today.ToString("yyyy-MM")}{Path.DirectorySeparatorChar}{filePrefix}_error_{today.ToString("yyyy-MM-dd")}.csv")));
        }

        [TestMethod]
        public async Task LogService_Purge()
        {
            var directoryPath = GetRandomDirectoryPath("Purge");
            var filePrefix = _file_Prefix + "_Purge";

            uint dayTokeep = 30;
            var dayToTests = 5;

            var logService = new LogService(filePrefix, directoryPath, dayTokeep);
            var today = DateTime.Now;
            Debug.WriteLine(directoryPath);

            var pastMonth = Path.Combine(directoryPath, today.AddMonths(-1).ToString("yyyy-MM"));

            // On crée le mois d'avant
            Directory.CreateDirectory(pastMonth);

            logService.Log("Test");
            await logService.FlushAsync();

            Directory.Exists(Path.Combine(directoryPath, today.ToString("yyyy-MM")));

            var dateFormat = Path.Combine(directoryPath, $"[DATE_MONTH]{Path.DirectorySeparatorChar}{filePrefix}_log_[DATE_DAY].csv");

            for (int i = 0; i < dayTokeep + dayToTests; i++)
            {
                var currentDay = today.AddDays(-i);
                var path = dateFormat.Replace("[DATE_MONTH]", currentDay.ToString("yyyy-MM"));
                path = path.Replace("[DATE_DAY]", currentDay.ToString("yyyy-MM-dd"));

                if (!File.Exists(path))
                {
                    File.Create(path).Dispose();
                }
            }

            await logService.PurgeOldDaysAsync().ConfigureAwait(false);

            var minDate = today.AddDays(-1 * dayTokeep);

            // Test if files exist
            for (int i = 0; i < dayTokeep + dayToTests; i++)
            {
                var currentDay = today.AddDays(-i);
                var day = currentDay.ToString("yyyy-MM");
                var filePath = dateFormat.Replace("[DATE_MONTH]", currentDay.ToString("yyyy-MM"));
                filePath = filePath.Replace("[DATE_DAY]", currentDay.ToString("yyyy-MM-dd"));

                var fileExist = File.Exists(filePath);
                if (currentDay >= minDate)
                {
                    Assert.IsTrue(fileExist, $"The file would not have to be deleted : {filePath}");
                }
                else
                {
                    Assert.IsFalse(fileExist, $"The file should have been deleted : {filePath}");
                }
            }
        }
    }
}
