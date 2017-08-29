using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.IO;
using System;
using Microsoft.Office.Interop.Excel;


namespace Selenium.Common
{
    public static class FileDownloadHelper
    {

        public static void DownloadEmptyTempletAndUpdate()
        {
            WaitHelper.Wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("xms-reports-filter-period-type")));
            var container = SeleniumDriver.driver.FindElements(By.ClassName("cards-container"));

            var buttonadmin = container[0].FindElements(By.ClassName("card-button-admin"));
            var cardcontent = container[0].FindElement(By.ClassName("card-content"));
            var cardname = "d" + container[0].FindElement(By.ClassName("card-name")).Text;
            var cardbuttons = cardcontent.FindElements(By.ClassName("btn-group"));
            var dropdown = cardbuttons[1].FindElement(By.ClassName("dropdown-toggle"));

            dropdown.Click();

            var dropdownmenu = cardbuttons[1].FindElement(By.ClassName("dropdown-menu"));

            var emptyTemplete = dropdownmenu.FindElements(By.XPath("//*[contains(text(), 'תבנית ריקה')]"));

            emptyTemplete[0].Click();
            Thread.Sleep(3000);
            WaitHelper.Wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.ClassName("loading-message")));
            if (!CheckFileDownloaded(cardname))
            {
                Assert.Fail("Fail to Download Empty Templet");
            }

            buttonadmin[0].Click();

            WaitHelper.Wait.Until(ExpectedConditions.ElementIsVisible(By.Id("report-admin-modal")));

            AdminPageHelper.SwitchToIframeInContainerId("report-admin-modal");

            GridHelper.GridId = "templatesGrid";
            GridHelper.CreateNewClick();

            WaitHelper.Wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/div[5]")));
            var windowNewTemplete = SeleniumDriver.driver.FindElement(By.XPath("/html/body/div[5]"));

            var description = windowNewTemplete.FindElement(By.Id("Description"));
            description.SendKeys("test");
            string path = Environment.GetEnvironmentVariable("USERPROFILE") + "\\Downloads\\" + cardname + ".xlsm";
            windowNewTemplete.FindElement(By.Id("templateUpload")).SendKeys(path);

            var updatebtn = windowNewTemplete.FindElement(By.ClassName("k-grid-update"));
            Thread.Sleep(500);
            updatebtn.Click();

            var dialog = WaitHelper.WaitToConfrimWindow();
            var btnOk = dialog.FindElement(By.XPath("//button[contains(text(),'אישור')]"));
            btnOk.Click();
            GridHelper.RowId = "test";
            if (!GridHelper.IsRowExists(true))
            {
                Assert.Fail("EmptyTemplet and not updated");
            }

            AdminPageHelper.SwitchToMainPage();
            SeleniumDriver.driver.Navigate().Refresh();

        }

        public static bool CheckFileDownloaded(string filename)
        {
            bool exist = false;
            string Path = Environment.GetEnvironmentVariable("USERPROFILE") + "\\Downloads";
            string[] filePaths = Directory.GetFiles(Path);
            foreach (string p in filePaths)
            {
                if (p.Contains(filename))
                {
                    FileInfo thisFile = new FileInfo(p);
                    //Check the file that are downloaded in the last 3 minutes
                    if (thisFile.LastWriteTime.ToShortTimeString() == DateTime.Now.ToShortTimeString() ||
                    thisFile.LastWriteTime.AddMinutes(1).ToShortTimeString() == DateTime.Now.ToShortTimeString() ||
                    thisFile.LastWriteTime.AddMinutes(2).ToShortTimeString() == DateTime.Now.ToShortTimeString() ||
                    thisFile.LastWriteTime.AddMinutes(3).ToShortTimeString() == DateTime.Now.ToShortTimeString())
                    {
                        exist = true;
                        //File.Delete(p);
                        break;
                    }

                }
            }
            return exist;
        }

        public static void CheckTempletsList()
        {
            WaitHelper.Wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("xms-reports-filter-period-type")));
            var container = SeleniumDriver.driver.FindElements(By.ClassName("cards-container"));

            var buttonadmin = container[0].FindElements(By.ClassName("card-button-admin"));
            var cardcontent = container[0].FindElement(By.ClassName("card-content"));
            var cardname = "d" + container[0].FindElement(By.ClassName("card-name")).Text;
            var cardbuttons = cardcontent.FindElements(By.ClassName("btn-group"));
            var dropdown = cardbuttons[1].FindElement(By.ClassName("dropdown-toggle"));

            dropdown.Click();

            var dropdownmenu = cardbuttons[1].FindElement(By.ClassName("dropdown-menu"));

            var templateslist = dropdownmenu.FindElements(By.XPath("//*[contains(text(), 'test')]"));

            if (templateslist.Count == 0)
            {
                Assert.Fail("Templetes List not contains the new templete");
            }
            dropdown.Click();
        }

        public static void DownloadReport()
        {
            WaitHelper.Wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("xms-reports-filter-period-type")));
            var container = SeleniumDriver.driver.FindElements(By.ClassName("cards-container"));

            var cardcontent = container[0].FindElement(By.ClassName("card-content"));
            var cardname = "d" + container[0].FindElement(By.ClassName("card-name")).Text;

            var cardbuttons = cardcontent.FindElements(By.ClassName("btn-group"));
            cardbuttons[1].Click();

            Thread.Sleep(3000);
            WaitHelper.Wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.ClassName("loading-message")));
            if (!CheckFileDownloaded(cardname))
            {
                Assert.Fail("Fail to Download Templet name " + cardname);
            }

        }

        public static void UploadEditedExcelReport()
        {
            WaitHelper.Wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("xms-reports-filter-period-type")));

            var container = SeleniumDriver.driver.FindElements(By.ClassName("cards-container"));

            var cardcontent = container[0].FindElement(By.ClassName("card-content"));
            var cardname = "d" + container[0].FindElement(By.ClassName("card-name")).Text;
            string path = Environment.GetEnvironmentVariable("USERPROFILE") + "\\Downloads\\" + cardname + ".xlsm";

            path = UpdateReportExcel(cardname);

            cardcontent.FindElement(By.Id("unique1")).SendKeys(path);

            WaitHelper.Wait.Timeout = TimeSpan.FromSeconds(20);

            WaitHelper.Wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.ClassName("loading-message")));

            WaitHelper.Wait.Until(ExpectedConditions.ElementIsVisible(By.Id("validation-messages-modal")));

            WaitHelper.Wait.Timeout = TimeSpan.FromSeconds(10);
            var validationmessages = SeleniumDriver.driver.FindElement(By.Id("validation-messages-modal"));
            validationmessages.FindElement(By.ClassName("button")).Click();
            Thread.Sleep(500);
            var status = cardcontent.FindElement(By.ClassName("card-xbrl-status"));
            if (!status.Text.Contains("דיווח שגוי"))
            {
                Assert.Fail("card-xbrl-status not change after excel errors ");
            }

            var cardlastupdatedate = cardcontent.FindElement(By.ClassName("card-last-update-date"));

            if (cardlastupdatedate.Text != DateTime.Now.ToString("dd/MM/yyyy"))
            {
                Assert.Fail("card-last-update-date not updated after update");
            }
            var cardlastupdatetime = cardcontent.FindElement(By.ClassName("card-last-update-time"));

            path = Environment.GetEnvironmentVariable("USERPROFILE") + "\\Downloads\\" + cardname + ".xlsm";

            cardcontent.FindElement(By.Id("unique1")).SendKeys(path);
            Thread.Sleep(500);
            WaitHelper.Wait.Timeout = TimeSpan.FromSeconds(20);
            WaitHelper.Wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.ClassName("loading-message")));
            WaitHelper.Wait.Timeout = TimeSpan.FromSeconds(10);

            WaitHelper.Wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("card-xbrl-status")));

            status = cardcontent.FindElement(By.ClassName("card-xbrl-status"));
            if (!status.Text.Contains("תקין"))
            {
                Assert.Fail("card-xbrl-status not change after excel errors ");
            }
            path = Environment.GetEnvironmentVariable("USERPROFILE") + "\\Downloads\\testing.xlsx";

            cardcontent.FindElement(By.Id("unique1")).SendKeys(path);

            WaitHelper.Wait.Timeout = TimeSpan.FromSeconds(20);
            WaitHelper.Wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.ClassName("loading-message")));

            WaitHelper.Wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("xms-reports-filter-period-type")));

            WaitHelper.Wait.Timeout = TimeSpan.FromSeconds(3);
            var dialog = WaitHelper.WaitToConfrimWindow();
            WaitHelper.Wait.Timeout = TimeSpan.FromSeconds(10);

            var btnOk = dialog.FindElement(By.ClassName("swal2-confirm"));
            btnOk.Click();
            Thread.Sleep(500);
            var lastupdates = cardcontent.FindElement(By.ClassName("card-last-update-log"));
            lastupdates.Click();
            Thread.Sleep(500);
            WaitHelper.Wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.ClassName("loading-message")));
            WaitHelper.Wait.Until(ExpectedConditions.ElementIsVisible(By.Id("log-modal")));
            var logmodal = SeleniumDriver.driver.FindElement(By.Id("log-modal"));

            var table = logmodal.FindElement(By.TagName("tbody"));
            var rows = table.FindElements(By.TagName("tr"));

            var td = rows[0].FindElements(By.TagName("td"));
            if (td[0].Text != DateTime.Now.ToString("dd/MM/yyyy"))
            {
                Assert.Fail("testing  Last reports updates failed");
            }

            if (td[3].Text != "תקין")
            {
                Assert.Fail("testing  Last reports updates failed");
            }

            var td1 = rows[1].FindElements(By.TagName("td"));
            if (td1[0].Text != DateTime.Now.ToString("dd/MM/yyyy"))
            {
                Assert.Fail("testing  Last reports updates failed");
            }
            if (td1[3].Text != "דיווח שגוי")
            {
                Assert.Fail("testing  Last reports updates failed");
            }

            var index = rows.Count - 1;
            var name = container[0].FindElement(By.ClassName("card-name")).Text + "." + index;
            var downloadexcel = td[4].FindElement(By.ClassName("download-xbrl-xls-button"));
            downloadexcel.Click();
            Thread.Sleep(500);
            WaitHelper.Wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.ClassName("loading-message")));
            if (!CheckFileDownloaded(name))
            {
                Assert.Fail("Can not download a version of a report from the last Updates window");
            }

            SeleniumDriver.driver.Navigate().Refresh();
        }

        public static void DeleteTemplet()
        {
            WaitHelper.Wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("xms-reports-filter-period-type")));
            var container = SeleniumDriver.driver.FindElements(By.ClassName("cards-container"));

            var buttonadmin = container[0].FindElements(By.ClassName("card-button-admin"));

            buttonadmin[0].Click();
            WaitHelper.Wait.Until(ExpectedConditions.ElementIsVisible(By.Id("report-admin-modal")));

            AdminPageHelper.SwitchToIframeInContainerId("report-admin-modal");

            GridHelper.GridId = "templatesGrid";
            GridHelper.RowId = "test";
            var row = GridHelper.GetRow();

            GridHelper.BtnClick("Delete");
            GridHelper.ConfrimButtonClick(true);

            var dialog = WaitHelper.WaitToConfrimWindow();
            var btnOk = dialog.FindElement(By.XPath("//button[contains(text(),'אישור')]"));
            btnOk.Click();

            AdminPageHelper.SwitchToMainPage();
            SeleniumDriver.driver.Navigate().Refresh();
            WaitHelper.Wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("xms-reports-filter-period-type")));
            container = SeleniumDriver.driver.FindElements(By.ClassName("cards-container"));

            buttonadmin = container[0].FindElements(By.ClassName("card-button-admin"));

            buttonadmin[0].Click();
            WaitHelper.Wait.Until(ExpectedConditions.ElementIsVisible(By.Id("report-admin-modal")));

            AdminPageHelper.SwitchToIframeInContainerId("report-admin-modal");

            row = GridHelper.GetRow();

            if (row != null)
            {
                Assert.Fail("Fail to delete templet");
            }
            AdminPageHelper.SwitchToMainPage();
            SeleniumDriver.driver.Navigate().Refresh();
        }

        public static string UpdateReportExcel(string cardname)
        { 
            var path = Environment.GetEnvironmentVariable("USERPROFILE") + "\\Downloads\\" + cardname + ".xlsm";

            Application xlsmApp = new Application();
            Workbook xlsm = xlsmApp.Workbooks.Open(path, ReadOnly: false);

            Application xlApp = new Application();
              
            Workbook xlWorkBook = xlsm;

            Worksheet xlWorkSheet = (Worksheet)xlWorkBook.Worksheets.get_Item(2);
            xlWorkSheet.Activate(); 

            xlApp.DefaultSheetDirection = (int)Constants.xlRTL;

            xlWorkSheet.Cells[14, 4] = 9;
             
            path = Environment.GetEnvironmentVariable("USERPROFILE") + "\\Downloads\\" + cardname + "update.xlsm";

            xlWorkBook.SaveAs(path);
            xlWorkBook.Close();
            xlApp.Quit();

            return path;

        }
    }
}