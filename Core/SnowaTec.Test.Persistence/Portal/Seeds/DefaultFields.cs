using SnowaTec.Test.Domain.Entities.Possibility;

namespace SnowaTec.Test.Persistence.Portal.Seeds
{
    public static class DefaultFields
    {
        public static List<Field> List()
        {
            var id = 1;
            return new List<Field>
            {
                new Field
                {
                    Id= id++,
                    Title = "نام",
                    Key = "FirstName",
                    Type = Domain.Enum.FieldType.String
                },
                new Field
                {
                    Id= id++,
                    Title = "نام خانوادگی",
                    Key = "LastName",
                    Type = Domain.Enum.FieldType.String
                },
                new Field
                {
                    Id= id++,
                    Title = "کدملی",
                    Key = "NationalCode",
                    Type = Domain.Enum.FieldType.NationalCode
                },
                new Field
                {
                    Id= id++,
                    Title = "موبایل",
                    Key = "Mobile",
                    Type = Domain.Enum.FieldType.Phone
                },
                new Field
                {
                    Id= id++,
                    Title = "جنسیت",
                    Key = "Sex",
                    Type = Domain.Enum.FieldType.SelectBox,
                    Setting = "مرد-زن"
                },
                new Field
                {
                    Id= id++,
                    Title = "آدرس",
                    Key = "Address",
                    Type = Domain.Enum.FieldType.LongString
                },
            };
        }
    }
}
