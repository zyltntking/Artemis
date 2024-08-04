﻿// <auto-generated />
using System;
using Artemis.Service.School.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Artemis.App.School.Migrations
{
    [DbContext(typeof(SchoolContext))]
    [Migration("20240804182647_InitialSchool")]
    partial class InitialSchool
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("School")
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Artemis.Service.School.Context.ArtemisClass", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasComment("标识");

                    b.Property<string>("Code")
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasComment("班级编码");

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

                    b.Property<DateTime?>("EstablishTime")
                        .HasColumnType("timestamp with time zone")
                        .HasComment("班级创建时间");

                    b.Property<string>("GradeName")
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasComment("年级名称");

                    b.Property<Guid?>("HeadTeacherId")
                        .HasColumnType("uuid")
                        .HasComment("班主任标识");

                    b.Property<string>("HeadTeacherName")
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasComment("班主任名称");

                    b.Property<int>("Length")
                        .HasColumnType("integer")
                        .HasComment("学制长度");

                    b.Property<string>("Major")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasComment("所学专业");

                    b.Property<string>("ModifyBy")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasComment("更新者标识");

                    b.Property<string>("Name")
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasComment("班级名称");

                    b.Property<string>("Remark")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasComment("备注");

                    b.Property<string>("RemoveBy")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasComment("删除者标识");

                    b.Property<Guid>("SchoolId")
                        .HasColumnType("uuid")
                        .HasComment("学校标识");

                    b.Property<string>("SchoolLength")
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasComment("学制");

                    b.Property<int>("SerialNumber")
                        .HasColumnType("integer")
                        .HasComment("班级序号");

                    b.Property<string>("StudyPhase")
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasComment("学段");

                    b.Property<string>("Type")
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasComment("班级类型");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TIMESTAMP")
                        .HasComment("更新时间");

                    b.HasKey("Id")
                        .HasName("PK_ArtemisClass");

                    b.HasIndex("Code")
                        .HasDatabaseName("IX_ArtemisClass_Code");

                    b.HasIndex("CreateBy")
                        .HasDatabaseName("IX_ArtemisClass_CreateBy");

                    b.HasIndex("CreatedAt")
                        .HasDatabaseName("IX_ArtemisClass_CreatedAt");

                    b.HasIndex("DeletedAt")
                        .HasDatabaseName("IX_ArtemisClass_DeletedAt");

                    b.HasIndex("GradeName")
                        .HasDatabaseName("IX_ArtemisClass_GradeName");

                    b.HasIndex("HeadTeacherId")
                        .IsUnique();

                    b.HasIndex("Length")
                        .HasDatabaseName("IX_ArtemisClass_Length");

                    b.HasIndex("ModifyBy")
                        .HasDatabaseName("IX_ArtemisClass_ModifyBy");

                    b.HasIndex("Name")
                        .HasDatabaseName("IX_ArtemisClass_Name");

                    b.HasIndex("RemoveBy")
                        .HasDatabaseName("IX_ArtemisClass_RemoveBy");

                    b.HasIndex("SchoolId");

                    b.HasIndex("SerialNumber")
                        .HasDatabaseName("IX_ArtemisClass_SerialNumber");

                    b.HasIndex("StudyPhase")
                        .HasDatabaseName("IX_ArtemisClass_StudyPhase");

                    b.HasIndex("UpdatedAt")
                        .HasDatabaseName("IX_ArtemisClass_UpdatedAt");

                    b.ToTable("ArtemisClass", "School", t =>
                        {
                            t.HasComment("班级数据集");
                        });
                });

            modelBuilder.Entity("Artemis.Service.School.Context.ArtemisSchool", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasComment("标识");

                    b.Property<string>("Address")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasComment("学校地址");

                    b.Property<string>("BindingTag")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasComment("绑定标记");

                    b.Property<string>("Code")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasComment("学校编码");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasComment("并发锁");

                    b.Property<string>("ContactNumber")
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasComment("学校联系电话");

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

                    b.Property<string>("DivisionCode")
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasComment("学校所在地行政区划代码");

                    b.Property<string>("Email")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasComment("学校邮箱");

                    b.Property<DateTime?>("EstablishTime")
                        .HasColumnType("TIMESTAMP")
                        .HasComment("学校建立时间");

                    b.Property<string>("Introduction")
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)")
                        .HasComment("学校简介");

                    b.Property<string>("ModifyBy")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasComment("更新者标识");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasComment("学校名称");

                    b.Property<string>("OrganizationCode")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasComment("组织机构代码");

                    b.Property<string>("Remark")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasComment("备注");

                    b.Property<string>("RemoveBy")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasComment("删除者标识");

                    b.Property<string>("Type")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasComment("学校类型");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TIMESTAMP")
                        .HasComment("更新时间");

                    b.Property<string>("WebSite")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasComment("学校网站");

                    b.HasKey("Id")
                        .HasName("PK_ArtemisSchool");

                    b.HasIndex("BindingTag")
                        .HasDatabaseName("IX_ArtemisSchool_BindingTag");

                    b.HasIndex("Code")
                        .HasDatabaseName("IX_ArtemisSchool_Code");

                    b.HasIndex("CreateBy")
                        .HasDatabaseName("IX_ArtemisSchool_CreateBy");

                    b.HasIndex("CreatedAt")
                        .HasDatabaseName("IX_ArtemisSchool_CreatedAt");

                    b.HasIndex("DeletedAt")
                        .HasDatabaseName("IX_ArtemisSchool_DeletedAt");

                    b.HasIndex("DivisionCode")
                        .HasDatabaseName("IX_ArtemisSchool_DivisionCode");

                    b.HasIndex("ModifyBy")
                        .HasDatabaseName("IX_ArtemisSchool_ModifyBy");

                    b.HasIndex("Name")
                        .HasDatabaseName("IX_ArtemisSchool_Name");

                    b.HasIndex("OrganizationCode")
                        .HasDatabaseName("IX_ArtemisSchool_OrganizationCode");

                    b.HasIndex("RemoveBy")
                        .HasDatabaseName("IX_ArtemisSchool_RemoveBy");

                    b.HasIndex("Type")
                        .HasDatabaseName("IX_ArtemisSchool_Type");

                    b.HasIndex("UpdatedAt")
                        .HasDatabaseName("IX_ArtemisSchool_UpdatedAt");

                    b.ToTable("ArtemisSchool", "School", t =>
                        {
                            t.HasComment("学校数据集");
                        });
                });

            modelBuilder.Entity("Artemis.Service.School.Context.ArtemisStudent", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasComment("标识");

                    b.Property<string>("Address")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasComment("住址");

                    b.Property<DateTime?>("Birthday")
                        .HasColumnType("TIMESTAMP")
                        .HasComment("学生生日");

                    b.Property<string>("Cert")
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasComment("证件号码");

                    b.Property<Guid?>("ClassId")
                        .HasColumnType("uuid")
                        .HasComment("班级标识");

                    b.Property<string>("Code")
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasComment("学生编码");

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

                    b.Property<string>("DivisionCode")
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasComment("住址区划代码");

                    b.Property<DateTime?>("EnrollmentDate")
                        .HasColumnType("TIMESTAMP")
                        .HasComment("入学时间");

                    b.Property<string>("Gender")
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasComment("学生性别");

                    b.Property<string>("ModifyBy")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasComment("更新者标识");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasComment("学生名称");

                    b.Property<string>("Nation")
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasComment("民族");

                    b.Property<string>("Remark")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasComment("备注");

                    b.Property<string>("RemoveBy")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasComment("删除者标识");

                    b.Property<Guid?>("SchoolId")
                        .HasColumnType("uuid")
                        .HasComment("学校标识");

                    b.Property<string>("StudentNumber")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasComment("学籍号");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TIMESTAMP")
                        .HasComment("更新时间");

                    b.HasKey("Id")
                        .HasName("PK_ArtemisStudent");

                    b.HasIndex("ClassId");

                    b.HasIndex("CreateBy")
                        .HasDatabaseName("IX_ArtemisStudent_CreateBy");

                    b.HasIndex("CreatedAt")
                        .HasDatabaseName("IX_ArtemisStudent_CreatedAt");

                    b.HasIndex("DeletedAt")
                        .HasDatabaseName("IX_ArtemisStudent_DeletedAt");

                    b.HasIndex("Gender")
                        .HasDatabaseName("IX_ArtemisStudent_Gender");

                    b.HasIndex("ModifyBy")
                        .HasDatabaseName("IX_ArtemisStudent_ModifyBy");

                    b.HasIndex("Name")
                        .HasDatabaseName("IX_ArtemisStudent_Name");

                    b.HasIndex("Nation")
                        .HasDatabaseName("IX_ArtemisStudent_Nation");

                    b.HasIndex("RemoveBy")
                        .HasDatabaseName("IX_ArtemisStudent_RemoveBy");

                    b.HasIndex("SchoolId");

                    b.HasIndex("StudentNumber")
                        .HasDatabaseName("IX_ArtemisStudent_StudentNumber");

                    b.HasIndex("UpdatedAt")
                        .HasDatabaseName("IX_ArtemisStudent_UpdatedAt");

                    b.ToTable("ArtemisStudent", "School", t =>
                        {
                            t.HasComment("学生数据集");
                        });
                });

            modelBuilder.Entity("Artemis.Service.School.Context.ArtemisTeacher", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasComment("标识");

                    b.Property<string>("Address")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasComment("家庭住址");

                    b.Property<DateTime?>("Birthday")
                        .HasColumnType("TIMESTAMP")
                        .HasComment("生日");

                    b.Property<string>("Code")
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasComment("教师编码");

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

                    b.Property<string>("Education")
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasComment("教师学历");

                    b.Property<string>("Email")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasComment("邮箱");

                    b.Property<DateTime?>("EntryTime")
                        .HasColumnType("TIMESTAMP")
                        .HasComment("入职时间");

                    b.Property<string>("Gender")
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasComment("教师性别");

                    b.Property<string>("IdCard")
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasComment("教师身份证号");

                    b.Property<string>("ModifyBy")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasComment("更新者标识");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasComment("教师名称");

                    b.Property<string>("NativePlace")
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasComment("教师籍贯");

                    b.Property<string>("Phone")
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasComment("联系电话");

                    b.Property<string>("PoliticalStatus")
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasComment("政治面貌");

                    b.Property<string>("Remark")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasComment("备注");

                    b.Property<string>("RemoveBy")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasComment("删除者标识");

                    b.Property<Guid?>("SchoolId")
                        .HasColumnType("uuid")
                        .HasComment("学校标识");

                    b.Property<string>("Title")
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasComment("教师职称");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TIMESTAMP")
                        .HasComment("更新时间");

                    b.HasKey("Id")
                        .HasName("PK_ArtemisTeacher");

                    b.HasIndex("Code")
                        .HasDatabaseName("IX_ArtemisTeacher_Code");

                    b.HasIndex("CreateBy")
                        .HasDatabaseName("IX_ArtemisTeacher_CreateBy");

                    b.HasIndex("CreatedAt")
                        .HasDatabaseName("IX_ArtemisTeacher_CreatedAt");

                    b.HasIndex("DeletedAt")
                        .HasDatabaseName("IX_ArtemisTeacher_DeletedAt");

                    b.HasIndex("ModifyBy")
                        .HasDatabaseName("IX_ArtemisTeacher_ModifyBy");

                    b.HasIndex("Name")
                        .HasDatabaseName("IX_ArtemisTeacher_Name");

                    b.HasIndex("RemoveBy")
                        .HasDatabaseName("IX_ArtemisTeacher_RemoveBy");

                    b.HasIndex("SchoolId");

                    b.HasIndex("UpdatedAt")
                        .HasDatabaseName("IX_ArtemisTeacher_UpdatedAt");

                    b.ToTable("ArtemisTeacher", "School", t =>
                        {
                            t.HasComment("教师数据集");
                        });
                });

            modelBuilder.Entity("Artemis.Service.School.Context.ArtemisClass", b =>
                {
                    b.HasOne("Artemis.Service.School.Context.ArtemisTeacher", "HeadTeacher")
                        .WithOne("HeadTeacherClass")
                        .HasForeignKey("Artemis.Service.School.Context.ArtemisClass", "HeadTeacherId")
                        .HasConstraintName("FK_ArtemisTeacher_ArtemisClass");

                    b.HasOne("Artemis.Service.School.Context.ArtemisSchool", "School")
                        .WithMany("Classes")
                        .HasForeignKey("SchoolId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_ArtemisClass_ArtemisSchool");

                    b.Navigation("HeadTeacher");

                    b.Navigation("School");
                });

            modelBuilder.Entity("Artemis.Service.School.Context.ArtemisStudent", b =>
                {
                    b.HasOne("Artemis.Service.School.Context.ArtemisClass", "Class")
                        .WithMany("Students")
                        .HasForeignKey("ClassId")
                        .HasConstraintName("FK_ArtemisClass_ArtemisStudent");

                    b.HasOne("Artemis.Service.School.Context.ArtemisSchool", "School")
                        .WithMany("Students")
                        .HasForeignKey("SchoolId")
                        .HasConstraintName("FK_ArtemisStudent_ArtemisSchool");

                    b.Navigation("Class");

                    b.Navigation("School");
                });

            modelBuilder.Entity("Artemis.Service.School.Context.ArtemisTeacher", b =>
                {
                    b.HasOne("Artemis.Service.School.Context.ArtemisSchool", "School")
                        .WithMany("Teachers")
                        .HasForeignKey("SchoolId")
                        .HasConstraintName("FK_ArtemisTeacher_ArtemisSchool");

                    b.Navigation("School");
                });

            modelBuilder.Entity("Artemis.Service.School.Context.ArtemisClass", b =>
                {
                    b.Navigation("Students");
                });

            modelBuilder.Entity("Artemis.Service.School.Context.ArtemisSchool", b =>
                {
                    b.Navigation("Classes");

                    b.Navigation("Students");

                    b.Navigation("Teachers");
                });

            modelBuilder.Entity("Artemis.Service.School.Context.ArtemisTeacher", b =>
                {
                    b.Navigation("HeadTeacherClass");
                });
#pragma warning restore 612, 618
        }
    }
}
