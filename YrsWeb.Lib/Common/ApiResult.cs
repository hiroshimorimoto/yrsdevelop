using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YrsWeb.Lib.Common
{
    /// <summary>
    /// APIリザルトクラス
    /// </summary>
    public class ApiResult<DataType> : ApiResultBase
    {

        /// <summary>
        /// リザルトデータを取得または設定します
        /// </summary>
        public DataType ResultData { get; set; }

    }
}