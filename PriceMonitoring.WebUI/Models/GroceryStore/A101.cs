using OpenQA.Selenium;
using PriceMonitoring.WebUI.Models.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PriceMonitoring.WebUI.Models.GroceryStore
{
    public class A101
    {
        public IEnumerable<ProductModel> GetProducts(string url)
        {
            var numbers = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            ICustomChromeDriver chromeDriverModel = new CustomChromeDriver();
            var productList = new List<ProductModel>();
            chromeDriverModel.GoToUrl(url: url);
            chromeDriverModel.FindElement(by: By.CssSelector("div.hype-row > button:nth-child(2)")).Click();
            var productCount = chromeDriverModel.FindElement(By.CssSelector("div.total-product-count.hidden-xs ")).Text.Split(" ").Where(x => numbers.Any(y => x.Contains(y.ToString()))).ToList()[0];
            int pageCount = Convert.ToInt32(Math.Ceiling(double.Parse(productCount) / 48));
            string currentUrl = chromeDriverModel.GetCurrentUrl();
            int firstScrollHeight = 1000;
            for (int i = 1; i <= pageCount; i++)
            {
                long lastScrollHeight = 0;
                string newUrl = currentUrl + $"?page={i}";
                chromeDriverModel.GoToUrl(newUrl);
                // Scroll whole page
                while (true)
                {
                    chromeDriverModel.ExecuteScript($"window.scrollTo({lastScrollHeight}, {lastScrollHeight + firstScrollHeight})");
                    long newScrollHeight = (long)chromeDriverModel.ExecuteScript("return document.body.scrollHeight;");
                    Thread.Sleep(200);
                    lastScrollHeight += firstScrollHeight;
                    if (newScrollHeight / firstScrollHeight == lastScrollHeight / firstScrollHeight) break;
                }

                var products = chromeDriverModel.FindElements(by: By.CssSelector("div[class*='products-list'] > div > ul > li"));
                foreach (var item in products)
                {
                    string name = item.FindElement(By.CssSelector(" header > hgroup > h3")).Text;
                    string price = item.FindElement(By.CssSelector("section.prices > span")).Text;
                    string image = item.FindElement(By.CssSelector(" div > a > figure > img")).GetAttribute("src");
                    var model = new ProductModel { Name = name, Image = image, Price = price , WebsiteId = 2};
                    productList.Add(model);
                }

            }
            chromeDriverModel.DriverClose();

            return productList;
        }
    }
}
