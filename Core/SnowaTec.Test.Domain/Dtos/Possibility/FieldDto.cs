using SnowaTec.Test.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SnowaTec.Test.Domain.DTO.Possibility
{
    public class FieldDto : BaseEntity
    {

        [Display(Name = "عنوان")]
        [MaxLength(200)]
        public string Title { get; set; }

        [Display(Name = "کلید / عنوان لاتین")]
        [MaxLength(100)]
        public string? Key { get; set; }

        [Display(Name = "راهنما")]
        [MaxLength(500)]
        public string? Tooltip { get; set; }

        [Display(Name = "نوع مقدار")]
        public FieldType Type { get; set; }

        [NotMapped]
        public string TypeValue { get { return FieldTypeFunction.GetValue(Type); } }

        [Display(Name = "مقدار پیشفرض")]
        public string? DefaultValue { get; set; }

        [Display(Name = "تنظیمات فیلد")]
        public string Setting { get; set; }
    }
}
