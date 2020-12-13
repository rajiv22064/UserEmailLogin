using System;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using UITestAutomation.Actions;

namespace UITestAutomation
{
    [TestClass]
    public class TestMethods
    {

        #region UI Initialization Engine
        [ClassInitialize()]
        public static void ClassTestInitialization(TestContext context)
        {
            Logger.LogInitialization(context, "User Login Test");
        }

        public TestContext TestContext { get; set; }

        [ClassCleanup]
        public static void ClassTestCleanUp()
        {
            Logger.TestLogClosure();
        }

        [TestInitialize]
        public void TestInitialization()
        {
            Logger.TestLogInitialize(TestContext.TestName,"Login Test");
            UIDriver.InitilizeDriver(TestContext.TestName);
        }

        [TestCleanup]
        public void TestCleanups()
        {
            UIDriver.CloseWebdriver();
        }
        #endregion

        [TestMethod]
        public void UserLoginToGmail()
        {
            UserLogin.LoginToApplication("Gmail");
        }

        [TestMethod]
        public void UserLoginToYahoo()
        {
            UserLogin.LoginToApplication("Yahoo");
        }

    }
}
