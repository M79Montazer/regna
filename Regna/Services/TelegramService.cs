using Newtonsoft.Json;
using Regna.Dto;
using Regna.Dto.APIDto.Request;
using Regna.Dto.APIDto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Regna.Services
{
    public static class TelegramService
    {
        static readonly string uri = "https://api.telegram.org/bot1159825795:AAHKNHjYfJduYmNNLecXfbOf6EZju3ybj0g";
        static int offset = 0;
        public static bool GetMe()
        {
            var client = new HttpClient() { BaseAddress = new Uri(uri+"/getme") };
            var apiResponse = client.GetAsync("").Result.Content.ReadAsStringAsync().Result;
            Console.WriteLine(apiResponse);
            return true;
        }
        public static List<UpdateDto> GetUpdates()
        {
            try
            {

                var client = new HttpClient() { BaseAddress = new Uri(uri + "/GetUpdates?offset=" + offset.ToString()) };
                var apiResponse = client.GetAsync("").Result.Content.ReadAsStringAsync().Result;
                //Console.WriteLine(apiResponse);
                var res = JsonConvert.DeserializeObject<TelegramDto<List<TUpdateDto>>>(apiResponse);
                var r = res.Result.Select(a => new UpdateDto { Text = a.Message.Text, UserId = a.Message.Chat.Id }).ToList();
                offset = res.Result.Last().Update_id + 1;
                return r;
            }
            catch (Exception)
            {

                return null; ;
            }
        }
        public static bool SendMessage(int id, string text,List<List<string>> options = null)
        {
            var m = new TMessage();
            if (options == null)
            {
                m.chat_id = id;
                m.reply_markup = new ReplyKeyboardRemove { remove_keyboard =true };
                m.text = text;
                var client = new HttpClient() { BaseAddress = new Uri(uri + "/SendMessage") };
                var apiResponse = client.PostAsJsonAsync("",m).Result.Content.ReadAsStringAsync().Result;

            }
            else
            {
                
                m.chat_id = id;
                m.text = text;
                m.reply_markup = new ReplyKeyboardMarkup { keyboard = options.Select(a => a.Select(b => new KeyboardButton { text = b }).ToList()).ToList() };

                var client = new HttpClient() { BaseAddress = new Uri(uri + "/SendMessage") };
                var apiResponse = client.PostAsJsonAsync("", m).Result.Content.ReadAsStringAsync().Result;
            }
            return true;
        }
    }
}
