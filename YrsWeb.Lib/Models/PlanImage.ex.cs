using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YrsWeb.Lib.Models
{
    public partial class PlanImage
    {
        public string EditMode { get; set; } = "Nothing";

        /// <summary>
        /// 編集用 削除フラグ
        /// </summary>
        public bool DeleteFlg { get; set; } = false;

        public string LocalImageFolder => String.Format("/Plan/{0}/images", this.PlanId);


        /// <summary>
        /// ローカル イメージパス
        /// </summary>
        public string LocalImagePath => String.Format("{0}/{1}", this.LocalImageFolder, this.FileName);


        /// <summary>
        /// ローカル イメージURL
        /// </summary>
        public string LocalImageUrl
        {
            get
            {
                return String.Format("/files{0}", this.LocalImagePath);
            }
        }

        /// <summary>
        /// パブリックイメージパス
        /// </summary>
        public string PublicImageUrl
        {
            get
            {
                return String.Format("/imgs/plans/{0}/tmp/{1}", this.PlanId, this.ImageNo);
            }
        }
    }

}

