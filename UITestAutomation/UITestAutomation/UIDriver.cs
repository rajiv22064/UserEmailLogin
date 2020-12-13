using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

namespace UITestAutomation
{
    public class UIDriver
    {
        public static IWebDriver WebDriver { get; set; }

        // Initializing the web driver
        public static IWebDriver InitilizeDriver(string testname)
        {
            Logger.Log("Initializing Web driver for test: "+testname);
            WebDriver = new ChromeDriver();
            WebDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            WebDriver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(360);
            WebDriver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(360);
            WebDriver.Manage().Window.Maximize();
            return WebDriver;
        }

        // Close the Web Drivers
        public static void CloseWebdriver()
        {
            Logger.Log("Close Web Driver");
            try
            {
                if (WebDriver == null) return;
                WebDriver.Close();
                WebDriver.Quit();
                WebDriver = null;
            }
            catch (Exception ex)
            {
                Logger.Fail("Web Driver Cleanup failed: " + ex.Message);
            }
        }

        public static void WaitForElementTo(string xPath, int TimeOutInSec = 10, string status = "visible")
        {
            WebDriverWait wait = new WebDriverWait(WebDriver, TimeSpan.FromSeconds(TimeOutInSec));
            try
            {


                switch (status.ToLower())
                {
                    case "visible":
                        wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(xPath)));
                        Logger.Log("Object is visible now");
                        break;
                    case "clickable":
                        wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xPath)));
                        Logger.Log("Object is clickable now");
                        break;
                    default:
                        Console.WriteLine("No status check found");
                        break;
                }
            }
            catch(Exception)
            {
                Logger.Fail(string.Format("Failed to check the status: '{0}' for this object",status));
                throw new Exception("Object not found");
            }
        }
    }
}
