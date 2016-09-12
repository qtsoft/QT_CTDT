using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using HSKR.Identity.IdentityModels;

namespace HSKR.Identity
{ 

    public class MyDbContext : IdentityDbContext<MyUser, MyRole, long, MyLogin, MyUserRole, MyClaim>
    {

        public MyDbContext()
            : base("IdentityConnection")
        {

        }
        public static MyDbContext Create()
        {
            return new MyDbContext();
        }
        public virtual IDbSet<MyGroup> Groups { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Map Entities to their tables.
            modelBuilder.Entity<MyUser>().ToTable("User");
            modelBuilder.Entity<MyRole>().ToTable("Role");
            modelBuilder.Entity<MyClaim>().ToTable("UserClaim");
            modelBuilder.Entity<MyLogin>().ToTable("UserLogin");
            modelBuilder.Entity<MyUserRole>().ToTable("UserRole");
            modelBuilder.Entity<MyGroup>().ToTable("Group");
            modelBuilder.Entity<MyUserGroup>().ToTable("UserGroup");
            modelBuilder.Entity<MyGroupRole>().ToTable("GroupRole");

            modelBuilder.Entity<MyUserGroup>().HasKey(r => r.GroupId);
            modelBuilder.Entity<MyUserGroup>().HasKey(r => r.UserId);
            modelBuilder.Entity<MyGroupRole>().HasKey(r => r.GroupId);
            modelBuilder.Entity<MyGroupRole>().HasKey(r => r.RoleId);

            // Set AutoIncrement-Properties
            modelBuilder.Entity<MyUser>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<MyGroup>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<MyClaim>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<MyRole>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            // Override some column mappings that do not match our default
            modelBuilder.Entity<MyUser>().Property(r => r.UserName).HasColumnName("Login");
            modelBuilder.Entity<MyUser>().Property(r => r.PasswordHash).HasColumnName("Password");
        }

    }
}