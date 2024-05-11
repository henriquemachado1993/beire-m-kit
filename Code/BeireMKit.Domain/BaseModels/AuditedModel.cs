using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeireMKit.Domain.BaseModels
{
    public class AuditedModel
    {
        public string? UserName { get; set; }
        public string? UserIdentifier { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }

        public string? CreatedAtText => CreatedAt?.ToString("dd/MM/yyyy HH:mm:ss");
        public string? LastModifiedAtText => CreatedAt?.ToString("dd/MM/yyyy HH:mm:ss");
    }
}
