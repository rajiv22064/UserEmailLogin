using Microsoft.VisualStudio.TestTools.UnitTesting;
using RelevantCodes.ExtentReports;
using System;
using System.Configuration;
using System.Globalization;
using System.IO;

namespace UITestAutomation
{
    public static class Logger
    {
        public static string LogFolder { get; set; }
        public static string txtLogFileName;
        public static string HtmlLogFileName;
        public static ExtentReports GetExtentReportsInstance;
        private static ExtentTest GetExtentTestInstance;

        static Logger()
        {
            LogFolder = Directory.GetCurrentDirectory();
            string configDirectoryPath = ConfigurationManager.AppSettings["LogsFolderLocation"];
            if (!string.IsNullOrEmpty(configDirectoryPath))
            {
                LogFolder = Directory.Exists(configDirectoryPath) ? configDirectoryPath : Directory.CreateDirectory(configDirectoryPath).FullName;
            }
        }

        public static void LogInitialization(TestContext testContext, string testDescription)
        {
            string testName = "UserLogin";
            string newLogFolderNameName = string.Format("{0}-{1}-{2}", testName, DateTime.Now.ToString("MMM_dd", CultureInfo.CurrentCulture),
                 Guid.NewGuid().ToString().Substring(0, 4));
            LogFolder = Path.Combine(LogFolder, newLogFolderNameName);

            if (!Directory.Exists(LogFolder))
            {
                Directory.CreateDirectory(LogFolder);
            }

            txtLogFileName = Path.Combine(LogFolder, testName + "_TextLog.txt");
            HtmlLogFileName = Path.Combine(LogFolder, testName + "_HTMLLog.html");
            GetExtentReportsInstance = new ExtentReports(Path.Combine(LogFolder, testName + "_Extent" + ".html"));

        }

        public static void TestLogInitialize(string testName, string testDescription)
        {
            GetExtentTestInstance = GetExtentReportsInstance.StartTest(testName, testDescription);
            GetExtentTestInstance.AssignCategory();
        }

        public static void TestLogClosure(TestContext testContext = null)
        {
            if (testContext != null)
            {
                string testDetails = testContext.DataRow != null ? string.Join("\t", testContext.DataRow.ItemArray) : string.Empty;
                Log("Test execution Completed");
            }
            GetExtentReportsInstance.EndTest(GetExtentTestInstance);
            GetExtentReportsInstance.Flush();
        }

        private static void _Log(string logMessage, params object[] param)
        {
            if (param.Length == 0) param = new object[] { string.Empty };
            string message = DateTime.Now.ToString(CultureInfo.InvariantCulture) + " : " + string.Format(CultureInfo.CurrentCulture, logMessage, param);
            File.AppendAllLines(txtLogFileName, new[] { message });
        }

        public static void LogInfo(string logMessage, params object[] param)
        {
            _Log("Info : " + logMessage, param);
            GetExtentTestInstance.Log(LogStatus.Info, string.Format(CultureInfo.CurrentCulture, logMessage, param));
        }

        public static void Log(string logMessage, params object[] param)
        {
            _Log("Log : " + logMessage, param);
            GetExtentTestInstance.Log(LogStatus.Info, string.Format(CultureInfo.CurrentCulture, logMessage, param));
           
        }

        public static void LogWarning(string warningMessage, params object[] paramObjects)
        {
            _Log("Warning : {0}" + warningMessage, paramObjects);
            GetExtentTestInstance.Log(LogStatus.Warning, string.Format(CultureInfo.CurrentCulture, warningMessage, paramObjects));
        }

        public static void Fail(string warningMessage, params object[] paramObjects)
        {
            _Log("Fail : {0}" + warningMessage, paramObjects);
            GetExtentTestInstance.Log(LogStatus.Fail, string.Format(CultureInfo.CurrentCulture, warningMessage, paramObjects));
        }

        public static void Pass(string warningMessage, params object[] paramObjects)
        {
            _Log("Pass : {0}" + warningMessage, paramObjects);
            GetExtentTestInstance.Log(LogStatus.Pass, string.Format(CultureInfo.CurrentCulture, warningMessage, paramObjects));
        }

    }
}
