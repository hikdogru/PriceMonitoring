using OpenQA.Selenium;
using PriceMonitoring.WebUI.Models.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PriceMonitoring.WebUI.Models.GroceryStore
{
    public class Migros
    {

        public IEnumerable<ProductModel> GetProducts(string url)
        {
            ICustomChromeDriver chromeDriverModel = new CustomChromeDriver();
            var productList = new List<ProductModel>();
            chromeDriverModel.GoToUrl(url: url);
            int pageCount = Convert.ToInt32(Math.Ceiling(double.Parse(chromeDriverModel.FindElement(By.CssSelector("div > div.filter__header > div")).Text.Split(" ")[0]) / 30));
            string currentUrl = chromeDriverModel.GetCurrentUrl();
            int firstScrollHeight = 500;
            for (int i = 1; i <= pageCount; i++)
            {
                long lastScrollHeight = 0;
                string newUrl = currentUrl + $"?sayfa={i}";
                chromeDriverModel.GoToUrl(newUrl);
                chromeDriverModel.ChromeDriver.Manage().Timeouts().PageLoad.Add(TimeSpan.FromSeconds(30));
                // Scroll whole page
                while (true)
                {
                    Thread.Sleep(750);
                    chromeDriverModel.ExecuteScript($"window.scrollTo({lastScrollHeight}, {lastScrollHeight + firstScrollHeight})");
                    long newScrollHeight = (long)chromeDriverModel.ExecuteScript("return document.body.scrollHeight;");
                    lastScrollHeight += firstScrollHeight;
                    if (newScrollHeight / firstScrollHeight == lastScrollHeight / firstScrollHeight) break;
                }
                var products = chromeDriverModel.FindElements(by: By.CssSelector("div[class*='product-cards'] *[class*='mdc-card']"));
                foreach (var item in products)
                {
                    string name = item.FindElement(By.CssSelector("a[class*='product-name']")).Text;
                    string price = item.FindElement(By.CssSelector("div[class*='price-new'] span[class*='amount']")).Text;
                    string image = item.FindElement(By.TagName("img")).GetAttribute("src");
                    var model = new ProductModel { Name = name, Image = image, Price = price, WebsiteId = 1 };
                    productList.Add(model);
                }
            }
            chromeDriverModel.DriverClose();

            return productList;
        }
    }
}
