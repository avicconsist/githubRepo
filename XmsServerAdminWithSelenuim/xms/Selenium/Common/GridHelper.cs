using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using Selenium.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Linq;
using System;

namespace Selenium.Common
{
    public static class GridHelper
    {
        #region properties

        public static List<ColumnsToEdit> ColumnsToEdit { get; set; }
        public static List<DropDownsToEdit> DropDownsToEdit { get; set; }
        public static List<string> ForceToEdit { get; set; }
        public static int DropDownTaxonomyValue { get; set; }
        public static string DropDownTaxonomyName { get; set; }
        public static IList<IWebElement> RowTD { get; set; }
        public static string GridId { get; set; }
        public static string IframeInContainerId { get; set; }
        public static string RowId { get; set; }
        public static string NewRowId { get; set; }
        public static string ActiveRow { get; set; }
        public static string TabId { get; set; }
        public static int ActiveTab { get; set; }
        public static bool UpdateIdColumn { get; set; }
        public static IList<IWebElement> tableRows { get; set; }
        public static IList<IWebElement> DropDownListValues { get; set; }
        public static List<string> DropDownListTextValues { get; set; }

        #endregion

        #region Helpers

        public static void TestGrid()
        {
            CreateNewRow();
            CreateNewRowWithExistId();
            UpdateRow();
            DeleteRow();
        }

        public static void SetTestSetting(int activeTab, string gridId, string rowId, string newRowId, string dropDownTaxonomyName, int dropDownTaxonomyValue, bool updateIdColumn, List<ColumnsToEdit> columnsToEdit, List<string> forceToEdit, List<DropDownsToEdit> dropDownsToEdit)
        {
            ActiveTab = activeTab;
            GridId = gridId;
            RowId = rowId;
            NewRowId = newRowId;
            DropDownTaxonomyValue = dropDownTaxonomyValue;
            ColumnsToEdit = columnsToEdit;
            ForceToEdit = forceToEdit;
            DropDownsToEdit = dropDownsToEdit;
            DropDownTaxonomyName = dropDownTaxonomyName;
            UpdateIdColumn = updateIdColumn;
        }

        public static void ChangeTaxonomyDropDown()
        {
            GoToActiveTab();

            WaitHelper.Wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName(DropDownTaxonomyName)));

            tableRows = GetGridRows();
            if (tableRows.Count == 0)
            {
                Assert.Fail("To chack this test you must have table rows");
            }

            var firstRow = tableRows[0].Text;

            UpdateBtnClick(true);

            DropDownTaxonomyClick();

            var activeTaxonomy = GetActiveTaxonomy();

            IList<IWebElement> containers = DropDownGetContainers();

            IList<IWebElement> optionslist = containers[containers.Count - 1].FindElements(By.TagName("li"));

