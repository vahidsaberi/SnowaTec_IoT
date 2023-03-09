using SnowaTec.Test.Domain.Entities.Possibility;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace SnowaTec.Test.Web.Extensions
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class LimitCountAttribute : ValidationAttribute
    {
        private readonly int _min;
        private readonly int _max;

        public LimitCountAttribute(int min, int max)
        {
            _min = min;
            _max = max;
        }

        public override bool IsValid(object value)
        {
            var list = value as IList;

            return list.Count >= _min && list.Count <= _max;
        }
    }
}
