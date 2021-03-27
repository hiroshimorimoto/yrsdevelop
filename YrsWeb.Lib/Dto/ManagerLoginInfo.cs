using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using YrsWeb.Lib.Models;

namespace YrsWeb.Lib.Dto
{
    public class ManagerLoginInfo
    {
        public const string SESS_KEY_LOGIN_INFO = "SESS_KEY_LOGIN_INFO_M";
        private static ManagerLoginInfo _instacne = null;
        public static ManagerLoginInfo Instance => _instacne;

        public ManagerLoginInfo()
        {
            _instacne = this;
        }

        public static void Logout()
        {
            _instacne = null;
        }

        public Manager_M Manager { get; set; } = null;

    }
}
