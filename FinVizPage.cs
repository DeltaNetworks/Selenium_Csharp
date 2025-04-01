using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFram.PageObjects
{
    internal class FinVizPage : BasePageObjects
    {
        private By locator;
        private IWebElement Elmnt;
        public FinVizPage(IWebDriver _driver, string _testguid) : base(_driver, _testguid)
        {
        }

        public void Screener(string stock)
        {
            locator = By.XPath("//input[@placeholder]");
            helper.TrySetTextInField(stock, locator, "search");
            Elmnt = helper.WaitForElement(driver, locator);
            Elmnt.SendKeys(Keys.Enter);
            helper.Sleep(3);
            //IList<string> windowHandles = new List<string>(driver.WindowHandles);
            //driver.SwitchTo().Window(windowHandles[1]);
        }
        public void OpenTab()
        {
            driver.SwitchTo().NewWindow(WindowType.Tab);
            driver.Navigate().GoToUrl(gdata.url_finviz);
            helper.Sleep(3);
        }
    }
}
