//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはテンプレートから生成されました。
//
//     このファイルを手動で変更すると、アプリケーションで予期しない動作が発生する可能性があります。
//     このファイルに対する手動の変更は、コードが再生成されると上書きされます。
// </auto-generated>
//------------------------------------------------------------------------------

namespace YrsWeb.Lib.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Provider_M
    {
        public string ProviderId { get; set; }
        public byte RegistStatus { get; set; }
        public string ProviderPassword { get; set; }
        public string ProviderName { get; set; }
        public string ProviderIndustry { get; set; }
        public string ProviderAddress { get; set; }
        public string TantoName { get; set; }
        public string TantoMailAddress { get; set; }
        public string TantoPhone { get; set; }
        public Nullable<System.DateTime> PreRegistDate { get; set; }
        public Nullable<System.DateTime> ProRegistDate { get; set; }
        public byte RegistMailCount { get; set; }
        public bool DeleteFlg { get; set; }
        public string PersonalinfoManagement { get; set; }
    }
}
