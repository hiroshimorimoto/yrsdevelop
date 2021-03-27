using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YrsWeb.Lib.Models
{
    public partial class PassResetRequest
    {

        /// <summary>
        /// リクエストタイプ 列挙体
        /// </summary>
        public enum RequestTypes : byte
        {
            /// <summary>
            /// 1:管理者パスワードリセット
            /// </summary>
            ManagerPassReset = 1,
            /// <summary>
            /// 2:事業者パスワードリセット
            /// </summary>
            ProviderPassReset = 2,
        }

        /// <summary>
        /// リクエストステータス列挙体
        /// </summary>
        public enum RequestStatuses : byte
        {
            /// <summary>
            /// 0:リセット中
            /// </summary>
            Resetting = (byte)0,
            /// <summary>
            /// 1:リセット完了
            /// </summary>
            Complete = (byte)1,
        }

        public RequestTypes RequestTypeType
        {
            get
            {
                return (RequestTypes)this.RequestType;
            }
            set
            {
                this.RequestType = (byte)value;
            }
        }


        public RequestStatuses RequestStatusType
        {
            get
            {
                return (RequestStatuses)this.RequestStatus;
            }
            set
            {
                this.RequestStatus = (byte)value;
            }
        }
    }
}
