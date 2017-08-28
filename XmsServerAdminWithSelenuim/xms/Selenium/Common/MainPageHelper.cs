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
    public static class MainPageHelper
    {
        #region properties

        public static List<string> periodTypes { get; set; }
        #endregion

        #region Helpers

        public static void CheckUpperBarAndFilters()
        {

            WaitHelper.Wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("xms-reports-filter-period-type")));
            SeleniumDriver.driver.FindElement(By.ClassName("xms-reports-filter-period-type")).Click();
            var periodTypeDropDwon = SeleniumDriver.driver.FindElement(By.ClassName("transition"));
            var optionList = periodTypeDropDwon.FindElements(By.ClassName("item"));
             
            foreach (IWebElement optionVal in optionList)
            {
                if (!periodTypes.Contains(optionVal.Text))
                {
                    Assert.Fail(optionVal + " Is not a valid value for periodTypeDropDwon");
                }
            }

            SeleniumDriver.driver.FindElement(By.ClassName("xms-reports-filter-period-type")).Click();
        }

        public static void PeriodTypeFilter()
        {
            var type = string.Empty;
            for (int i = 0; i < periodTypes.Count; i++)
            {
                 
                if (periodTypes[i] == "חודשי")
                {
                    type = PeriodTypeFilterClick(i, "monthly");
                    if (!type.Contains("חודש"))
                    {
                        Assert.Fail("calendar is מot adjusted to the period selection for month");
                    }
                    continue;
                }
                if (periodTypes[i] == "רבעוני")
                {
                    type = PeriodTypeFilterClick(i, "quarterly");
                    if (!type.Contains("שנה"))
                    {
                        Assert.Fail("calendar is מot adjusted to the period selection for quarter");
                    }
                    continue;
                }
                if (periodTypes[i] == "שנתי")
                {
                    type = PeriodTypeFilterClick(i, "yearly");
                    if (!type.Contains("שנה"))
                    {
                        Assert.Fail("calendar is מot adjusted to the period selection for year");
                    }
                    continue;
                }
                if (periodTypes[i] == "חצי-שנתי")
                {
                    type = PeriodTypeFilterClick(i, "half-yearly");
                    if (!type.Contains("שנה"))
                    {
                        Assert.Fail("calendar is מot adjusted to the period selection for half-year");
                    }
                    continue;
                }
                if (periodTypes[i] == "מיידי")
                {
                    type = PeriodTypeFilterClick(i, "immediate");
                    if (!type.Contains("יום"))
                    {
                        Assert.Fail("calendar is מot adjusted to the period selection for immediate");
                    }
                    continue;
                }
            }
        }

        public static string PeriodTypeFilterClick(int index, string periodtype)
        {
            WaitHelper.Wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("xms-reports-filter-period-type")));
            SeleniumDriver.driver.FindElement(By.ClassName("xms-reports-filter-period-type")).Click();

            WaitHelper.Wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("transition")));

            var periodTypeDropDwon = SeleniumDriver.driver.FindElement(By.ClassName("transition"));

            var optionList = periodTypeDropDwon.FindElements(By.ClassName("item"));

            var selectedtype = optionList[index].Text;
            Thread.Sleep(500);
            optionList[index].Click();

            IWebElement filters = SeleniumDriver.driver.FindElement(By.ClassName("xms-reports-filters"));

            var period = filters.FindElement(By.ClassName("xms-period-" + periodtype));

            var element = period.FindElement(By.ClassName("bootstrap-iso"));

            if (periodtype == "quarterly")
            {
                var selectedQuarter = filters.FindElement(By.ClassName("xms-reports-filter-period-input-quarter"));

                if (selectedQuarter == null)
                {
                    Assert.Fail("selectedQuarter input not exist");
                }
            }
     
            var databind = element.FindElement(By.TagName("input")).GetAttribute("data-bind");
             
            var dateString = "maxDate : moment('" + DateTime.Now.ToString("yyyy-MM-dd");

            if (!databind.Contains(dateString))
            {
                Assert.Fail("Can choose a future period on period name : " + periodtype);
            }
 
            return element.FindElement(By.TagName("input")).GetAttribute("placeholder");
        }

        #endregion

        #region Inputs


        #endregion
    }
}