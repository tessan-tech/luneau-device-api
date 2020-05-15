using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuneauPortal
{
    public class JwtInfos
    {
        public string PublicKey { get; set; }
    }

    public class AppSettings
    {
        public JwtInfos Jwt { get; set; }
    }
}
