using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using Selenium.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System;
using System.Linq;

namespace Selenium.Common
{
    public static class AdminPageHelper
    {
        #region properties
        public static List<string> TaxonomyList { get; set; }
        public static List<string> Roles { get; set; }
        public static string PermissionsToReport { get; set; }
        public static List<int> RolesToAdd { get; set; }
        #endregion

        #region Helpers

        public static void AddRolesToEntity()
        {
            AddEntity();

            EntityGridRowClick();

            AddEntityRoleAndChackRoleList();

            AddRoleThatExsistinEntity(RolesToAdd[0]);

            SetReporsForRole();

            ChackTaxonomyList();

            //log in with the user that you gave the permissions to and check if the user have permissions to the reports
            //ChackPermissions();

            DeletePermissions();

            DeleteEntity();
        }


        public static void OpenAdminPage()
        {
            AdminButtonClick();
        }
         
        public static void AddEntity()
        {
            GridHelper.CreateNewRow();
        }

        public static void DeleteEntity()
        {
            GridHelper.ActiveTab = 1;
            GridHelper.GoToActiveTab();
            GridHelper.GridId = "entitiesGrid";
            GridHelper.RowId = "test";
            GridHelper.BtnClick("Delete");
            GridHelper.ConfrimButtonClick(true);
            Thread.Sleep(500);
            if (GridHelper.IsRowExists()) 
                Assert.Fail("DeleteRow Fail");
            
        }
         
        public static void SwitchToIframeInContainerId(string containerid)
        {
            //find the outer frame, and use switch to frame method
             IWebElement container  = SeleniumDriver.driver.FindElement(By.Id(containerid));
            IWebElement containerFrame = container.FindElement(By.TagName("iframe"));
            SeleniumDriver.driver.SwitchTo().Frame(containerFrame);
        }

        public static void SwitchToMainPage()
        {
            SeleniumDriver.driver.SwitchTo().DefaultContent();
        }

        public static void AddEntityRoleAndChackRoleList()
        {
            GridHelper.GridId = "entityRolesDetailsGrid";

            foreach (var role in RolesToAdd)
            {
                AddEntity(role);
            }
        }

        public static void AddEntity(int role)
        {
            GridHelper.CreateNewClick();

            WaitHelper.Wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/div[7]")));

            var popupWindow = SeleniumDriver.driver.FindElement(By.XPath("/html/body/div[7]"));

            Thread.Sleep(500);

            var dropdown = popupWindow.FindElement(By.ClassName("xms-role-type"));

            var buttonsdiv = popupWindow.FindElement(By.ClassName("k-edit-buttons"));

            var buttons = buttonsdiv.FindElements(By.TagName("a"));

            dropdown.Click();

            Thread.Sleep(500);

            IList<IWebElement> containers = GridHelper.DropDownGetContainers();

            IList<IWebElement> optionslist = containers[containers.Count - 1].FindElements(By.TagName("li"));

            if (Roles == null)
            {
                Roles = new List<string>();
                Roles.AddRange(optionslist.Select(x => x.Text));
                ChackDropDownRols();
            }

            IWebElement option = optionslist[role];

            WaitHelper.Wait.Until(ExpectedConditions.ElementToBeClickable(option));

            GridHelper.RowId = option.Text;

            option.Click();

            buttons[0].Click();

            Thread.Sleep(500);

            if (!GridHelper.IsRowExists(false))
            {
                Assert.Fail("AddRow to Permissions table failed");
            }
        }

        public static void ChackDropDownRols()
        {
            List<string> PermissionsType = new List<string>() {
                        "מנהל",
                        "מנהל טקסונמיה",
                        "דוחות - קריאה",
                        "דוחות - כתיבה",
                        "דוחות - ניהול",
                        "הפצה"
                    };

            foreach (var item in Roles)
            {
                if (!PermissionsType.Contains(item))
                {
                    Assert.Fail("Permissions Type drop down missing type : " + item);
                }
            }
        }

        public static void AddRoleThatExsistinEntity(int role)
        {
            AddEntity(role);
            try
            {
                WaitHelper.WaitToDialogWindow();

                SeleniumDriver.driver.FindElement(By.XPath("//button")).Click();
                Thread.Sleep(500);

            }
            catch (Exception)
            {

                Assert.Fail("AddRoleThatExsistinEntity fail");
            }
        }

