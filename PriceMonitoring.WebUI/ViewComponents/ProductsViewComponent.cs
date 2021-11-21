using Microsoft.AspNetCore.Mvc;

namespace PriceMonitoring.WebUI.ViewComponents
{
    public class ProductsViewComponent : ViewComponent
    {

        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
