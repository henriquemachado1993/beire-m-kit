using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeireMKit.Domain.BaseModels
{
    public class BaseModel : AuditedModel
    {
        public int Id { get; set; }
    }
}
