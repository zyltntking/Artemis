﻿// <auto-generated />

#nullable disable

using Artemis.Service.Task.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Artemis.App.Task.Migrations
{
    [DbContext(typeof(TaskContext))]
    [Migration("20240723071149_InitTask")]
    partial class InitTask
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Task")
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Artemis.Service.Task.Context.ArtemisAgent", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasComment("标识");

                    b.Property<string>("AgentCode")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasComment("代理编码");

                    b.Property<string>("AgentName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasComment("代理名称");

                    b.Property<string>("AgentType")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasComment("代理类型");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasComment("并发锁");

                    b.Property<string>("CreateBy")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasComment("创建者标识");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TIMESTAMP")
                        .HasComment("创建时间");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("TIMESTAMP")
                        .HasComment("删除时间");

                    b.Property<string>("ModifyBy")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasComment("更新者标识");

                    b.Property<int>("Partition")
                        .HasColumnType("integer")
                        .HasComment("分区标识");

                    b.Property<string>("RemoveBy")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasComment("删除者标识");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TIMESTAMP")
                        .HasComment("更新时间");

                    b.HasKey("Id")
                        .HasName("PK_ArtemisAgent");

                    b.HasIndex("CreateBy")
                        .HasDatabaseName("IX_ArtemisAgent_CreateBy");

                    b.HasIndex("CreatedAt")
                        .HasDatabaseName("IX_ArtemisAgent_CreatedAt");

                    b.HasIndex("DeletedAt")
                        .HasDatabaseName("IX_ArtemisAgent_DeletedAt");

                    b.HasIndex("ModifyBy")
                        .HasDatabaseName("IX_ArtemisAgent_ModifyBy");

                    b.HasIndex("Partition")
                        .HasDatabaseName("IX_ArtemisAgent_Partition");

                    b.HasIndex("RemoveBy")
                        .HasDatabaseName("IX_ArtemisAgent_RemoveBy");

                    b.HasIndex("UpdatedAt")
                        .HasDatabaseName("IX_ArtemisAgent_UpdatedAt");

                    b.ToTable("ArtemisAgent", "Task", t =>
                        {
                            t.HasComment("代理数据集");
                        });
                });

            modelBuilder.Entity("Artemis.Service.Task.Context.ArtemisTask", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasComment("标识");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasComment("并发锁");

                    b.Property<string>("CreateBy")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
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
                        .HasComment("任务描述");

                    b.Property<DateTime?>("EndTime")
                        .HasColumnType("TIMESTAMP")
                        .HasComment("任务结束时间");

                    b.Property<string>("ModifyBy")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasComment("更新者标识");

                    b.Property<string>("NormalizedTaskName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasComment("任务名称");

                    b.Property<Guid>("ParentId")
                        .HasColumnType("uuid")
                        .HasComment("父任务标识");

                    b.Property<int>("Partition")
                        .HasColumnType("integer")
                        .HasComment("分区标识");

                    b.Property<string>("RemoveBy")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasComment("删除者标识");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("TIMESTAMP")
                        .HasComment("任务开始时间");

                    b.Property<string>("TaskMode")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasComment("任务模式");

                    b.Property<string>("TaskName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasComment("任务名称");

                    b.Property<string>("TaskShip")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasComment("任务归属");

                    b.Property<string>("TaskStatus")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasComment("任务状态");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TIMESTAMP")
                        .HasComment("更新时间");

                    b.HasKey("Id")
                        .HasName("PK_ArtemisTask");

                    b.HasIndex("CreateBy")
                        .HasDatabaseName("IX_ArtemisTask_CreateBy");

                    b.HasIndex("CreatedAt")
                        .HasDatabaseName("IX_ArtemisTask_CreatedAt");

                    b.HasIndex("DeletedAt")
                        .HasDatabaseName("IX_ArtemisTask_DeletedAt");

                    b.HasIndex("EndTime")
                        .HasDatabaseName("IX_ArtemisTask_EndTime");

                    b.HasIndex("ModifyBy")
                        .HasDatabaseName("IX_ArtemisTask_ModifyBy");

                    b.HasIndex("NormalizedTaskName")
                        .IsUnique()
                        .HasDatabaseName("IX_ArtemisTask_TaskName");

                    b.HasIndex("ParentId");

                    b.HasIndex("Partition")
                        .HasDatabaseName("IX_ArtemisTask_Partition");

                    b.HasIndex("RemoveBy")
                        .HasDatabaseName("IX_ArtemisTask_RemoveBy");

                    b.HasIndex("StartTime")
                        .HasDatabaseName("IX_ArtemisTask_StartTime");

                    b.HasIndex("TaskMode")
                        .HasDatabaseName("IX_ArtemisTask_TaskMode");

                    b.HasIndex("TaskShip")
                        .HasDatabaseName("IX_ArtemisTask_TaskShip");

                    b.HasIndex("TaskStatus")
                        .HasDatabaseName("IX_ArtemisTask_TaskStatus");

                    b.HasIndex("UpdatedAt")
                        .HasDatabaseName("IX_ArtemisTask_UpdatedAt");

                    b.ToTable("ArtemisTask", "Task", t =>
                        {
                            t.HasComment("任务数据集");
                        });
                });

            modelBuilder.Entity("Artemis.Service.Task.Context.ArtemisTaskAgent", b =>
                {
                    b.Property<Guid>("TaskId")
                        .HasColumnType("uuid")
                        .HasComment("任务标识");

                    b.Property<Guid>("AgentId")
                        .HasColumnType("uuid")
                        .HasComment("代理标识");

                    b.HasKey("TaskId", "AgentId")
                        .HasName("PK_ArtemisTaskAgent");

                    b.HasIndex("AgentId");

                    b.ToTable("ArtemisTaskAgent", "Task", t =>
                        {
                            t.HasComment("任务代理配置");
                        });
                });

            modelBuilder.Entity("Artemis.Service.Task.Context.ArtemisTaskTarget", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasComment("标识");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasComment("并发锁");

                    b.Property<string>("CreateBy")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasComment("创建者标识");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TIMESTAMP")
                        .HasComment("创建时间");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("TIMESTAMP")
                        .HasComment("删除时间");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasComment("任务描述");

                    b.Property<DateTime?>("EndTime")
                        .HasColumnType("timestamp with time zone")
                        .HasComment("任务结束时间");

                    b.Property<string>("ModifyBy")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasComment("更新者标识");

                    b.Property<int>("Partition")
                        .HasColumnType("integer")
                        .HasComment("分区标识");

                    b.Property<string>("RemoveBy")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasComment("删除者标识");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("timestamp with time zone")
                        .HasComment("任务开始时间");

                    b.Property<Guid>("TargetId")
                        .HasColumnType("uuid")
                        .HasComment("目标标识");

                    b.Property<Guid>("TaskId")
                        .HasColumnType("uuid")
                        .HasComment("任务标识");

                    b.Property<string>("TaskStatus")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasComment("任务状态");

                    b.Property<Guid>("TaskUnitId")
                        .HasColumnType("uuid")
                        .HasComment("任务单元标识");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TIMESTAMP")
                        .HasComment("更新时间");

                    b.HasKey("Id")
                        .HasName("PK_ArtemisTaskTarget");

                    b.HasIndex("CreateBy")
                        .HasDatabaseName("IX_ArtemisTaskTarget_CreateBy");

                    b.HasIndex("CreatedAt")
                        .HasDatabaseName("IX_ArtemisTaskTarget_CreatedAt");

                    b.HasIndex("DeletedAt")
                        .HasDatabaseName("IX_ArtemisTaskTarget_DeletedAt");

                    b.HasIndex("ModifyBy")
                        .HasDatabaseName("IX_ArtemisTaskTarget_ModifyBy");

                    b.HasIndex("Partition")
                        .HasDatabaseName("IX_ArtemisTaskTarget_Partition");

                    b.HasIndex("RemoveBy")
                        .HasDatabaseName("IX_ArtemisTaskTarget_RemoveBy");

                    b.HasIndex("TaskUnitId");

                    b.HasIndex("UpdatedAt")
                        .HasDatabaseName("IX_ArtemisTaskTarget_UpdatedAt");

                    b.ToTable("ArtemisTaskTarget", "Task", t =>
                        {
                            t.HasComment("任务目标数据集");
                        });
                });

            modelBuilder.Entity("Artemis.Service.Task.Context.ArtemisTaskUnit", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasComment("标识");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasComment("并发锁");

                    b.Property<string>("CreateBy")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasComment("创建者标识");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TIMESTAMP")
                        .HasComment("创建时间");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("TIMESTAMP")
                        .HasComment("删除时间");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasComment("任务描述");

                    b.Property<DateTime?>("EndTime")
                        .HasColumnType("timestamp with time zone")
                        .HasComment("任务结束时间");

                    b.Property<string>("ModifyBy")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasComment("更新者标识");

                    b.Property<int>("Partition")
                        .HasColumnType("integer")
                        .HasComment("分区标识");

                    b.Property<string>("RemoveBy")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasComment("删除者标识");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("timestamp with time zone")
                        .HasComment("任务开始时间");

                    b.Property<Guid>("TaskId")
                        .HasColumnType("uuid")
                        .HasComment("任务标识");

                    b.Property<string>("TaskStatus")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasComment("任务状态");

                    b.Property<string>("UnitName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasComment("任务单元名称");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TIMESTAMP")
                        .HasComment("更新时间");

                    b.HasKey("Id")
                        .HasName("PK_ArtemisTaskUnit");

                    b.HasIndex("CreateBy")
                        .HasDatabaseName("IX_ArtemisTaskUnit_CreateBy");

                    b.HasIndex("CreatedAt")
                        .HasDatabaseName("IX_ArtemisTaskUnit_CreatedAt");

                    b.HasIndex("DeletedAt")
                        .HasDatabaseName("IX_ArtemisTaskUnit_DeletedAt");

                    b.HasIndex("ModifyBy")
                        .HasDatabaseName("IX_ArtemisTaskUnit_ModifyBy");

                    b.HasIndex("Partition")
                        .HasDatabaseName("IX_ArtemisTaskUnit_Partition");

                    b.HasIndex("RemoveBy")
                        .HasDatabaseName("IX_ArtemisTaskUnit_RemoveBy");

                    b.HasIndex("TaskId");

                    b.HasIndex("UpdatedAt")
                        .HasDatabaseName("IX_ArtemisTaskUnit_UpdatedAt");

                    b.ToTable("ArtemisTaskUnit", "Task", t =>
                        {
                            t.HasComment("任务单元数据集");
                        });
                });

            modelBuilder.Entity("Artemis.Service.Task.Context.ArtemisTaskUnitAgent", b =>
                {
                    b.Property<Guid>("TaskUnitId")
                        .HasColumnType("uuid")
                        .HasComment("任务单元标识");

                    b.Property<Guid>("AgentId")
                        .HasColumnType("uuid")
                        .HasComment("代理标识");

                    b.HasKey("TaskUnitId", "AgentId")
                        .HasName("PK_ArtemisTaskUnitAgent");

                    b.HasIndex("AgentId");

                    b.ToTable("ArtemisTaskUnitAgent", "Task", t =>
                        {
                            t.HasComment("任务单元代理配置");
                        });
                });

            modelBuilder.Entity("Artemis.Service.Task.Context.ArtemisTask", b =>
                {
                    b.HasOne("Artemis.Service.Task.Context.ArtemisTask", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_ArtemisTask_ArtemisTask");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("Artemis.Service.Task.Context.ArtemisTaskAgent", b =>
                {
                    b.HasOne("Artemis.Service.Task.Context.ArtemisAgent", "Agent")
                        .WithMany("TaskAgents")
                        .HasForeignKey("AgentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_ArtemisTaskAgent_ArtemisAgent");

                    b.HasOne("Artemis.Service.Task.Context.ArtemisTask", "Task")
                        .WithMany("TaskAgents")
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_ArtemisTaskAgent_ArtemisTask");

                    b.Navigation("Agent");

                    b.Navigation("Task");
                });

            modelBuilder.Entity("Artemis.Service.Task.Context.ArtemisTaskTarget", b =>
                {
                    b.HasOne("Artemis.Service.Task.Context.ArtemisTaskUnit", "TaskUnit")
                        .WithMany("TaskTargets")
                        .HasForeignKey("TaskUnitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_ArtemisTaskTarget_ArtemisTaskUnit");

                    b.Navigation("TaskUnit");
                });

            modelBuilder.Entity("Artemis.Service.Task.Context.ArtemisTaskUnit", b =>
                {
                    b.HasOne("Artemis.Service.Task.Context.ArtemisTask", "Task")
                        .WithMany("TaskUnits")
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_ArtemisTaskUnit_ArtemisTask");

                    b.Navigation("Task");
                });

            modelBuilder.Entity("Artemis.Service.Task.Context.ArtemisTaskUnitAgent", b =>
                {
                    b.HasOne("Artemis.Service.Task.Context.ArtemisAgent", "Agent")
                        .WithMany("TaskUnitAgents")
                        .HasForeignKey("AgentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_ArtemisTaskUnitAgent_ArtemisAgent");

                    b.HasOne("Artemis.Service.Task.Context.ArtemisTaskUnit", "TaskUnit")
                        .WithMany("TaskUnitAgents")
                        .HasForeignKey("TaskUnitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_ArtemisTaskUnitAgent_ArtemisTaskUnit");

                    b.Navigation("Agent");

                    b.Navigation("TaskUnit");
                });

            modelBuilder.Entity("Artemis.Service.Task.Context.ArtemisAgent", b =>
                {
                    b.Navigation("TaskAgents");

                    b.Navigation("TaskUnitAgents");
                });

            modelBuilder.Entity("Artemis.Service.Task.Context.ArtemisTask", b =>
                {
                    b.Navigation("Children");

                    b.Navigation("TaskAgents");

                    b.Navigation("TaskUnits");
                });

            modelBuilder.Entity("Artemis.Service.Task.Context.ArtemisTaskUnit", b =>
                {
                    b.Navigation("TaskTargets");

                    b.Navigation("TaskUnitAgents");
                });
#pragma warning restore 612, 618
        }
    }
}