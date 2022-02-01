using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Regna.Dto.APIDto.Request
{
    public class MessageDto
    {
        public int Message_id { get; set; }
        public TUserDto From { get; set; }
        public long Date { set;get; }
        public DateTime Time { get; set; }
        public ChatDto Chat { get; set; }
        public string Text { get; set; }
    }
}