        public static void SetReporsForRole()
        {

            GridHelper.GridId = "entityRolesDetailsGrid";
            var tableRows = GridHelper.GetGridRows();


            foreach (var row in tableRows)
            {
                var rowBtns = row.FindElements(By.TagName("a"));

                try
                {
                    if (row.Text.Contains("דוחות"))
                    {
                        var reportsBtn = rowBtns[2];

                        reportsBtn.Click();

                        WaitHelper.Wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("close-button")));
                        Thread.Sleep(500);
                        var window = SeleniumDriver.driver.FindElement(By.XPath("/html/body/div[4]"));

                        var taxnomyDropDown = SeleniumDriver.driver.FindElement(By.ClassName("k-dropdown"));

                        GridHelper.GridId = "entityRolesDetailsGrid_ReportsWindow_reportsGrid";

                        tableRows = GridHelper.GetGridRows();
                        PermissionsToReport = tableRows[0].Text;

                        var input = tableRows[0].FindElement(By.TagName("input"));
                        input.Click();

                        var saveBtn = window.FindElement(By.ClassName(" k-grid-save-changes"));
                        saveBtn.Click();

                        WaitHelper.WaitToDialogWindow();
                        SeleniumDriver.driver.FindElement(By.XPath("//button")).Click();
                        Thread.Sleep(500);

                        WaitHelper.Wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("close-button")));
                        var closeBtn = window.FindElement(By.ClassName("close-button"));
                        closeBtn.Click();
                        Thread.Sleep(500);
                    }
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }

        public static void ChackTaxonomyList()
        {
            GridHelper.GridId = "entityRolesDetailsGrid";
            var tableRows = GridHelper.GetGridRows();

            foreach (var row in tableRows)
            {
                var rowBtns = row.FindElements(By.TagName("a"));

                try
                {
                    if (row.Text.Contains("דוחות"))
                    {
                        var reportsBtn = rowBtns[2];

                        reportsBtn.Click();
                        Thread.Sleep(500);
                        WaitHelper.Wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/div[4]")));
                        var window = SeleniumDriver.driver.FindElement(By.XPath("/html/body/div[4]"));

                        var taxnomyDropDown = SeleniumDriver.driver.FindElement(By.ClassName("k-dropdown"));
                        taxnomyDropDown.Click();

                        Thread.Sleep(500);

                        IList<IWebElement> containers = GridHelper.DropDownGetContainers();

                        IList<IWebElement> optionslist = containers[containers.Count - 1].FindElements(By.TagName("li"));

                        foreach (var item in optionslist)
                        {
                            if (!TaxonomyList.Contains(item.Text))
                            {
                                Assert.Fail("Permissions Type drop down missing type : " + item.Text);
                            }
                        }

                        WaitHelper.Wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("close-button")));
                        var closeBtn = window.FindElement(By.ClassName("close-button"));
                        closeBtn.Click();
                        Thread.Sleep(500);
                    }
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }

        public static void DeletePermissions()
        {
            GridHelper.GridId = "entityRolesDetailsGrid";
            IList<IWebElement> tableRows = GridHelper.GetGridRows();

            var rows = tableRows.Count;

            for (int i = 0; i < rows; i++)
            {
                var rowBtns = tableRows[0].FindElements(By.TagName("a"));
                rowBtns[1].Click();

                WaitHelper.Wait.Timeout = TimeSpan.FromSeconds(3);
                var dialog = WaitHelper.WaitToConfrimWindow();
                WaitHelper.Wait.Timeout = TimeSpan.FromSeconds(10);

                var btnOk = dialog.FindElement(By.XPath("//button[contains(text(),'אישור')]"));

                btnOk.Click();
                Thread.Sleep(500);

                tableRows = GridHelper.GetGridRows();

            } 

            if (tableRows.Count > 0)
            {
                Assert.Fail("Fail to DeleteRols for Entity : " + GridHelper.RowId);
            }
        }

        #endregion

        #region Inputs

        public static void AdminButtonClick()
        {
            WaitHelper.Wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("xms-reports-buttons")));
            var adminBtns = SeleniumDriver.driver.FindElement(By.ClassName("xms-reports-buttons"));
            var adminBtn = adminBtns.FindElement(By.ClassName("xms-reports-admin-button"));
            WaitHelper.Wait.Until(ExpectedConditions.ElementToBeClickable(adminBtn));
            adminBtn.Click();

            WaitHelper.WaitUntilModelIsVisible();
        }

        public static void EntityGridRowClick()
        {
            GridHelper.GridId = "entitiesMasterGrid";
            GridHelper.RowId = "test";
            GridHelper.ActiveTab = 2;

            GridHelper.GoToActiveTab();

            var row = GridHelper.FindRow();

            if (row == null)
            {
                Assert.Fail("Entity not exsist in entitiesMasterGrid");
            }
            WaitHelper.Wait.Until(ExpectedConditions.ElementToBeClickable(row));

            row.Click();
        }


        #endregion
    }
}