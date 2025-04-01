using AutoFram.UIReport;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AutoFram.DataTest
{
    public class TestParams
    {
        private GlobalData gdata;
        public string testguid;
        public string testdata;
        // test data for report
        private int testCounterIns = 0;
        private string teststart = null;
        private Stopwatch swatch;
        private TimeSpan timeSpan;
        public string runningclient = "";
        public string classname;
        public string testname;
        private string timestapstr = null;
        public string driverexception;
        public string currentbrowser;
        public string test_msg;
        public string temp_folder;
        public TestParams()
        {
            testguid = GenRandomString(15);
            testdata = "Start";
            gdata = new GlobalData();
            teststart = DateTime.Now.ToShortTimeString();
            swatch = Stopwatch.StartNew();
            driverexception = "";
            classname = "";
            testname = "";
            currentbrowser = "Chrome";
            runningclient = "";
            test_msg = "";
            temp_folder = "";
        }

        private string GenRandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] rnd = new byte[1];
            while (builder.Length < size)
            {
                rng.GetBytes(rnd);
                char c = (char)rnd[0];
                if (Char.IsLetter(c) && rnd[0] < 127)
                {
                    builder.Append(c.ToString());
                }
            }
            return builder.ToString();
        }
        public void AddTestCase(IWebDriver driver, string testresult)
        {
            string failreason = "";
            string getenv = "";
            if (testresult.Contains("Pass"))
                testdata = "Pass";
            if (testresult.Contains("Fail") && testdata.Contains("Start"))
            {
                testdata = "Fail";
                failreason = GetFailReason(driver, driverexception);
                Report.WriteTestLog("fail reason: ", testguid);
            }
            TakeScreenShot(driver);
            string tstduration = "00";
            double testspan = 0;
            if (swatch != null)
            {
                tstduration = gdata.GetTestTime(swatch.Elapsed.ToString());
                testspan = swatch.Elapsed.TotalSeconds;
            }
            Report.WriteTestLog($"test duration: {tstduration} | time-span: {testspan}", testguid);
            swatch.Stop();
            #region test name details
            if (driverexception != null && driverexception.Contains("\n"))
                driverexception = driverexception.Split('\n')[0];
            if (driverexception.Length > 86)
                driverexception = driverexception.Remove(85);
            if (driver != null)
            {
                swatch.Stop();
                try
                {
                    if (driver.Url.Contains("qa-dashboard"))
                        getenv = "QA";
                    else
                        getenv = "UAT";
                }
                catch (Exception ex)
                {
                    Report.WriteTestLog($"Tear Down | fail get url: {ex.Message}", testguid);
                }
                //string runmessage = ""; //TestContext.CurrentContext.Result.Message;
                #endregion
                if (testspan > 15)
                {
                    testCounterIns = UIReport.Report.ManageTestCounter();
                    UIReport.Report.SaveTestCase(testCounterIns.ToString(), classname, testname, runningclient, currentbrowser, teststart, tstduration, timestapstr, testdata, failreason, testdata, failreason);
                    UIReport.Report.WriteTestLog($"test Case is saved to html: {testCounterIns} | class: {classname} | test: {testname} | {currentbrowser} | start: {teststart} | duration: {tstduration} | {testdata}", testguid);
                }
                if (testspan < 15)
                    UIReport.Report.WriteTestLog("test is skipp => not include in report.", testguid);
            }
            if (driver == null)
            {
                string teststart = DateTime.Now.ToLongTimeString();
                UIReport.Report.SaveTestCase(testCounterIns.ToString(), classname, testname, runningclient, currentbrowser, teststart, "", "", "Failed", currentbrowser + " driver is failed", testdata, failreason);
                UIReport.Report.WriteTestLog("Driver Init is Failed", testguid);
            }
            CreateReport(getenv);
        }
        private void TakeScreenShot(IWebDriver driver)
        {
            if (testdata != "Pass")
            {
                if (driver != null)
                {
                    try
                    {
                        long timestamp = (DateTime.Now.Ticks) / 1000;
                        timestapstr = timestamp.ToString() + ".Png";
                        string screenShotlocation = Report.screenShotLocation + timestapstr;
                        Screenshot sshot = ((ITakesScreenshot)driver).GetScreenshot();
                        sshot.SaveAsFile(screenShotlocation);
                        Report.WriteTestLog($"SCREEN SHOT FILE: {screenShotlocation} | [SCREEN SHOT URL]{driver.Url}", testguid);
                        //byte[] content = ((ITakesScreenshot)_driver).GetScreenshot().AsByteArray;
                        //AllureLifecycle.Instance.AddAttachment("screen-shot", "image/png", content);
                    }
                    catch (Exception ex)
                    {
                        Report.WriteTestLog(ex.Message);
                    }
                }
            }
        }
        private void CreateReport(string envname = null)
        {
            Report.WriteTestLog($"TearDown => START", testguid);
            #region total tests details
            string testenv = envname; //BaseTask.TestSettings.Url;
                                      //testenv = testenv.Substring(8);
                                      //if (testenv.Contains("-"))
                                      //testenv = testenv.Split('-')[0].ToUpper();
            string totalwatchstr = Report.totalwatch.Elapsed.ToString();
            if (totalwatchstr.Length > 9)
                totalwatchstr = totalwatchstr.Remove(8);
            #endregion
            Report.SetTotalsSummary(testenv, totalwatchstr);
            Report.WriteTestLog($"Test Totals is saved to html. | Test_Counter  is == {Report.testCounter}", testguid);
        }
        public void SetTestResult(string value)
        {
            Report.WriteTestLog($"set test: {value}", testguid);
            Console.WriteLine($"Set_Test_Result: {value}");
            if (value.Contains("Pass"))
                testdata = "Pass";
            if (value.Contains("Data"))
            {
                testdata = value;
                throw new Exception("throw exception due to - test has no data.");
            }
        }
        public string CreateCacheDirectory(string localStorageForCache)
        {
            long timestamp = 0;
            Thread.Sleep(100);
            var now = DateTime.Now;
            timestamp = (now.Ticks) / 1000;
            timestamp += gdata.GenRandomNumber(100, 999);
            return "--user-data-dir=" + (localStorageForCache + timestamp);
        }
        public void CleanTempFiles()
        {
            try
            {
                //Directory.Delete(temp_folder, true);
                Report.WriteTestLog($"deleted temp folder: {temp_folder}", testguid);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            // delete all "scoped_dir" temp folders 
            string tempfolder = Path.GetTempPath();
            Cleaner(tempfolder, "scoped_dir*");
            tempfolder = @"C:\Program Files";
            Cleaner(tempfolder, "chrome_*");
            Cleaner(tempfolder, "msedge_url*");
            tempfolder = @"C:\Program Files (x86)";
            Cleaner(tempfolder, "chrome_*");
            Cleaner(tempfolder, "scoped_dir*");
        }
        private void Cleaner(string foldername, string filepattern)
        {
            Report.WriteTestLog($"Cleaner start clean: {foldername} => {filepattern}", testguid);
            try
            {
                string[] tempfiles = Directory.GetDirectories(foldername, filepattern, SearchOption.AllDirectories);
                foreach (string tempfile in tempfiles)
                {
                    try
                    {
                        DirectoryInfo directory = new DirectoryInfo(foldername);
                        foreach (DirectoryInfo subDirectory in directory.GetDirectories())
                        {
                            subDirectory.Delete(true);
                            Report.WriteTestLog($"CleanTempFiles | temp folder: {subDirectory} deleted.", testguid);
                        }
                    }
                    catch (Exception ex)
                    {
                        Report.WriteTestLog("CleanTempFiles | File '" + tempfile + "' could not be deleted. Exception: " + ex.Message + ".", testguid);
                    }
                }
            }
            catch (Exception exc)
            {
                Report.WriteTestLog("Exception: " + exc.Message, testguid);
            }
        }
        private string GetFailReason(IWebDriver driver, string message)
        {
            string text = "";
            Report.WriteTestLog("try get fail/exception reason", testguid);
            if (driver != null && message.Length < 2)
            {
                try
                {
                    Actions action = new Actions(driver);
                    action.SendKeys(Keys.F12).Perform();
                    Thread.Sleep(500);
                }
                catch { }
                List<string> messages = new List<string>();
                messages.Add("needs to review the security");
                messages.Add("unknown error occurred");
                messages.Add("able to fetch");
                messages.Add("is an unknown connection issue");
                messages.Add("exceeded");
                messages.Add("while fetching ");
                messages.Add("report file matching");
                messages.Add("Authentification failed");
                messages.Add("to fetch");
                messages.Add("server is unavailable");
                messages.Add("IP is unauthorized");
                foreach (string msg in messages)
                {
                    By errorlocator = By.XPath($"//*[contains(text(), '{msg}')]");
                    IWebElement captcha = null;
                    try
                    {
                        captcha = driver.FindElement(errorlocator);
                    }
                    catch { }
                    if (captcha != null)
                    {
                        if (msg.Contains("security"))
                            text = "Captcha";
                        if (msg.Contains("unknown error"))
                            text = "Unknown Error";
                        if (msg.Contains("fetch"))
                            text = "Fetch Error";
                        if (msg.Contains("connection"))
                            text = "Connect Error";
                        if (msg.Contains("exceeded"))
                            text = "Call Exceeded";
                        if (msg.Contains("file matching"))
                            text = "Match Error";
                        if (msg.Contains("Authentification failed"))
                            text = "Authentic. Fail";
                        if (msg.Contains("unavailable"))
                            text = "server unavailable";
                        if (msg.Contains("is unauthorized"))
                            text = "IP unauthorized";
                        break;
                    }
                }
            }
            if (driver == null && message.Length > 2)
            {
                if (message.Contains("session timed out"))
                {
                    text = "Session Timed Out";
                }
                if (message.Contains("session not created"))
                {
                    text = "Fail Create Session";
                }
            }
            return text;
        }
    }
}
