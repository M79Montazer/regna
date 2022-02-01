using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Regna.Dto.APIDto.Request
{
    public class TelegramDto<T>
    {
        public bool Ok { get; set; }
        public T Result { get; set; }
    }
}
