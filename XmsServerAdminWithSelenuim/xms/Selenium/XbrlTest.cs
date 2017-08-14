using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Selenium.Common;
using Selenium.Models;

namespace Selenium
{
    [TestClass]
    public class XbrlTest
    {
        public XbrlTest()
        {
            if (SeleniumDriver.driver==null)
            {
                SeleniumDriver.driver = new ChromeDriver(@"C:\Users\tomerb\Documents\Visual Studio 2015\Projects\xms\Selenium");
                SeleniumDriver.driver.Manage().Window.Maximize();
                SeleniumDriver.driver.Navigate().GoToUrl("https://israel.consist.co.il:7540/XMSServerAdmin/Tabs/Index");
                SeleniumGridHelper.Wait = new WebDriverWait(SeleniumDriver.driver, TimeSpan.FromSeconds(10));
            }
        }

        [TestMethod]
        public void Test1_LocalTaxonomyReports()
        {
            SeleniumGridHelper.SetTestSetting(4,
                "localReportGrid",
                "test",
                "testUpdated",
                "taxonomyForLocalReport",  
                1,
                true,
                new List<ColumnsToEdit>()
                {
                    new ColumnsToEdit() {ColumnNum = 1, ColumnName = "Id"},
                    new ColumnsToEdit() {ColumnNum = 3, ColumnName = "Description"}
                }, 
                new List<DropDownsToEdit>()
                {
                    new DropDownsToEdit() {ContainsEmptyValue = false, ColumnNum = 2, selectedValue = 1 },
                    new DropDownsToEdit() {ContainsEmptyValue = true, ColumnNum = 4, selectedValue = 1 },
                    new DropDownsToEdit() {ContainsEmptyValue = true, ColumnNum = 5, selectedValue = 1 }
                });


            SeleniumGridHelper.ChangeTaxonomyDropDown();
            SeleniumGridHelper.TestGrid(); 
        }

        [TestMethod]
        public void Test2_TaxonomyReports()
        {
            SeleniumGridHelper.SetTestSetting(3,
                "taxonomyReportGrid",
                "test",
                "testUpdated",
                 "taxonomyForTaxonomyReport",
                1,
                false,
                new List<ColumnsToEdit>()
                {
                    new ColumnsToEdit() {ColumnNum = 1, ColumnName = "Id"},
                    new ColumnsToEdit() {ColumnNum = 2, ColumnName = "Description"},
                    new ColumnsToEdit() {ColumnNum = 4, ColumnName = "EntryUri"},
                    new ColumnsToEdit() {ColumnNum = 5, ColumnName = "FileName"},
                    new ColumnsToEdit() {ColumnNum = 6, ColumnName = "EntitySchema"},
                    new ColumnsToEdit() {ColumnNum = 7, ColumnName = "EntityIdentifire"},
                },
                new List<DropDownsToEdit>()
                {
                    new DropDownsToEdit() {ContainsEmptyValue = false, ColumnNum = 3, selectedValue = 1 }, 
                });


            SeleniumGridHelper.ChangeTaxonomyDropDown();
            SeleniumGridHelper.TestGrid();
        }

        [TestMethod]
        public void Test3_TaxonomyEntity()
        {
            SeleniumGridHelper.SetTestSetting(1,
                "taxonomyGrid",
                "test",
                "testUpdated",
                 "",
                0,
                true,
                new List<ColumnsToEdit>()
                {
                    new ColumnsToEdit() {ColumnNum = 1, ColumnName = "TaxonomyId"},
                    new ColumnsToEdit() {ColumnNum = 2, ColumnName = "Description"}, 
                    new ColumnsToEdit() {ColumnNum = 4, ColumnName = "EntityIdentifier"},
                    new ColumnsToEdit() {ColumnNum = 5, ColumnName = "Currency"},
                    new ColumnsToEdit() {ColumnNum = 6, ColumnName = "Decimals"},
                    new ColumnsToEdit() {ColumnNum = 7, ColumnName = "EntitySchema"}, 
                    new ColumnsToEdit() {ColumnNum = 9, ColumnName = "TnProcessorId"},
                },
                null);

            SeleniumGridHelper.TestGrid(); 
             
        }

        [TestMethod]
        public void Test4_localEntityGrid()
        {
            SeleniumGridHelper.SetTestSetting(2,
                "localEntityGrid",
                "test",
                "testUpdated",
                 "",
                0,
                true,
                new List<ColumnsToEdit>()
                {
                    new ColumnsToEdit() {ColumnNum = 1, ColumnName = "Id"},
                    new ColumnsToEdit() {ColumnNum = 2, ColumnName = "Description"},  
                },
                null);

            SeleniumGridHelper.TestGrid();

            SeleniumDriver.driver.Quit();
        }

        [TestMethod]
        public void CheckFileDownloaded()
        {

            if (!FileDownloadHelper.CheckFileDownloaded("XBRL"))
            {
                Assert.Fail("File not Downloaded ");
            }

            SeleniumDriver.driver.Quit();
        }  
    }
}
