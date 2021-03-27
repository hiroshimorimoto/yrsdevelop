using System.Collections.Generic;
using System.Linq;

using YrsWeb.Controllers;
using YrsWeb.Lib.Models;


namespace YrsWeb.Biz
{
	public class MasterDataBiz : AbstractBiz
	{
		public MasterDataBiz(BaseController controller, YRS_DBEntities dbContext) : base(controller, dbContext) { }


		public List<Category_M> GetCategoryList()
		{

			var q = base.DbContext.Category_M.AsQueryable();
			q = q.OrderBy(e => e.CategoryLevel).ThenBy(e => e.CategoryId);

			List<Category_M> ret = q.ToList();
			return ret;
		}
	}
}
