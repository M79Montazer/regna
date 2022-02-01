using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Regna.Models
{
    public class BaseEntity
    {
        public DateTime CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public bool Deleted { get; set; }

    }
}
