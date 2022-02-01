using Regna.Data;
using Regna.Enum;
using Regna.Manager;
using Regna.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Regna.Services
{
    public static class MatchService
    {
        static readonly Context db = new Context();
        public static void StartGame(int p1, int p2)
        {
            try
            {
                var Player1 = db.Users.First(a => !a.Deleted && a.Id == p1);
                var Player2 = db.Users.First(a => !a.Deleted && a.Id == p2);
                var match = new Match
                {
                    CreateDate = DateTime.Now,
                    Deleted = false,
                    Player1Id = Player1.Id,
                    Player2Id = Player2.Id,
                    StartTime = DateTime.Now,
                    Active = true
                };
                db.Matches.Add(match);
                db.SaveChanges();
                ff(Player1, Player2,match.Id);
                ff(Player2, Player1,match.Id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        static void ff(User p, User opponent,int matchId)
        {
            p.Status = PlayerStatus.Playing;
            p.IStatus = IStatus.threeToPlace;
            p.MatchId = matchId;
            p.OpponentId = opponent.Id;
            p.MP = 20;
            p.Ready = false;
            db.SaveChanges();
            var deck = db.CardInDecks.Where(a => !a.Deleted && a.UserId == p.Id).Include(a => a.OCard).Select(a => a.OCard).ToList();
            foreach (var c in deck)
            {
                db.Cards.Add(new Card {OCardId=c.Id, IsSpell = c.IsSpell, Name = c.Name, AP = c.AP, HP = c.HP, MatchId = matchId, MP = c.MP, UserId = p.Id, CreateDate = DateTime.Now, CardStatus = CardStatus.InDeck });
            }
            db.SaveChanges();
            TelegramService.SendMessage(p.TelegramId, (
                p.Language == Enum.Language.English ? "Game started. your opponent is: " : "بازی شروع شد. حریف شما: ")
                + opponent.Username);
            GameManager.ManageStatusChange(p);
        }
    }
}
