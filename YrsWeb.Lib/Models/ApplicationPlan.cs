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
    
    public partial class ApplicationPlan
    {
        public int ApplicationId { get; set; }
        public int PlanId { get; set; }
        public System.DateTime AcceptDate { get; set; }
        public int Fee_Adult { get; set; }
        public int Fee_Child { get; set; }
        public int Fee_Infant { get; set; }
        public System.DateTime InsertDate { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public bool DeleteFlg { get; set; }
    }
}
