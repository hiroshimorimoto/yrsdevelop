//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはテンプレートから生成されました。
//
//     このファイルを手動で変更すると、アプリケーションで予期しない動作が発生する可能性があります。
//     このファイルに対する手動の変更は、コードが再生成されると上書きされます。
// </auto-generated>
//------------------------------------------------------------------------------

namespace YrsWeb.Lib.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class YRS_DBEntities : DbContext
    {
        public YRS_DBEntities()
            : base("name=YRS_DBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Provider_M> Provider_M { get; set; }
        public virtual DbSet<PassResetRequest> PassResetRequest { get; set; }
        public virtual DbSet<PlanCategory> PlanCategory { get; set; }
        public virtual DbSet<PlanArea> PlanArea { get; set; }
        public virtual DbSet<PlanImage> PlanImage { get; set; }
        public virtual DbSet<PlanDateStr> PlanDateStr { get; set; }
        public virtual DbSet<PlanInfo> PlanInfo { get; set; }
        public virtual DbSet<PublicPlanInfo> PublicPlanInfo { get; set; }
        public virtual DbSet<PlanDate> PlanDate { get; set; }
        public virtual DbSet<Category_M> Category_M { get; set; }
        public virtual DbSet<Manager_M> Manager_M { get; set; }
        public virtual DbSet<ApplicationHeader> ApplicationHeader { get; set; }
        public virtual DbSet<ApplicationPlan> ApplicationPlan { get; set; }
        public virtual DbSet<ApplicationUser> ApplicationUser { get; set; }
        public virtual DbSet<PlanAppInfo> PlanAppInfo { get; set; }
        public virtual DbSet<ApplicationPayment> ApplicationPayment { get; set; }
    }
}
