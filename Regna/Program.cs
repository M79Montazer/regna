using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Regna.Services;
using Regna.Data;
using Regna.Models;

namespace Regna
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {

                //CoreService.Regna();
                //khello
                var db = new Context();


                //var players = db.Users.Where(a => true).Take(2).ToList();
                //MatchService.StartGame(players[0].Id, players[1].Id);
                //var options = new List<List<string>>();
                //var o1 = new List<string>();
                //o1.Add("1");
                //o1.Add("2");
                //var o2 = new List<string>();
                //o2.Add("3");
                //o2.Add("4");
                //options.Add(o1);
                //options.Add(o2);
                //TelegramService.SendMessage(players[0].TelegramId, "h");
                //var a = TelegramService.GetUpdates();
                //TelegramService.SendMessage(a.First().UserId, "kkk", null);

                var rnd = new Random();
                for (int i = 0; i < 20; i++)
                {
                    db.CardInDecks.Add(new CardInDeck
                    {
                        CreateDate=DateTime.Now,
                        Deleted=false,
                        OCardId=i+43,
                        UserId=2
                    });
                }
                db.SaveChanges();
                //var c = db.OCards.Where(a => !a.Deleted).ToList();
                //foreach (var item in c)
                //{
                //    item.AP = rnd.Next(1, 5);
                //    item.HP = rnd.Next(1, 5);
                //    item.MP = rnd.Next(1, 5);
                //}
                //db.SaveChanges();



                //var b =db.CardInDecks.Add(new CardInDeck {OCardId });
                //b.Username = "kkk";
                //db.SaveChanges();
            }
            catch (Exception e)
            {

                throw;
            }
        }
    }
}
