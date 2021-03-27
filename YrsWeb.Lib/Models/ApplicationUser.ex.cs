using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YrsWeb.Lib.Models
{
	public partial class ApplicationUser
	{

		public string UserName_Full => String.Format("{0} {1}",this.UserName1,this.UserName2);

		public string UserRuby_Full => String.Format("{0} {1}", this.UserRuby1, this.UserRuby2);

		public string UserAddress_Full => String.Format("{0} {1} {2}", this.UserAddress1, this.UserAddress2, this.UserAddress3);

		public string BirthDateStr => this.BirthDate.ToString("yyyy年MM月dd日");
	}
}
