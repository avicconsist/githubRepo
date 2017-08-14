using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using Selenium.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace Selenium.Common
{
    public static class SeleniumGridHelper
    {
        #region properties

        public static List<ColumnsToEdit> ColumnsToEdit { get; set; }
        public static List<DropDownsToEdit> DropDownsToEdit { get; set; }
        public static int DropDownTaxonomyValue { get; set; }
        public static string DropDownTaxonomyName { get; set; }
        public static IList<IWebElement> RowTD { get; set; }
        public static WebDriverWait Wait { get; set; }
        public static string GridId { get; set; }
        public static string RowId { get; set; }
        public static string NewRowId { get; set; }
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
            ColumensInRowToUpdate();
            DeleteRow();
        }

        public static void SetTestSetting(int activeTab, string gridId, string rowId, string newRowId, string dropDownTaxonomyName, int dropDownTaxonomyValue, bool updateIdColumn, List<ColumnsToEdit> columnsToEdit, List<DropDownsToEdit> dropDownsToEdit)
        {
            ActiveTab = activeTab;
            GridId = gridId;
            RowId = rowId;
            NewRowId = newRowId;
            DropDownTaxonomyValue = dropDownTaxonomyValue;
            ColumnsToEdit = columnsToEdit;
            DropDownsToEdit = dropDownsToEdit;
            DropDownTaxonomyName = dropDownTaxonomyName;
            UpdateIdColumn = updateIdColumn;
        }

        public static void ChangeTaxonomyDropDown()
        {
            GoToActiveTab();

            Wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName(DropDownTaxonomyName)));

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

                Wait.Until(ExpectedConditions.ElementToBeClickable(option));

                option.Click();

                ConfrimButtonClick(false);

                var taxonomy = GetActiveTaxonomy();

                if (taxonomy.Text != activeTaxonomy.Text)
                {
                    Assert.Fail("Fail to chancge taxonomy when canceled confirm dialog");
                }

                DropDownTaxonomyClick();

                Wait.Until(ExpectedConditions.ElementToBeClickable(option));
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
            Wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='tabstrip']/ul/li[" + ActiveTab + "]")));
            SeleniumDriver.driver.FindElement(By.XPath("//*[@id='tabstrip']/ul/li[" + ActiveTab + "]")).Click();
        }

        public static bool IsRowExists()
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
                    rowTdText = RowTD[ColumnsToEdit[1].ColumnNum].Text;
                }
                if (rowTdText.Equals(RowId))
                {
                    return true;
                }
            }

            return false;
        }

        public static void CreateNewRow()
        {
            GoToActiveTab();
            // CreateNewClick
            CreateNewClick();

             
            if (ColumnsToEdit != null)
            {

                // Check required fields
                foreach (var col in ColumnsToEdit)
                {
                    UpdateBtnClick();

                    Wait.Until(ExpectedConditions.ElementIsVisible(By.Name(col.ColumnName)));
                    if (col.Required)
                    {
                        if (!SeleniumDriver.driver.FindElement(By.XPath("//*[@data-for='" + col.ColumnName + "']")).Text.Contains(col.ColumnName))
                        {
                            Assert.Fail("tooltip required field " + col.ColumnName + "is not visible");
                        }
                    } 

                    SendKeys(col.ColumnName);
                }
            }


            //Wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/div[2]/div/div[2]/ul")));



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

                    ////Check PeriodType DropDownList values
                    //if (dropDown.ColumnName == "PeriodType")
                    //{
                    //    List<string> PeriodTypes = new List<string>() { "חצי שנתי", "מיידי", "חודשי", "רבעוני", "שבועי", "שנתי" };

                    //    foreach (var value in DropDownListTextValues)
                    //    {
                    //        if (!PeriodTypes.Contains(value))
                    //        {
                    //            Assert.Fail("PeriodType values missing value : ", value);
                    //        }
                    //    }
                    //} 
                }
            }

            //UpdateBtnClick
            UpdateBtnClick();

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
            UpdateBtnClick();

            DialogWindowBtnClick();
            CancelBtnClick();
        }

        public static void ColumensInRowToUpdate()
        {
            tableRows = GetGridRows();

            foreach (IWebElement row in tableRows)
            {
                RowTD = row.FindElements(By.TagName("td"));

                var rowTdText = RowTD[1].Text;

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
            IWebElement tableElement = SeleniumDriver.driver.FindElement(By.XPath("//*[@id='" + GridId + "']/div[3]/table"));
            tableRows = tableElement.FindElements(By.TagName("tr"));
            return tableRows;
        }

        public static void RefreshPage()
        {
            SeleniumDriver.driver.Navigate().Refresh();

            GoToActiveTab();

            WaitHelper.WaitUntilTableIsVisible(GridId);

            if (!string.IsNullOrWhiteSpace(DropDownTaxonomyName))
            {
                DropDownTaxonomyClick();

                Wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/div[2]/div/div[2]/ul")));

                var taxonomylist = SeleniumDriver.driver.FindElement(By.XPath("/html/body/div[2]/div/div[2]/ul"));

                var activeTaxonomy = GetActiveTaxonomy();

                IList<IWebElement> values = taxonomylist.FindElements(By.TagName("li"));

                values[DropDownTaxonomyValue].Click();

                WaitHelper.WaitToFinishloading();
            }
        }

        public static IWebElement GetActiveTaxonomy()
        {
            return SeleniumDriver.driver.FindElement(By.XPath("//*[@id='tabstrip-" + ActiveTab + "']/span/span/span[1]"));
        }

        #endregion

        #region Inputs

        public static void CreateNewClick()
        {
            WaitHelper.WaitUntilTableIsVisible(GridId);
            Wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='" + GridId + "']/div[1]/a")));
            SeleniumDriver.driver.FindElement(By.XPath("//*[@id='" + GridId + "']/div[1]/a")).Click();
            WaitHelper.WaitToFinishloading();
        }

        public static void EditBtnClick()
        {
            ClickOnButton(0, false);
        }

        public static void UpdateBtnClick(bool clickOnFirstRow = false)
        {
            if (clickOnFirstRow)
            {
                ClickOnButton(0, false, false, true);
            }
            else
            {
                ClickOnButton(0, false);
            }
        }

        public static void DeleteBtnClick()
        {
            ClickOnButton(1, true);
            //Is the new row exists in table
            if (!IsRowExists())
            {
                Assert.Fail("DeleteRow Fail");
            }

            RefreshPage();

            ClickOnButton(1, true, true);

            if (IsRowExists())
            {
                Assert.Fail("DeleteRow Fail");
            }
        }

        public static void CancelBtnClick()
        {
            ClickOnButton(1, false);
        }

        public static void ClickOnButton(int buttonNum, bool isDeleteButton = false, bool btnConfrim = false, bool firstRow = false)
        {
            WaitHelper.WaitToFinishloading();

            tableRows = GetGridRows();

            foreach (IWebElement row in tableRows)
            {

                RowTD = row.FindElements(By.TagName("td"));
                IList<IWebElement> buttons = RowTD[0].FindElements(By.TagName("a"));

                var rowTdText = RowTD[1].Text;

                if (firstRow)
                {
                    buttons[buttonNum].Click();
                    break;
                }

                if (isDeleteButton)
                {
                    if (!UpdateIdColumn)
                    {
                        rowTdText = RowTD[2].Text;
                    }
                    if (rowTdText.Equals(RowId))
                    {
                        buttons[buttonNum].Click();
                        ConfrimButtonClick(btnConfrim);
                        break;
                    }
                }
                else
                {
                    if (!UpdateIdColumn)
                    {
                        buttons[buttonNum].Click();
                        break;
                    }
                    if (rowTdText.Equals(RowId) || rowTdText == string.Empty)
                    {
                        buttons[buttonNum].Click();
                        break;
                    }
                }
            }
            WaitHelper.WaitToFinishloading();
        }

        public static void ConfrimButtonClick(bool btnConfrim)
        {
            WaitHelper.WaitToConfrimWindow();

            var dialog = SeleniumDriver.driver.FindElement(By.ClassName("k-confirm"));

            IList<IWebElement> confrimButtons = dialog.FindElements(By.TagName("button"));

            if (btnConfrim)
            {
                confrimButtons[0].Click();
            }
            else
            {
                confrimButtons[1].Click();
            }
            WaitHelper.WaitToFinishloading();
        }

        public static void DialogWindowBtnClick()
        {
            WaitHelper.WaitToDialogWindow();

            SeleniumDriver.driver.FindElement(By.XPath("//button[@type='button' and text()= 'אישור']")).Click();

            WaitHelper.WaitToFinishloading();
        }

        public static void SendKeys(string columnNameToEdit)
        {
            Wait.Until(ExpectedConditions.ElementIsVisible(By.Name(columnNameToEdit)));
            SeleniumDriver.driver.FindElement(By.Name(columnNameToEdit)).SendKeys(RowId);
        }

        public static void SelectValueDropDownList(DropDownsToEdit dropDown)
        {
            tableRows = GetGridRows();

            foreach (IWebElement row in tableRows)
            {
                RowTD = row.FindElements(By.TagName("td"));
                Wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("k-select")));
                IWebElement select = RowTD[dropDown.ColumnNum].FindElement(By.ClassName("k-select"));
                select.Click();

                IList<IWebElement> containers = DropDownGetContainers();


                IWebElement list = containers[dropDown.NumOfDropDownOnGrid].FindElement(By.TagName("ul"));

                Wait.Until(ExpectedConditions.ElementToBeClickable(list));

                DropDownListValues = list.FindElements(By.TagName("li"));

                SetDropDownListTextValuesFromDropDownListValues();

                IWebElement option;

                if (dropDown.ContainsEmptyValue)
                {
                    option = DropDownListValues[dropDown.selectedValue];

                    Wait.Until(ExpectedConditions.ElementToBeClickable(option));

                    option.Click();
                }
                else
                {
                    option = DropDownListValues[dropDown.selectedValue];

                    Wait.Until(ExpectedConditions.ElementToBeClickable(option));

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
            Wait.Until(ExpectedConditions.ElementExists(By.ClassName("k-animation-container")));
            IList<IWebElement> containers = SeleniumDriver.driver.FindElements(By.ClassName("k-animation-container"));
            return containers;
        }


        #endregion 
    }
}