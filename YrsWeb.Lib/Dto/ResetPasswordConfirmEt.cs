using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YrsWeb.Lib.Dto
{
    public class ResetPasswordConfirmEt
    {
        /// <summary>
        /// 運営者の場合＝原則Null、もしくはデフォルトマネージャID
        /// 事業者の場合＝事業者ID（＝メールアドレス）
        /// </summary>
        public string UserId { get; set; }
        public string AuthCode { get; set; }
        public string NewPassword { get; set; }
        public string NewPasswordConf { get; set; }
        public string KeyCode { get; set; }
    }
}
