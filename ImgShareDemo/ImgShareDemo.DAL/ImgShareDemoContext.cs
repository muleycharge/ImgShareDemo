namespace ImgShareDemo.DAL
{
    using ImgShareDemo.BO.Entities;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;

    public class ImgShareDemoContext : DbContext
    {
        public ImgShareDemoContext() : base() { }

        public DbSet<User> Users { get; set; }
        public DbSet<LinkedInUser> LinkedInUsers { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<AssetTag> AssetTags { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
