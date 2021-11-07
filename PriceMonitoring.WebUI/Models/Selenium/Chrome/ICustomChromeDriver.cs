using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.Generic;

namespace PriceMonitoring.WebUI.Models.Selenium.Chrome
{
    public interface ICustomChromeDriver
    {
        IWebDriver ChromeDriver { get; }
        ChromeOptions ChromeOptions { get; }
        IJavaScriptExecutor JavaScriptExecutor { get; set; }
        object ExecuteScript(string script);
        IWebElement FindElement(By by);
        IReadOnlyCollection<IWebElement> FindElements(By by);
        string GetCurrentUrl();
        void GoToUrl(string url);
        void DriverClose();
    }
}