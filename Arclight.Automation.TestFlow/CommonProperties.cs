/*
 * -----------------------------------------------------------------------------------------------------------------------
 * Class which contains control variables to handle the flow of the test.
 * -----------------------------------------------------------------------------------------------------------------------
 * Code needed to initiate every test in the application with some common variables needed like reporting initiation,video recording and test disposing.
 * 
 * Developed with Selenium C# to locate elements on the page and MbUnit for assertions.
 * 
 */


using System;
using System.Diagnostics;
using System.Globalization;
using Arclight.Automation.Selenium;
using Microsoft.Expression.Encoder.ScreenCapture;

namespace Arclight.Automation.TestFlow
{
    public abstract class CommonProperties
    {
        // Variables used to control the functionalities and flow of the test.
        public readonly Stopwatch TestStopWatch = new Stopwatch();
        public ScreenCaptureJob Scj { get; set; }
        public bool IsVideoEnabled { get; set; }
        public static bool Failed { get; set; }


        /// <summary>
        /// Initializes browser and generates name of log file.
        /// </summary>
        public void InitializeVariables(string testCaseName, string testCaseFileName)
        {
            Browser.ClassInitialize(Browser.GetConfigValue("BROWSER_ARCLIGHT"));
            var fileName = GenerateFileName("-Test Report Summary.csv");
            Logger.WriteTcHeading(testCaseName, fileName);
        }

        /// <summary>
        /// Procedure that initiates video capture of the test.
        /// </summary>
        /// <param name="testCasename">Name of the test suite.</param>
        public void OptionalVideoCapture(string testCasename)
        {
            var strVideo = Browser.GetConfigValue("VIDEO").ToUpper();
            IsVideoEnabled = strVideo.ToUpper() == "YES";
            if (IsVideoEnabled)
            {
                Scj = new ScreenCaptureJob
                {
                    OutputScreenCaptureFileName = Browser.GetConfigValue("TEST_RESULTS_FOLDER_PATH") +
                                                  DateTime.Now.ToString("yyyy-dd-MM.hh.mm.ss") +
                                                  "-" + testCasename + ".wmv"
                };
                Scj.Start();
            }
        }

        // Cleaning resources needed for the test.
        public void DisposeTest()
        {
            // Closes browser.
            Browser.ClassCleanup();

            // Ends stopWatch and logs time taken by the test.
            TestStopWatch.Stop();
            Logger.EndTestCase("Time Taken : " + TestStopWatch.Elapsed.Hours.ToString(CultureInfo.InvariantCulture)
                                + " hrs: " + TestStopWatch.Elapsed.Minutes.ToString(CultureInfo.InvariantCulture) + " min: "
                                + TestStopWatch.Elapsed.Seconds.ToString(CultureInfo.InvariantCulture) + " sec: " +
                                TestStopWatch.Elapsed.Milliseconds.ToString(CultureInfo.InvariantCulture) + " milsec.");

            // Ends video recording of the test.
            if (IsVideoEnabled)
            {
                Scj.Stop();
            }

            // Cleaning variables used by the test.
            if (ArclightInput.TestValues != null)
            {
                ArclightInput.TestValues.Clear();
            }
            Failed = false;

            // Calling garbage collector.
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        /// <summary>
        /// Procedure that generates the filename of the test by appending the current date of the system.
        /// </summary>
        /// <param name="name">Name of the test.</param>
        /// <returns>Generated file name.</returns>
        public string GenerateFileName(string name)
        {
            var folderToSearch = Browser.GetConfigValue("TEST_RESULTS_FOLDER_PATH");
            folderToSearch = folderToSearch + DateTime.Now.ToString("yyyy-dd-MM") + name;
            return folderToSearch;
        }
    }
}
