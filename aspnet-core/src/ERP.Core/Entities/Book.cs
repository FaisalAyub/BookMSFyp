using ERP.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace ERP.Entities
{
	[Table("Books")]
    [Audited]
    public class Book : FullAuditedEntity 
    {

		public virtual string Title { get; set; }
		
		public virtual string ISBN { get; set; }
		
		public virtual string Author { get; set; }
		
		public virtual string Description { get; set; }
		
		public virtual string Publisher { get; set; }
		
		public virtual int Price { get; set; }
		
		public virtual int Quantity { get; set; }
		

		public virtual long OwnerId { get; set; }
		
        [ForeignKey("OwnerId")]
		public User OwnerFk { get; set; }
		
    }
}