using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PriceMonitoring.WebUI.Models.Selenium.Chrome
{
    public class CustomChromeDriver : ICustomChromeDriver
    {
        public IWebDriver ChromeDriver { get; }
        public ChromeOptions ChromeOptions { get; }
        public IJavaScriptExecutor JavaScriptExecutor { get; set; }

        public CustomChromeDriver()
        {
            ChromeOptions = GetChromeOptions();
            ChromeDriver = GetChromeDriver();
        }

        private IWebDriver GetChromeDriver()
        {
            IWebDriver driver = new ChromeDriver(options: ChromeOptions);
            return driver;
        }

        public void GoToUrl(string url)
        {
            ChromeDriver.Navigate().GoToUrl(url: url);
        }

        public object ExecuteScript(string script)
        {
            JavaScriptExecutor = (IJavaScriptExecutor)ChromeDriver;
            return JavaScriptExecutor.ExecuteScript(script: script);
        }

        public IWebElement FindElement(By by)
        {
            var element = ChromeDriver.FindElement(by: by);
            return element;
        }

        public IReadOnlyCollection<IWebElement> FindElements(By by)
        {
            var elements = ChromeDriver.FindElements(by: by);
            return elements;
        }

        public string GetCurrentUrl()
        {
            return ChromeDriver.Url;
        }

        public void DriverClose()
        {
            ChromeDriver.Close();
        }

        private ChromeOptions GetChromeOptions()
        {
            var chromeOptions = new ChromeOptions();
            var userAgent = "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.50 Safari/537.36";
            chromeOptions.AddArgument("--window-size=1920,1080");
            chromeOptions.AddArgument("--start-maximized");
            chromeOptions.AddArgument($"user_agent={userAgent}");
            chromeOptions.AddArgument("--ignore-certificate-errors");
            chromeOptions.AddArgument("no-sandbox");
            chromeOptions.AddArgument("--disable-gpu");
            chromeOptions.AddArgument("--headless");
            return chromeOptions;
        }
    }
}
