using AutoFram.UIReport;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFram.Browsers
{
    internal class BrowserHelper
    {
        private IWebDriver _driver;

        private const double renderwait = 0.3;

        public BrowserHelper(IWebDriver driver)
        {
            _driver = driver;
        }

        public void ClickElem(IWebElement el, string elementName)
        {
            Report.WriteTestLog($"ClickElem: {elementName}");
            if (el == null)
                throw new Exception(string.Format($"Failed to Click on [{elementName}] - element can't be empty!"));
            IWebElement temp = el;

            try
            {
                try
                {
                    el.Click();
                    Console.WriteLine($"Click Elem: {elementName}");
                }
                catch (Exception e)
                {
                    Actions act = new Actions(_driver);
                    act.Click(temp).Release().Perform();
                    //((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", el);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(string.Format($"Failed to Click on [{elementName}]."));
            }
            Sleep(renderwait * 2);
        }
        public IWebElement WaitForElement(IWebDriver driver, By locator, int timeOutInSeconds = 5)
        {
            Report.WriteTestLog($"WaitForElement: {locator}");
            //WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeOutInSeconds));
            //IWebElement element = null;
            //try
            //{
            //    element = wait.Until(x => x.FindElement(locator));
            //    Sleep(renderwait);
            //    return element;
            //}
            //catch (Exception e)
            //{
            //    Log.Info($"element: {locator} is empty.");
            //    Log.Error($"url: {_driver.Url}");
            //    return element;
            //}
            IWebElement elmnt = null;
            for (int i = 0; i < 12; i++)
            {
                try
                {
                    elmnt = driver.FindElement(locator);
                }
                catch
                {
                }
                if (elmnt != null)
                {
                    Report.WriteTestLog($"element {locator} found iteration: {i}");
                    break;
                }
                Thread.Sleep(300);
            }
            return elmnt;
        }
        public ReadOnlyCollection<IWebElement> WaitForElements(IWebDriver driver, By locator, int timeOutInSeconds = 5)
        {
            //Report.WriteTestLog($"WaitForElements: {locator}");
            //ReadOnlyCollection<IWebElement> elemntCollection = new ReadOnlyCollection<IWebElement>(new List<IWebElement>());
            //WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeOutInSeconds));
            //try
            //{
            //    elemntCollection = wait.Until(x => x.FindElements(locator));
            //    Sleep(renderwait);
            //    return elemntCollection;
            //    //return (ReadOnlyCollection<IWebElement>)wait.Until(x => x.FindElements(locator));
            //    //return (ReadOnlyCollection<IWebElement>)wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(locator));
            //}
            //catch (Exception e)
            //{
            //    if (e.Message.Contains("invalid session"))
            //        driver = null;
            //    Log.Info($"element collection: {locator} is empty.");
            //    return elemntCollection;
            //}
            ReadOnlyCollection<IWebElement> elemntCollection = null;
            for (int i = 0; i < 12; i++)
            {
                try
                {
                    elemntCollection = driver.FindElements(locator);
                }
                catch
                {
                }
                if (elemntCollection.Count > 0)
                {
                    Report.WriteTestLog($"elements collection {locator} found iteration: {i}");
                    break;
                }
                Thread.Sleep(300);
            }
            return elemntCollection;
        }
        public void TrySetTextInField(string setValue, By locator, string fieldName, int timeOut = 0, bool toCleanField = true, bool isPressTAB = false, Action method = null)
        {
            var elements = WaitForElements(_driver, locator, timeOut);
            if (elements.Count > 0)
            {
                ScrollElementIntoView(_driver, elements.ElementAt(0));
                SetTextToInputField(elements.First(), fieldName, setValue, toCleanField, isPressTAB);
                if (method != null)
                {
                    method.Invoke();
                }
            }
        }
        public void ScrollElementIntoView(IWebDriver driver, IWebElement element)
        {
            if (element != null)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
                ((IJavaScriptExecutor)driver).ExecuteScript("javascript:window.scrollBy(0, 300)");
            }
        }
        internal void SetTextToInputField(IWebElement inputElem, String inputElementText, String sValue, bool clearField = true, bool useTab = false, bool verify = false)
        {
            Report.WriteTestLog("helper | Set Text");
            if (inputElem == null)
                throw new Exception(string.Format($"Failed, input control with [{inputElementText}] text: {sValue},  can't be Empty!"));

            Actions act = new Actions(_driver);
            act.MoveToElement(inputElem).Build().Perform();

            string actual = "";
            try
            {
                try
                {
                    inputElem.Click();
                }
                catch
                {
                    act.ClickAndHold(inputElem).Release().Build().Perform();
                }
                Sleep(0.1);

                if (clearField)
                {
                    inputElem.SendKeys(Keys.Control + "a");
                    inputElem.SendKeys(Keys.Delete);
                }

                inputElem.SendKeys(sValue);

                if (useTab)
                {
                    act.SendKeys(Keys.Tab).Release().Build().Perform();
                }
                Sleep(0.2);

                if (verify)
                {
                    actual = GetElementValue(inputElem, inputElementText);
                    if (actual == sValue)
                    {
                    }
                    else
                    {
                        Report.WriteTestLog($"SetTextToInputField is failded | value = {sValue}");
                        throw new Exception();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format($"Failed Set [{sValue}] value to [{inputElementText}] field, actual [{actual}] value was set."), ex);
            }
        }
        internal void SetTextToInputField4(IWebElement inputElem, String inputElementText, String sValue, bool clearField = true, bool useTab = false, bool verify = false)
        {
            Report.WriteTestLog("helper | Set Text");
            if (inputElem == null)
                throw new Exception(string.Format($"Failed, input control with [{inputElementText}] text: {sValue},  can't be Empty!"));

            Actions act = new Actions(_driver);
            act.MoveToElement(inputElem).Build().Perform();
            Thread.Sleep(100);
            if (clearField)
            {
                inputElem.SendKeys(Keys.Control + "a");
                inputElem.SendKeys(Keys.Delete);
            }

            inputElem.SendKeys(sValue);
            Thread.Sleep(50);
        }
        internal void Sleep(double seconds)
        {
            Thread.Sleep(TimeSpan.FromSeconds(seconds));
        }
        public string GetElementValue(IWebElement element, string elementName)
        {
            string result = "";
            try
            {
                new Actions(_driver).MoveToElement(element).Release();
                result = element.GetAttribute("value");
            }
            catch (Exception e)
            {
            }
            if (result == null)
                result = element.Text;
            return result.Trim();
        }
        public string GetElementValueJScript(IWebElement element, string locator, string elementName)
        {
            string result = "";
            try
            {
                new Actions(_driver).MoveToElement(element).Release();
                result = ((IJavaScriptExecutor)_driver).ExecuteScript($"return document.GetElementByXpath('{locator}').value").ToString();
                //result = element.GetAttribute("value");
            }
            catch (Exception e)
            {
            }
            if (result == null)
                result = element.Text;
            return result.Trim();
        }
        public bool WaitForElementToDisappear(IWebDriver driver, By locator, int timeOutInSeconds = 60)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeOutInSeconds));
            IWebElement elmnt = wait.Until(x => x.FindElement(locator));
            if (elmnt != null)
                return wait.Until(x => x.FindElement(locator)) == null;
            return true;
        }
        public void MouseClickXYposition(IWebDriver driver, IWebElement elmnt, int addx)
        {
            Actions action = new Actions(driver);
            Point point = elmnt.Location;
            int cordy = point.Y; cordy += addx;
            //action.MoveByOffset(5, cordy).Build().Perform();
            action.Click();
        }
        public void MouseOverSelectChildAndClick(IWebDriver driver, By mouseOverElementLocator, string selectFromListItemName, string mouseOverElementName)
        {
            var mouseOverElement = WaitForElement(driver, mouseOverElementLocator, 10);
            Actions act = new Actions(driver);
            if (mouseOverElement == null)
                throw new Exception($"Failed to find [{mouseOverElementName}], please check input params.");

            //mouseOverElement.Click();
            MouseHover(driver, mouseOverElement); // available to click on element too

            var childList = mouseOverElement.FindElements(By.XPath("//ul[contains(@class,'drop')]//li")).Where(se => se.Text.Contains(selectFromListItemName));
            if (childList.Count() == 0)
            {
                var parent = mouseOverElement.FindElement(By.XPath("./parent::*"));
                childList = parent.FindElements(By.XPath($"//span[contains(text(),'{selectFromListItemName}')]"));
            }

            if (childList.Count() > 0)
            {

                act.MoveToElement(childList.First()).Perform();
                try
                {
                    act.Click(childList.First());//.Release().Build().Perform();
                }
                catch
                {
                    childList.First().Click();
                }
                act.MoveByOffset(-100, 50).Perform();
                // loger   Steps($"Selected [{selectFromListItemName}] from drop down {mouseOverElementName}.");
            }
            else
            {
                act.MoveByOffset(-100, 50).Perform();
                throw new Exception($"Failed to select [{selectFromListItemName}] from  [{mouseOverElementName}].");
            }
        }
        public bool ExecuteActionUntill(Func<bool> func, int timeOut = 10, string errorMessage = "")
        {
            string errorInFunc = string.Empty;
            try
            {
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeOut));
                wait.Until((x) =>
                {
                    try
                    {
                        return func();
                    }
                    catch (Exception ex)
                    {
                        errorInFunc = ex.Message;
                        return false;
                    }

                });
            }
            catch (Exception e)
            {
                if (errorMessage != null)
                {
                    throw new Exception("Your error: " + errorMessage + ". Error from function: " + errorInFunc + ". Original error: " + e.Message);
                }
                else
                    throw new Exception("Error from function: " + errorInFunc + ". Original error: " + e.Message);
            }
            return true;
        }
        internal void MouseHover(IWebDriver driver, IWebElement element)
        {
            Report.WriteTestLog($"mouse hover on: {element}");
            new Actions(driver).MoveToElement(element).Perform();
            Sleep(0.2);
        }
        public void CheckPageErrors(By elLocator, int timeOut = 15)
        {
            IWebElement result = null;

            try
            {
                result = WaitForElement(_driver, elLocator, timeOut);
                if (result != null)
                {
                    return;
                }
            }
            catch (Exception e)
            {
                throw new Exception($"exception: HTML ITEM: {elLocator.ToString()} - DID NOT FOUND.");
            }
        }
        public bool WaitUntillClickable(IWebElement el, IWebDriver driver, string elname, int timeOut = 50)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeOut));
                wait.Until((d) =>
                {
                    try
                    {
                        return el.Displayed && el.Enabled;
                    }
                    catch
                    {
                        return false;
                    }
                });
            }
            catch
            {
                return false;
            }

            return true;
        }
        public string GetElementColor(IWebDriver driver, IWebElement el, string elname, int timeOut = 20)
        {
            string color = null;
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeOut));
                wait.Until((d) =>
                {
                    try
                    {
                        if (el.Displayed)
                        {
                            color = el.GetCssValue("color");
                        }
                        if (color == null)
                            Sleep(0.5);
                        return el.GetCssValue("color");
                    }
                    catch
                    {
                        return color;
                    }
                });
            }
            catch
            {
                return color;
            }
            return color;
        }

        public string GetElementBackGroundColor(IWebDriver driver, IWebElement el, string elname, int timeOut = 20)
        {
            string color = null;
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeOut));
                wait.Until((d) =>
                {
                    try
                    {
                        if (el.Displayed)
                        {
                            color = el.GetCssValue("background");
                        }
                        if (color == null)
                            Sleep(0.5);
                        return el.GetCssValue("background");
                    }
                    catch
                    {
                        return color;
                    }
                });
            }
            catch
            {
                return color;
            }
            return color;
        }
        public void SelectFromDropDown(IWebElement dropDown, string selectValue, string elementName)
        {
            if (dropDown == null || selectValue == null)
                throw new Exception($"Failed to select [{selectValue}] value in [{elementName}] dropdown. DropDown element or value is null!");

            IReadOnlyCollection<IWebElement> myChose = new ReadOnlyCollection<IWebElement>(new List<IWebElement>());
            List<IWebElement> choses = new List<IWebElement>();
            Actions act = new Actions(_driver);
            try
            {
                //Sleep(0.2);
                act.MoveToElement(dropDown).Build().Perform();
                Sleep(0.2);
                //act.MoveToElement(dropDown).Perform();   //div[contains(@class,'menu-')]//div[contains(text(),'Equity')]
                dropDown.Click();
                Sleep(0.2);
            }
            catch (Exception e)
            {
                act.ClickAndHold(dropDown).Release().Build().Perform();
            }
            var DDListValues = WaitForElements(_driver, By.XPath($"//div[contains(@class,'menu-')]//div[contains(text(),'{selectValue}')]"));
            choses = DDListValues.Where(x => DeleteAllSpaces(new string(x.Text.Where(char.IsLetterOrDigit).ToArray())).ToLower().Contains(new String(selectValue.Where(y => char.IsLetterOrDigit(y)).ToArray()).ToLower())).ToList();
            if (DDListValues.Count > 0)
            {
                act.MoveToElement(DDListValues.First()).Build().Perform();
                Sleep(0.2);
                DDListValues.First().Click();
                Sleep(0.2);
            }
        }
        public void DragAndDrop(IWebElement first, IWebElement last)
        {
            Actions action = new Actions(_driver);
            //action.DragAndDrop(first, last).Perform();
            //action.ClickAndHold(first).Release().Build().Perform();
            var drag = action.ClickAndHold(first).MoveToElement(last).Release().Build();
            drag.Perform();
            Report.WriteTestLog($"mouse drag and drop: {first.Text} and {last.Text}");
            Sleep(0.2);
        }
        public void ClickHoldDrag(IWebElement first)
        {
            Actions action = new Actions(_driver);
            ClickElem(first, "");
            action.ClickAndHold(first).Perform();
            for (int i = 2; i < 28; i++)
            {
                MouseHover(_driver, WaitForElement(_driver, By.XPath($"//span[text()='{i}']")));
                Sleep(0.1);
            }
            action.Release();
        }
        public void WaitRenderHtml(IWebDriver driver, By elLocator, int timeOut = 20)
        {
            IWebElement result = null;
            try
            {
                result = WaitForElement(driver, elLocator, timeOut);
                if (result != null)
                {
                    UIReport.Report.WriteTestLog($"element: {elLocator.ToString()} -is found.");
                    return;
                }
            }
            catch (Exception e)
            {
                throw new Exception($"HTML: {elLocator} - ITEM NOT ENABLE OR DID NOT FOUND.");
            }
        }
        private string DeleteAllSpaces(string value)
        {
            return value.Replace(" ", "");
        }
    }
}
