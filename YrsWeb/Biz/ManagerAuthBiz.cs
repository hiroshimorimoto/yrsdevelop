using Newtonsoft.Json;

using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using YrsWeb.Controllers;
using YrsWeb.Lib.Common;
using YrsWeb.Lib.Dto;
using YrsWeb.Lib.Models;

namespace YrsWeb.Biz
{
    public class ManagerAuthBiz : AbstractBiz
    {
        public ManagerAuthBiz(ApiController controller, YRS_DBEntities dbContext) : base(controller, dbContext) { }

        /// <summary>
        /// ログインチェック
        /// </summary>
        /// <remarks>
        /// ログインされている場合、<see cref="ManagerLoginInfo" />が返ります。ログインされてない場合は<see cref="UnauthorizedAccessException" />がThrowされます
        /// </remarks>
        /// <returns></returns>
        public ManagerLoginInfo CheckLogin()
        {
            //ログイン情報をSessionから取得
            ManagerLoginInfo loginInfo = YrsWeb.SessionExtensions.Get<ManagerLoginInfo>(base.Controller.HttpContext.Session, ManagerLoginInfo.SESS_KEY_LOGIN_INFO);

            if (loginInfo == null)
            {
                throw new UnauthorizedAccessException("ログインされていません");
            }

            return loginInfo;
        }

        /// <summary>
        /// ログイン処理
        /// </summary>
        /// <param name="loginpassword">ログインパスワード</param>
        /// <returns></returns>
        public ManagerLoginInfo Login(string loginpassword)
        {
            return this._login(Statics.DEFAULT_MANAGER_ID, loginpassword);
        }

        private ManagerLoginInfo _login(string managerId, string loginpassword)
        {
            //認証
            Manager_M manager = this._auth(managerId, loginpassword);

            //認証OKなら、ログイン情報を取得・設定
            ManagerLoginInfo loginInfo = new ManagerLoginInfo();
            loginInfo.Manager = manager;

            //ログイン情報をSessionに保存
            YrsWeb.SessionExtensions.Set<ManagerLoginInfo>(base.Controller.HttpContext.Session, ManagerLoginInfo.SESS_KEY_LOGIN_INFO, loginInfo);

            return loginInfo;
        }

        /// <summary>
        /// ログアウト
        /// </summary>
        /// <returns></returns>
        public int Logout()
        {
            //ログイン情報をSessionから破棄
            YrsWeb.SessionExtensions.Remove(base.Controller.HttpContext.Session, ManagerLoginInfo.SESS_KEY_LOGIN_INFO);
            return 0;
        }



        /// <summary>
        /// パスワード変更処理
        /// </summary>
        /// <param name="oldPassword">旧(現)パスワード</param>
        /// <param name="newPassword">新パスワード</param>
        /// <param name="newPasswordConf">新パスワード(確認)</param>
        /// <returns></returns>
        public Manager_M ChangePassword(string oldPassword, string newPassword, string newPasswordConf)
        {
            return this.ChangePassword(Statics.DEFAULT_MANAGER_ID, oldPassword, newPassword, newPasswordConf);
        }

        /// <summary>
        /// パスワード変更処理
        /// </summary>
        /// <param name="managerId">運営者ID</param>
        /// <param name="oldPassword">旧(現)パスワード</param>
        /// <param name="newPassword">新パスワード</param>
        /// <param name="newPasswordConf">新パスワード(確認)</param>
        /// <returns></returns>
        public Manager_M ChangePassword(string managerId, string oldPassword, string newPassword, string newPasswordConf)
        {
            //旧パスワードで一旦ログイン
            Manager_M managerM = this._auth(managerId, oldPassword);

            if (newPassword != newPasswordConf)
            {
                throw new YrsWebException("指定されたパスワードが不正です");
            }

            //パスワード更新
            managerM.ManagerPassword = newPassword;

            //保存
            base.DbContext.SaveChanges();

            return managerM;
        }

        /// <summary>
        /// パスワードリセット依頼
        /// </summary>
        /// <param name="authCode">認証コード</param>
        /// <returns></returns>
        public int ResetPasswordRequest(string authCode)
        {
            return this.ResetPasswordRequest(Statics.DEFAULT_MANAGER_ID, authCode);
        }

        /// <summary>
        /// パスワードリセット依頼
        /// </summary>
        /// <param name="managerId">運営者ID</param>
        /// <param name="authCode">認証コード</param>
        /// <returns></returns>
        public int ResetPasswordRequest(string managerId, string authCode)
        {
            if (String.IsNullOrEmpty(authCode))
            {
                throw new YrsWebException("認証コードが設定されていません");
            }

            //マネージャエンティティ
            Manager_M manager = base.DbContext.Manager_M.FirstOrDefault(e => e.ManagerId == managerId);
            if (manager == null)
            {
                throw new YrsWebException("運営者が存在しません");
            }

            //リクエストデータの作成
            DateTime now = DateTime.Now;
            DateTime expiration = now.AddMinutes(base.Controller.YrsAppSettings.PassResetExpirationMin);

            PassResetRequest request = new PassResetRequest();
            request.RequestDateTime = now;
            request.ExpirationDateTime = expiration;
            request.RequestTypeType = PassResetRequest.RequestTypes.ManagerPassReset;
            request.RequestStatusType = PassResetRequest.RequestStatuses.Resetting;
            request.MailAddress = manager.MailAddress;
            request.AuthCode = authCode;

            base.DbContext.PassResetRequest.Add(request);
            base.DbContext.SaveChanges();

            //メールの送信
            _sendPassResetMail(request);
            return request.RequestID;
        }


