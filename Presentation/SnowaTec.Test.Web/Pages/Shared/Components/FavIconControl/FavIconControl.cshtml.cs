using Microsoft.AspNetCore.Mvc;

namespace SnowaTec.Test.Web.Pages.Shared.Components.FavIconControl
{
    [ViewComponent(Name = "FavIconControl")]
    public class FavIconControl : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("FavIconControl");
        }
    }
}
