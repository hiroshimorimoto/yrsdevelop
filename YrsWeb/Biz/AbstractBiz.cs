using Microsoft.Azure.Storage.File;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using YrsWeb.Controllers;
using YrsWeb.Lib.Models;

namespace YrsWeb.Biz
{
    public abstract class AbstractBiz
    {
        protected const string ENTITY_NAME_PREFIX = "YrsWeb.Lib.Models";

        private BaseController _controller;
        private YRS_DBEntities _dbContext;

        public AbstractBiz(BaseController controller, YRS_DBEntities dbContext)
        {
            this._controller = controller;
            this._dbContext = dbContext;
        }


        /// <summary>
        /// BizのCallerであるApiControllerを取得します
        /// </summary>
        protected BaseController Controller => this._controller;


        /// <summary>
        /// YRS_DB のDBコンテキストを取得します
        /// </summary>
        protected YRS_DBEntities DbContext => this._dbContext;


        /// <summary>
        /// Azure FileClientを取得します
        /// </summary>
        protected CloudFileClient FileClient => this._controller.FileClient;



        protected Type GetEntityType(string entityName)
        {
            string entityTypeName = String.Format("{0}.{1},YrsWeb.Lib", ENTITY_NAME_PREFIX, entityName);
            Type entityType = Type.GetType(entityTypeName);

            return entityType;
        }
    }
}
