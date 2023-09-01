using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Artemis.App.IdentityApplication.Data
{
    public class ArtemisIdentityDbContext : IdentityDbContext<
        ArtemisIdentityUser, 
        ArtemisIdentityRole, 
        Guid, 
        ArtemisIdentityUserClaim, 
        ArtemisIdentityUserRole, 
        ArtemisIdentityUserLogin, 
        ArtemisIdentityRoleClaim, 
        ArtemisIdentityUserToken>
    {
        public ArtemisIdentityDbContext(DbContextOptions<ArtemisIdentityDbContext> options)
            : base(options)
        {
        }

        #region Overrides of IdentityDbContext<ArtemisIdentityUser,ArtemisIdentityRole,Guid,ArtemisIdentityUserClaim,ArtemisIdentityUserRole,ArtemisIdentityUserLogin,ArtemisIdentityRoleClaim,ArtemisIdentityUserToken>

        /// <summary>
        /// Configures the schema needed for the identity framework.
        /// </summary>
        /// <param name="builder">
        /// The builder being used to construct the model for this context.
        /// </param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ArtemisIdentityUser>(entity =>
            {
                entity.ToTable(nameof(ArtemisIdentityUser));
            });

            builder.Entity<ArtemisIdentityUserClaim>(entity =>
            {
                entity.ToTable(nameof(ArtemisIdentityUserClaim));
            });

            builder.Entity<ArtemisIdentityUserLogin>(entity =>
            {
                entity.ToTable(nameof(ArtemisIdentityUserLogin));
            });

            builder.Entity<ArtemisIdentityUserToken>(entity =>
            {
                entity.ToTable(nameof(ArtemisIdentityUserToken));
            });

            builder.Entity<ArtemisIdentityRole>(entity =>
            {
                entity.ToTable(nameof(ArtemisIdentityRole));
            });

            builder.Entity<ArtemisIdentityRoleClaim>(entity =>
            {
                entity.ToTable(nameof(ArtemisIdentityRoleClaim));
            });

            builder.Entity<ArtemisIdentityUserRole>(entity =>
            {
                entity.ToTable(nameof(ArtemisIdentityUserRole));
            });
        }

        #endregion
    }
}