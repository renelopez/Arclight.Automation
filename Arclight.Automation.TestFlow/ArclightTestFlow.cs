using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arclight.Automation.PageObjects;
using Arclight.Automation.Selenium;
using MbUnit.Framework;

namespace Arclight.Automation.TestFlow
{
    public class ArclightTestFlow
    {
        public static void LogUser()
        {
            try
            {
                ArclightPages.LoginPage.GoToLoginPage();
                ArclightPages.LoginPage.Login(ArclightInput.ArclightUsername, ArclightInput.ArclightPassword);
                Logger.ConsoleMessage(Browser.BrowserType, Browser.TestCase,"Login into Arclight Portal.",
                    "Login into arclight with credentials: Username:" + ArclightInput.ArclightUsername + " Password:" + ArclightInput.ArclightPassword);
            }
            catch (Exception)
            {
                // Logs exception and throws exception if login failed so test can be ended.
                Logger.FailMessage(Browser.BrowserType, Browser.TestCase, "Error while login into Arclight portal with credentials: "
                                                                        + "Username:" + ArclightInput.ArclightUsername + " Password:" +
                                                                        ArclightInput.ArclightPassword, "", "", "", "Error");
                throw;
            }
        }

        /// <summary>
        /// Navigates to create box art type page.
        /// </summary>
        public static void NavigateToCreateBoxArtForm()
        {
            try
            {
                Logger.ConsoleMessage(Browser.BrowserType, Browser.TestCase,"Navigate to Create Box Art Form.",
                                       "");
                ArclightPages.DashboardPage.GoToCreateBoxArtPage();
            }
            catch (Exception)
            {
                // Logs error and throws exception something failed in this procedure.
                Logger.FailMessage(Browser.BrowserType, Browser.TestCase, "Error while navigating to Create Box Art Form.",
                                    "", "", "", "Error");
                throw;
            }
        }

        /// <summary>
        /// Navigates to create box art type page.
        /// </summary>
        public static void FillBoxArtForm()
        {
            try
            {
                Logger.ConsoleMessage(Browser.BrowserType, Browser.TestCase, "Starting to fill the Box Art Form.",
                                       "");
                ArclightPages.CreateBoxArtPage.GoToSelectMediaMetadataPopup();
                ArclightPages.CreateBoxArtPage.SelectMyLastDayTitle();
                ArclightPages.CreateBoxArtPage.GoToSelectBoxArtTypePopup();
                ArclightPages.CreateBoxArtPage.SelectSmallTypeBoxArt();
                ArclightPages.CreateBoxArtPage.WriteFileName(ArclightInput.Filename);
                ArclightPages.CreateBoxArtPage.WriteOvp(ArclightInput.OVP);
            }
            catch (Exception)
            {
                // Logs error and throws exception something failed in this procedure.
                Logger.FailMessage(Browser.BrowserType, Browser.TestCase, "Error while filling the Box Art Form",
                                    "", "", "", "Error");
                throw;
            }
        }

        /// <summary>
        /// Navigates to create box art type page.
        /// </summary>
        public static void SubmitBoxArt()
        {
            try
            {
                Logger.ConsoleMessage(Browser.BrowserType, Browser.TestCase, "Submitting Box Art","");
                ArclightPages.CreateBoxArtPage.SubmitBoxArt();
            }
            catch (Exception)
            {
                // Logs error and throws exception something failed in this procedure.
                Logger.FailMessage(Browser.BrowserType, Browser.TestCase, "Error while submitting the Box Art Form",
                                    "", "", "", "Error");
                throw;
            }
        }

        /// <summary>
        /// Navigates to create box art type page.
        /// </summary>
        public static void NavigateToBoxArtList()
        {
            try
            {
                Logger.ConsoleMessage(Browser.BrowserType, Browser.TestCase, "Navigate to box art list page.","");
                ArclightPages.DashboardPage.GoToCreateBoxArtPage();
            }
            catch (Exception)
            {
                // Logs error and throws exception something failed in this procedure.
                Logger.FailMessage(Browser.BrowserType, Browser.TestCase, "Error while navigating to box art list page.",
                                    "", "", "", "Error");
                throw;
            }
        }

        /// <summary>
        /// Navigates to create box art type page.
        /// </summary>
        public static void RetrieveLastCreatedBoxArtList()
        {
            try
            {
                Logger.ConsoleMessage(Browser.BrowserType, Browser.TestCase, "Retrieving last box art elements created", "");
                ArclightPages.BoxArtListPage.GoToLastResultsPage(ArclightInput.Filename);
            }
            catch (Exception)
            {
                // Logs error and throws exception something failed in this procedure.
                Logger.FailMessage(Browser.BrowserType, Browser.TestCase, "Error while retrieving last elements created.",
                                    "", "", "", "Error");
                throw;
            }
        }

        public static void AssertThatCreatedBoxArtExistsOnPage()
        {
            try
            {
                Assert.IsTrue(ArclightPages.BoxArtListPage.IsCreatedBoxArtDisplayed(ArclightInput.Filename));
            }
            catch (Exception)
            {
                // Logs error and throws exception something failed in this procedure.
                Logger.FailMessage(Browser.BrowserType, Browser.TestCase, "Box art wasn't created",
                                    "", "", "", "Error");
                throw;
            }
        }





        
    }
}