        /// <summary>
        /// パスワードリセット確認
        /// </summary>
        /// <param name="dto">リセット確認パラメータDTO</param>
        /// <returns></returns>
        public int ResetPasswordConfirm(ResetPasswordConfirmEt dto)
        {
            if (string.IsNullOrEmpty(dto.UserId)) dto.UserId = Statics.DEFAULT_MANAGER_ID;

            //KeyCodeのパース
            AesUtil.Decrypt(dto.KeyCode, out string mailAddress, out int requestId, false);

            if (requestId <= 0)
            {
                throw new YrsWebException("キーコードが無効です");
            }

            if (String.IsNullOrEmpty(mailAddress))
            {
                throw new YrsWebException("キーコードが無効です");
            }


            //依頼データの検索
            PassResetRequest req = base.DbContext.PassResetRequest.FirstOrDefault(e => e.RequestID == requestId);
            if (req == null)
            {
                throw new YrsWebException("パスワードの再設定が依頼されていません");
            }
            else if (req.RequestStatusType != PassResetRequest.RequestStatuses.Resetting)
            {
                throw new YrsWebException("パスワードの再設定が依頼されていません");
            }

            //メールアドレス（依頼ユーザ）のチェック
            if (!String.IsNullOrEmpty(req.MailAddress))
            {
                if (!String.Equals(req.MailAddress, mailAddress, StringComparison.OrdinalIgnoreCase))
                {
                    throw new YrsWebException("パスワードの再設定が依頼されていません");
                }
            }

            //認証コードをチェック
            if (req.AuthCode != dto.AuthCode)
            {
                throw new YrsWebException("認証コードが不正です");
            }

            //有効期限のチェック
            if (req.ExpirationDateTime < DateTime.Now)
            {
                throw new YrsWebException("パスワードの再設定の有効期限が切れています\r\n再度パスワード再設定を行ってください");
            }

            if (dto.NewPassword != dto.NewPasswordConf)
            {
                throw new YrsWebException("指定されたパスワードが不正です");
            }


            //運営者エンティティ取得
            Manager_M manager = base.DbContext.Manager_M.FirstOrDefault(e => e.ManagerId == dto.UserId);
            manager.ManagerPassword = dto.NewPassword;

            //依頼データを更新
            req.RequestStatusType = PassResetRequest.RequestStatuses.Complete;


            //Commit;
            base.DbContext.SaveChanges();


            return requestId;
        }



        private Manager_M _auth(string managerId, string loginpassword)
        {
            if (String.IsNullOrEmpty(managerId))
            {
                throw new ArgumentException("ログインIDが指定されていません");
            }
            if (String.IsNullOrEmpty(loginpassword))
            {
                throw new ArgumentException("パスワードが指定されていません");
            }

            //ユーザの検索
            Manager_M result = base.DbContext.Manager_M.FirstOrDefault(e => e.ManagerId == managerId);

            if (result == null)
            {
                //throw new UnauthorizedAccessException("ログインIDまたはパスワードが不正です");
                throw new YrsUnAuthorizedException("ログインIDまたはパスワードが不正です");
            }

            if (!result.ManagerPassword.Equals(loginpassword))
            {
                //throw new UnauthorizedAccessException("ログインIDまたはパスワードが不正です");
                throw new YrsUnAuthorizedException("ログインIDまたはパスワードが不正です");
            }

            return result;
        }
        private string _sendPassResetMail(PassResetRequest request)
        {
            //暗号化URLの生成
            string encryptKey = AesUtil.Encrypt(request.MailAddress, request.RequestID, true);

            string pageUrl = "/manager/remind.html#/passconf";
            string resetUrl = String.Format("{0}://{1}{2}?k={3}", base.Controller.HttpContext.Request.Scheme, base.Controller.HttpContext.Request.Host, pageUrl, encryptKey);



            StringBuilder sb = new StringBuilder();

            JsonSerializer js = JsonSerializer.CreateDefault();
            var sendMailParam = new
            {
                system_name = Statics.SYSTEM_NAME,
                from_address = base.Controller.YrsAppSettings.SendMail_FromAddress,
                to_address = request.MailAddress,
                subject = "【吉野リザーブ】パスワードリセットメール",
                resetUrl = resetUrl
            };

            js.Serialize(new JsonTextWriter(new StringWriter(sb)), sendMailParam);

            var client = new HttpClient();

            //同期呼び出し
            HttpResponseMessage result = client.PostAsync(
                base.Controller.YrsAppSettings.SendMail_ManagerPassReset,
                 new StringContent(sb.ToString(), Encoding.UTF8, "application/json")
            ).Result;

            var statusCode = result.StatusCode.ToString();
            return statusCode;
        }





    }
}
