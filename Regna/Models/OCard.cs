using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Regna.Models
{
    public class OCard : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsSpell { get; set; }
        public int AP { get; set; }
        public int HP { get; set; }
        public int MP { get; set; }
        public virtual List<Event> Events { get; set; }
    }
}
