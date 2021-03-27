using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YrsWeb.Lib.Common
{
    /// <summary>
    /// APIリザルトベースクラス
    /// </summary>
    public class ApiResultBase
    {
        /// <summary>
        /// エラーが発生しているかどうかを取得します
        /// </summary>
        public bool HasError { get; set; } = false;


        /// <summary>
        /// エラーが発生している場合 メッセージを返します
        /// </summary>
        public string ErrorMessage { get; set; } = null;

        public void SetError(Exception ex)
        {
            this.HasError = true;
            ErrorMessage = ex.Message;
        }
    }
}