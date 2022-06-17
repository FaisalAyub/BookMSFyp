using ERP.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.Entities
{
    [Table("OrderItems")]
    public class OrderItem : FullAuditedEntity
    {

        public virtual int Quantity { get; set; }

        public virtual int Price { get; set; }

        public virtual int BookId { get; set; }

        [ForeignKey("BookId")]
        public Book BookFk { get; set; }

    }
}