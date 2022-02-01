using Regna.Data;
using Regna.Dto;
using Regna.Enum;
using Regna.Models;
using Regna.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using Regna.Models.Enum;

namespace Regna.Manager
{
    public static class GameManager
    {
        private static readonly Random rnd = new Random();
        private static readonly Context db = new Context();
        private static List<string> log = new List<string>();
        private static bool firstTime = true;

        public static void Manage(UpdateDto update, int userId)
        {
            var user = db.Users.First(a => !a.Deleted && a.Id == userId);
            try
            {
                switch (user.IStatus)
                {
                    case IStatus.threeToPlace:
                        {
                            var card = db.Cards.First(a => !a.Deleted && a.UserId == user.Id && a.Name == update.Text);
                            Play(user, card.Id);
                            user.IStatus = IStatus.twoToPlace;
                            db.SaveChanges();
                            //ShowGround(user.MatchId ?? 0, user.Id);
                            ManageStatusChange(user);
                        }
                        break;
                    case IStatus.twoToPlace:
                        {
                            var card = db.Cards.First(a => !a.Deleted && a.UserId == user.Id && a.Name == update.Text);
                            Play(user, card.Id);
                            user.IStatus = IStatus.oneToPlace;
                            db.SaveChanges();
                            //ShowGround(user.MatchId ?? 0, user.Id);
                            ManageStatusChange(user);
                        }
                        break;
                    case IStatus.oneToPlace:
                        {
                            if (user.Ready)
                            {
                                TelegramService.SendMessage(user.TelegramId, user.Language == Language.English ? "It is your opponent's turn!" : "الآن نوبت حریف شماست!!");
                            }
                            else
                            {
                                var card = db.Cards.First(a => !a.Deleted && a.UserId == user.Id && a.Name == update.Text);
                                Play(user, card.Id);
                                user.IStatus = IStatus.placeCard;
                                db.SaveChanges();
                                var opponent = db.Users.First(a => !a.Deleted && a.Id == user.OpponentId);
                                if (opponent.Ready)
                                {
                                    ManageStatusChange(user);
                                    ManageStatusChange(opponent);
                                    opponent.Ready = false;
                                    db.SaveChanges();
                                }
                                else
                                {
                                    TelegramService.SendMessage(user.TelegramId, user.Language == Language.English ? "Waiting for opponet move..." : "در حال انتظار برای حرکت حریف...");
                                    user.Ready = true;
                                    db.SaveChanges();
                                }
                            }
                        }
                        break;
                    case IStatus.placeCard:
                        {
                            if (user.Ready)
                            {
                                TelegramService.SendMessage(user.TelegramId, user.Language == Language.English ? "It is your opponent's turn!" : "الآن نوبت حریف شماست!!");
                            }
                            else
                            {
                                var card = db.Cards.First(a => !a.Deleted && a.UserId == user.Id && a.Name == update.Text);
                                Play(user, card.Id);
                                user.IStatus = IStatus.SelectCard;
                                db.SaveChanges();
                                var opponent = db.Users.First(a => !a.Deleted && a.Id == user.OpponentId);
                                if (opponent.Ready)
                                {
                                    ManageStatusChange(user);
                                    ManageStatusChange(opponent);
                                    opponent.Ready = false;
                                    db.SaveChanges();
                                }
                                else
                                {
                                    TelegramService.SendMessage(user.TelegramId, user.Language == Language.English ? "Waiting for opponet move..." : "در حال انتظار برای حرکت حریف...");
                                    user.Ready = true;
                                    db.SaveChanges();
                                }
                            }
                        }
                        break;
                    case IStatus.SelectCard:
                        {
                            var selected = db.Cards.First(a => !a.Deleted && a.UserId == user.Id && a.Name == update.Text);
                            selected.IsSelectedForAction = true;
                            user.IStatus = IStatus.SelectTarget;
                            db.SaveChanges();
                            ManageStatusChange(user);

                        }
                        break;
                    case IStatus.SelectTarget:
                        {
                            var target = db.Cards.First(a => !a.Deleted && a.UserId == user.OpponentId && a.Name == update.Text);
                            var selected = db.Cards.First(a => !a.Deleted && a.UserId == user.Id && a.IsSelectedForAction == true);
                            selected.IsSelectedForAction = false;
                            user.IStatus = IStatus.placeCard;
                            db.SaveChanges();
                            Action(user, selected.Id, target.Id);
                            var opponent = db.Users.First(a => !a.Deleted && a.Id == user.OpponentId);
                            if (opponent.Ready)
                            {
                                ManageStatusChange(user);
                                ManageStatusChange(opponent);
                                opponent.Ready = false;
                                log = new List<string>();
                                db.SaveChanges();
                            }
                            else
                            {
                                TelegramService.SendMessage(user.TelegramId, user.Language == Language.English ? "Waiting for opponet move..." : "در حال انتظار برای حرکت حریف...");
                                user.Ready = true;
                                db.SaveChanges();
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
        }
        public static void ManageStatusChange(User user)
        {
            try
            {
                switch (user.IStatus)
                {
                    case IStatus.threeToPlace:
                        InitHand(user);
                        ShowHand(user.Id);
                        break;
                    case IStatus.twoToPlace:
                        ShowHand(user.Id);
                        break;
                    case IStatus.oneToPlace:
                        ShowHand(user.Id);
                        Draw(user);
                        Draw(user);
                        break;
                    case IStatus.placeCard:
                        if (firstTime)
                        {
                            ShowGround(user.MatchId ?? 0, user.Id);
                            firstTime = false;
                        }
                        else
                        {
                            var pm = "";
                            foreach (var item in log)
                            {
                                pm += item + '\n';
                            }
                            TelegramService.SendMessage(user.TelegramId, pm);
                        }
                        Draw(user);
                        ShowHand(user.Id);
                        break;
                    case IStatus.SelectCard:
                        ShowGround(user.MatchId ?? 0, user.Id);
                        ShowGroundCards(user.Id);
                        break;
                    case IStatus.SelectTarget:
                        var targets = GetAvailableTargets(db.Cards.First(a => !a.Deleted && a.UserId == user.Id && a.IsSelectedForAction == true).Id);
                        var selected = db.Cards.First(a => !a.Deleted && a.UserId == user.Id && a.IsSelectedForAction == true);
                        db.SaveChanges();
                        var cardNames = targets.Select(a => a.Name).ToList();
                        var options = new List<List<string>>();
                        foreach (var option in cardNames)
                        {
                            options.Add(new List<string> { option });
                        }
                        TelegramService.SendMessage(user.TelegramId, user.Language == Language.English ? "Select target card" : "کارت هدف را انتخاب کنید", options);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
        }
        public static bool ShowGroundCards(int userId)
        {
            try
            {
                var user = db.Users.First(a => !a.Deleted && a.Id == userId);
                var cards = db.Cards.Where(a => !a.Deleted && a.UserId == user.Id && a.CardStatus == CardStatus.OnGround).ToList();
                var cardNames = cards.Select(a => a.Name).ToList();
                var options = new List<List<string>>();
                foreach (var option in cardNames)
                {
                    options.Add(new List<string> { option });
                }
                TelegramService.SendMessage(user.TelegramId, user.Language == Language.English ? "Select a card for action" : "یک کارت برای استفاده کردن انتخاب کنید", options);
                return true;
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                return false;
            }
        }
        public static bool ShowHand(int userId)
        {
            try
            {
                var user = db.Users.First(a => !a.Deleted && a.Id == userId);
                var cards = db.Cards.Where(a => !a.Deleted && a.UserId == user.Id && a.CardStatus == CardStatus.InHand).ToList();
                var cardNames = cards.Select(a => a.Name).ToList();
                var options = new List<List<string>>();
                foreach (var option in cardNames)
                {
                    options.Add(new List<string> { option });
                }
                TelegramService.SendMessage(user.TelegramId, user.Language == Language.English ? "Select a card to play" : "یک کارت برای بازی کردن انتخاب کنید", options);
                return true;
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                return false;
            }
        }
        public static List<Card> GetAvailableTargets(int cardId)
        {
            try
            {
                var card = db.Cards.Where(a => !a.Deleted && a.Id == cardId).Include(a => a.User).FirstOrDefault();
                var cards = db.Cards.Where(a => !a.Deleted && a.UserId == card.User.OpponentId && a.CardStatus == CardStatus.OnGround).ToList();
                return cards;
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                return null;
            }
        }
        public static bool InitHand(User user)
        {
            try
            {
                var deck = db.Cards.Where(a => !a.Deleted && !a.IsSpell && a.UserId == user.Id && a.CardStatus == CardStatus.InDeck).ToList();
                for (int i = 0; i < 5; i++)
                {
                    var randomCard = deck[rnd.Next(deck.Count)];
                    deck.Remove(randomCard);
                    randomCard.CardStatus = CardStatus.InHand;
                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                return false;
            }
        }
        public static bool Draw(User user)
        {
            try
            {
                var deck = db.Cards.Where(a => !a.Deleted && a.UserId == user.Id && a.CardStatus == CardStatus.InDeck).ToList();
                var randomCard = deck[rnd.Next(deck.Count)];
                randomCard.CardStatus = CardStatus.InHand;
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                return false;
            }
        }
        public static bool Play(User user, int cardId)
        {
            try
            {
                var card = db.Cards.First(a => !a.Deleted && a.Id == cardId);
                card.CardStatus = CardStatus.OnGround;
                EventManager.HandleEvent(cardId, EventListener.OnPlay);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                return false;
            }
        }
        public static bool Action(User user, int cardId, int targetId)
        {
            try
            {
                var card = db.Cards.First(a => !a.Deleted && a.Id == cardId);
                var target = db.Cards.First(a => !a.Deleted && a.Id == targetId);
                var l = "Card : " + target.Name + " HP " + target.HP + "->";
                target.HP -= card.AP;
                l += target.HP;
                log.Add(l);
                if (target.HP <= 0)
                {
                    var opponentPlayer = db.Users.First(a => !a.Deleted && a.Id == target.UserId);
                    target.CardStatus = CardStatus.Dead;
                    var l2 = "Player : " + opponentPlayer.Username + " MP " + opponentPlayer.MP + "->";
                    opponentPlayer.MP -= target.MP;
                    l2 += opponentPlayer.MP;
                    log.Add(l2);
                    if (opponentPlayer.MP <= 0)
                    {
                        var match = db.Matches.First(a => !a.Deleted && a.Id == opponentPlayer.MatchId);
                        var winner = db.Users.First(a => !a.Deleted && a.Id == card.UserId);
                        FinishMatch(match.Id, winner.Id);
                    }
                }
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                return false;
            }
        }
        public static bool FinishMatch(int matchId, int winnerId)
        {
            var match = db.Matches.First(a => !a.Deleted && a.Id == matchId);
            var winner = db.Users.First(a => !a.Deleted && a.Id == winnerId);
            var looser = db.Users.First(a => !a.Deleted && a.Id == winner.OpponentId);

            TelegramService.SendMessage(winner.TelegramId, "you won!!!");
            TelegramService.SendMessage(looser.TelegramId, "you lost :(((");
            return true;
        }
        public static bool ShowGround(int matchId, int userId)
        {
            var match = db.Matches.First(a => !a.Deleted && a.Id == matchId);
            var user = db.Users.First(a => !a.Deleted && a.Id == userId);
            var opponent = db.Users.First(a => !a.Deleted && a.Id == user.OpponentId);
            var userCards = db.Cards.Where(a => !a.Deleted && a.UserId == user.Id && a.CardStatus == CardStatus.OnGround);
            var opponentCards = db.Cards.Where(a => !a.Deleted && a.UserId == opponent.Id && a.CardStatus == CardStatus.OnGround);

            var ground = (user.Language == Language.English ? "Play Ground:" : "زمین بازی") + "\n\n\n---------------------\n";
            ground += (user.Language == Language.English ? "Your cards:" : "کارت های شما") + "\n";
            foreach (var card in userCards)
            {
                ground += card.Name + " AP: " + card.AP.ToString() + " HP: " + card.HP.ToString() + " MP: " + card.MP.ToString() + '\n';
            }

            ground += "---------------------\n" + (user.Language == Language.English ? "Enemy cards:" : "کارت های دشمن") + "\n";
            foreach (var card in opponentCards)
            {
                ground += card.Name + " AP: " + card.AP.ToString() + " HP: " + card.HP.ToString() + " MP: " + card.MP.ToString() + '\n';
            }

            TelegramService.SendMessage(user.TelegramId, ground);
            return true;
        }
        //public static bool WaitForOtherPlayer(User user)
        //{
        //    try
        //    {

        //        return true;
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //        return false;
        //    }
        //}
    }
}
