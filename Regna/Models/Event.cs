using Regna.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Regna.Models
{
    public class Event : BaseEntity
    {
        public int Id { get; set; }
        public EventListener EventListener{get;set;}
        public int OCardId { set; get; }
        public OCard OCard { get; set; }
    }
}
