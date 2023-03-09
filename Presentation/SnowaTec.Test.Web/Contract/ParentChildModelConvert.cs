using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;

namespace SnowaTec.Test.Web.Contract
{
    public static class ParentChildModelConvert
    {
        public static List<T> RecursiceContent<T>(List<T> data, long? parentId, string propIdName, string propParentIdName, string propChildName)
        {
            try
            {
                var model = data.Where(x => (long?)x.GetType().GetProperty(propParentIdName).GetValue(x, null) == parentId).ToList();

                model.ForEach(x =>
                {
                    var prop = x.GetType().GetProperty(propChildName, BindingFlags.Public | BindingFlags.Instance);

                    if (prop != null && prop.CanWrite)
                    {
                        var childParentId = x.GetType().GetProperty(propIdName).GetValue(x, null);
                        prop.SetValue(x, RecursiceContent<T>(data, (long)childParentId, propIdName, propParentIdName, propChildName));
                    }
                });

                return model;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
