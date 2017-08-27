using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Selenium.Common
{
    public static class SeleniumDriver
    { 
        public static IWebDriver driver { get; set; } 
    }
}