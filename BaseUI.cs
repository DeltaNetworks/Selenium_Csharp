using AutoFram.UIReport;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFram.DataTest;
using AutoFram.Browsers;
using AutoFram.PageObjects;

namespace AutoFram.BasePages
{
    public class BaseUI
    {
        public IWebDriver driver;
        public GlobalData globalData;
        public TestParams test;
        private string headless = "false";
        public BaseUI() { }
        [SetUp]
        public void Init()
        {
            BaseTask.ReadConfig();
            globalData = new GlobalData();
            Random rm = new Random();
            int num = rm.Next(0, 9);
            Thread.Sleep(globalData.sleeptimes[num]);
            if (Report.threadcounter < 1)
            {
                try
                {
                    Report.ManageThreadCounter();
                    Report.CreateReportFolder();
                    Report.CreateTemplate();
                    BackProcess background = new BackProcess();
                    background.CloseProcessBeforeCopy();
                    background.StartDriverManager();
                    background.SetScreenResolution();
                    //Report.WriteTestLog($"driver-manager download files. | driver-wait for download set: {BaseTask.TestSettings.DriverManagerWait}");
                    //Report.WriteTestLog(background.CopyBrowsersDrivers());
                    background.StartTestListener();
                    Report.backfinish = true;
                    globalData.Delete_BeforeTestStart();
                    Report.WriteTestLog("clean allure-report.");
                    //headless = BaseTask.TestSettings.Headless;
                    Report.WriteTestLog($"headless status: {headless}");
                }
                catch (Exception ex)
                {
                    Report.WriteTestLog($"Init is failed | {ex.Message}");
                }
            }
            //if (Report.testCounter < 6)
                //Thread.Sleep(5000);
            //if (BaseTask.TestSettings.DriverManagerInit == "1")
            //    Thread.Sleep(int.Parse(BaseTask.TestSettings.DriverManagerWait));
        }
        public void SetUp(string browserName, string site = null, string lang = null, bool devtools = false)
        {
            //browserName = "Firefox";
            test = new TestParams
            {
                classname = globalData.GetCurrentTestClassName(TestContext.CurrentContext.Test.ClassName),
                testname = globalData.GetCurrentTestName(TestContext.CurrentContext.Test.Name),
                currentbrowser = browserName
            };
            Report.WriteTestLog($"----- test is start ----- | browser is: {browserName}", test.testguid);
            string path = test.CreateCacheDirectory(globalData.localStorageForCache);
            test.temp_folder = path.Split("=")[1];
            ChromeOptions chromeOptions = new ChromeOptions();
            EdgeOptions opt = new EdgeOptions();
            try
            {
                switch (browserName)
                {
                    case "Chrome":
                        chromeOptions.PageLoadStrategy = PageLoadStrategy.None;
                        chromeOptions.AddArgument(path);
                        if (lang == ("ja"))
                            chromeOptions.AddArguments("--lang=ja");
                        chromeOptions.AddArgument("--disable-notifications");
                        chromeOptions.AddArgument("disable-infobars");
                        //if (BaseTask.TestSettings.Headless == "true" && Report.currentMachineName.Contains(Report.QAmachine))
                        //chromeOptions.AddArguments("--headless", "--start-maximized");
                        ChromeDriverService service = null;
                        try
                        {
                            service = ChromeDriverService.CreateDefaultService(globalData.driverDirectory);
                        }
                        catch (Exception ex)
                        {
                            Report.WriteTestLog(ex.ToString());
                        }
                        if (devtools)
                        {
                            service.LogPath = Report.reportLog + "DEVTOOLS";
                            service.EnableVerboseLogging = true;
                            service.EnableAppendLog = true;
                        }
                        //return new ChromeDriver(globalData.driverDirectory, chromeOptions);
                        //driver = new ChromeDriver(service, chromeOptions);
                        driver = BaseTask.GetChromeDriver(service, chromeOptions);
                        break;
                    case "Firefox":
                        driver = new FirefoxDriver(globalData.driverDirectory);
                        break;
                    case "Edge":
                        opt.PageLoadStrategy = PageLoadStrategy.None;
                        opt.AddArgument("--disable-notifications");
                        opt.AddArgument("disable-infobars");
                        driver = new EdgeDriver(globalData.driverDirectory, opt);
                        break;
                    default:
                        driver = new ChromeDriver(globalData.driverDirectory);
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                test.driverexception = ex.Message;
                Report.WriteTestLog($"driver: {browserName} try to start and crashed - exception: " + test.driverexception, test.testguid);
                throw new Exception(ex.StackTrace);
            }
            if (driver == null)
            {
                Report.WriteTestLog($"driver: {browserName} try to start and crashed.", test.testguid);
                test.driverexception = "session not created";
                throw new Exception("fail create driver instance.");
            }
            Thread.Sleep(500);
            //driver.Navigate().GoToUrl(globalData.url_coffee);
            if (site == null)
            {
                //driver.Navigate().GoToUrl(globalData.url_coffee);
            }
            else if (site == "us-dash")
                driver.Navigate().GoToUrl(BaseTask.TestSettings.Urlus);
            else if (site == "sso")
                driver.Navigate().GoToUrl(BaseTask.TestSettings.Urlsso);
            driver.Manage().Window.Maximize();
            Thread.Sleep(200);
            driver.Navigate().GoToUrl(globalData.url_ronen_ytube);
            Thread.Sleep(300);
            if (browserName.Contains("Chrome") || browserName.Contains("Edge"))
            {
                //chromeOptions.PageLoadStrategy = PageLoadStrategy.Normal;
                //opt.PageLoadStrategy = PageLoadStrategy.Normal;
                //driver.Navigate().Refresh();
                if (site == null && !driver.Url.Contains("-dashboard"))
                {
                    //driver.Navigate().GoToUrl(BaseTask.TestSettings.Url);
                }
                if (site != null && site != "sso" && site.Contains("") && !driver.Url.Contains("-dashboard"))
                {
                    driver.Navigate().GoToUrl(BaseTask.TestSettings.Urlus);
                }
            }
            if (Report.currentMachineName.Contains("EC2AMAZ"))
            {
                driver.Manage().Window.FullScreen();
                ((IJavaScriptExecutor)driver).ExecuteScript($"document.body.style.zoom = '{BaseTask.TestSettings.ScreenZoom}%'");
                Report.WriteTestLog($"on QA-Machine 'EC2AMAZ' set screen size: full screen with '{BaseTask.TestSettings.ScreenZoom}%' zoom.", test.testguid);
            }
            Thread.Sleep(100);
            //Report.WriteTestLog($"settings url: {BaseTask.TestSettings.Url}", test.testguid);
            Report.WriteTestLog($"driver navigated to url: {driver.Url}", test.testguid);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
        }
        [TearDown]
        public void Dispose()
        {
            Report.WriteTestLog($"TearDown | start test end process | NUNIT result: {TestContext.CurrentContext.Result.Outcome.Status}.", test.testguid);
            test.AddTestCase(driver, TestContext.CurrentContext.Result.Outcome.Status.ToString());
            try
            {
                Report.WriteTestLog("driver is delete cookies.", test.testguid);
                //driver.Manage().Cookies.DeleteAllCookies();
                driver.Dispose();
            }
            catch { }
            test.CleanTempFiles();
            Report.WriteTestLog("----- test is end -----", test.testguid);
        }
        [OneTimeTearDown]
        public void OneTimeTear()
        {
            Console.WriteLine("one time tear down.");
            Report.WriteTestLog("one time tear down.", test.testguid);
        }
        public void Flow(string UserName, string TabName = "", string SubTabb = "")
        {
            new Flow(driver, UserName, TabName, SubTabb);
            GetCurrentUser();
            Thread.Sleep(600);
        }
        private void GetCurrentUser()
        {
            try
            {
                string name = "";
                if (driver.Url.Contains("/cap-"))
                    name = driver.FindElement(By.XPath("//div[contains(@class,'_UserButton')]")).Text;
                else
                    name = driver.FindElement(By.XPath("//div[contains(@class,'_UserButton')]")).Text;
                // //span[contains(@class,'Username')]
                test.runningclient = name;
                switch (name)
                {
                    case "F":
                        test.runningclient = "forTrade";
                        break;
                    case "A":
                        test.runningclient = "avaTrade";
                        break;
                    case "T":
                        test.runningclient = "thinkMarkets";
                        break;
                    case "C":
                        test.runningclient = "cchang@rjobrien.com";
                        break;
                    case "S":
                        test.runningclient = "swissQuotes";
                        break;
                    case "N":
                        test.runningclient = "Noa-us-test";
                        break;
                    default:
                        test.runningclient = name;
                        break;
                }
                Report.WriteTestLog($"get current user: {test.runningclient}", test.testguid);
                Report.WriteTestLog($"get current url: {driver.Url}", test.testguid);
            }
            catch { }
        }
    }
}
