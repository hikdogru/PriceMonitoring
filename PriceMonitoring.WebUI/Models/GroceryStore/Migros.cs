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
            chromeDriverModel.ExecuteScript("window.scrollTo(0, 1000)");
            Thread.Sleep(1000);
            int pageCount = Convert.ToInt32(Math.Ceiling(double.Parse(chromeDriverModel.FindElement(By.CssSelector("div > div.filter__header > div")).Text.Split(" ")[0]) / 30));
            string currentUrl = chromeDriverModel.GetCurrentUrl();

            for (int i = 1; i <= pageCount; i++)
            {
                string newUrl = currentUrl + $"?sayfa={i}";
                chromeDriverModel.GoToUrl(newUrl);
                Thread.Sleep(1000);
                chromeDriverModel.ExecuteScript("window.scrollTo(0, 750)");
                Thread.Sleep(500);
                chromeDriverModel.ExecuteScript("window.scrollTo(750, 1500)");
                var products = chromeDriverModel.FindElements(by: By.CssSelector("div[class*='product-cards'] *[class*='mdc-card']"));
                foreach (var item in products)
                {
                    string name = item.FindElement(By.CssSelector("a[class*='product-name']")).Text;
                    string price = item.FindElement(By.CssSelector("div[class*='price-new'] span[class*='amount']")).Text;
                    string image = item.FindElement(By.TagName("img")).GetAttribute("src");
                    var model = new ProductModel { Name = name, Image = image, Price = price };
                    productList.Add(model);
                }
            }
            chromeDriverModel.DriverClose();

            return productList;
        }
    }
}
