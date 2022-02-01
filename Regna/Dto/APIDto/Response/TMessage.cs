using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Regna.Dto.APIDto.Response
{
    public class TMessage
    {
        public int chat_id { get; set; }
        public string text { get; set; }
        public object reply_markup { get; set; }
    }
}
