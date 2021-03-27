using Newtonsoft.Json;

using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;

using YrsWeb.Controllers;
using YrsWeb.Lib.Common;
using YrsWeb.Lib.Dto;
using YrsWeb.Lib.Models;

namespace YrsWeb.Biz
{
    public class ProviderAuthBiz : AbstractBiz
    {
        public ProviderAuthBiz(ApiController controller, YRS_DBEntities dbContext) : base(controller, dbContext) { }

        /// <summary>
        /// ログインチェック
        /// </summary>
        /// <remarks>
        /// ログインされている場合、<see cref="ProviderLoginInfo" />が返ります。ログインされてない場合は<see cref="UnauthorizedAccessException" />がThrowされます
        /// </remarks>
        /// <returns></returns>
        public ProviderLoginInfo CheckLogin()
        {
            //ログイン情報をSessionから取得
            ProviderLoginInfo loginInfo = YrsWeb.SessionExtensions.Get<ProviderLoginInfo>(base.Controller.HttpContext.Session, ProviderLoginInfo.SESS_KEY_LOGIN_INFO);

            if (loginInfo == null)
            {
                throw new UnauthorizedAccessException("ログインされていません");
            }

            return loginInfo;
        }

        /// <summary>
        /// ログイン
        /// </summary>
        /// <param name="providerId">事業者ID</param>
        /// <param name="loginpassword">パスワード</param>
        /// <returns></returns>
        public ProviderLoginInfo Login(string providerId, string loginpassword)
        {
            //認証
            Provider_M provider = this._auth(providerId, loginpassword);

            //認証OKなら、ログイン情報を取得・設定
            ProviderLoginInfo loginInfo = new ProviderLoginInfo();
            loginInfo.Provider = provider;

            //ログイン情報をSessionに保存
            YrsWeb.SessionExtensions.Set<ProviderLoginInfo>(base.Controller.HttpContext.Session, ProviderLoginInfo.SESS_KEY_LOGIN_INFO, loginInfo);

            return loginInfo;
        }


        /// <summary>
        /// ログアウト
        /// </summary>
        /// <returns></returns>
        public int Logout()
        {
            //ログイン情報をSessionから破棄
            YrsWeb.SessionExtensions.Remove(base.Controller.HttpContext.Session, ProviderLoginInfo.SESS_KEY_LOGIN_INFO);
            return 0;
        }



        /// <summary>
        /// パスワード変更処理
        /// </summary>
        /// <param name="providerId">事業者ID</param>
        /// <param name="oldPassword">旧(現)パスワード</param>
        /// <param name="newPassword">新パスワード</param>
        /// <param name="newPasswordConf">新パスワード(確認)</param>
        /// <returns></returns>
        public Provider_M ChangePassword(string providerId, string oldPassword, string newPassword, string newPasswordConf)
        {
            //旧パスワードで一旦ログイン
            Provider_M providerM = this._auth(providerId, oldPassword);

            if (newPassword != newPasswordConf)
            {
                throw new YrsWebException("指定されたパスワードが不正です");
            }

            //パスワード更新
            providerM.ProviderPassword = newPassword;

            //保存
            base.DbContext.SaveChanges();

            return providerM;
        }

        /// <summary>
        /// パスワードリセット依頼
        /// </summary>
        /// <param name="providerId">事業者ID</param>
        /// <param name="authCode">認証コード</param>
        /// <returns></returns>
        public int ResetPasswordRequest(string providerId, string authCode)
        {
            if (String.IsNullOrEmpty(authCode))
            {
                throw new YrsWebException("認証コードが設定されていません");
            }

            //事業者エンティティ
            Provider_M provider = base.DbContext.Provider_M.FirstOrDefault(e => e.ProviderId == providerId);
            if (provider == null)
            {
                throw new YrsWebException("事業者が存在しません");
            }

            //リクエストデータの作成
            DateTime now = DateTime.Now;
            DateTime expiration = now.AddMinutes(base.Controller.YrsAppSettings.PassResetExpirationMin);

            PassResetRequest request = new PassResetRequest();
            request.RequestDateTime = now;
            request.ExpirationDateTime = expiration;
            request.RequestTypeType = PassResetRequest.RequestTypes.ProviderPassReset;//事業者
            request.RequestStatusType = PassResetRequest.RequestStatuses.Resetting;
            request.MailAddress = provider.ProviderId;
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


            //事業者エンティティ取得
            Provider_M provider = base.DbContext.Provider_M.FirstOrDefault(e => e.ProviderId == dto.UserId);
            provider.ProviderPassword = dto.NewPassword;

            //依頼データを更新
            req.RequestStatusType = PassResetRequest.RequestStatuses.Complete;


            //Commit;
            base.DbContext.SaveChanges();


            return requestId;
        }



        private Provider_M _auth(string providerId, string loginpassword)
        {
            if (String.IsNullOrEmpty(providerId))
            {
                throw new ArgumentException("ログインIDが指定されていません");
            }
            if (String.IsNullOrEmpty(loginpassword))
            {
                throw new ArgumentException("パスワードが指定されていません");
            }

            //ユーザの検索
            Provider_M result = base.DbContext.Provider_M.FirstOrDefault(e => e.ProviderId == providerId && !e.DeleteFlg);

            if (result == null)
            {
                //throw new UnauthorizedAccessException("ログインIDまたはパスワードが不正です");
                throw new YrsUnAuthorizedException("ログインIDまたはパスワードが不正です");
            }

            if (!result.ProviderPassword.Equals(loginpassword))
            {
                //throw new UnauthorizedAccessException("ログインIDまたはパスワードが不正です");
                throw new YrsUnAuthorizedException("ログインIDまたはパスワードが不正です");
            }

            if (result.RegistStatusType == Provider_M.RegistStatusTypes.WaitProRegist)
            {
                //メール送信済 で、ログイン認証が通った場合、本登録済 とする
                result.RegistStatusType = Provider_M.RegistStatusTypes.Registed;
                result.ProRegistDate = DateTime.Now;
                base.DbContext.SaveChanges();
            }

            return result;
        }
        private string _sendPassResetMail(PassResetRequest request)
        {
            //暗号化URLの生成
            string encryptKey = AesUtil.Encrypt(request.MailAddress, request.RequestID, true);

            string pageUrl = "/provider/remind.html#/passconf";
            string _resetUrl = String.Format("{0}://{1}{2}?k={3}", base.Controller.HttpContext.Request.Scheme, base.Controller.HttpContext.Request.Host, pageUrl, encryptKey);



            StringBuilder sb = new StringBuilder();

            JsonSerializer js = JsonSerializer.CreateDefault();
            var sendMailParam = new
            {
                system_name = Statics.SYSTEM_NAME,
                from_address = base.Controller.YrsAppSettings.SendMail_FromAddress,
                to_address = request.MailAddress,
                subject = String.Format("【{0}】パスワードリセットメール", Statics.SYSTEM_NAME),
                resetUrl = _resetUrl
            };

            js.Serialize(new JsonTextWriter(new StringWriter(sb)), sendMailParam);

            var client = new HttpClient();

            //同期呼び出し
            HttpResponseMessage result = client.PostAsync(
                base.Controller.YrsAppSettings.SendMail_ProviderPassReset,
                 new StringContent(sb.ToString(), Encoding.UTF8, "application/json")
            ).Result;

            var statusCode = result.StatusCode.ToString();
            return statusCode;
        }



    }
}