            if (optionslist.Count > 1)
            {

                IWebElement option = optionslist[DropDownTaxonomyValue];

                WaitHelper.Wait.Until(ExpectedConditions.ElementToBeClickable(option));

                option.Click();

                ConfrimButtonClick(false);

                var taxonomy = GetActiveTaxonomy();

                if (taxonomy.Text != activeTaxonomy.Text)
                {
                    Assert.Fail("Fail to chancge taxonomy when canceled confirm dialog");
                }

                DropDownTaxonomyClick();

                WaitHelper.Wait.Until(ExpectedConditions.ElementToBeClickable(option));
                option.Click();

                ConfrimButtonClick(true);

                tableRows = GetGridRows();

                if (tableRows[0].Text.Equals(firstRow))
                {
                    Assert.Fail("To chack this test you must have table rows");
                }
            }
            else
            {
                Assert.Fail("To chack this test you must have mor then one taxonomy");
            }
        }

        public static void GoToActiveTab()
        {
            WaitHelper.Wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='" + TabId + "']/ul/li[" + ActiveTab + "]")));

            var tab = SeleniumDriver.driver.FindElement(By.XPath("//*[@id='" + TabId + "']/ul/li[" + ActiveTab + "]"));

            WaitHelper.Wait.Until(ExpectedConditions.ElementToBeClickable(tab));

            tab.Click();

        }

        public static bool IsRowExists(bool containBtnLastPage= true)
        {
            var rowExists = FindRow();

            if (rowExists == null)
            {
                if (containBtnLastPage)
                {
                    var goToLastPage = SeleniumDriver.driver.FindElement(By.ClassName("k-pager-last"));

                    goToLastPage.Click();

                    rowExists = FindRow();
                  
                }

                if (rowExists == null)
                {
                    return false;
                } 
            }

            return true;
        }

        public static IWebElement GetRow()
        {
            var rowExists = FindRow();

            if (rowExists == null)
            {
                try
                {
                    var goToLastPage = SeleniumDriver.driver.FindElement(By.ClassName("k-pager-last"));

                    goToLastPage.Click();

                    rowExists = FindRow();
                    return rowExists;

                }
                catch (Exception)
                {
                    
                }

                if (rowExists == null)
                {
                    return null;
                } 
            }

            return null;
        }

        public static IWebElement FindRow()
        {
            WaitHelper.WaitToFinishloading();

            tableRows = GetGridRows();

            foreach (IWebElement row in tableRows)
            {
                RowTD = row.FindElements(By.TagName("td"));

                var rowTdText = string.Empty;

                if (UpdateIdColumn)
                {
                    rowTdText = RowTD[ColumnsToEdit[0].ColumnNum].Text;
                }
                else
                { 
                    if (ColumnsToEdit!=null)
                    {
                        rowTdText = RowTD[ColumnsToEdit[1].ColumnNum].Text; 
                    }
                    else
                    {
                        rowTdText = RowTD[0].Text;
                    }
                }
                if (rowTdText.Equals(RowId))
                {
                    return row;
                }
            }
            
            return null;
        }

        public static void CreateNewRow()
        {
            GoToActiveTab();

            // CreateNew Click
            CreateNewClick();

            if (ColumnsToEdit != null)
            {
                // Check required fields
                foreach (var col in ColumnsToEdit)
                {
                    if (col.Required)
                    {
                        UpdateBtnClick(true);

                        WaitHelper.Wait.Until(ExpectedConditions.ElementIsVisible(By.Name(col.ColumnName)));
                        if (SeleniumDriver.driver.FindElement(By.XPath("//*[@data-for='" + col.ColumnName + "']")).Text.Contains("שדה חובה") ||
                             SeleniumDriver.driver.FindElement(By.XPath("//*[@data-for='" + col.ColumnName + "']")).Text.Contains("נדרש"))
                        {
                            SendKeys(col.ColumnName);
                        }
                        else
                        {
                            Assert.Fail("tooltip required field " + col.ColumnName + "is not visible");
                        }
                    }
                }
            }

            //SelectValueDropDownList
            if (DropDownsToEdit != null)
            {

                IList<IWebElement> containers = DropDownGetContainers();

                var index = containers.Count;

                foreach (var dropDown in DropDownsToEdit)
                {
                    dropDown.NumOfDropDownOnGrid = index;

                    SelectValueDropDownList(dropDown);

                    index++;
                }
            }

            //UpdateBtnClick
            UpdateBtnClick(true);

            RefreshPage();
            //Is the new row exists in table
            if (!IsRowExists())
            {
                Assert.Fail("AddRow Fail");
            }
        }

        public static void CreateNewRowWithExistId()
        {
            GoToActiveTab();
            // CreateNewClick
            CreateNewClick();

            //SendKeys 
            if (ColumnsToEdit != null)
            {
                foreach (var col in ColumnsToEdit)
                {
                    SendKeys(col.ColumnName);
                }
            }

            //SelectValueDropDownList
            if (DropDownsToEdit != null)
            {
                IList<IWebElement> containers = DropDownGetContainers();
                var index = containers.Count;

                foreach (var dropDown in DropDownsToEdit)
                {
                    dropDown.NumOfDropDownOnGrid = index;
                    SelectValueDropDownList(dropDown);
                    index++;
                }
            }

            //UpdateButton Click
            UpdateBtnClick(true);

            DialogWindowBtnClick();

            CancelBtnClick();
        }

        public static void UpdateRow()
        {
            tableRows = GetGridRows();

            foreach (IWebElement row in tableRows)
            {
                RowTD = row.FindElements(By.TagName("td"));

                var rowTdText = RowTD[ColumnsToEdit[0].ColumnNum].Text;

                if (rowTdText.Contains("מחק"))
                {
                    rowTdText = RowTD[RowTD.Count - 1].Text;
                }

                if (ColumnsToEdit != null)
                {
                    if (!UpdateIdColumn)
                    {
                        rowTdText = RowTD[ColumnsToEdit[1].ColumnNum].Text;
                    }

                    if (rowTdText.Equals(RowId))
                    {
                        EditBtnClick();

                        RowId = NewRowId;

                        foreach (var col in ColumnsToEdit)
                        {
                            if (col.ColumnName.Contains("Id"))
                            {
                                if (ForceToEdit != null && ForceToEdit.Contains(col.ColumnName))
                                {
                                    RowTD[col.ColumnNum].Click();
                                    var cellid = RowTD[col.ColumnNum].FindElement(By.TagName("input"));
                                    cellid.Clear();
                                    SendKeys(col.ColumnName);
                                    continue;
                                }
                                if (!UpdateIdColumn)
                                {
                                    continue;
                                }
                            }

                            RowTD[col.ColumnNum].Click();
                            var id = RowTD[col.ColumnNum].FindElement(By.TagName("input"));
                            id.Clear();
                            SendKeys(col.ColumnName);
                        }

                        UpdateBtnClick();

                        break;
                    }
                }
            }
            RefreshPage();

            //Is the new row exists in table
            if (!IsRowExists())
            {
                Assert.Fail("UpdateRow Fail");
            }
        }

        public static void DeleteRow()
        {
            DeleteBtnClick(); 
        }

        public static IList<IWebElement> GetGridRows()
        {
            WaitHelper.WaitUntilTableIsVisible(GridId);

            var grid = SeleniumDriver.driver.FindElement(By.Id(GridId));
            var table = grid.FindElement(By.TagName("tbody"));
            tableRows = table.FindElements(By.TagName("tr"));
            return tableRows;
        }

        public static void RefreshPage()
        {
            SeleniumDriver.driver.Navigate().Refresh();
             
            AdminPageHelper.AdminButtonClick();
            
            AdminPageHelper.SwitchToIframeInContainerId(IframeInContainerId);

            GoToActiveTab();

            WaitHelper.WaitUntilTableIsVisible(GridId);

            if (!string.IsNullOrWhiteSpace(DropDownTaxonomyName))
            {
                DropDownTaxonomyClick();

                IList<IWebElement> containers = DropDownGetContainers();

                IList<IWebElement> optionslist = containers[containers.Count - 1].FindElements(By.TagName("li"));

                IWebElement option = optionslist[DropDownTaxonomyValue];

                WaitHelper.Wait.Until(ExpectedConditions.ElementToBeClickable(option));

                option.Click();

                WaitHelper.WaitToFinishloading();
            }
        }

        public static IWebElement GetActiveTaxonomy()
        {
            return SeleniumDriver.driver.FindElement(By.XPath("//*[@id='" + TabId + "-" + ActiveTab + "']/span/span/span[1]"));
        }

        #endregion

        #region Inputs

        public static void CreateNewClick()
        {
            WaitHelper.WaitUntilTableIsVisible(GridId);
            WaitHelper.Wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='" + GridId + "']/div[1]/a")));
            SeleniumDriver.driver.FindElement(By.XPath("//*[@id='" + GridId + "']/div[1]/a")).Click();
            WaitHelper.WaitToFinishloading();
        }

        public static void EditBtnClick()
        {
            BtnClick("Edit");
        }

        public static void UpdateBtnClick(bool clickOnFirstRow = false)
        {
            if (clickOnFirstRow)
            {
                BtnClick("UpdateNewRow");
            }
            else
            {
                BtnClick("Update");
            }
        }

        public static void DeleteBtnClick()
        {
            BtnClick("Delete");
            ConfrimButtonClick(false);
            if (NewRowId!=null)
                RowId = NewRowId;

            //Is the new row exists in table
            if (!IsRowExists())
            {
                Assert.Fail("DeleteRow Fail");
            }

            RefreshPage();

            BtnClick("Delete");
            ConfrimButtonClick(true);
            RefreshPage();

            if (IsRowExists())
            {
                Assert.Fail("DeleteRow Fail");
            }
        }

        public static void CancelBtnClick()
        {
            BtnClick("Cancel");
        }

        public static void ConfrimButtonClick(bool btnConfrim)
        {
            WaitHelper.Wait.Timeout = TimeSpan.FromSeconds(3);
            var dialog = WaitHelper.WaitToConfrimWindow();
            WaitHelper.Wait.Timeout = TimeSpan.FromSeconds(10);

            var btnOk = dialog.FindElement(By.XPath("//button[contains(text(),'אישור')]"));
            var btnCancel = dialog.FindElement(By.XPath("//button[contains(text(),'ביטול')]"));

            if (btnConfrim)
            {
                btnOk.Click();
            }
            else
            {
                btnCancel.Click();
            }
            WaitHelper.WaitToFinishloading();
        }

        public static void DialogWindowBtnClick()
        {
            WaitHelper.WaitToDialogWindow();

            SeleniumDriver.driver.FindElement(By.XPath("//button")).Click();

            WaitHelper.WaitToFinishloading();
        }

        public static void SendKeys(string columnNameToEdit)
        {
            WaitHelper.Wait.Until(ExpectedConditions.ElementIsVisible(By.Name(columnNameToEdit)));
            SeleniumDriver.driver.FindElement(By.Name(columnNameToEdit)).SendKeys(RowId);
        }

        public static void SelectValueDropDownList(DropDownsToEdit dropDown)
        {
            tableRows = GetGridRows();

            foreach (IWebElement row in tableRows)
            {
                RowTD = row.FindElements(By.TagName("td"));
                WaitHelper.Wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("k-select")));
                IWebElement select = RowTD[dropDown.ColumnNum].FindElement(By.ClassName("k-select"));
                select.Click();

                IList<IWebElement> containers = DropDownGetContainers();


                IWebElement list = containers[dropDown.NumOfDropDownOnGrid].FindElement(By.TagName("ul"));

                WaitHelper.Wait.Until(ExpectedConditions.ElementToBeClickable(list));

                DropDownListValues = list.FindElements(By.TagName("li"));

                SetDropDownListTextValuesFromDropDownListValues();

                IWebElement option;

                if (dropDown.ContainsEmptyValue)
                {
                    option = DropDownListValues[dropDown.selectedValue];

                    WaitHelper.Wait.Until(ExpectedConditions.ElementToBeClickable(option));

                    option.Click();
                }
                else
                {
                    option = DropDownListValues[dropDown.selectedValue];

                    WaitHelper.Wait.Until(ExpectedConditions.ElementToBeClickable(option));

                    option.Click();
                }
                break;
            }
            WaitHelper.WaitToFinishloading();

        }

        public static void SetDropDownListTextValuesFromDropDownListValues()
        {
            DropDownListTextValues = new List<string>();
            foreach (IWebElement val in DropDownListValues)
            {
                DropDownListTextValues.Add(val.Text);
            }
        }

        public static void DropDownTaxonomyClick()
        {
            SeleniumDriver.driver.FindElement(By.ClassName(DropDownTaxonomyName)).Click();
        }

        public static IList<IWebElement> DropDownGetContainers()
        {
            //WaitHelper.Wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("k-animation-container")));
            IList<IWebElement> containers = SeleniumDriver.driver.FindElements(By.ClassName("k-animation-container"));
            return containers;
        }

        public static IList<IWebElement> GetRowButtons(string rowId, bool getFirstRowButtns = false)
        {
            WaitHelper.WaitToFinishloading();

            tableRows = GetGridRows();

            foreach (IWebElement row in tableRows)
            {
                RowTD = row.FindElements(By.TagName("td"));
                var rowTdText = string.Empty;

                if (rowTdText.Contains("מחק"))
                {
                    rowTdText = RowTD[RowTD.Count - 1].Text;
                }

                if (ColumnsToEdit!=null)
                {
                    if (ColumnsToEdit.Count > 1)
                    {
                        rowTdText = RowTD[ColumnsToEdit[1].ColumnNum].Text;
                    }
                }
                else
                {
                    rowTdText = RowTD[0].Text;
                }
                  
                IList<IWebElement> buttons = row.FindElements(By.TagName("a"));
                if (UpdateIdColumn)
                {
                    rowTdText = RowTD[ColumnsToEdit[0].ColumnNum].Text;
                }

                if (getFirstRowButtns)
                {
                    return buttons;
                }
                if (rowTdText.Equals("יש להכניס מזהה ייחודי") ||
                   rowTdText.Equals("שדה חובה"))
                {
                    return buttons;
                }
                if (rowTdText.Equals(rowId) ||
                    rowTdText.Equals(string.Empty))
                {
                    return buttons;
                }
            }
            return null;
        }

        public static void BtnClick(string type)
        {
            var buttons = GetRowButtons(RowId);
            WaitHelper.WaitToFinishloading();
            if (type == "UpdateNewRow")
            {
                buttons = GetRowButtons(RowId, true);
            }

            if (buttons != null)
            {
                switch (type)
                {
                    case "Edit":
                        WaitHelper.Wait.Until(ExpectedConditions.ElementToBeClickable(buttons[0]));
                        buttons[0].Click();
                        break;

                    case "Update":
                        WaitHelper.Wait.Until(ExpectedConditions.ElementToBeClickable(buttons[0]));
                        buttons[0].Click();
                        break;

                    case "UpdateNewRow":
                        WaitHelper.Wait.Until(ExpectedConditions.ElementToBeClickable(buttons[0]));
                        buttons[0].Click();
                        break;

                    case "Cancel":
                        var td = RowTD.Where(x => x.Text.Contains("בטל עריכה")).FirstOrDefault();

                        if (td != null)
                        {
                            Thread.Sleep(100);
                            td.Click();
                        }
                        break;

                    case "Delete":
                        td = RowTD.Where(x => x.Text.Contains("מחק")).FirstOrDefault();

                        if (td != null)
                        {
                            Thread.Sleep(100);
                            td.Click();
                        }
                        break;

                    default:
                        break;
                }
            }

            WaitHelper.WaitToFinishloading();
        }

        #endregion
    }
}