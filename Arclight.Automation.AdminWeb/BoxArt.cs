using System;
using System.Collections.Generic;
using Arclight.Automation.Selenium;
using Arclight.Automation.TestFlow;
using Gallio.Model;
using MbUnit.Framework;

namespace Arclight.Automation.AdminWeb
{
    /// <summary>
    /// Main test automation class that Adds,Approves and Denies a case into Insight portals.
    /// </summary>
    [TestFixture]
    public class BoxArt : CommonProperties
    {
        ///<summary>
        ///Test initialization procedure required to run.
        ///</summary>
        [FixtureSetUp]
        public void Init()
        {
            TestStopWatch.Start();
            const string testCaseSuite = "CreateBoxArt";
            Browser.TestCase = "Create a box art and check if it is displayed.";
            InitializeVariables(Browser.TestCase, testCaseSuite);
            OptionalVideoCapture(testCaseSuite); 
            ArclightInput.TestValues = new Dictionary<string, string>();
        }

        [Test]
        public void CreateNewBoxArt()
        {
            try
            {
                Logger.WriteCaseStart();
                ArclightTestFlow.LogUser();
                ArclightTestFlow.NavigateToCreateBoxArtForm();
                ArclightTestFlow.FillBoxArtForm();
                ArclightTestFlow.SubmitBoxArt();
                ArclightTestFlow.NavigateToBoxArtList();
                ArclightTestFlow.RetrieveLastCreatedBoxArtList();
                ArclightTestFlow.AssertThatCreatedBoxArtExistsOnPage();

            }
            catch (Exception e) // If something went wrong, log the error and end test.
            {
                Failed = true;
                var filePath = GenerateFileName("Arclight-AddNewBoxArt.png");
                Logger.FailMessage(Browser.BrowserType, Browser.TestCase, "The test ended with errors.", "", "",
                                    e.ToString().Replace("\n", "").Replace("\r", "").Replace(",", ""), "Error");
                Logger.TakeScreenshot(filePath);
                Assert.Terminate(TestOutcome.Failed);
            }
        }

        /// <summary>
        /// Cleans the test after it completely finishes or an error came up.
        /// </summary>
        [FixtureTearDown]
        public void Clean()
        {
            DisposeTest();
        }
    }
}