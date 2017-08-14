using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Selenium.Common
{
    public static class WaitHelper
    {
        
        public static void WaitToFinishloading()
        {
            SeleniumGridHelper.Wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.ClassName("k-loading-image")));
        }

        public static void WaitToConfrimWindow()
        {
            SeleniumGridHelper.Wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("k-confirm")));
        }
        public static void WaitToDialogWindow()
        {  
            SeleniumGridHelper.Wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@type='button' and text()= 'אישור']")));
        }

        public static void WaitUntilTableIsVisible(string gridId)
        {
            SeleniumGridHelper.Wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='" + gridId + "']/div[3]/table")));
        }
    }
}