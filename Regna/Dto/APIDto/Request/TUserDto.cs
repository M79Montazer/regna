using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Regna.Dto.APIDto.Request
{
    public class TUserDto
    {
        public int Id { get; set; }
        public bool Is_bot { get; set; }
        public string First_name{ get; set; }
        public string Last_name { get; set; }
        public string Username { get; set; }
        public string Language_code { get; set; }
    }
}
