using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arclight.Automation.Selenium;
using OpenQA.Selenium;

namespace Arclight.Automation.PageObjects
{
    public class CreateBoxArtPage
    {
        private IWebElement SelectMediaMetadataButton
        {
            get
            {
                return Browser.GetElement(By.XPath("(//span[@class='btn-group'])[1]/a"));
            }
        }

        private IWebElement MyLastDayTitleButton
        {
            get
            {
                return Browser.GetElement(By.XPath("(//td[@objectid='1'])[11]/a"));
            }
        }

        private IWebElement SelectBoxArtTypeButton
        {
            get
            {
                return Browser.GetElement(By.XPath("(//span[@class='btn-group'])[3]/a"));
            }
        }

        private IWebElement SmallTypeButton
        {
            get
            {
                return Browser.GetElement(By.XPath("(//td[@objectid='1'])[16]/a"));
            }
        }

        private IWebElement FileNameInputText
        {
            get
            {
                return Browser.GetElement(By.XPath("(//input[@required='required'])[3]"));
            }
        }

        private IWebElement OvpURLInputText
        {
            get
            {
                return Browser.GetElement(By.XPath("(//input[@required='required'])[4]"));
            }
        }

        private IWebElement CreateBoxArtButton
        {
            get
            {
                return Browser.GetElement(By.Name("btn_create_and_edit"));
            }
        }

        private IWebElement BoxArtListLink
        {
            get
            {
                return Browser.GetElement(By.XPath("/html/body/div[2]/ul/li[2]/a"));
            }
        }



        public void GoToSelectMediaMetadataPopup()
        {
            SelectMediaMetadataButton.Click();
            Browser.WaitUntilVisible(By.XPath("/html/body/div[4]"),Browser.MIN_WAIT);
        }

        public void SelectMyLastDayTitle()
        {
            MyLastDayTitleButton.Click();
            Browser.Wait(By.XPath("//span[@class='inner-field-short-description']/a"),Browser.MIN_WAIT);
        }

        public void GoToSelectBoxArtTypePopup()
        {
            SelectBoxArtTypeButton.Click();
            Browser.WaitUntilVisible(By.XPath("/html/body/div[5]"), Browser.MIN_WAIT);
        }

        public void SelectSmallTypeBoxArt()
        {
            SmallTypeButton.Click();
            Browser.Wait(By.XPath("(//span[@class='inner-field-short-description']/a)[2]"), Browser.MIN_WAIT);
        }

        public void WriteFileName(string filename)
        {
            FileNameInputText.SendKeys(filename);
        }

        public void WriteOvp(string ovp)
        {
            OvpURLInputText.SendKeys(ovp);
        }

        public void SubmitBoxArt()
        {
            CreateBoxArtButton.Click();
            Browser.WaitForTextPresent("Item has been successfully created.",Browser.MIN_WAIT);
        }

        public void GoToBoxArtList()
        {
            BoxArtListLink.Click();
            Browser.WaitUntilVisible(By.ClassName("sonata-ba-list"),Browser.MIN_WAIT);
        }




    }
}
