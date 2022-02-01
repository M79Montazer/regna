using Regna.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Regna.Models
{
    public class OCardEventListener : BaseEntity
    {
        public int Id { get; set; }
        public EventListener EventListener{ get; set; }
        public int OCardId { get; set; }
        public OCard OCard { get; set; }
    }
}
