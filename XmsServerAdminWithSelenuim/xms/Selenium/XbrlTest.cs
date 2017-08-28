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
using AutoIt;

namespace Selenium
{
    [TestClass]
    public class XbrlTest
    {

        //need to chack 
        // tabs elmentId ???  (tabstrip || AdminTabs)


        public XbrlTest()
        {
            if (SeleniumDriver.driver == null)
            {
                SeleniumDriver.driver = new ChromeDriver(@"C:\Users\tomerb\Documents\Visual Studio 2015\Projects\xms\Selenium");
                SeleniumDriver.driver.Manage().Window.Maximize();
                SeleniumDriver.driver.Navigate().GoToUrl("https://israel.consist.co.il:7540/XMSServer");//"https://israel.consist.co.il:7540/XMSServerAdmin/Tabs/Index"
                WaitHelper.Wait = new WebDriverWait(SeleniumDriver.driver, TimeSpan.FromSeconds(10));

                //AutoItX.WinWait("Authentication Required");

                AutoItX.WinActivate("Authentication Required");
                AutoItX.Send("dev2");
                AutoItX.Send("{TAB}");
                AutoItX.Send("dev2");
                AutoItX.Send("{ENTER}");

                GridHelper.TabId = "tabstrip";

            }
        }

        [TestMethod]
        public void Test1_LocalTaxonomyReports()
        {

            GridHelper.SetTestSetting(4,
                "localReportGrid",
                "test",
                "testUpdated",
                "taxonomyForLocalReport",
                1,
                true,
                new List<ColumnsToEdit>()
                {
                    new ColumnsToEdit() {ColumnNum = 1, ColumnName = "Id" ,Required= true},
                    new ColumnsToEdit() {ColumnNum = 3, ColumnName = "Description"}
                },
                null,
                new List<DropDownsToEdit>()
                {
                    new DropDownsToEdit() {ContainsEmptyValue = false, ColumnNum = 2, selectedValue = 1 },
                    new DropDownsToEdit() {ContainsEmptyValue = true, ColumnNum = 4, selectedValue = 1 ,ColumnName= "PeriodType"},
                    new DropDownsToEdit() {ContainsEmptyValue = true, ColumnNum = 5, selectedValue = 1 }
                });

            GridHelper.ChangeTaxonomyDropDown();

            GridHelper.TestGrid();
        }

        [TestMethod]
        public void Test2_TaxonomyReports()
        {
            GridHelper.SetTestSetting(3,
                "taxonomyReportGrid",
                "test",
                "testUpdated",
                 "taxonomyForTaxonomyReport",
                1,
                false,
                new List<ColumnsToEdit>()
                {
                    new ColumnsToEdit() {ColumnNum = 1, ColumnName = "Id",Required= true},
                    new ColumnsToEdit() {ColumnNum = 2, ColumnName = "Description",Required= true},
                    new ColumnsToEdit() {ColumnNum = 4, ColumnName = "EntryUri",Required= true},
                    new ColumnsToEdit() {ColumnNum = 5, ColumnName = "FileName",Required= true},
                    new ColumnsToEdit() {ColumnNum = 6, ColumnName = "EntitySchema",Required= true},
                    new ColumnsToEdit() {ColumnNum = 7, ColumnName = "EntityIdentifire",Required= true},
                },
                new List<string>()
                {
                     "EntityIdentifire"
                },
                new List<DropDownsToEdit>()
                {
                    new DropDownsToEdit() {ContainsEmptyValue = false, ColumnNum = 3, selectedValue = 1 },
                });

            GridHelper.ChangeTaxonomyDropDown();
            GridHelper.TestGrid();
        }

