using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeireMKit.Domain.BaseModels
{
    public class DataItem<T>
    {
        public string Key { get; set; } = string.Empty;
        public T? Value { get; set; }
    }
}
