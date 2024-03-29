﻿// <auto-generated />
using System;
using Artemis.Services.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Artemis.App.Identity.Migrations
{
    [DbContext(typeof(ArtemisIdentityContext))]
    partial class ArtemisIdentityContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("identity")
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Artemis.Services.Identity.Data.ArtemisClaim", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasComment("标识");

                    b.Property<string>("CheckStamp")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasComment("校验戳");

                    b.Property<string>("ClaimType")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasComment("凭据类型");

                    b.Property<string>("ClaimValue")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasComment("凭据值");

                    b.Property<string>("Description")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasComment("凭据描述");

                    b.HasKey("Id")
                        .HasName("PK_ArtemisClaim");

                    b.HasIndex("CheckStamp")
                        .IsUnique()
                        .HasDatabaseName("IX_ArtemisClaim_CheckStamp");

                    b.HasIndex("ClaimType", "ClaimValue")
                        .HasDatabaseName("IX_ArtemisClaim_ClaimType_ClaimValue");

                    b.ToTable("ArtemisClaim", "identity", t =>
                        {
                            t.HasComment("认证凭据数据集");
                        });
                });

            modelBuilder.Entity("Artemis.Services.Identity.Data.ArtemisRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasComment("标识");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasComment("并发锁");

                    b.Property<string>("Description")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasComment("角色描述");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasComment("角色名");

                    b.Property<string>("NormalizedName")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasComment("规范化角色名");

                    b.HasKey("Id")
                        .HasName("PK_ArtemisRole");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("IX_ArtemisRole_Name");

                    b.ToTable("ArtemisRole", "identity", t =>
                        {
                            t.HasComment("认证角色数据集");
                        });
                });

            modelBuilder.Entity("Artemis.Services.Identity.Data.ArtemisRoleClaim", b =>
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
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasComment("凭据类型");

                    b.Property<string>("ClaimValue")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasComment("凭据值");

                    b.Property<string>("Description")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasComment("凭据描述");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid")
                        .HasComment("角色标识");

                    b.HasKey("Id")
                        .HasName("PK_ArtemisRoleClaim");

                    b.HasIndex("CheckStamp")
                        .IsUnique()
                        .HasDatabaseName("IX_ArtemisRoleClaim_CheckStamp");

                    b.HasIndex("RoleId");

                    b.HasIndex("ClaimType", "ClaimValue")
                        .HasDatabaseName("IX_ArtemisRoleClaim_ClaimType_ClaimValue");

                    b.ToTable("ArtemisRoleClaim", "identity", t =>
                        {
                            t.HasComment("认证角色凭据数据集");
                        });
                });

            modelBuilder.Entity("Artemis.Services.Identity.Data.ArtemisUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasComment("标识");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer")
                        .HasComment("尝试错误数量");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasComment("并发锁");

                    b.Property<string>("Email")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasComment("邮箱地址");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean")
                        .HasComment("是否确认邮箱地址");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean")
                        .HasComment("是否允许锁定用户");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone")
                        .HasComment("用户锁定到期时间标记");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasComment("规范化邮箱地址");

                    b.Property<string>("NormalizedPhoneNumber")
                        .HasMaxLength(16)
                        .HasColumnType("character varying(16)")
                        .HasComment("规范化电话号码");

                    b.Property<string>("NormalizedUserName")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasComment("规范化用户名");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasComment("密码哈希");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(16)
                        .HasColumnType("character varying(16)")
                        .HasComment("电话号码");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean")
                        .HasComment("是否确认电话号码");

                    b.Property<string>("SecurityStamp")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasComment("密码锁");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean")
                        .HasComment("是否允许双步认证");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasComment("用户名");

                    b.HasKey("Id")
                        .HasName("PK_ArtemisUser");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("IX_ArtemisUser_Email");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("IX_ArtemisUser_UserName");

                    b.HasIndex("PhoneNumber")
                        .HasDatabaseName("IX_ArtemisUser_PhoneNumber");

                    b.ToTable("ArtemisUser", "identity", t =>
                        {
                            t.HasComment("认证用户数据集");
                        });
                });

            modelBuilder.Entity("Artemis.Services.Identity.Data.ArtemisUserClaim", b =>
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
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasComment("凭据类型");

                    b.Property<string>("ClaimValue")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasComment("凭据类型");

                    b.Property<string>("Description")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasComment("凭据描述");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasComment("用户标识");

                    b.HasKey("Id")
                        .HasName("PK_ArtemisUserClaim");

                    b.HasIndex("CheckStamp")
                        .IsUnique()
                        .HasDatabaseName("IX_ArtemisUserClaim_CheckStamp");

                    b.HasIndex("UserId");

                    b.HasIndex("ClaimType", "ClaimValue")
                        .HasDatabaseName("IX_ArtemisUserClaim_ClaimType_ClaimValue");

                    b.ToTable("ArtemisUserClaim", "identity", t =>
                        {
                            t.HasComment("认证用户数据集");
                        });
                });

            modelBuilder.Entity("Artemis.Services.Identity.Data.ArtemisUserLogin", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasComment("认证提供程序");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasComment("认证提供程序提供的第三方标识");

                    b.Property<string>("ProviderDisplayName")
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasComment("认证提供程序显示的用户名");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasComment("用户标识");

                    b.HasKey("LoginProvider", "ProviderKey")
                        .HasName("PK_ArtemisUserLogin");

                    b.HasIndex("UserId");

                    b.ToTable("ArtemisUserLogin", "identity", t =>
                        {
                            t.HasComment("认证用户登录数据集");
                        });
                });

            modelBuilder.Entity("Artemis.Services.Identity.Data.ArtemisUserProfile", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasComment("用户标识");

                    b.Property<string>("Key")
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasComment("用户信息键");

                    b.Property<string>("Value")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasComment("用户信息值");

                    b.HasKey("UserId", "Key", "Value")
                        .HasName("PK_ArtemisUserProfile");

                    b.HasIndex("Key", "Value")
                        .IsUnique()
                        .HasDatabaseName("IX_ArtemisUserProfile_Key_Value");

                    b.ToTable("ArtemisUserProfile", "identity", t =>
                        {
                            t.HasComment("认证用户信息数据集");
                        });
                });

            modelBuilder.Entity("Artemis.Services.Identity.Data.ArtemisUserRole", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasComment("用户标识");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid")
                        .HasComment("角色标识");

                    b.HasKey("UserId", "RoleId")
                        .HasName("PK_ArtemisUserRole");

                    b.HasIndex("RoleId");

                    b.ToTable("ArtemisUserRole", "identity", t =>
                        {
                            t.HasComment("认证用户角色映射数据集");
                        });
                });

            modelBuilder.Entity("Artemis.Services.Identity.Data.ArtemisUserToken", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasComment("用户标识");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasComment("认证提供程序");

                    b.Property<string>("Name")
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasComment("认证令牌名");

                    b.Property<string>("Value")
                        .HasColumnType("text")
                        .HasComment("认证令牌");

                    b.HasKey("UserId", "LoginProvider", "Name")
                        .HasName("PK_ArtemisUserToken");

                    b.ToTable("ArtemisUserToken", "identity", t =>
                        {
                            t.HasComment("认证用户令牌数据集");
                        });
                });

            modelBuilder.Entity("Artemis.Services.Identity.Data.ArtemisRoleClaim", b =>
                {
                    b.HasOne("Artemis.Services.Identity.Data.ArtemisRole", "Role")
                        .WithMany("RoleClaims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_ArtemisUserClaim_ArtemisRole_Id");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Artemis.Services.Identity.Data.ArtemisUserClaim", b =>
                {
                    b.HasOne("Artemis.Services.Identity.Data.ArtemisUser", "User")
                        .WithMany("UserClaims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_ArtemisUserClaim_ArtemisUser_Id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Artemis.Services.Identity.Data.ArtemisUserLogin", b =>
                {
                    b.HasOne("Artemis.Services.Identity.Data.ArtemisUser", "User")
                        .WithMany("UserLogins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_ArtemisUserLogin_ArtemisUser_Id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Artemis.Services.Identity.Data.ArtemisUserProfile", b =>
                {
                    b.HasOne("Artemis.Services.Identity.Data.ArtemisUser", "User")
                        .WithMany("UserProfiles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_ArtemisUserProfile_ArtemisUser_Id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Artemis.Services.Identity.Data.ArtemisUserRole", b =>
                {
                    b.HasOne("Artemis.Services.Identity.Data.ArtemisRole", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_ArtemisUserRole_ArtemisRole_Id");

                    b.HasOne("Artemis.Services.Identity.Data.ArtemisUser", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_ArtemisUserRole_ArtemisUser_Id");

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Artemis.Services.Identity.Data.ArtemisUserToken", b =>
                {
                    b.HasOne("Artemis.Services.Identity.Data.ArtemisUser", "User")
                        .WithMany("UserTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_ArtemisUserToken_ArtemisUser_Id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Artemis.Services.Identity.Data.ArtemisRole", b =>
                {
                    b.Navigation("RoleClaims");

                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("Artemis.Services.Identity.Data.ArtemisUser", b =>
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
