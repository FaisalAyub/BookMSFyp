using ERP.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.Entities
{
    [Table("Orders")]
    public class Order : FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual DateTime OrderDate { get; set; }

        public virtual string Description { get; set; }

        public virtual string Status { get; set; }

        public virtual int TotalBill { get; set; }

        public virtual string OrderName { get; set; }

        public virtual long? OrderBy { get; set; }

        [ForeignKey("OrderBy")]
        public User OrderByFk { get; set; }

    }
}