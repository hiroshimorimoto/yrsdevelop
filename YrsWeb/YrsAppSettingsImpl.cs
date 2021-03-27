using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YrsWeb
{
    class YrsAppSettingsImpl : IYrsAppSettings
    {
        public int PassResetExpirationMin { get; set; }

        public string SendMail_FromAddress { get; set; }

        public string SendMail_ManagerPassReset { get; set; }

        public string SendMail_ProviderPassReset { get; set; }

        public string SendMail_ProviderRegist { get; set; }

        public string SendMail_ProviderInitPass { get; set; }

        public string SendMail_AppComp{ get; set; }

        public string FileStorage_ShareName { get; set; }
    }
}
