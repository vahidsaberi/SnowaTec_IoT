using SnowaTec.Test.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SnowaTec.Test.Domain.DTO.Possibility
{
    public class DocumentDto : BaseEntity
    {
        [Display(Name = "نام فایل")]
        [MaxLength(100)]
        public string Name { get; set; }

        [Display(Name = "نوع محتوا")]
        [MaxLength(100)]
        public string ContentType { get; set; }

        [Display(Name = "پسوند فایل")]
        [MaxLength(5)]
        public string Extension { get; set; }

        [Display(Name = "سایز")]
        public long Size { get; set; }

        [NotMapped]
        public string SizeValue
        {
            get
            {
                string[] suffixes = { "B", "KB", "MB", "GB", "TB", "PB" };
                int counter = 0;
                decimal number = Size;
                while (Math.Round(number / 1024) >= 1)
                {
                    number = number / 1024;
                    counter++;
                }
                return string.Format("{0:n1}{1}", number, suffixes[counter]);
            }
        }

        [Display(Name = "محتوا")]
        public byte[] Content { get; set; }

        [Display(Name = "تصویر بندانگشتی")]
        [NotMapped]
        public byte[] Thumbnail { get; set; }

        [Display(Name = "عبارت کلیدی")]
        public string? Key { get; set; }

        [Display(Name = "ماژول")]
        public ModuleType? ModuleId { get; set; }

        [Display(Name = "شناسه رکورد در ماژول")]
        public long? ModuleRecordId { get; set; }

        [Display(Name = "شناسه زیر رکورد در ماژول")]
        public long? ModuleSubRecordId { get; set; }

        [Display(Name = "شناسه کاربر")]
        public long UserId { get; set; }

        [Display(Name = "تگ ها")]
        public string? Tags { get; set; }

        [NotMapped]
        public List<string> TagsValue
        {
            get
            {
                return this.Tags != null ? this.Tags.Split(',').ToList() : new List<string>();
            }
        }

        [Display(Name = "ترتیب نمایش")]
        public int Order { get; set; }
    }
}
