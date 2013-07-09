using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arclight.Automation.Selenium;
using OpenQA.Selenium;

namespace Arclight.Automation.PageObjects
{
    public class DashboardPage
    {
        private IWebElement CreateBoxArtButton
        {
            get
            {
                return Browser.GetElement(By.XPath("(//div[@class='btn-group'])[7]/a[1]"));
            }
        }

        public void GoToCreateBoxArtPage()
        {
            CreateBoxArtButton.Click();
            Browser.Wait(By.ClassName("sonata-ba-collapsed-fields"),Browser.MIN_WAIT);
        }
    }
}
