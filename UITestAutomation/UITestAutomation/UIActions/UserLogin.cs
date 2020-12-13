using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UITestAutomation.Actions
{
   public class UserLogin
    {
        public static void LoginToApplication(string loginType)
        {
            string PageName = loginType.ToLower() == "gmail" ? "Gmail" : "Yahoo";
            GetLoginDetails(loginType);
            UIDriver.WebDriver.Navigate().GoToUrl(CommonVariables.URL);
            Logger.Log("Navigating to the URL: '{0}'", CommonVariables.URL);

            try
            {
                Logger.Log("Entering user name as: '" + CommonVariables.UserName + "'");
                string xpathUserName = UIHelpers.GetXpathValue("LoginPage", PageName, "UserNameTextBox");
                UIDriver.WaitForElementTo(xpathUserName, 15, "Visible");
                IWebElement txtLogin = UIDriver.WebDriver.FindElement(By.XPath(xpathUserName));
                txtLogin.SendKeys(CommonVariables.UserName);

                string xpathBtNext = UIHelpers.GetXpathValue("LoginPage", PageName, "NextButton");
                IWebElement btnNext = UIDriver.WebDriver.FindElement(By.XPath(xpathBtNext));
                btnNext.Click();

                // Wait untill Password Element Appears in the next Page
                string xpathPassowrd = UIHelpers.GetXpathValue("LoginPage", PageName, "PasswordTextBox");
                UIDriver.WaitForElementTo(xpathPassowrd, 15, "clickable");

                Logger.Log("Entering password as: **********" ); // to be encrypted
                IWebElement txtPassword = UIDriver.WebDriver.FindElement(By.XPath(xpathPassowrd));
                txtPassword.SendKeys(CommonVariables.Password);

                // Click on the Next\Login button to Login 
                string xpathBtLogin = UIHelpers.GetXpathValue("LoginPage", PageName, "LoginButton");
                IWebElement btnLogin = UIDriver.WebDriver.FindElement(By.XPath(xpathBtLogin));
                btnLogin.Click();

                //Wait for the Inbox object to be appears
                if (PageName == "Yahoo")
                {
                    string xpathEmailFolder = UIHelpers.GetXpathValue("InboxPage", PageName, "EmailFolder");
                    UIDriver.WaitForElementTo(xpathEmailFolder, 15, "visible");
                    IWebElement btnEmailBox = UIDriver.WebDriver.FindElement(By.XPath(xpathEmailFolder));
                    btnEmailBox.Click();
                }

                string xpathInboxFolder = UIHelpers.GetXpathValue("InboxPage", PageName, "Inbox");
                UIDriver.WaitForElementTo(xpathInboxFolder, 15, "visible");
                Logger.Pass("Login to '"+ PageName + "' is succesfull");
            }
            catch (Exception ex)
            {
                Logger.Fail("Failed to login in "+ loginType);
                Logger.Log(ex.Message);
            }
        }


        private static void GetLoginDetails(string LoginType)
        {
            if(LoginType.ToLower() == "yahoo")
            {
                CommonVariables.URL = ConfigurationManager.AppSettings["YahooLoginURL"];
                CommonVariables.UserName = ConfigurationManager.AppSettings["YahooUserName"];
                CommonVariables.Password = ConfigurationManager.AppSettings["YahooPassword"];
            }else if(LoginType.ToLower() == "gmail"){
                CommonVariables.URL = ConfigurationManager.AppSettings["GmailLoginURL"];
                CommonVariables.UserName = ConfigurationManager.AppSettings["GmailUserName"];
                CommonVariables.Password = ConfigurationManager.AppSettings["GmailPassword"];
            }
            else
            {
                Logger.Fail("Please select the valid logn type");
            }
        }

    }
}
