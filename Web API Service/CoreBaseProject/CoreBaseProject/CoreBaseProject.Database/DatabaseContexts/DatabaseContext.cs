using CoreBaseProject.Model.Models.MasterSettings;
using CoreBaseProject.Model.Models.MasterSettings.AccessControl;
using CoreBaseProject.Model.Models.MasterSettings.Enum;
using CoreBaseProject.Model.Models.MasterSettings.ReportAccessControl;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CoreBaseProject.Database.DatabaseContexts
{
    public class DatabaseContext : IdentityDbContext
    {
        public DatabaseContext() { }

        public DatabaseContext(DbContextOptions<DatabaseContext> dbContextOptions) : base(dbContextOptions) { }

        #region Master Settings
        public DbSet<EnumType> EnumTypes { get; set; }
        public DbSet<EnumTypeCollection> EnumTypeCollections { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Model.Models.MasterSettings.AccessControl.Action> Actions { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<FeatureActionMapping> FeatureActionMappings { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserAccessMapping> UserAccessMappings { get; set; }
        public DbSet<RoleActionMapping> RoleActionMappings { get; set; }
        public DbSet<UserLoginHistory> UserLoginHistories { get; set; }
        public DbSet<ReportRegistry> ReportRegistries { get; set; }
        public DbSet<ReportUserAccess> ReportUserAccesses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();

                var connectionString = configuration.GetConnectionString("Default");
                optionsBuilder.UseSqlServer(connectionString);
                optionsBuilder.EnableSensitiveDataLogging();
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.Restrict;

        }
        #endregion
    }
}
