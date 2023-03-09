using System;
using System.Collections.Generic;
using System.Linq;

namespace SnowaTec.Test.Domain.Helper
{
    public class ModelMapHelper
    {
        public static object GetFrom(object destination, object source, List<string> NotMap = null, params KeyValuePair<string, object>[] specials)
        {
            try
            {
                var properties = destination.GetType().GetProperties();

                foreach (var item in properties)
                {
                    if (specials.Any(p => p.Key == item.Name))
                    {
                        var special = specials.FirstOrDefault(p => p.Key == item.Name);
                        var prop = destination.GetType().GetProperty(item.Name);
                        if (prop != null)
                        {
                            prop.SetValue(destination, Convert.ChangeType(special.Value, prop.PropertyType), null);
                        }
                        else
                        {
                            throw new Exception("نام فیلد ها با یک دیگر تطابق ندارند");
                        }
                    }
                    else
                    {
                        var prop = destination.GetType().GetProperty(item.Name);
                        if (prop == null) continue;

                        var sp = source.GetType().GetProperty(item.Name);
                        if (sp == null) continue;

                        var value = sp.GetValue(source);
                        if (prop.Name == "Id" && (long)value == 0)
                            continue;

                        if (NotMap != null && NotMap.Any(x => x == prop.Name))
                            continue;

                        prop.SetValue(destination, Convert.ChangeType(value, prop.PropertyType), null);
                    }
                }
                return destination;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
