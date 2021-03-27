using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json.Linq;

using YrsWeb.Lib.Dto;

namespace YrsWeb.Controllers
{
    [Route("/page/manager")]
    public class ManagerController : Controller
    {
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

        public ManagerController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            //IHostingEnvironment をフィールドに保持
            this._hostingEnvironment = hostingEnvironment;
        }


        public IActionResult Index()
        {
            
            return Index("index.html");
        }

         [HttpGet("{path}")]
        public IActionResult Index(string path)
        {
            if(path == "login")
            {
                return View("/Views/manager/login.html");
            }

            if(path == "bundle.js")
            {
                string filePath = this._hostingEnvironment.ContentRootPath + "\\Views\\manager\\bundle.js";
                return new PhysicalFileResult(filePath, "application / javascript");
            }

            //ログインのチェック
            ManagerLoginInfo loginInfo = YrsWeb.SessionExtensions.Get<ManagerLoginInfo>(base.HttpContext.Session, ManagerLoginInfo.SESS_KEY_LOGIN_INFO);
            if (loginInfo == null)
            {
                //ログイン画面へ遷移
                return base.Redirect("/manager/login");
            }
            else
            {
                string viewName = String.Format("/Views/manager/{0}", path);
                return View(viewName);
            }
        }
    }
}
