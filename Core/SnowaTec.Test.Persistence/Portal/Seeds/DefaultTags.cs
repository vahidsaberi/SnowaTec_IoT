using SnowaTec.Test.Domain.Entities.Possibility;
using System.Collections.Generic;

namespace SnowaTec.Test.Persistence.Portal.Seeds
{
    public static class DefaultTags
    {
        public static List<Tag> List()
        {
            var id = 1;
            return new List<Tag>
            {
                new Tag
                {
                    Id = id++,
                    ParentId = null,
                    Title = "اطلاعات پرسنلی"
                },
                new Tag
                {
                    Id = id++,
                    ParentId = 1,
                    Title = "شناسنامه"
                },
                new Tag
                {
                    Id = id++,
                    ParentId = 1,
                    Title = "کارت ملی"
                },
                new Tag
                {
                    Id = id++,
                    ParentId = 1,
                    Title = "عکس پرسنلی"
                }
            };
        }
    }
}
