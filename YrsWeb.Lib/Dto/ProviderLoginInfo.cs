using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using YrsWeb.Lib.Models;

namespace YrsWeb.Lib.Dto
{
    public class ProviderLoginInfo
    {
        public const string SESS_KEY_LOGIN_INFO = "SESS_KEY_LOGIN_INFO_P";
        private static ProviderLoginInfo _instacne = null;
        public static ProviderLoginInfo Instance => _instacne;

        public ProviderLoginInfo()
        {
            _instacne = this;
        }

        public static void Logout()
        {
            _instacne = null;
        }

        public Provider_M Provider{ get; set; } = null;

    }
}
