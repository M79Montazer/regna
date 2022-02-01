using Regna.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Regna.Data
{
    public class Context : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<OCard> OCards { get; set; }
        public DbSet<CardInDeck> CardInDecks { get; set; }
        public DbSet<Event> Events { get; set; }

    }
}
