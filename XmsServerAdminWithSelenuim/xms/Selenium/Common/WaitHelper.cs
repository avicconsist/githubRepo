using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Selenium.Common
{
    public static class WaitHelper
    {
        public static WebDriverWait Wait { get; set; }

        public static void WaitToFinishloading()
        {
            Wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.ClassName("k-loading-image")));
        }

        public static IWebElement WaitToConfrimWindow()
        {
            try
            { 
                Wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("k-confirm")));
                return SeleniumDriver.driver.FindElement(By.ClassName("k-confirm"));
            }
            catch (System.Exception)
            { 
                Wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("show-swal2")));
                return SeleniumDriver.driver.FindElement(By.ClassName("show-swal2"));
            } 

        }

        public static void WaitToDialogWindow()
        {
            Wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button")));
        }

        public static void WaitUntilTableIsVisible(string gridId)
        {
            Wait.Until(ExpectedConditions.ElementIsVisible(By.Id(gridId)));
            var grid = SeleniumDriver.driver.FindElement(By.Id(gridId));
            Wait.Until(ExpectedConditions.ElementToBeClickable(grid));
            var table = grid.FindElement(By.TagName("table"));
            Wait.Until(ExpectedConditions.ElementToBeClickable(table));
        }

        public static void WaitUntilModelIsVisible()
        {
            Wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='adminModal']")));
        }
    }
}