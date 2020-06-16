using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyNote.YoutubeManagement.Configs
{
    public class Config
    {
        public static string Domain = string.Empty;
        public static string Solgan = string.Empty;
        public static void Init(IConfiguration configuration)
        {
            Domain = configuration["Domain"];
            Domain = configuration["Solgan"];
        }
    }
}
