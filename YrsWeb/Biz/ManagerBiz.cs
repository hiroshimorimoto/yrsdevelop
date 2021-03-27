using Microsoft.CodeAnalysis.CSharp.Syntax;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.AccessControl;
using System.Text;

using YrsWeb.Controllers;
using YrsWeb.Lib.Common;
using YrsWeb.Lib.Dto;
using YrsWeb.Lib.Models;

namespace YrsWeb.Biz
{
    public class ManagerBiz : AbstractBiz
    {
        public ManagerBiz(ApiController controller, YRS_DBEntities dbContext) : base(controller, dbContext) { }


        public Provider_M GetProvider(string providerId)
        {
#if DEBUG
            base.DbContext.Database.Log = log => System.Diagnostics.Debug.WriteLine(log);
#endif

            Provider_M ret = base.DbContext.Provider_M.SingleOrDefault(e => e.ProviderId == providerId && !e.DeleteFlg);

            return ret;

        }

        public List<Provider_M> GetProviderList(ProviderListSearchEt searchEt)
        {
#if DEBUG
            base.DbContext.Database.Log = log => System.Diagnostics.Debug.WriteLine(log);
#endif

            var q = base.DbContext.Provider_M.AsQueryable();

            q = q.Where(e => !e.DeleteFlg);

            q = q.OrderByDescending(e => e.ProviderId);//受注番号 が新しいものからの降順

            List<Provider_M> ret = q.ToList();

            return ret;
        }

        public Provider_M PostProvider(ProviderPostEt postEt)
        {
#if DEBUG
            base.DbContext.Database.Log = log => System.Diagnostics.Debug.WriteLine(log);
#endif
            DateTime now = DateTime.Now;

            if (postEt.EditModeType == EditModes.Add)
            {//追加
                //存在チェック(過去削除されたものも含めてチェック）
                if (base.DbContext.Provider_M.Any(e => e.ProviderId == postEt.ProviderEt.ProviderId))
                {
                    throw new YrsWebException("この事業者は既に登録されています");
                }

                //DB保存
                postEt.ProviderEt.RegistStatusType = Provider_M.RegistStatusTypes.PreRegist;//仮登録
                postEt.ProviderEt.PreRegistDate = now;//登録日時
                postEt.ProviderEt.RegistMailCount = 0;//通知メール送信回数

                //初期パスワード 自動生成
                string initPass = System.Web.Security.Membership.GeneratePassword(8, 0);
                postEt.ProviderEt.ProviderPassword = initPass;


                postEt.ProviderEt = this.DbContext.Provider_M.Add(postEt.ProviderEt);
                base.DbContext.SaveChanges();


                if (postEt.IsSendRegistMail)
                {
                    if (postEt.ProviderEt.RegistStatusType != Provider_M.RegistStatusTypes.Registed)
                    {
                        //DODO:本登録用メール送信

                    }
                }

            }
            else if (postEt.EditModeType == EditModes.Edit)
            {//更新
                //存在チェック
                if (!base.DbContext.Provider_M.Any(e => e.ProviderId == postEt.ProviderEt.ProviderId && !e.DeleteFlg))
                {
                    throw new YrsWebException("事業者が存在しません");
                }

                //DB保存
                postEt.ProviderEt = this.DbContext.Provider_M.Attach(postEt.ProviderEt);//DbContextにアタッチ
                DbContext.Entry(postEt.ProviderEt).State = System.Data.Entity.EntityState.Modified;
                DbContext.SaveChanges();
            }
            else if (postEt.EditModeType == EditModes.Delete)
            {//削除
                //存在チェック
                if (!base.DbContext.Provider_M.Any(e => e.ProviderId == postEt.ProviderEt.ProviderId && !e.DeleteFlg))
                {
                    throw new YrsWebException("事業者が存在しません");
                }

                //DB保存
                postEt.ProviderEt = this.DbContext.Provider_M.Attach(postEt.ProviderEt);//DbContextにアタッチ
                postEt.ProviderEt.DeleteFlg = true;
                //DbContext.Provider_M.Remove(postEt.ProviderEt);
                DbContext.SaveChanges();
            }



            return postEt.ProviderEt;
        }

        public Provider_M SendRegistMail(string providerId)
        {
            Provider_M providerEt = base.DbContext.Provider_M.SingleOrDefault(e => e.ProviderId == providerId);

            //存在チェック
            if (providerEt == null)
            {
                throw new YrsWebException("事業者が存在しません");
            }


            //メール送信サービス用
            var client = new HttpClient();

            //事業者 登録完了通知メール送信（パスワードなし）
            this._sendRegistMail(client, providerEt);

            // 事業者 初期パスワード通知メール送信
            this._sendInitPassMail(client, providerEt);



            providerEt.RegistStatusType = Provider_M.RegistStatusTypes.WaitProRegist;
            providerEt.RegistMailCount++;//送信カウントUP

            base.DbContext.SaveChanges();

            return providerEt;
        }


        /// <summary>
        /// 事業者 登録完了通知メール送信（パスワードなし）
        /// </summary>
        /// <param name="client"></param>
        /// <param name="providerEt"></param>
        /// <returns></returns>
        private string _sendRegistMail(HttpClient client, Provider_M providerEt)
        {
            //ログインURLの生成
            string pageUrl = "/provider/index.html";
            string _loginUrl = String.Format("{0}://{1}{2}", base.Controller.HttpContext.Request.Scheme, base.Controller.HttpContext.Request.Host, pageUrl);

            StringBuilder sb = new StringBuilder();

            JsonSerializer js = JsonSerializer.CreateDefault();
            var sendMailParam = new
            {
                system_name = Statics.SYSTEM_NAME,
                provider_name = providerEt.ProviderName,
                from_address = base.Controller.YrsAppSettings.SendMail_FromAddress,
                to_address = providerEt.ProviderId,
                subject = String.Format("【{0}】事業者 登録完了通知メール", Statics.SYSTEM_NAME),
                loginUrl = _loginUrl
            };

            js.Serialize(new JsonTextWriter(new StringWriter(sb)), sendMailParam);

            //同期呼び出し
            HttpResponseMessage result = client.PostAsync(
                base.Controller.YrsAppSettings.SendMail_ProviderRegist,
                 new StringContent(sb.ToString(), Encoding.UTF8, "application/json")
            ).Result;

            var statusCode = result.StatusCode.ToString();
            return statusCode;
        }

        /// <summary>
        /// 事業者 初期パスワード通知メール送信
        /// </summary>
        /// <param name="client"></param>
        /// <param name="providerEt"></param>
        /// <returns></returns>
        private string _sendInitPassMail(HttpClient client, Provider_M providerEt)
        {
            StringBuilder sb = new StringBuilder();

            JsonSerializer js = JsonSerializer.CreateDefault();
            var sendMailParam = new
            {
                system_name = Statics.SYSTEM_NAME,
                provider_name = providerEt.ProviderName,
                from_address = base.Controller.YrsAppSettings.SendMail_FromAddress,
                to_address = providerEt.ProviderId,
                subject = String.Format("【{0}】事業者 初期パスワードの通知", Statics.SYSTEM_NAME),
                init_pass = providerEt.ProviderPassword
            };

            js.Serialize(new JsonTextWriter(new StringWriter(sb)), sendMailParam);

            //同期呼び出し
            HttpResponseMessage result = client.PostAsync(
                base.Controller.YrsAppSettings.SendMail_ProviderInitPass,
                 new StringContent(sb.ToString(), Encoding.UTF8, "application/json")
            ).Result;

            var statusCode = result.StatusCode.ToString();
            return statusCode;
        }
    }
}
