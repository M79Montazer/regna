using Regna.Data;
using Regna.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Regna.Services
{
    public static class CoreService
    {
        public static void Regna()
        {
            var t = DateTime.Now;
            var db = new Context();
            var player1Id = 1;
            var player2Id =2;
            MatchService.StartGame(player1Id, player2Id);

            while (true)
            {
                var updates = TelegramService.GetUpdates();
                if (updates == null)
                {
                    Thread.Sleep(1000);
                }
                else
                {
                    try
                    {
                        foreach (var update in updates)
                        {
                            Console.WriteLine(update.Text);
                            var user = db.Users.First(a => !a.Deleted && a.TelegramId == update.UserId);
                            switch (user.Status)
                            {
                                case Enum.PlayerStatus.a:
                                    break;
                                case Enum.PlayerStatus.b:
                                    break;
                                case Enum.PlayerStatus.Idle:
                                    break;
                                case Enum.PlayerStatus.Playing:
                                    GameManager.Manage(update, user.Id);
                                    break;
                                case Enum.PlayerStatus.LookingForGame:
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        continue;
                    }
                }
            }

        }
    }
}
