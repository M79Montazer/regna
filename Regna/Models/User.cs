using Regna.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Regna.Models
{
    public class User : BaseEntity
    {
        public int Id { get; set; }
        public int TelegramId { get; set; }
        public string Username { get; set; }
        public PlayerStatus Status { get; set; }
        public Language Language { get; set; }
        public int? MatchId { get; set; }
        public Match Match { get; set; }
        public IStatus IStatus { get; set; }
        public string LastText { get; set; }
        public string TextHolder { get; set; }
        public int? MP { get; set; }
        public int? OpponentId { get; set; }
        public User Opponent { get; set; }
        public bool Ready { get; set; }
    }
}
