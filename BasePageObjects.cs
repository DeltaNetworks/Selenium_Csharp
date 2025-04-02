using AutoFram.Browsers;
using AutoFram.DataTest;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFram.PageObjects
{
    internal class BasePageObjects
    {
        public IWebDriver driver;
        public BrowserHelper helper;
        public string testguid;
        private By locator = null;
        private IWebElement elmnt = null;
        public IWebElement Applybtn { get; set; }
        private By navigationlocator = By.XPath("//div[contains(@class,'Navigation')]");
        private By tableexpandLocator = By.XPath("//button//*[@data-cap='table-expand']");
        private By filterDateBtnLocator = By.XPath("//h4[contains(text(),'Date Range')]");
        public By ApplyButtonLocator = By.XPath("//button[text()='Apply' or contains(text(), 'Apply')]");
        private By BarsLocator = By.XPath("//*[contains(@class,'highcharts-point') and @height > 12]");
        public By ExportLocator = By.XPath("//button[@data-cap='download-csv']");
        public By RowsLocator = By.XPath("//div[@role='row']");
        public By ClearSearchBoxbuttonLocator = By.XPath("//button[@data-cap='close-search-button'][1]");
        private By FilterDateRangeSelectAllLocator = By.XPath("//*//following::span[text()='Select all']");
        private By LinksLocator = By.XPath("//h4");
        private By FilterIconLocator = By.XPath("//button[contains(@title,' Filter Panel')]");
        private By CloseButtonLocator = By.XPath("//button[@type='button' and contains(text(),'Close')]");
        private By DateRangeBackButtonLocator = By.XPath("//*[contains(@class,'rdrPprevButton')]");
        private By DateRangeApplyButtonLocator = By.XPath("//*[contains(@class,'PrimaryButton') and text()='Apply' or contains(text(),'Apply selection')]");
        private By LoadMoreLocator = By.XPath("//button[contains(text(),'Load more')]");
        private By TopDismissLocator = By.XPath("//button[@title='Dismiss' or contains(@class,'CloseButton')]");
        public By spinnerlocator = By.XPath("//*[contains(@class,'Spinner') and @speed]");
        private By Accountlocator = By.XPath("//span[contains(text(), 'Account')]");
        public GlobalData gdata;
        private ReadOnlyCollection<IWebElement> Bars { get; set; }
        private IWebElement ApplyBtn { get; set; }
        private IWebElement filterDateBtn { get; set; }

        public BasePageObjects(IWebDriver _driver, string _testguid)
        {
            driver = _driver;
            helper = new BrowserHelper(driver);
            testguid = _testguid;
            gdata = new GlobalData();
        }
        public bool IsText(string value)
        {
            helper.Sleep(0.3);
            UIReport.Report.WriteTestLog($"check text exist: {value}.", testguid);
            locator = By.XPath($"//*[contains(text(),'{value}')]");
            IWebElement element = helper.WaitForElement(driver, locator);
            if (element != null)
                UIReport.Report.WriteTestLog($"text is exist: {value}.", testguid);
            if (element == null)
                UIReport.Report.WriteTestLog($"text: {value} not found.", testguid);
            return element != null;
        }
        public int IsTextCount(string value)
        {
            UIReport.Report.WriteTestLog($"check text count exist: {value}.");
            locator = By.XPath($"//*[contains(text(),'{value}')]");
            ReadOnlyCollection<IWebElement> elements = helper.WaitForElements(driver, locator);
            return elements.Count;
        }
        public void SelectDate(string value)
        {
            HoverQuestionMark();
            Console.WriteLine($"select date-range filter: {value}");
            UIReport.Report.WriteTestLog($"select date-range filter: {value}", testguid);
            ClickOpenDateFilter();
            HoverQuestionMark();
            if (!driver.Url.Contains("rebranding"))
            {
                By datelocator = By.XPath($"//span[text()='{value}']");
                IWebElement dateoption = helper.WaitForElement(driver, datelocator);
                helper.ClickElem(dateoption, value);
                HoverQuestionMark();
            }
            ApplyBtn = helper.WaitForElement(driver, ApplyButtonLocator);
            helper.ClickElem(ApplyBtn, "Filter Date Btn.");
            helper.Sleep(1);
            HoverQuestionMark();
            WaitSpinner();
            WaitBarsClickable();
        }
        private void ClickCalendar(int day)
        {
            By datelocator1 = By.XPath($"//button[contains(@class,'rdrDay')]//span[contains(text(),'{day}')]");
            IWebElement enddate = helper.WaitForElement(driver, datelocator1);
            helper.ClickElem(enddate, "end date");
        }
        public void HoverQuestionMark()
        {
            UIReport.Report.WriteTestLog("hover question mark.", testguid);
            By locator = By.XPath("//button//*[contains(@class,'UserButt')]");
            locator = By.XPath("//div[contains(@class,'_UserButton')]");
            IWebElement elmnt = helper.WaitForElement(driver, locator);
            if (elmnt != null)
                helper.MouseHover(driver, elmnt);
        }
        private void ClickOpenDateFilter()
        {
            filterDateBtn = helper.WaitForElement(driver, filterDateBtnLocator, 10);
            helper.ClickElem(filterDateBtn, "Filter Date Btn.");
            helper.Sleep(0.2);
        }
        private void WaitBarsClickable()
        {
            Bars = helper.WaitForElements(driver, BarsLocator, 20);
            if (Bars.Count > 0)
                helper.WaitUntillClickable(Bars[0], driver, "bars");
            helper.Sleep(1);
        }
        public void SelectDateRangeURL(string start, string last)
        {
            string url = driver.Url;
            int indexcontinue = 0;
            bool seturl = false;
            // set end-date
            for (int i = 0; i < url.Length; i++)
            {
                if (url[i] == '=')
                {
                    if (url[i - 5] == 'd')
                    {
                        url = url.Remove(i + 1, 10);
                        url = url.Insert(i + 1, last);
                    }
                    if (url[i - 5] == 't')
                    {
                        url = url.Remove(i + 1, 10);
                        url = url.Insert(i + 1, start);
                    }

                    indexcontinue = i + 12;
                    seturl = true;
                    UIReport.Report.WriteTestLog($"set date-range: {start} => {last}", testguid);
                    break;
                }
            }
            for (int k = indexcontinue; k < url.Length; k++)
            {
                if (url[k] == '=')
                {
                    if (url[k - 5] == 'd')
                    {
                        url = url.Remove(k + 1, 10);
                        url = url.Insert(k + 1, last);
                    }
                    if (url[k - 5] == 't')
                    {
                        url = url.Remove(k + 1, 10);
                        url = url.Insert(k + 1, start);
                    }
                    //url = url.Remove(k + 1, 10);
                    //url = url.Insert(k + 1, start);
                    break;
                }
            }
            UIReport.Report.WriteTestLog($"driver | current url: {driver.Url}", testguid);
            if (seturl)
                driver.Navigate().GoToUrl(url);
            helper.Sleep(5);
            WaitSpinner();
            UIReport.Report.WriteTestLog($"driver | click refresh | url: {driver.Url}", testguid);
            HoverQuestionMark();
        }
        public void SelectDateRangeMonths(string start, string last, int monthcount)
        {
            last = DateTime.Today.AddDays(-1).Day.ToString();
            ClickOpenDateFilter();
            UIReport.Report.WriteTestLog($"date time filter | user select month back: {monthcount}", testguid);
            By startlocator = By.XPath($"//button//span//span[text()='{start}']");
            By lastlocator = By.XPath($"//button//span//span[text()='{last}']");

            // one click on end
            ReadOnlyCollection<IWebElement> elmnts = helper.WaitForElements(driver, lastlocator);
            helper.ClickElem(elmnts[elmnts.Count - 1], "end");
            helper.Sleep(0.3);

            // click on month-back
            for (int i = 0; i < monthcount; i++)
            {
                elmnt = helper.WaitForElement(driver, DateRangeBackButtonLocator);
                helper.ClickElem(elmnt, "back");
                helper.Sleep(0.1);
            }

            // one click on start
            elmnt = helper.WaitForElement(driver, startlocator);
            helper.ClickElem(elmnt, "start");
            helper.Sleep(0.3);

            // click apply
            elmnt = helper.WaitForElement(driver, DateRangeApplyButtonLocator);
            helper.ClickElem(elmnt, "apply");
        }
        public void FilterSelect(string mainFilter, string value1, string value2, bool selectall = true)
        {
            helper.Sleep(0.2);
            HoverQuestionMark();
            By firstFilterLocator = By.XPath($"//h4[text()='{mainFilter}']");
            IWebElement elmnt = helper.WaitForElement(driver, firstFilterLocator);
            helper.ClickElem(elmnt, mainFilter);

            if (selectall)
                FilterClickAll();
            HoverQuestionMark();
            By valueLocator = null;
            if (value1.Length > 1)
            {
                valueLocator = By.XPath($"//div[contains(@class,'simplebar-content')]//div[text()='{value1}']");
                valueLocator = By.XPath($"//div[contains(@class,'simplebar-content')]//span[text()='{value1}']");
                elmnt = helper.WaitForElement(driver, valueLocator);
                if (elmnt == null)
                {
                    valueLocator = By.XPath($"//span[text()='{value1}']");
                    elmnt = helper.WaitForElement(driver, valueLocator);
                }
                helper.ClickElem(elmnt, value1);
            }

            if (value2.Length > 1)
            {
                valueLocator = By.XPath($"//div[contains(@class,'simplebar-content')]//span[text()='{value2}']");
                elmnt = helper.WaitForElement(driver, valueLocator);
                helper.ClickElem(elmnt, value2);
            }
            HoverQuestionMark();
            Applybtn = helper.WaitForElement(driver, ApplyButtonLocator);
            helper.ClickElem(Applybtn, "apply.");
            helper.Sleep(1);
            WaitSpinner();
        }
        public void FilterClickAll()
        {
            FilterDateRangeSelectAllLocator = By.XPath("//*//following::label//span[text()='Select all']");
            elmnt = helper.WaitForElement(driver, FilterDateRangeSelectAllLocator, 2);
            if (elmnt != null)
            {
                helper.ClickElem(elmnt, "select all is canceled");
            }
        }
        public bool IsMainScreen()
        {
            UIReport.Report.WriteTestLog("verify: main screen is not blank.");
            IWebElement elmnt = helper.WaitForElement(driver, navigationlocator);
            return elmnt != null;
        }
        public bool IsFilterAndNavigationLinks()
        {
            ReadOnlyCollection<IWebElement> elmnts = helper.WaitForElements(driver, LinksLocator);
            if (elmnts != null && elmnts.Count > 0)
                UIReport.Report.WriteTestLog($"found elements count: {elmnts.Count}", testguid);
            return elmnts.Count >= 2;
        }
        public void ClickCloseFilter()
        {
            elmnt = helper.WaitForElement(driver, FilterIconLocator);
            if (elmnt != null)
                helper.ClickElem(elmnt, "close filter");
            helper.Sleep(0.4);
        }
        public void ClickGridTableExpand()
        {
            UIReport.Report.WriteTestLog("grid or table expand/collapse.", testguid);
            IWebElement expand = helper.WaitForElement(driver, tableexpandLocator);
            if (expand != null)
            {
                helper.ClickElem(expand, "grid or table expand/collapse.");
                helper.Sleep(0.5);
            }
        }
        public void ClickTableCollaps()
        {
            UIReport.Report.WriteTestLog("start check collaps button.");
            By donutlocator = By.XPath("//div[contains(@data-cap,'donut ')]");
            ReadOnlyCollection<IWebElement> elmnts = helper.WaitForElements(driver, donutlocator);
            if (elmnts.Count > 1 && !elmnts[0].Displayed)
            {
                UIReport.Report.WriteTestLog("table is expand => click collaps.");
                ClickGridTableExpand();
            }
        }
        public bool IsElementHasAttribute(string attributename, string value)
        {
            UIReport.Report.WriteTestLog("check: IsElementHasAttribute");
            By locator = By.XPath($"//*[contains({attributename},'{value}') or contains(text(),'ARM Accepted')]");
            ReadOnlyCollection<IWebElement> box = helper.WaitForElements(driver, locator);
            return box.Count > 0;
        }
        public void ClickClose()
        {
            CloseButtonLocator = By.XPath("//button[contains(@class,'Button') and contains(text(),'Close')]");
            IWebElement elmnt = helper.WaitForElement(driver, CloseButtonLocator);
            if (elmnt != null)
                helper.ClickElem(elmnt, "close");
        }
        public void ClearGridSearchBox()
        {
            UIReport.Report.WriteTestLog("click clear grid search-box.", testguid);
            elmnt = helper.WaitForElement(driver, ClearSearchBoxbuttonLocator);
            helper.ClickElem(elmnt, "clear search.");
            helper.Sleep(0.2);
        }
        public bool IsGridRowContains(string value)
        {
            UIReport.Report.WriteTestLog("start check: IsGridRowContains", testguid);
            int counter = 1;
            ReadOnlyCollection<IWebElement> elmnts = helper.WaitForElements(driver, RowsLocator);
            for (int i = 0; i < elmnts.Count; i++)
            {
                if (i == 0)
                {
                    counter++;
                    continue;
                }
                if (elmnts[i].Text.Contains(value))
                {
                    counter++;
                }
            }
            int result = Math.Abs(elmnts.Count - counter);
            UIReport.Report.WriteTestLog($"IsGridRowContains | result match is: {result}", testguid);
            if (result == 0 || result == 1)
                return true;
            return false;
        }
        public bool IsGridRowContainsMultipileOptions(List<string> items)
        {
            UIReport.Report.WriteTestLog("start check: IsGridRowContainsMultipileOptions");
            int counter = 1;
            ReadOnlyCollection<IWebElement> elmnts = helper.WaitForElements(driver, RowsLocator);
            for (int i = 0; i < elmnts.Count; i++)
            {
                string line = helper.GetElementValue(elmnts[i], "row");
                if (i == 0)
                {
                    counter++;
                    continue;
                }
                if (line.Contains(items[0]) || line.Contains(items[1]) || line.Contains(items[2]))
                {
                    counter++;
                }
            }
            int result = Math.Abs(elmnts.Count - counter);
            return result < 2;
        }
        public void ClickLoadMore()
        {
            int counter = 0;
            UIReport.Report.WriteTestLog("start method: click 'load more'", testguid);
            elmnt = helper.WaitForElement(driver, LoadMoreLocator);
            while (counter < 3)
            {
                helper.Sleep(0.7);
                elmnt = helper.WaitForElement(driver, LoadMoreLocator);
                if (elmnt != null)
                {
                    helper.ClickElem(elmnt, "load more.");
                    UIReport.Report.WriteTestLog("click 'load more'", testguid);
                }
                counter++;
            }
        }
        public void ClickTopDismissBarMessage()
        {
            UIReport.Report.WriteTestLog("try click top 'Dismiss' message", testguid);
            elmnt = helper.WaitForElement(driver, TopDismissLocator);
            if (elmnt != null)
                helper.ClickElem(elmnt, "top bar 'Dismiss' message.");
            helper.Sleep(0.1);
        }
        public bool IsFilterSyncWithGrid(string filter, List<string> items, List<string> griditems)
        {
            Console.WriteLine("checking Grid is filtering.");
            int counter = 0;
            for (int i = 0; i < items.Count; i++)
            {
                UIReport.Report.WriteTestLog($"start check {filter}: {items[i]}", testguid);
                Console.WriteLine($"start check {filter}: {items[i]}", testguid);
                HoverQuestionMark();
                FilterSelect($"{filter}", items[i], "");
                GridBar grid = new GridBar(driver, testguid);
                grid.WaitForGridData();
                ClickGridTableExpand();
                if (grid.GetGridTable(griditems[i]))
                    counter++;
                FilterSelect($"{filter}", "", "", true);
                Thread.Sleep(500);
            }
            ClickGridTableExpand();
            int result = Math.Abs(items.Count - counter);
            if (result == 0 || result == 1)
                return true;
            return false;
        }
        public void LogOffandLoginAgain()
        {
            ClickUserButton();
            UIReport.Report.WriteTestLog($"user going to logout.", testguid);
            locator = By.XPath("//button[contains(text(),'Log out')]");
            elmnt = helper.WaitForElement(driver, locator);
            helper.ClickElem(elmnt, "log out.");
            helper.Sleep(0.6);
        }
        public void ClickLinkLogin()
        {
            UIReport.Report.WriteTestLog("start click link to login.", testguid);
            locator = By.XPath("//a[@href]");
            elmnt = helper.WaitForElement(driver, locator);
            helper.ClickElem(elmnt, "link to login");
        }
        public void ClickUserButton()
        {
            By locator = By.XPath("//*[contains(@class,'_UserButton')]");
            IWebElement elmnt = helper.WaitForElement(driver, locator, 2);
            UIReport.Report.WriteTestLog($"click user button: {GetUserDashboardName()}", testguid);
            helper.ClickElem(elmnt, "user button.");
        }
        public string GetUserDashboardName()
        {
            IWebElement userelmnt = helper.WaitForElement(driver, By.XPath("//span[contains(text(), 'avaAuto') or contains(text(), 'tmAuto')]"));
            if (userelmnt != null)
            {
                return helper.GetElementValue(userelmnt, "dashboard-user");
            }
            return "UNKNOW";
        }
        public void SelectDashboardLanguage(string value)
        {
            ClickUserButton();
            By locator = By.XPath($"//label[contains(text(),'{value}')]");
            IWebElement elmnt = helper.WaitForElement(driver, locator);
            helper.ClickElem(elmnt, value);
            WaitForMainDashboard();
        }
        public void WaitForMainDashboard()
        {
            UIReport.Report.WriteTestLog("start waiting for main dashboard display", testguid);
            for (int i = 0; i < 45; i++)
            {
                ReadOnlyCollection<IWebElement> table = helper.WaitForElements(driver, LinksLocator, 1);
                if (table.Count < 2)
                {
                    UIReport.Report.WriteTestLog("waiting for main dashboard display", testguid);
                    helper.Sleep(1);
                }
                else if (table[1] != null && table[1].Enabled && table[1].Displayed)
                {
                    UIReport.Report.WriteTestLog($"main dashboard is display - break loop - [{i}]", testguid);
                    break;
                }
                if (i == 44)
                    throw new Exception("throw exception due to - data is not display.");
            }
            Thread.Sleep(800);
        }
        public void WaitSpinner()
        {
            UIReport.Report.WriteTestLog("start check spinner.", testguid);
            helper.Sleep(2);
            IWebElement spinner = null;
            for (int i = 0; i < 30; i++)
            {
                try
                {
                    spinner = helper.WaitForElement(driver, spinnerlocator, 1);
                    if (spinner == null)
                        break;
                    spinner = helper.WaitForElement(driver, spinnerlocator, 1);
                    if (spinner != null && spinner.Displayed)
                    {
                        UIReport.Report.WriteTestLog("spinner is display => waiting", testguid);
                        helper.Sleep(1);
                        continue;
                    }
                    if (spinner != null && !spinner.Displayed && i > 2)
                    {
                        UIReport.Report.WriteTestLog($"spinner is NOT display - break loop - [{i}]", testguid);
                        break;
                    }
                    if (spinner != null)
                        UIReport.Report.WriteTestLog($"spinner is display => waiting | iteration: {i}", testguid);
                }
                catch (Exception ex)
                {
                }
                Thread.Sleep(100);
                try
                {
                    spinner = helper.WaitForElement(driver, spinnerlocator, 1);
                    if (spinner != null && spinner.Displayed && i == 29)
                        throw new Exception("throw exception due to - data is not display.");
                }
                catch
                {
                }
            }
            UIReport.Report.WriteTestLog("spinner loop - is END.", testguid);
            helper.Sleep(1);
        }
        public void ClickEnter(IWebElement elmnt)
        {
            elmnt.SendKeys(Keys.Enter);
        }
        public void ClickFilterLink(string value)
        {
            UIReport.Report.WriteTestLog($"click button: {value}", testguid);
            By locator = By.XPath($"//a[contains(text(),'{value}')]");
            elmnt = helper.WaitForElement(driver, locator);
            helper.ClickElem(elmnt, value);
            helper.Sleep(1);
            WaitSpinner();
        }
        public bool IsUrlContain(string value)
        {
            UIReport.Report.WriteTestLog($"driver | current url: {driver.Url}", testguid);
            return driver.Url.Contains(value);
        }
        public void DateTimeFilter_ClickAllTime()
        {
            if (driver.Url.Contains("/data/static") || driver.Url.Contains("/data/transactions"))
            {
                UIReport.Report.WriteTestLog("start click all_time - uncheck.", testguid);
                By locator = By.XPath("//div[contains(@class,'styles_') and@disabled]");
                IWebElement elmnt = helper.WaitForElement(driver, locator);
                if (elmnt != null && elmnt.Displayed)
                {
                    helper.ClickElem(elmnt, "all time");
                    UIReport.Report.WriteTestLog("all_time - is uncheck.", testguid);
                }
            }
        }
        public bool IsElement(string valueLocator)
        {
            By locator = By.XPath(valueLocator);
            IWebElement elmnt = helper.WaitForElement(driver, locator);
            return elmnt != null;
        }
        public string SwitchAccount()
        {
            string lei = "";
            IWebElement ell = helper.WaitForElement(driver, Accountlocator);
            helper.ClickElem(ell, "accounts");
            helper.Sleep(0.2);
            ReadOnlyCollection<IWebElement> accountslist = helper.WaitForElements(driver, By.XPath("//input[@id]"));
            lei = helper.GetElementValue(accountslist[5], "account");
            helper.ClickElem(accountslist[5], "");
            return lei;
        }
        public string GetCellValueSelectedRow(string value)
        {
            UIReport.Report.WriteTestLog($"table row data | try get cell {value} value from selected row.", testguid);
            string text = "";
            By locator = By.XPath($"//*[contains(@class,'tr selected true')]//div[@role='cell' and contains(@class,'{value}')]");
            IWebElement elmnt = helper.WaitForElement(driver, locator);
            text = helper.GetElementValue(elmnt, value);
            UIReport.Report.WriteTestLog($"table row data | found text: {text} - from selected row.", testguid);
            return text;
        }
        public string GetTableCellDataByName(string value)
        {
            By locator = By.XPath($"//*[contains(@class,'{value}')]");
            ReadOnlyCollection<IWebElement> elmnts = helper.WaitForElements(driver, locator);
            string msg = helper.GetElementValue(elmnts[0], "get cell data");
            return msg;
        }
    }
}
