using Regna.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Regna.Models
{
    public class Card : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsSpell { get; set; }
        public int MatchId { get; set; }
        public Match Match { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int AP { get; set; }
        public int HP { get; set; }
        public int MP { get; set; }
        public CardStatus CardStatus { set; get; }
        public int OCardId { get; set; }
        public OCard OCard { get; set; }
        public bool? IsSelectedForAction { get; set; }
    }
}
