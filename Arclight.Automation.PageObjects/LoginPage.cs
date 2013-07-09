using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arclight.Automation.Selenium;
using OpenQA.Selenium;

namespace Arclight.Automation.PageObjects
{
    public class LoginPage
    {
        private IWebElement UserNameInputText
        {
            get
            {
                return Browser.GetElement(By.Id("username"));
            }
        }

        private IWebElement PasswordInputText
        {
            get
            {
                return Browser.GetElement(By.Id("password"));
            }
        }

        private IWebElement LoginButton
        {
            get
            {
                return Browser.GetElement(By.ClassName("form_submit"));
            }
        }

        private string InsightUrl
        {
            get { return Browser.GetConfigValue("ARCLIGHT_URL"); }
        }

        public void GoToLoginPage()
        {
            Browser.Open(InsightUrl);
        }

        public void Login(string username, string password)
        {
            UserNameInputText.SendKeys(username);
            PasswordInputText.SendKeys(password);
            LoginButton.Click();
            Browser.Wait(By.ClassName("container-fluid"), Browser.MID_WAIT);
        }
    }
}
