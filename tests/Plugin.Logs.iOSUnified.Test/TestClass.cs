using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Plugin.Logs.Model;

namespace Plugin.Logs.iOSUnified.Test
{
    [TestFixture]
    public class LogServiceTest
    {
        private readonly string FILE_PREFIX = "Test";

        string GetRandomDirectoryPath(string fileName)
        {
            var path = Path.Combine(Path.GetTempPath(), fileName);
            Directory.CreateDirectory(path);

            return path;
        }


        [Test]
        public async Task Logs_Service_LogInfoAsync()
        {
			var directoryPath = GetRandomDirectoryPath("Infos");

			var filePrefix = FILE_PREFIX + "LogInfo";
            using (var logService = new LogService(filePrefix, directoryPath))
            {
                Debug.WriteLine(directoryPath);
                logService.Log("log information test", LogLevel.Information);

                await logService.FlushAsync();
                var today = DateTime.Now;

                var fileName = Path.Combine(directoryPath, $"{today.ToString("yyyy-MM")}{Path.DirectorySeparatorChar}{filePrefix}_log_{today.ToString("yyyy-MM-dd")}.csv");
				Assert.IsTrue(File.Exists(fileName), $"File doesn't exist {fileName}");
            }
        }

		
        [Test]
        public async Task LogService_FlushAsync()
        {
            var directoryPath = GetRandomDirectoryPath("Flush");
            var filePrefix = FILE_PREFIX + "_flush";
            using (var logService = new LogService(filePrefix, directoryPath))
            {
                Debug.WriteLine(directoryPath);

                logService.Log("log information test", LogLevel.Information);
                await logService.FlushAsync();
                var today = DateTime.Today;
                var fileName = Path.Combine(directoryPath, Path.Combine($"{today.ToString("yyyy-MM")}",$"{filePrefix}_log_{today.ToString("yyyy-MM-dd")}.csv"));
                Assert.IsTrue(File.Exists(fileName), $"File doesn't exist {fileName}");
            }
        }

        [Test]
		public async Task LogService_WriteBigString()
		{
			var directoryPath = GetRandomDirectoryPath("BigString");
			var filePrefix = FILE_PREFIX + "_bigString";
            using (var logService = new LogService(filePrefix, directoryPath))
			{
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
		}

        [Test]
        public async Task LogService_LogErrorAsync()
        {
            var directoryPath = GetRandomDirectoryPath("LogError");
            var filePrefix = FILE_PREFIX + "_LogError";
            using (var logService = new LogService(filePrefix, directoryPath))
            {
                Debug.WriteLine(directoryPath);
                var inner = new ArgumentOutOfRangeException("out of range mother fucker");
                logService.Log(new ArgumentNullException("log error test", inner), LogLevel.Error);
				await logService.FlushAsync();
				var today = DateTime.Now;
                        Assert.IsTrue(File.Exists(Path.Combine(directoryPath, $"{today.ToString("yyyy-MM")}{Path.DirectorySeparatorChar}{filePrefix}_log_{today.ToString("yyyy-MM-dd")}.csv")));
                        Assert.IsTrue(File.Exists(Path.Combine(directoryPath, $"{today.ToString("yyyy-MM")}{Path.DirectorySeparatorChar}{filePrefix}_error_{today.ToString("yyyy-MM-dd")}.csv")));
            }
        }

        [Test]
        public async Task LogService_Purge()
        {
            var directoryPath = GetRandomDirectoryPath("Purge");
            var filePrefix = FILE_PREFIX + "_Purge";

            uint dayTokeep= 30;
            var dayToTests = 5;

            using (var logService = new LogService(filePrefix, directoryPath, dayTokeep))
            {
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
                        Assert.IsTrue(fileExist, $"Le fichier n'aurait pas du être supprimé : {filePath}");
                    }
                    else
                    {
                        Assert.IsFalse(fileExist, $"Le fichier aurait du être supprimé : {filePath}");
                    }
                }
            }
        }
	}
}
