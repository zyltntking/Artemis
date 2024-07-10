﻿// <auto-generated />
using System;
using Artemis.Service.Identity.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Artemis.App.Identity.Migrations
{
    [DbContext(typeof(IdentityContext))]
    partial class IdentityContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Identity")
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Artemis.Service.Identity.Context.IdentityClaim", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasComment("标识");

                    b.Property<string>("CheckStamp")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasComment("校验戳");

                    b.Property<string>("ClaimType")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasComment("凭据类型");

                    b.Property<string>("ClaimValue")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)")
                        .HasComment("凭据值");

                    b.Property<Guid>("CreateBy")
                        .HasColumnType("UUID")
                        .HasComment("创建者标识");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TIMESTAMP")
                        .HasComment("创建时间");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("TIMESTAMP")
                        .HasComment("删除时间");

                    b.Property<string>("Description")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasComment("凭据描述");

                    b.Property<Guid>("ModifyBy")
                        .HasColumnType("UUID")
                        .HasComment("更新者标识");

                    b.Property<Guid>("RemoveBy")
                        .HasColumnType("UUID")
                        .HasComment("删除者标识");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TIMESTAMP")
                        .HasComment("更新时间");

                    b.HasKey("Id")
                        .HasName("PK_IdentityClaim");

                    b.HasIndex("CheckStamp")
                        .IsUnique()
                        .HasDatabaseName("IX_IdentityClaim_CheckStamp");

                    b.HasIndex("CreateBy")
                        .HasDatabaseName("IX_IdentityClaim_CreateBy");

                    b.HasIndex("CreatedAt")
                        .HasDatabaseName("IX_IdentityClaim_CreatedAt");

                    b.HasIndex("DeletedAt")
                        .HasDatabaseName("IX_IdentityClaim_DeletedAt");

                    b.HasIndex("ModifyBy")
                        .HasDatabaseName("IX_IdentityClaim_ModifyBy");

                    b.HasIndex("RemoveBy")
                        .HasDatabaseName("IX_IdentityClaim_RemoveBy");

                    b.HasIndex("UpdatedAt")
                        .HasDatabaseName("IX_IdentityClaim_UpdatedAt");

                    b.HasIndex("ClaimType", "ClaimValue")
                        .HasDatabaseName("IX_IdentityClaim_ClaimType_ClaimValue");

                    b.ToTable("IdentityClaim", "Identity", t =>
                        {
                            t.HasComment("认证凭据数据集");
                        });
                });

            modelBuilder.Entity("Artemis.Service.Identity.Context.IdentityRole", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasComment("标识");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasComment("并发锁");

                    b.Property<Guid>("CreateBy")
                        .HasColumnType("UUID")
                        .HasComment("创建者标识");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TIMESTAMP")
                        .HasComment("创建时间");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("TIMESTAMP")
                        .HasComment("删除时间");

                    b.Property<string>("Description")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasComment("角色描述");

                    b.Property<Guid>("ModifyBy")
                        .HasColumnType("UUID")
                        .HasComment("更新者标识");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasComment("角色名");

                    b.Property<string>("NormalizedName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasComment("标准化角色名");

                    b.Property<Guid>("RemoveBy")
                        .HasColumnType("UUID")
                        .HasComment("删除者标识");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TIMESTAMP")
                        .HasComment("更新时间");

                    b.HasKey("Id")
                        .HasName("PK_IdentityRole");

                    b.HasIndex("CreateBy")
                        .HasDatabaseName("IX_IdentityRole_CreateBy");

                    b.HasIndex("CreatedAt")
                        .HasDatabaseName("IX_IdentityRole_CreatedAt");

                    b.HasIndex("DeletedAt")
                        .HasDatabaseName("IX_IdentityRole_DeletedAt");

                    b.HasIndex("ModifyBy")
                        .HasDatabaseName("IX_IdentityRole_ModifyBy");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("IX_IdentityRole_Name");

                    b.HasIndex("RemoveBy")
                        .HasDatabaseName("IX_IdentityRole_RemoveBy");

                    b.HasIndex("UpdatedAt")
                        .HasDatabaseName("IX_IdentityRole_UpdatedAt");

                    b.ToTable("IdentityRole", "Identity", t =>
                        {
                            t.HasComment("认证角色数据集");
                        });
                });

            modelBuilder.Entity("Artemis.Service.Identity.Context.IdentityRoleClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasComment("标识");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CheckStamp")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasComment("校验戳");

                    b.Property<string>("ClaimType")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasComment("凭据类型");

                    b.Property<string>("ClaimValue")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)")
                        .HasComment("凭据值");

                    b.Property<string>("Description")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasComment("凭据描述");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid")
                        .HasComment("角色标识");

                    b.HasKey("Id")
                        .HasName("PK_IdentityRoleClaim");

                    b.HasIndex("ClaimType", "ClaimValue")
                        .HasDatabaseName("IX_IdentityRoleClaim_ClaimType_ClaimValue");

                    b.HasIndex("RoleId", "CheckStamp")
                        .IsUnique()
                        .HasDatabaseName("IX_IdentityRoleClaim_RoleId_CheckStamp");

                    b.ToTable("IdentityRoleClaim", "Identity", t =>
                        {
                            t.HasComment("认证角色凭据数据集");
                        });
                });

            modelBuilder.Entity("Artemis.Service.Identity.Context.IdentityUser", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasComment("标识");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer")
                        .HasComment("失败尝试次数");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasComment("并发锁");

                    b.Property<Guid>("CreateBy")
                        .HasColumnType("UUID")
                        .HasComment("创建者标识");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TIMESTAMP")
                        .HasComment("创建时间");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("TIMESTAMP")
                        .HasComment("删除时间");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasComment("电子邮件");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean")
                        .HasComment("电子邮件确认戳");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean")
                        .HasComment("是否启用锁定");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone")
                        .HasComment("用户锁定到期时间标记");

                    b.Property<Guid>("ModifyBy")
                        .HasColumnType("UUID")
                        .HasComment("更新者标识");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasComment("标准化电子邮件");

                    b.Property<string>("NormalizedUserName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasComment("标准化用户名");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasComment("密码哈希");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasComment("电话号码");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean")
                        .HasComment("电话号码确认戳");

                    b.Property<Guid>("RemoveBy")
                        .HasColumnType("UUID")
                        .HasComment("删除者标识");

                    b.Property<string>("SecurityStamp")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasComment("密码锁");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean")
                        .HasComment("是否启用双因子认证");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TIMESTAMP")
                        .HasComment("更新时间");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasComment("用户名");

                    b.HasKey("Id")
                        .HasName("PK_IdentityUser");

                    b.HasIndex("CreateBy")
                        .HasDatabaseName("IX_IdentityUser_CreateBy");

                    b.HasIndex("CreatedAt")
                        .HasDatabaseName("IX_IdentityUser_CreatedAt");

                    b.HasIndex("DeletedAt")
                        .HasDatabaseName("IX_IdentityUser_DeletedAt");

                    b.HasIndex("ModifyBy")
                        .HasDatabaseName("IX_IdentityUser_ModifyBy");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("IX_IdentityUser_Email");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("IX_IdentityUser_UserName");

                    b.HasIndex("PhoneNumber")
                        .HasDatabaseName("IX_IdentityUser_PhoneNumber");

                    b.HasIndex("RemoveBy")
                        .HasDatabaseName("IX_IdentityUser_RemoveBy");

                    b.HasIndex("UpdatedAt")
                        .HasDatabaseName("IX_IdentityUser_UpdatedAt");

                    b.ToTable("IdentityUser", "Identity", t =>
                        {
                            t.HasComment("认证用户数据集");
                        });
                });

            modelBuilder.Entity("Artemis.Service.Identity.Context.IdentityUserClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasComment("标识");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CheckStamp")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasComment("校验戳");

                    b.Property<string>("ClaimType")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasComment("凭据类型");

                    b.Property<string>("ClaimValue")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)")
                        .HasComment("凭据值");

                    b.Property<string>("Description")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasComment("凭据描述");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasComment("用户标识");

                    b.HasKey("Id")
                        .HasName("PK_IdentityUserClaim");

                    b.HasIndex("ClaimType", "ClaimValue")
                        .HasDatabaseName("IX_IdentityUserClaim_ClaimType_ClaimValue");

                    b.HasIndex("UserId", "CheckStamp")
                        .IsUnique()
                        .HasDatabaseName("IX_IdentityUserClaim_UserId_CheckStamp");

                    b.ToTable("IdentityUserClaim", "Identity", t =>
                        {
                            t.HasComment("认证用户凭据数据集");
                        });
                });

            modelBuilder.Entity("Artemis.Service.Identity.Context.IdentityUserLogin", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasComment("登录提供程序");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasComment("提供程序密钥");

                    b.Property<string>("ProviderDisplayName")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasComment("提供程序显示名称");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasComment("用户标识");

                    b.HasKey("LoginProvider", "ProviderKey")
                        .HasName("PK_IdentityUserLogin");

                    b.HasIndex("UserId");

                    b.ToTable("IdentityUserLogin", "Identity", t =>
                        {
                            t.HasComment("认证用户角色登录数据集");
                        });
                });

            modelBuilder.Entity("Artemis.Service.Identity.Context.IdentityUserProfile", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasComment("用户标识");

                    b.Property<string>("Key")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasComment("用户档案数据键");

                    b.Property<string>("Value")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasComment("用户档案数据值");

                    b.HasKey("UserId", "Key")
                        .HasName("PK_IdentityUserProfile");

                    b.ToTable("IdentityUserProfile", "Identity", t =>
                        {
                            t.HasComment("认证用户角色档案数据集");
                        });
                });

            modelBuilder.Entity("Artemis.Service.Identity.Context.IdentityUserRole", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasComment("用户标识");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid")
                        .HasComment("角色标识");

                    b.HasKey("UserId", "RoleId")
                        .HasName("PK_IdentityUserRole");

                    b.HasIndex("RoleId");

                    b.ToTable("IdentityUserRole", "Identity", t =>
                        {
                            t.HasComment("认证用户角色映射数据集");
                        });
                });

            modelBuilder.Entity("Artemis.Service.Identity.Context.IdentityUserToken", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name")
                        .HasName("PK_IdentityUserToken");

                    b.ToTable("IdentityUserToken", "Identity", t =>
                        {
                            t.HasComment("认证用户令牌数据集");
                        });
                });

            modelBuilder.Entity("Artemis.Service.Identity.Context.IdentityRoleClaim", b =>
                {
                    b.HasOne("Artemis.Service.Identity.Context.IdentityRole", "Role")
                        .WithMany("RoleClaims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_IdentityRoleClaim_IdentityRole");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Artemis.Service.Identity.Context.IdentityUserClaim", b =>
                {
                    b.HasOne("Artemis.Service.Identity.Context.IdentityUser", "User")
                        .WithMany("UserClaims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_IdentityUserClaim_IdentityUser");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Artemis.Service.Identity.Context.IdentityUserLogin", b =>
                {
                    b.HasOne("Artemis.Service.Identity.Context.IdentityUser", "User")
                        .WithMany("UserLogins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_IdentityUserLogin_IdentityUser");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Artemis.Service.Identity.Context.IdentityUserProfile", b =>
                {
                    b.HasOne("Artemis.Service.Identity.Context.IdentityUser", "User")
                        .WithMany("UserProfiles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_IdentityUserProfile_IdentityUser");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Artemis.Service.Identity.Context.IdentityUserRole", b =>
                {
                    b.HasOne("Artemis.Service.Identity.Context.IdentityRole", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_IdentityUserRole_IdentityRole");

                    b.HasOne("Artemis.Service.Identity.Context.IdentityUser", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_IdentityUserRole_IdentityUser");

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Artemis.Service.Identity.Context.IdentityUserToken", b =>
                {
                    b.HasOne("Artemis.Service.Identity.Context.IdentityUser", "User")
                        .WithMany("UserTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_IdentityUserToken_IdentityUser");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Artemis.Service.Identity.Context.IdentityRole", b =>
                {
                    b.Navigation("RoleClaims");

                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("Artemis.Service.Identity.Context.IdentityUser", b =>
                {
                    b.Navigation("UserClaims");

                    b.Navigation("UserLogins");

                    b.Navigation("UserProfiles");

                    b.Navigation("UserRoles");

                    b.Navigation("UserTokens");
                });
#pragma warning restore 612, 618
        }
    }
}
