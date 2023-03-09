using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;

namespace SnowaTec.Test.Web.Extensions
{
    public class LoadPartialView
    {
        public static PartialViewResult Show<T>(string viewName, T model, ViewDataDictionary viewData = null, ITempDataDictionary tempData = null)
        {
            try
            {
                return new PartialViewResult()
                {
                    ViewName = viewName,
                    ViewData = new ViewDataDictionary<T>(viewData, model),
                    TempData = tempData
                };
            }
            catch
            {
            }

            return null;
        }
    }
}
