﻿// <auto-generated />
using System;
using Artemis.App.BroadcastApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Artemis.App.BroadcastApi.Migrations
{
    [DbContext(typeof(BroadcastContext))]
    partial class BroadcastContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Artemis.App.BroadcastApi.Data.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasComment("标识");

                    b.Property<int>("Count")
                        .HasColumnType("integer")
                        .HasComment("用餐人数");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TIMESTAMP")
                        .HasComment("创建时间,初始化后不再进行任何变更");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("TIMESTAMP")
                        .HasComment("删除时间,启用软删除时生效");

                    b.Property<string>("License")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)")
                        .HasComment("车牌号");

                    b.Property<string>("MealDate")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasComment("用餐日期");

                    b.Property<string>("MealType")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasComment("用餐类型");

                    b.Property<double>("Price")
                        .HasColumnType("double precision")
                        .HasComment("餐价");

                    b.Property<string>("Remark")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasComment("备注");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasComment("状态");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TIMESTAMP")
                        .HasComment("更新时间,初始为创建时间");

                    b.Property<int>("WaitFlag")
                        .HasColumnType("integer")
                        .HasComment("等待序列");

                    b.HasKey("Id")
                        .HasName("PK_Order");

                    b.HasIndex("CreatedAt")
                        .HasDatabaseName("IX_Order_CreatedAt");

                    b.HasIndex("DeletedAt")
                        .HasDatabaseName("IX_Order_DeletedAt");

                    b.HasIndex("MealDate")
                        .HasDatabaseName("IX_Order_MealDate");

                    b.HasIndex("Status")
                        .HasDatabaseName("IX_Order_Status");

                    b.HasIndex("UpdatedAt")
                        .HasDatabaseName("IX_Order_UpdatedAt");

                    b.HasIndex("WaitFlag")
                        .HasDatabaseName("IX_Order_WaitFlag");

                    b.ToTable("Order", null, t =>
                        {
                            t.HasComment("订单数据集");
                        });
                });

            modelBuilder.Entity("Artemis.App.BroadcastApi.Data.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasComment("标识");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TIMESTAMP")
                        .HasComment("创建时间,初始化后不再进行任何变更");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("TIMESTAMP")
                        .HasComment("删除时间,启用软删除时生效");

                    b.Property<string>("NormalizedUserName")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasComment("规范化用户名");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasComment("凭据值");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TIMESTAMP")
                        .HasComment("更新时间,初始为创建时间");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasComment("用户名");

                    b.HasKey("Id")
                        .HasName("PK_User");

                    b.HasIndex("CreatedAt")
                        .HasDatabaseName("IX_User_CreatedAt");

                    b.HasIndex("DeletedAt")
                        .HasDatabaseName("IX_User_DeletedAt");

                    b.HasIndex("NormalizedUserName")
                        .HasDatabaseName("IX_User_UserName");

                    b.HasIndex("UpdatedAt")
                        .HasDatabaseName("IX_User_UpdatedAt");

                    b.ToTable("User", null, t =>
                        {
                            t.HasComment("用户数据集");
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