        [TestMethod]
        public void Test3_TaxonomyEntity()
        {
            GridHelper.SetTestSetting(1,
                "taxonomyGrid",
                "test",
                "testUpdated",
                 "",
                0,
                true,
                new List<ColumnsToEdit>()
                {
                    new ColumnsToEdit() {ColumnNum = 1, ColumnName = "TaxonomyId",Required= true},
                    new ColumnsToEdit() {ColumnNum = 2, ColumnName = "Description",Required= true},
                    new ColumnsToEdit() {ColumnNum = 4, ColumnName = "EntityIdentifier",Required= true},
                    new ColumnsToEdit() {ColumnNum = 5, ColumnName = "Currency",Required= true},
                    new ColumnsToEdit() {ColumnNum = 6, ColumnName = "Decimals",Required= true},
                    new ColumnsToEdit() {ColumnNum = 7, ColumnName = "EntitySchema",Required= true},
                    new ColumnsToEdit() {ColumnNum = 9, ColumnName = "TnProcessorId",Required= true},
                },
                null,
                null);

            GridHelper.TestGrid();

        }

        [TestMethod]
        public void Test4_localEntityGrid()
        {
            GridHelper.SetTestSetting(2,
                "localEntityGrid",
                "test",
                "testUpdated",
                 "",
                0,
                true,
                new List<ColumnsToEdit>()
                {
                    new ColumnsToEdit() {ColumnNum = 1, ColumnName = "Id",Required= true},
                    new ColumnsToEdit() {ColumnNum = 2, ColumnName = "Description",Required= true},
                },
                 null,
                null);

            GridHelper.TestGrid();

            SeleniumDriver.driver.Quit();
        }
         
        [TestMethod]
        public void CheckMainPage()
        {
            GridHelper.IframeInContainerId = "";
            MainPageHelper.periodTypes = new List<string>()
            {
                "חודשי",
                "רבעוני",
                "שנתי",
                "חצי-שנתי",
                "מיידי"
            };

            MainPageHelper.CheckUpperBarAndFilters();
            MainPageHelper.PeriodTypeFilter();


        }

        [TestMethod]
        public void CreateReport()
        {
            GridHelper.IframeInContainerId = "adminModal";

            FileDownloadHelper.DownloadEmptyTempletAndUpdate();

            FileDownloadHelper.CheckTempletsList();

            FileDownloadHelper.DeleteTemplet();

            FileDownloadHelper.DownloadReport();

            FileDownloadHelper.UploadEditedExcelReport();
        }

        [TestMethod]
        public void CheckFileDownloaded()
        {

            if (!FileDownloadHelper.CheckFileDownloaded("XBRL"))
            {
                Assert.Fail("File not Downloaded");
            }

            SeleniumDriver.driver.Quit();
            SeleniumDriver.driver.Dispose();

        } 

        [TestMethod]
        public void XbrlAdminServer()
        {
            AdminPageHelper.OpenAdminPage();

            AdminPageHelper.TaxonomyList = new List<string>() {
                "גרסה 5.0 בנק ישראל", "גרסה 4.0 בנק ישראל", "גרסה 3.2 בנק ישראל", "גרסה 3.1 בנק ישראל", "גרסה 2.1 בנק ישראל", "גרסה 5.1.1 בנק ישראל"
            };

            AdminPageHelper.RolesToAdd = new List<int>() { 1, 2 };

            GridHelper.TabId = "AdminTabs";

            GridHelper.SetTestSetting(1,
              "entitiesGrid",
              "test",
              "testUpdated",
              "",
              0,
              true,
              new List<ColumnsToEdit>()
              {
                    new ColumnsToEdit() {ColumnNum = 0, ColumnName = "EntityName" ,Required= true}
              },
               null,
              new List<DropDownsToEdit>()
              {
                    new DropDownsToEdit() {ContainsEmptyValue = false, ColumnNum = 1, selectedValue = 0}
              });


            AdminPageHelper.SwitchToIframeInContainerId("adminModal");

            AdminPageHelper.AddRolesToEntity();

            SeleniumDriver.driver.Quit();
        }

    }
}



