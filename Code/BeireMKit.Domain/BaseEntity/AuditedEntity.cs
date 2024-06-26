﻿namespace BeireMKit.Domain.Entity
{
    public class AuditedEntity
    {
        public string? UserName { get; set; }
        public string? UserIdentifier { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }
    }
}
