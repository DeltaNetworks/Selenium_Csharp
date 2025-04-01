using AutoFram.BasePages;
using AutoFram.DataTest;
using AutoFram.PageObjects;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFram.TestCase.Stock_Trade
{
    internal class Finviz
    {
    }
    [Parallelizable]
    public class FinvizTest1 : BaseUI
    {
        [Test, Category("finviz"), TestCase("Chrome")]
        public void Ronen1(string browserName)
        {
            SetUp(browserName);
            driver.Navigate().GoToUrl(globalData.url_finviz);
            Thread.Sleep(3000);
            FinVizPage fin = new FinVizPage(driver, test.testguid);
            fin.Screener("msft");
            fin.OpenTab();
            fin.Screener("indi");
            fin.OpenTab();
            fin.Screener("intc");
            fin.OpenTab();
            fin.Screener("RCAT");
            fin.OpenTab();
            fin.Screener("PLX");
            fin.OpenTab();
            fin.Screener("BDN");
            fin.OpenTab();
            fin.Screener("HITI");
        }
    }
}
