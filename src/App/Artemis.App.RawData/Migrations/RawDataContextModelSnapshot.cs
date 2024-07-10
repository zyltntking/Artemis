﻿// <auto-generated />
using System;
using Artemis.Service.RawData.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Artemis.App.RawData.Migrations
{
    [DbContext(typeof(RawDataContext))]
    partial class RawDataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("RawData")
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Artemis.Service.RawData.Context.ArtemisOptometer", b =>
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

                    b.Property<Guid>("ModifyBy")
                        .HasColumnType("UUID")
                        .HasComment("更新者标识");

                    b.Property<int>("Partition")
                        .HasColumnType("integer")
                        .HasComment("分区标识");

                    b.Property<Guid>("RemoveBy")
                        .HasColumnType("UUID")
                        .HasComment("删除者标识");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TIMESTAMP")
                        .HasComment("更新时间");

                    b.HasKey("Id")
                        .HasName("PK_ArtemisOptometer");

                    b.HasIndex("CreateBy")
                        .HasDatabaseName("IX_ArtemisOptometer_CreateBy");

                    b.HasIndex("CreatedAt")
                        .HasDatabaseName("IX_ArtemisOptometer_CreatedAt");

                    b.HasIndex("DeletedAt")
                        .HasDatabaseName("IX_ArtemisOptometer_DeletedAt");

                    b.HasIndex("ModifyBy")
                        .HasDatabaseName("IX_ArtemisOptometer_ModifyBy");

                    b.HasIndex("Partition")
                        .HasDatabaseName("IX_ArtemisOptometer_Partition");

                    b.HasIndex("RemoveBy")
                        .HasDatabaseName("IX_ArtemisOptometer_RemoveBy");

                    b.HasIndex("UpdatedAt")
                        .HasDatabaseName("IX_ArtemisOptometer_UpdatedAt");

                    b.ToTable("ArtemisOptometer", "RawData", t =>
                        {
                            t.HasComment("验光仪数据集");
                        });
                });

            modelBuilder.Entity("Artemis.Service.RawData.Context.ArtemisVisualChart", b =>
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

                    b.Property<Guid>("ModifyBy")
                        .HasColumnType("UUID")
                        .HasComment("更新者标识");

                    b.Property<int>("Partition")
                        .HasColumnType("integer")
                        .HasComment("分区标识");

                    b.Property<Guid>("RemoveBy")
                        .HasColumnType("UUID")
                        .HasComment("删除者标识");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TIMESTAMP")
                        .HasComment("更新时间");

                    b.HasKey("Id")
                        .HasName("PK_ArtemisVisualChart");

                    b.HasIndex("CreateBy")
                        .HasDatabaseName("IX_ArtemisVisualChart_CreateBy");

                    b.HasIndex("CreatedAt")
                        .HasDatabaseName("IX_ArtemisVisualChart_CreatedAt");

                    b.HasIndex("DeletedAt")
                        .HasDatabaseName("IX_ArtemisVisualChart_DeletedAt");

                    b.HasIndex("ModifyBy")
                        .HasDatabaseName("IX_ArtemisVisualChart_ModifyBy");

                    b.HasIndex("Partition")
                        .HasDatabaseName("IX_ArtemisVisualChart_Partition");

                    b.HasIndex("RemoveBy")
                        .HasDatabaseName("IX_ArtemisVisualChart_RemoveBy");

                    b.HasIndex("UpdatedAt")
                        .HasDatabaseName("IX_ArtemisVisualChart_UpdatedAt");

                    b.ToTable("ArtemisVisualChart", "RawData", t =>
                        {
                            t.HasComment("视力表数据集");
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
