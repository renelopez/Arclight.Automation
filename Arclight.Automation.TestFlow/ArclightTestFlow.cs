using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arclight.Automation.PageObjects;
using Arclight.Automation.Selenium;

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
                                       "Navigating to edit procedure properties.");
                ArclightPages.DashboardPage.GoToCreateBoxArtPage();
            }
            catch (Exception)
            {
                // Logs error and throws exception if login failed so test can be ended.
                Logger.FailMessage(Browser.BrowserType, Browser.TestCase, "Error while navigating to Create Box Art Form.",
                                    "", "", "", "Error");
                throw;
            }
        }
    }
}
