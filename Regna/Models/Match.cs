using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Regna.Models
{
    public class Match : BaseEntity
    {
        public int Id { get; set; }
        public int Player1Id { get; set; }
        public int Player2Id { get; set; }
        public DateTime StartTime { get ; set; }
        public int WinnerId { get; set; }
        public bool Active { get; set; }
    }
}
