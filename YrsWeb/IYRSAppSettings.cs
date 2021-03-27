using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YrsWeb
{
    public interface IYrsAppSettings
    {
        int PassResetExpirationMin { get; }

        string SendMail_ManagerPassReset { get; }

        string SendMail_FromAddress { get; }

        string SendMail_ProviderPassReset { get; }

        string SendMail_ProviderRegist { get; }

        string SendMail_ProviderInitPass { get; }

        string SendMail_AppComp{ get; }

        string FileStorage_ShareName { get; }

    }
}
