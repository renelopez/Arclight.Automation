using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Gallio.Framework;
using OpenQA.Selenium;

namespace Arclight.Automation.Selenium
{
    /// <summary>
    /// Main class for logging results to CSV.
    /// </summary>
    public static class Logger
    {
        public static string ResultsFileName;
        private static string Date
        {
            get { return DateTime.Now.ToShortDateString(); }
        }

        private static string Time
        {
            get { return DateTime.Now.ToShortTimeString(); }
        }

        /// <summary>
        /// Method to write title to the CSV file
        /// </summary>
        /// <param name="overallTestCase">Test Case Name of the output to be logged</param>
        /// <param name="filename">Name that the results file that </param>
        public static void WriteTcHeading(string overallTestCase, string filename)
        {
            ResultsFileName = "";
            ResultsFileName = filename;
            using (var reportMsg = new StreamWriter(ResultsFileName, true))
            {
                if (new FileInfo(filename).Length == 0)
                {
                    reportMsg.WriteLine(
                        "Browser,Date,Time,Test Case,Test Case Step,Test Case Input" +
                        ",Test Case Output(If available),Pass/Fail,Error/Console Message(if available)");
                    reportMsg.WriteLine();
                    reportMsg.WriteLine(overallTestCase);
                }
                else
                {
                    reportMsg.WriteLine();
                    reportMsg.WriteLine();
                    reportMsg.WriteLine();
                    reportMsg.WriteLine(overallTestCase);
                }
                reportMsg.WriteLine();
            }
        }

        public static void WriteCaseStart()
        {
            using (var reportMsg = new StreamWriter(ResultsFileName, true))
            {
                reportMsg.WriteLine("{0},{1},{2}," + "Test Case Start", Browser.BrowserType, Date, Time);
            }

            using (TestLog.BeginSection("Log"))
            {
                TestLog.WriteLine("Test Execution Begins.");
            }
        }

        /// <summary>
        /// Method to write Pass Messages to output file.
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="testCase">Name of the test case.</param>
        /// <param name="testStep">Name of the test case step</param>
        /// <param name="testInput">Description of the input received by the test.</param>
        /// <param name="testOutput">Description of the output received by the test.</param>
        public static void PassMessage(string browser, string testCase, string testStep, string testInput, string testOutput)
        {
            using (var reportMsg = new StreamWriter(ResultsFileName, true))
            {
                reportMsg.WriteLine("{0},{1},{2},{3},{4},{5},{6}," + "Pass", browser, Date, Time, testCase, testStep, testInput, testOutput);
            }
            using (TestLog.BeginSection("Assertion Passed."))
            {
                TestLog.WriteLine(testOutput);
            }

        }

        /// <summary>
        /// Method to write Fail Messages to output file.
        /// </summary>
        /// <param name="browser">Browser where the test is currently running.</param>
        /// <param name="testCase">Name of the test case.</param>
        /// <param name="testStep">Name of the test case step.</param>
        /// <param name="testInput">Description of the input received by the test.</param>
        /// <param name="testOutput">Description of the output received by the test.</param>
        /// <param name="failMessage">Description of the fail message thrown by the test.</param>
        /// <param name="type">Type of log that will be in the Gallio Console</param>
        public static void FailMessage(string browser, string testCase, string testStep, string testInput, string testOutput, string failMessage, string type)
        {
            using (var reportMsg = new StreamWriter(ResultsFileName, true))
            {
                reportMsg.WriteLine("{0},{1},{2},{3},{4},{5},{6}," + "Fail:" + ",{7}", browser, Date, Time, testCase, testStep, testInput, testOutput, failMessage);
            }
            switch (type)
            {
                case "Assertion":
                    using (TestLog.Warnings.BeginSection("Assertion failed."))
                    {
                        TestLog.Warnings.WriteLine(failMessage);
                    } break;
                case "Error":
                    using (TestLog.Failures.BeginSection("Execution failed."))
                    {
                        TestLog.Failures.WriteLine(failMessage);
                    } break;
            }
        }

        /// <summary>
        /// Method to write Fail Messages to output file.
        /// </summary>
        /// <param name="browser">Browser where the test is currently running.</param>
        /// <param name="testCase">Name of the test case.</param>
        /// <param name="testStep">Name of the test case step.</param>
        /// <param name="consoleMessage">Description of the console message to display</param>
        public static void ConsoleMessage(string browser, string testCase, string testStep, string consoleMessage)
        {
            using (var reportMsg = new StreamWriter(ResultsFileName, true))
            {
                reportMsg.WriteLine("{0},{1},{2},{3},{4},Console Log,{5}", browser, Date, Time, testCase, testStep, consoleMessage);
            }
            using (TestLog.BeginSection("Log"))
            {
                TestLog.WriteLine(consoleMessage);
            }

        }

        /// <summary>
        /// Method to write test footer and test time completion.
        /// </summary>
        /// <param name="timeElapsed">Time taked by the test.</param>
        /// <param name="failed">Bool to identify if test failed or not.</param>
        public static void EndTestCase(string timeElapsed, bool failed = false)
        {
            using (var reportMsg = new StreamWriter(ResultsFileName, true))
            {
                if (failed)
                {
                    reportMsg.WriteLine("Test Case could not continue." + timeElapsed);
                }
                else
                {
                    reportMsg.WriteLine("Test Case completed." + timeElapsed);
                }
            }

            using (TestLog.BeginSection("Log"))
            {
                TestLog.WriteLine("Test execution completed.");
            }
        }

        public static void TakeScreenshot(string path)
        {
            var screenCapture = ((ITakesScreenshot)Browser.Driver).GetScreenshot();
            screenCapture.SaveAsFile(path, System.Drawing.Imaging.ImageFormat.Png);
            var bitmap = Capture.Screenshot();
            using (TestLog.BeginSection("Screenshot and Error Messages"))
            {
                TestLog.EmbedImage("Screenshot", bitmap);
            }
        }


    }
}
