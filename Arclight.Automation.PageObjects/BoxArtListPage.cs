using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arclight.Automation.Selenium;
using OpenQA.Selenium;

namespace Arclight.Automation.PageObjects
{
    public class BoxArtListPage
    {
        private IWebElement LastPageButton
        {
            get
            {
                return Browser.GetElement(By.XPath("/html/body/div[2]/div[3]/div/div[2]/form/table/tbody[2]/tr[2]/td/div/ul/li[27]/a"));
            }
        }

        public void GoToLastResultsPage(string capturedValue)
        {
            LastPageButton.Click();
            Browser.WaitForTextPresent(capturedValue,Browser.MAX_WAIT);
        }
    }
}
