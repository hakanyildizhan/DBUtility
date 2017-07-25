using DBUtility.WebUI.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace DBUtility.WebUI
{
    public class DBUtilityContext : DbContext
    {

        public DBUtilityContext() : base("DBUtilityContext")
        {
        }

        public DbSet<DBActionMeta> DBActions { get; set; }
        public DbSet<DBActionDetail> DBActionDetails { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}