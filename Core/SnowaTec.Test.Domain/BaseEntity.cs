using System.ComponentModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace SnowaTec.Test.Domain
{
    public abstract class BaseEntity<TKey>
    {
        [Key]
        public TKey Id { get; set; }

        public DateTime CreateRowDate { get; set; }

        public DateTime UpdateRowDate { get; set; }

        [DefaultValue(false)]
        public bool Deleted { get; set; }
    }

    public abstract class BaseEntity : BaseEntity<long>
    {
        public BaseEntity()
        {
            CreateRowDate = DateTime.Now;
            UpdateRowDate = DateTime.Now;
        }
    }
}
