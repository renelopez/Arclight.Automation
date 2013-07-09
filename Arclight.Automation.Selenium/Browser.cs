/*
 * -----------------------------------------------------------------------------------------------------------------------
 * Base Class that includes common functionality to search elements on the page.
 * -----------------------------------------------------------------------------------------------------------------------
 * 
 * This class lets you interact with all of the components of a web page with the help of Selenium.
 * Most of its functionality includes locating elements on the page by its id,CSS classname,and XPath queries.
 * 
 * Developed with Selenium C# to locate elements on the page and MbUnit for assertions.
 * 
 */


using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace Arclight.Automation.Selenium
{
    public static class Browser
    {
        // Variables that hold browser and default base urls.

        // Implicit Wait Times designed to wait for elements to appear on the page.
        public static string TestCase { get; set; }
        public const int MIN_WAIT = 20;
        public const int MID_WAIT = 40;
        public const int MAX_WAIT = 60;
        public static string BrowserType { get; set; }
        public static Random random = new Random();
        public static IJavaScriptExecutor ScriptExecutor
        {
            get
            {
                var exec = (IJavaScriptExecutor)Driver;
                return exec;
            }
        }


        // The instance of the explorer.
        public static IWebDriver Driver { get; private set; }

        /// <summary>
        /// Method that initializes the browser to start the automation.
        /// </summary>
        /// <param name="browser">Name of the browser in which the test will run.</param>
        public static void ClassInitialize(string browser)
        {
            Driver = CreateDriverInstance(browser);
            BrowserType = browser;
            Driver.Manage().Window.Maximize();
        }

        /// <summary>
        /// Method used to dispose the test.
        /// </summary>
        public static void ClassCleanup()
        {
            try
            {
                Driver.Quit();
            }
            catch (Exception e)
            {
                Logger.FailMessage(BrowserType, TestCase, "Error while trying to close the browser", e.ToString(),
                                    "", "", "Error");
                // Ignore errors if unable to close the browser
            }
        }

        /// <summary>
        /// Method used to create an instance depending the browser choosen for the test.
        /// </summary>
        /// <param name="browser">Name of the browser to create an instance of it.</param>
        /// <returns>Instance of a Driver of selected explorer.</returns>
        private static IWebDriver CreateDriverInstance(string browser)
        {
            switch (browser)
            {
                case "FIREFOX":
                    var firefoxCapabilities = DesiredCapabilities.Firefox();
                    return new FirefoxDriver(firefoxCapabilities);
                case "IE":
                    var ieOptions = new InternetExplorerOptions
                        {
                            IntroduceInstabilityByIgnoringProtectedModeSettings = true
                        };
                    return new InternetExplorerDriver(ieOptions);
                case "CHROME":
                    return new ChromeDriver();
                default:
                    return new FirefoxDriver();
            }
        }

        /// <summary>
        /// Opens a web page with the address indicated.
        /// </summary>
        /// <param name="url">URL address to open on the browser.</param>
        public static void Open(string url)
        {
            Driver.Navigate().GoToUrl(url.Trim('~'));
        }

        /// <summary>
        /// Sends a mouse click to an html component on the page.
        /// </summary>
        /// <param name="locator">The locator object used for search an element into the page.</param>
        public static void Click(By locator)
        {
            try
            {
                Wait(locator, 5);
                var element = Driver.FindElement(locator);
                Thread.Sleep(200);
                element.Click();
            }
            catch (Exception)
            {
                Logger.FailMessage(BrowserType, TestCase, "Error while clicking the following element:" + locator,
                                    "", "", "", "Error");
                throw;
            }
        }

        /// <summary>
        /// Clicks to a page and waits for a URL to be displayed on the browser.
        /// </summary>
        /// <param name="locator">The locator object used for search an element into the page.</param>
        /// <param name="newUrl">The Url needed to wait for.</param>
        public static void ClickAndWait(By locator, string newUrl)
        {
            Wait(locator, 5);
            var element = Driver.FindElement(locator);
            Thread.Sleep(200);
            element.Click();
            var wait = new WebDriverWait(Driver,
                                         TimeSpan.FromSeconds(MID_WAIT));
            wait.Until(d => d.Url.Contains(newUrl.Trim('~')));
        }

        /// <summary>
        /// Method that looks for an element on a combobox control.
        /// </summary>
        /// <param name="id">Id of the combobox control</param>
        /// <param name="valueToBeSelected">Value of the combobox to be selected.</param>
        public static void Select(string id, string valueToBeSelected)
        {
            try
            {
                Wait(By.Id(id), 10);
                var select = new SelectElement(Driver.FindElement(By.Id(id)));
                Thread.Sleep(200);
                var option = @select.Options.First(opt => opt.Text.Equals(valueToBeSelected));
                option.Click();
                PauseExecution();
            }
            catch (Exception)
            {
                Logger.FailMessage(BrowserType, TestCase,
                                    "Error while selecting element from select." + "Control Id:" + id +
                                    ". Value to be selected:" + valueToBeSelected + ".", "", "", "", "Error");
                throw;
            }
        }

        public static void Select(IWebElement element, string valueToBeSelected)
        {
            try
            {
                var select = new SelectElement(element);
                Thread.Sleep(200);
                var option = @select.Options.First(opt => opt.Text.Equals(valueToBeSelected));
                option.Click();
                PauseExecution();
            }
            catch (Exception)
            {
                Logger.FailMessage(BrowserType, TestCase,
                                    "Error while selecting element from select." + "Control Id:" + element +
                                    ". Value to be selected:" + valueToBeSelected + ".", "", "", "", "Error");
                throw;
            }
        }

        /// <summary>
        /// Method that looks for an element on a combobox control via Jquery.
        /// </summary>
        /// <param name="id">Id of the combobox control</param>
        /// <param name="valueToBeSelected">Value of the combobox to be selected.</param>
        public static void SelectByJquery(string id, string valueToBeSelected)
        {
            try
            {
                var js = (IJavaScriptExecutor)Driver;
                js.ExecuteScript("$('#" + id + " option:contains(" + valueToBeSelected + ")').attr('selected', 'selected')");
                js.ExecuteScript("$('#" + id + "').change()");
                PauseExecution();
            }
            catch (Exception)
            {
                Logger.FailMessage(BrowserType, TestCase,
                                    "Error while selecting element from select." + "Control Id:" + id +
                                    ". Value to be selected:" + valueToBeSelected + ".", "", "", "", "Error");
                throw;
            }
        }

        /// <summary>
        /// Finds an element of type IWebElement by its id.
        /// </summary>
        /// <param name="id">ID of the element to look on the page.</param>
        /// <returns>An IWebElement object.</returns>
        public static IWebElement GetElement(string id)
        {
            try
            {
                Wait(By.Id(id), 10);
                var element = Driver.FindElement(By.Id(id));
                Thread.Sleep(200);
                return element;
            }
            catch (Exception)
            {
                Logger.FailMessage(BrowserType, TestCase,
                                    "Error while finding element." + " Control ID:" + id + ".",
                                    "", "", "", "Error");
                throw;
            }
        }

        /// <summary>
        /// Finds an element of type IWebElement by its id by not throwing an exception to the main function of the test.
        /// </summary>
        /// <param name="locator"></param>
        /// <returns>An IWebElement object.</returns>
        public static IWebElement GetElementWithoutThrowException(By locator)
        {
            try
            {
                var element = Driver.FindElement(locator);
                Thread.Sleep(200);
                return element;
            }
            catch (Exception)
            {
                return null;
            }
        }



        /// <summary>
        /// Finds an element of type IWebElement by a locator object.
        /// </summary>
        /// <param name="locator">Object of type "By" used for look an element on the page.</param>
        /// <returns>An IWebElement object.</returns>
        public static IWebElement GetElement(By locator)
        {
            try
            {
                Wait(locator, 10);
                var element = Driver.FindElement(locator);
                Thread.Sleep(200);
                return element;
            }
            catch (Exception)
            {
                Logger.FailMessage(BrowserType, TestCase, "Error while obtaining following element:" + locator, "",
                                    "", "", "Error");
                throw;
            }
        }

        public static ReadOnlyCollection<IWebElement> GetElements(By locator)
        {
            try
            {
                Wait(locator, 10);
                var element = Driver.FindElements(locator);
                Thread.Sleep(200);
                return element;
            }
            catch (Exception)
            {
                Logger.FailMessage(BrowserType, TestCase, "Error while obtaining following elements:" + locator, "",
                                    "", "", "Error");
                throw;
            }
        }

        /// <summary>
        /// Obtains the text that is embedded on an html element
        /// </summary>
        /// <param name="locator">Object of type "By" used for look an element on the page.</param>
        /// <returns>Text contained in the web element.</returns>
        public static string GetText(By locator)
        {
            try
            {
                Wait(locator, 10);
                var element = Driver.FindElement(locator);
                Thread.Sleep(200);
                return element.Text;
            }
            catch (Exception)
            {
                Logger.FailMessage(BrowserType, TestCase, "Error while locating text from element:" + locator, "",
                                    "", "", "Error");
                throw;
            }
        }

        /// <summary>
        /// Method that returns a boolean if given text is contained in one of the attributes of the html element.
        /// </summary>
        /// <param name="id">Id of the element</param>
        /// <param name="attributeName">Name of the attribute to be located</param>
        /// <param name="substringValue">Name of the string value of the attributte to look.</param>
        /// <returns>Boolean indicating that the attribute is contained on html control.</returns>
        public static bool IsTextContainedInAttribute(string id, string attributeName, string substringValue)
        {
            try
            {
                var strAttribute = GetElement(id).GetAttribute(attributeName);
                return strAttribute.Contains(substringValue);
            }
            catch (Exception)
            {
                Logger.FailMessage(BrowserType, TestCase,
                                    "Error while obtaining attribute from a control. Control ID:" + id +
                                    " AttributeName:" + attributeName + " Substring Value:" + substringValue, "", "", "", "Error");
                return false;
            }
        }

        /// <summary>
        /// Method that returns a boolean if given text is contained in one of the attributes of the html element.
        /// </summary>
        /// <param name="locator">Id of the element</param>
        /// <param name="attribute">Name of the attribute to be located</param>
        /// <param name="text">Name of the string value of the attributte to look.</param>
        /// <returns>Boolean indicating that the attribute is contained on html control.</returns>
        public static bool IsTextContainedInAttribute(By locator, string attribute, string text)
        {
            try
            {
                var strAttribute = GetElement(locator).GetAttribute(attribute);
                return strAttribute.Contains(text);
            }
            catch (Exception)
            {
                Logger.FailMessage(BrowserType, TestCase,
                                    "Error while obtaining attribute fromcontrol:" + locator + " AttributeName:" +
                                    attribute + " Substring Value:" + text, "", "", "", "Error");
                return false;
            }
        }

        /// <summary>
        /// Write text into a textbox control of the web page.
        /// </summary>
        /// <param name="id">ID of the textbox control to write.</param>
        /// <param name="text">Text to write on the control.</param>
        public static void Type(string id, string text)
        {
            try
            {
                var element = GetElement(id);
                element.Clear();
                Thread.Sleep(200);
                element.SendKeys(text);
            }
            catch (Exception)
            {
                Logger.FailMessage(BrowserType, TestCase,
                                    "Error while typing on control with following ID:" + " Control ID:" + id +
                                    ". Text to type:" + text, "", "", "", "Error");
                throw;
            }
        }

        /// <summary>
        /// Write text into a textbox control of the web page.
        /// </summary>
        /// <param name="locator">Object of type "By" used for look an element on the page.</param>
        /// <param name="text">Text to write on the control.</param>
        public static void Type(By locator, string text)
        {
            try
            {
                var element = GetElement(locator);
                element.Clear();
                Thread.Sleep(200);
                element.SendKeys(text);
            }
            catch (Exception)
            {
                Logger.FailMessage(BrowserType, TestCase,
                                    "Error while typing on control. Locator:" + locator + ". Text to type:" + text, "",
                                    "", "", "Error");
                throw;
            }
        }

        /// <summary>
        /// Write text into a textbox control of the web page without clearing the textbox first.
        /// </summary>
        /// <param name="locator">Object of type "By" used for look an element on the page.</param>
        /// <param name="text">Text to write on the control.</param>
        public static void TypeWithoutClear(By locator, string text)
        {
            try
            {
                Wait(locator, 10);
                var element = GetElement(locator);
                Thread.Sleep(200);
                element.SendKeys(text);
            }
            catch (Exception)
            {
                Logger.FailMessage(BrowserType, TestCase,
                                    "Error while typing on control. Locator:" + locator + ". Text to type:" + text, "",
                                    "", "", "Error");
                throw;
            }
        }

        /// <summary>
        /// Write text into a textbox control of the web page without clearing the textbox first.
        /// </summary>
        /// <param name="id">ID of the textbox control to write.</param>
        /// <param name="text">Text to write on the control.</param>
        public static void TypeWithoutClear(string id, string text)
        {
            try
            {
                var element = GetElement(By.Id(id));
                element.SendKeys(text);
            }
            catch (Exception)
            {
                Logger.FailMessage(BrowserType, TestCase,
                                    "Error while typing on control. ID:" + id + ". Text to type:" + text, "", "", "", "Error");
                throw;
            }
        }

        /// <summary>
        /// Method that determines if given text is present on the html page.
        /// </summary>
        /// <param name="value">Text to be searched on the page.</param>
        /// <returns>Returns true if text is displayed on the page.</returns>
        public static bool VerifyTextPresent(string value)
        {
            try
            {
                var temp = Driver.PageSource.Contains(value);
                return temp;
            }
            catch (Exception)
            {
                Logger.FailMessage(BrowserType, TestCase,
                                    "Error while finding text from the page.Value to find:" + value, "", "", "", "Error");
                throw;
            }
        }

        /// <summary>
        /// Gets the text element of the given key on the application configuration file. 
        /// </summary>
        /// <param name="key">Key to be searched on the config file.</param>
        /// <returns>Value of the configuration file.</returns>
        public static string GetConfigValue(string key)
        {
            try
            {
                return ConfigurationManager.AppSettings[key];
            }
            catch (Exception)
            {
                Logger.FailMessage(BrowserType, TestCase,
                                    "Error while retrieving following value from config file:" + key, "", "", "", "Error");
                throw;
            }
        }

        /// <summary>
        /// Time wait for an element to be rendered.
        /// </summary>
        /// <param name="locator">Object of type "By" used for look an element on the page.</param>
        /// <param name="time">Time to wait for the control to be present on the html page.</param>
        public static void Wait(By locator, int time)
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(time));
                wait.Until(x => x.FindElement(locator));
            }
            catch (Exception)
            {
                Logger.FailMessage(BrowserType, TestCase,
                                    "Error while waiting for an element. Locator:" + locator + " Time to wait:" + time,
                                    "", "", "", "Error");
                throw;
            }
        }

        /// <summary>
        /// Time wait for an element to be visible for selenium.
        /// </summary>
        /// <param name="locator">Object of type "By" used for look an element on the page.</param>
        /// <param name="time">Time to wait for the control to be visible on the html page.</param>
        public static void WaitUntilVisible(By locator, int time)
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(time));
                //wait.Until(ExpectedConditions.ElementIsVisible(locator));
                wait.Until(x => x.FindElement(locator).Displayed);
            }
            catch (Exception)
            {
                Logger.FailMessage(BrowserType, TestCase,
                                    "Error while waiting for an element. Locator:" + locator + " Time to wait:" + time,
                                    "", "", "", "Error");
                throw;
            }
        }

        /// <summary>
        /// Time wait for an element to be rendered.
        /// </summary>
        /// <param name="text">Object of type "By" used for look an element on the page.</param>
        /// <param name="time">Time to wait for the control to be present on the html page.</param>
        public static void WaitForTextPresent(string text, int time)
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(time));
                wait.Until(x => x.PageSource.Contains(text));
            }
            catch (Exception)
            {
                Logger.FailMessage(BrowserType, TestCase,
                                    "Error while waiting text to be present:" + text + " Time to wait:" + time, "", "",
                                    "", "Error");
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="time"></param>
        public static void WaitForEnabled(By locator, int time)
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(time));
                wait.Until(x => x.FindElement(locator).Enabled);
            }
            catch (Exception)
            {
                Logger.FailMessage(BrowserType, TestCase,
                                    "Error while waiting element to be enabled:" + locator + " Time to wait:" + time, "",
                                    "", "", "Error");
                throw;
            }
        }

        public static void WaitForDisabled(By locator, int time)
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(time));
                wait.Until(x => x.FindElement(locator).Enabled == false);
            }
            catch (Exception)
            {
                Logger.FailMessage(BrowserType, TestCase,
                                    "Error while waiting element to be disabled:" + locator + " Time to wait:" + time,
                                    "",
                                    "", "", "Error");
                throw;
            }
        }

        public static void WaitForAttributeUpdate(By locator, int time, string attribute, string newValue)
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(time));
                wait.Until(x => x.FindElement(locator).GetAttribute(attribute).Contains(newValue));
            }
            catch (Exception)
            {
                Logger.FailMessage(BrowserType, TestCase,
                                    " Error while waiting for attribute to change" + locator + " Time to wait:" + time +
                                    " Attribute:" + attribute + " Value:" + newValue, "",
                                    "", "", "Error");
                throw;
            }
        }

        public static void WaitForUrl(string value, int time)
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(time));
                wait.Until(x => x.Url.Contains(value));
            }
            catch (Exception)
            {
                Logger.FailMessage(BrowserType, TestCase,
                                    " Error while waiting URL to change" + value + " Time to wait:" + time, "",
                                    "", "", "Error");
                throw;
            }
        }


        /// <summary>
        /// Method to determine that waits until the page is completely loaded.
        /// </summary>
        public static void WaitPageForLoad()
        {
            try
            {
                IWait<IWebDriver> wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(30.00));

                wait.Until(
                    driver1 =>
                    ((IJavaScriptExecutor)driver1).ExecuteScript("return document.readyState").Equals("complete"));
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Suspends the execution of the application to the default time of three seconds.
        /// </summary>
        public static void PauseExecution()
        {
            Thread.Sleep(3000);
        }

        /// <summary>
        /// Suspends the execution of the application to given time in miliseconds.
        /// </summary>
        /// <param name="timeToWait">Time in miliseconds to put application in suspension.</param>
        public static void PauseExecution(int timeToWait)
        {
            Thread.Sleep(timeToWait);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeoutInSeconds"></param>
        /// <returns></returns>
        public static bool WaitForAjax(int timeoutInSeconds)
        {
            try
            {
                var resp = false;
                var iterations = 0;
                var threadSleepInterval = 100; //ms
                var maxWaitTime = timeoutInSeconds;
                var maxIterations = (maxWaitTime * 1000) / threadSleepInterval;

                while (true) // Handle timeout somewhere
                {
                    iterations++;

                    var ajaxIsComplete =
                        (bool)((IJavaScriptExecutor)Driver).ExecuteScript("return jQuery.active == 0");
                    if (ajaxIsComplete)
                    {
                        resp = true;
                        break;
                    }
                    Thread.Sleep(threadSleepInterval);
                    if (iterations >= maxIterations)
                    {
                        break;
                    }
                }
                return resp;
            }
            catch (Exception)
            {
                Logger.FailMessage(BrowserType, TestCase,
                                    "Error while waiting for Jquery on this time interval:" + timeoutInSeconds, "", "",
                                    "", "Error");
                throw;
            }
        }

        public static void WaitForSUGAR(int timeOut)
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeOut));
                wait.Until(
                    d =>
                    (bool)((IJavaScriptExecutor)d).ExecuteScript("return SUGAR.util.ajaxCallInProgress() == false"));
            }
            catch (Exception)
            {
                Logger.FailMessage(BrowserType, TestCase,
                                    "Error while waiting javascript to process.Time to wait:" + timeOut + "", "",
                                    "", "", "Error");
                throw;
            }
        }

        public static void WaitForUploaded(By locator, int timeOut)
        {
            try
            {
                for (var i = 1; i <= 5; i++)
                {
                    try
                    {
                        var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeOut));
                        wait.Until(
                            d =>
                                //(bool)((IJavaScriptExecutor)d).ExecuteScript("return uploadInProgress == false"));
                            GetElement(locator).Displayed);
                    }
                    catch (Exception) { Logger.ConsoleMessage(BrowserType, TestCase, "", "Exception expected,continuing on the execution"); }
                }
            }
            catch (Exception)
            {
                Logger.FailMessage(BrowserType, TestCase,
                                    "Error while waiting javascript to process.Time to wait:" + timeOut + "", "",
                                    "", "", "Error");
                throw;
            }
        }

        public static void WaitForClear(int timeOut)
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeOut));
                wait.Until(
                    d =>
                    (bool)((IJavaScriptExecutor)d).ExecuteScript("return SUGAR.savedViews.clearColumns == true"));
            }
            catch (Exception)
            {
                Logger.FailMessage(BrowserType, TestCase,
                                    "Error while waiting javascript to process.Time to wait:" + timeOut + "", "",
                                    "", "", "Error");
                throw;
            }
        }



        /// <summary>
        /// Function that returns an IAlert element if a popup alert is displayed on the page.
        /// </summary>
        /// <param name="driver">The current driver instance that is currently working.</param>
        /// <returns></returns>
        private static IAlert AlertIsPresent(IWebDriver driver)
        {
            try
            {
                // Attempt to switch to an alert
                return driver.SwitchTo().Alert();
            }
            catch (Exception)
            {
                // We ignore this exception, as it means there is no alert present...yet.
                return null;
            }
            // Other exceptions will be ignored and up the stack
        }

        /// <summary>
        /// Method that waits until a method returns and instance of IAlert or a null if the popup window is not present. 
        /// </summary>
        /// <param name="timeOut">The time limit for the popup to be displayed on the page.</param>
        /// <returns></returns>
        public static IAlert WaitForAlert(int timeOut)
        {
            try
            {
                IWait<IWebDriver> wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeOut));

                var alert = wait.Until(AlertIsPresent);
                return alert;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Function that gives control to the popup window displayed to the page.
        /// </summary>
        /// <returns>String containing the ID of the parent control.</returns>
        public static string SwitchToPopup()
        {
            Thread.Sleep(2000);
            var parentWindowId = Driver.CurrentWindowHandle;
            var allWindows = Driver.WindowHandles;
            if (allWindows != null)
            {
                foreach (var windowId in allWindows)
                {
                    try
                    {
                        Thread.Sleep(1000);
                        if (Driver.SwitchTo().Window(windowId).Title.Equals("Insight"))
                        {
                            return parentWindowId;
                        }
                    }
                    catch (NoSuchWindowException e)
                    {
                        Logger.FailMessage(BrowserType, TestCase,
                                            "Error while switching the control to window named: Insight.", "", "", "", "Error");
                        Console.WriteLine(e.ToString());
                        throw;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Scroll down the window 200 pixels down.
        /// </summary>
        public static void ScrollDown(int pixels = 200)
        {
            try
            {
                var executor = (IJavaScriptExecutor)Driver;
                executor.ExecuteScript("window.scrollBy(0," + pixels + ")", "");
                Thread.Sleep(3000);
            }
            catch (Exception)
            {
                Logger.FailMessage(BrowserType, TestCase, "Error while scrolling down the browser.", "", "", "", "Error");
                throw;
            }
        }

        /// <summary>
        /// Scroll all the page to bottom.
        /// </summary>
        public static void ScrollBottom()
        {
            try
            {
                var executor = (IJavaScriptExecutor)Driver;
                executor.ExecuteScript("window.scrollTo(0, document.body.offsetHeight);");
                Thread.Sleep(3000);
            }
            catch (Exception)
            {
                Logger.FailMessage(BrowserType, TestCase, "Error while scrolling down the browser.", "", "", "", "Error");
                throw;
            }
        }



        /// <summary>
        /// Scroll down the window 200 pixels up.
        /// </summary>
        public static void ScrollUp(int pixels = 200)
        {
            try
            {
                var executor = (IJavaScriptExecutor)Driver;
                executor.ExecuteScript("window.scrollTo(0,-" + pixels + ")", "");
                Thread.Sleep(3000);
            }
            catch (Exception)
            {
                Logger.FailMessage(BrowserType, TestCase, "Error while scrolling up the browser.", "", "", "", "Error");
                throw;
            }
        }

        /// <summary>
        /// Scroll the browser until the element is visible.
        /// </summary>
        public static void ScrollToClick(By locator)
        {
            try
            {
                Wait(locator, 10);
                var element = Driver.FindElement(locator);
                ((IJavaScriptExecutor)Driver).ExecuteScript(String.Format("window.scrollTo(0,{0});", element.Location.Y));
            }
            catch (Exception)
            {
                Logger.FailMessage(BrowserType, TestCase,
                                    "Error while scrolling following element to click it:" + locator, "", "", "", "Error");
                throw;
            }
        }

        public static IWebElement GetRandomElementFromSelect(string id)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            wait.Until(x => x.FindElement(By.Id(id)));
            var select = new SelectElement(Driver.FindElement(By.Id(id)));
            var elementsCount = select.Options.Count;
            var element = elementsCount > 1 ? random.Next(1, elementsCount - 1) : 0;
            return select.Options[element];
        }
        public static string GetTextFromRandomSelectElement(string id)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            wait.Until(x => x.FindElement(By.Id(id)));
            var select = new SelectElement(Driver.FindElement(By.Id(id)));
            var elementsCount = select.Options.Count;
            var element = elementsCount > 1 ? random.Next(1, elementsCount - 1) : 0;
            var htmlText =
                ((IJavaScriptExecutor)Driver).ExecuteScript(
                    "return document.getElementById('" + id + "').options['" +
                    element + "'].innerHTML").ToString();
            htmlText = HttpUtility.HtmlDecode(htmlText);
            return htmlText;
        }

        public static string GetTextFromRandomSelectElement(string id, int initialElement)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            wait.Until(x => x.FindElement(By.Id(id)));
            var select = new SelectElement(Driver.FindElement(By.Id(id)));
            var elementsCount = select.Options.Count;
            var element = elementsCount > 1 ? random.Next(initialElement, elementsCount - 1) : 0;
            var htmlText =
                ((IJavaScriptExecutor)Driver).ExecuteScript(
                    "return document.getElementById('" + id + "').options['" +
                    element + "'].innerHTML").ToString();
            htmlText = HttpUtility.HtmlDecode(htmlText);
            return htmlText;
        }

        public static IWebElement GetRandomElementFromSelect(string id, int initialElement)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            wait.Until(x => x.FindElement(By.Id(id)));
            var select = new SelectElement(Driver.FindElement(By.Id(id)));
            var elementsCount = select.Options.Count;
            var element = elementsCount > 1 ? random.Next(initialElement, elementsCount - 1) : 0;
            return select.Options[element];
        }

        public static IWebElement GetRandomElementFromTable(string xpath)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            wait.Until(x => x.FindElement(By.XPath(xpath)));
            var elements = Driver.FindElements(By.XPath(xpath));
            var elementsCount = elements.Count;
            var element = elementsCount > 1 ? random.Next(1, elementsCount - 1) : 0;
            return elements[element];
        }

        public static string GetRandomBirthDate()
        {
            var start = new DateTime(1960, 1, 1);
            var range = (DateTime.Today - start).Days;
            var randomDate = start.AddDays(random.Next(range));
            return randomDate.ToString("MM/dd/yyyy");
        }

        public static string GetRandomSurgeryDate()
        {
            var start = DateTime.Now.AddDays(-30);
            var end = DateTime.Now.AddDays(30);
            var range = (end - start).Days;
            var randomDate = start.AddDays(random.Next(range));
            return randomDate.ToString("MM/dd/yyyy");
        }

        public static string GetRandomDataFromCsv(string path, int lineNumber)
        {
            var codes = new string[] { };
            try
            {
                using (var stream = new StreamReader(path))
                {
                    var i = 1;
                    while (i < lineNumber)
                    {
                        stream.ReadLine();
                        i++;
                    }
                    var readLine = stream.ReadLine();
                    if (readLine != null)
                    {
                        codes = readLine.Split(',');
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("Cannot Load File of Codes");
            }
            var codeCounts = codes.Length;
            var randomPosition = random.Next(0, codeCounts - 1);
            return codes[randomPosition];
        }


        public static string GetRandomZipCode()
        {
            return random.Next(10000, 99999).ToString(CultureInfo.InvariantCulture);
        }

        public static StringBuilder GetRandomPhone()
        {
            var telephone = new StringBuilder(10);
            telephone.Append(random.Next(100, 999));
            telephone.Append(random.Next(100, 999));
            telephone.Append(random.Next(1000, 9999));
            return telephone;
        }

        public static string GetRandomDataFromTxt(string file)
        {
            string[] lines;
            try
            {
                lines = File.ReadAllLines((file));
            }
            catch (Exception)
            {
                throw new Exception("Cannot Load Text File.");
            }
            var lineCount = lines.Length;
            var randomPosition = random.Next(0, lineCount - 1);
            return lines[randomPosition];
        }
    }
}