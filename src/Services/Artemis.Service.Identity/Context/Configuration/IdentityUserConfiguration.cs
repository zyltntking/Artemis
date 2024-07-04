using Artemis.Data.Shared;
using Artemis.Data.Store.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Service.Identity.Context.Configuration;

/// <summary>
///     认证用户实体配置
/// </summary>
internal sealed class IdentityUserConfiguration : ConcurrencyModelEntityConfiguration<IdentityUser>
{
    #region Overrides of ModelConfiguration<IdentityUser>

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "认证用户数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(IdentityUser).TableName();

    /// <summary>
    ///     实体关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void EntityRelationConfigure(EntityTypeBuilder<IdentityUser> builder)
    {
        // User Index
        builder.HasIndex(user => user.NormalizedUserName)
            .HasDatabaseName(IndexName("UserName"))
            .IsUnique();

        builder.HasIndex(user => user.NormalizedEmail)
            .HasDatabaseName(IndexName("Email"));

        builder.HasIndex(user => user.PhoneNumber)
            .HasDatabaseName(IndexName("PhoneNumber"));

        // Each User can have many UserProfiles
        builder.HasMany(user => user.UserProfiles)
            .WithOne(userProfile => userProfile.User)
            .HasForeignKey(userProfile => userProfile.UserId)
            .HasConstraintName(ForeignKeyName(
                nameof(IdentityUserProfile).TableName(),
                nameof(IdentityUser).TableName()))
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // Each User can have many UserClaims
        builder.HasMany(user => user.UserClaims)
            .WithOne(userClaim => userClaim.User)
            .HasForeignKey(userClaim => userClaim.UserId)
            .HasConstraintName(ForeignKeyName(
                nameof(IdentityUserClaim).TableName(),
                nameof(IdentityUser).TableName()))
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // Each User can have many UserLogins
        builder.HasMany(user => user.UserLogins)
            .WithOne(userLogin => userLogin.User)
            .HasForeignKey(userLogin => userLogin.UserId)
            .HasConstraintName(ForeignKeyName(
                nameof(IdentityUserLogin).TableName(),
                nameof(IdentityUser).TableName()))
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // Each User can have many UserTokens
        builder.HasMany(user => user.UserTokens)
            .WithOne(userToken => userToken.User)
            .HasForeignKey(userToken => userToken.UserId)
            .HasConstraintName(ForeignKeyName(
                nameof(IdentityUserToken).TableName(),
                nameof(IdentityUser).TableName()))
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // Each User can have many UserProfiles
        builder.HasMany(user => user.UserProfiles)
            .WithOne(userProfile => userProfile.User)
            .HasForeignKey(userProfile => userProfile.UserId)
            .HasConstraintName(ForeignKeyName(
                nameof(IdentityUserProfile).TableName(),
                nameof(IdentityUser).TableName()))
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }

    #endregion
}