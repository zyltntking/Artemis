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
    [Migration("20240710043017_InitialSchool")]
    partial class InitialSchool
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("School")
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Artemis.Service.School.Context.ArtemisClass", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasComment("标识");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasComment("班级编码");

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

                    b.Property<DateTime>("EstablishTime")
                        .HasColumnType("timestamp with time zone")
                        .HasComment("班级创建时间");

                    b.Property<Guid>("HeadTeacherId")
                        .HasColumnType("uuid")
                        .HasComment("班主任标识");

                    b.Property<int>("Length")
                        .HasColumnType("integer")
                        .HasComment("学制长度");

                    b.Property<string>("Major")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasComment("所学专业");

                    b.Property<Guid>("ModifyBy")
                        .HasColumnType("UUID")
                        .HasComment("更新者标识");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasComment("班级名称");

                    b.Property<int>("Partition")
                        .HasColumnType("integer")
                        .HasComment("分区标识");

                    b.Property<Guid>("RemoveBy")
                        .HasColumnType("UUID")
                        .HasComment("删除者标识");

                    b.Property<Guid>("SchoolId")
                        .HasColumnType("uuid")
                        .HasComment("学校标识");

                    b.Property<string>("SchoolLength")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasComment("学制");

                    b.Property<int>("SerialNumber")
                        .HasColumnType("integer")
                        .HasComment("班级序号");

                    b.Property<string>("StudyPhase")
                        .IsRequired()
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

                    b.HasIndex("CreateBy")
                        .HasDatabaseName("IX_ArtemisClass_CreateBy");

                    b.HasIndex("CreatedAt")
                        .HasDatabaseName("IX_ArtemisClass_CreatedAt");

                    b.HasIndex("DeletedAt")
                        .HasDatabaseName("IX_ArtemisClass_DeletedAt");

                    b.HasIndex("HeadTeacherId")
                        .IsUnique();

                    b.HasIndex("ModifyBy")
                        .HasDatabaseName("IX_ArtemisClass_ModifyBy");

                    b.HasIndex("Partition")
                        .HasDatabaseName("IX_ArtemisClass_Partition");

                    b.HasIndex("RemoveBy")
                        .HasDatabaseName("IX_ArtemisClass_RemoveBy");

                    b.HasIndex("SchoolId");

                    b.HasIndex("UpdatedAt")
                        .HasDatabaseName("IX_ArtemisClass_UpdatedAt");

                    b.ToTable("ArtemisClass", "School", t =>
                        {
                            t.HasComment("班级数据集");
                        });
                });

            modelBuilder.Entity("Artemis.Service.School.Context.ArtemisClassStudent", b =>
                {
                    b.Property<Guid>("ClassId")
                        .HasColumnType("uuid")
                        .HasComment("班级标识");

                    b.Property<Guid>("StudentId")
                        .HasColumnType("uuid")
                        .HasComment("学生标识");

                    b.Property<DateTime>("MoveIn")
                        .HasColumnType("TIMESTAMP")
                        .HasComment("转入时间");

                    b.Property<DateTime?>("MoveOut")
                        .HasColumnType("TIMESTAMP")
                        .HasComment("转出时间");

                    b.HasKey("ClassId", "StudentId", "MoveIn")
                        .HasName("PK_ArtemisClassStudent");

                    b.HasIndex("MoveIn")
                        .HasDatabaseName("IX_ArtemisClassStudent_MoveIn");

                    b.HasIndex("MoveOut")
                        .HasDatabaseName("IX_ArtemisClassStudent_MoveOut");

                    b.HasIndex("StudentId");

                    b.ToTable("ArtemisClassStudent", "School", t =>
                        {
                            t.HasComment("班级学生映射数据集");
                        });
                });

            modelBuilder.Entity("Artemis.Service.School.Context.ArtemisClassTeacher", b =>
                {
                    b.Property<Guid>("ClassId")
                        .HasColumnType("uuid")
                        .HasComment("班级标识");

                    b.Property<Guid>("TeacherId")
                        .HasColumnType("uuid")
                        .HasComment("教师标识");

                    b.Property<DateTime>("MoveIn")
                        .HasColumnType("TIMESTAMP")
                        .HasComment("转入时间");

                    b.Property<DateTime?>("MoveOut")
                        .HasColumnType("TIMESTAMP")
                        .HasComment("转出时间");

                    b.HasKey("ClassId", "TeacherId", "MoveIn")
                        .HasName("PK_ArtemisClassTeacher");

                    b.HasIndex("MoveIn")
                        .HasDatabaseName("IX_ArtemisClassTeacher_MoveIn");

                    b.HasIndex("MoveOut")
                        .HasDatabaseName("IX_ArtemisClassTeacher_MoveOut");

                    b.HasIndex("TeacherId");

                    b.ToTable("ArtemisClassTeacher", "School", t =>
                        {
                            t.HasComment("班级教师映射数据集");
                        });
                });

            modelBuilder.Entity("Artemis.Service.School.Context.ArtemisSchool", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasComment("标识");

                    b.Property<string>("Address")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasComment("学校地址");

                    b.Property<string>("Code")
                        .IsRequired()
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

                    b.Property<Guid>("CreateBy")
                        .HasColumnType("UUID")
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
                        .HasColumnType("timestamp with time zone")
                        .HasComment("学校建立时间");

                    b.Property<Guid>("ModifyBy")
                        .HasColumnType("UUID")
                        .HasComment("更新者标识");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasComment("学校名称");

                    b.Property<string>("OrganizationCode")
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasComment("组织机构代码");

                    b.Property<int>("Partition")
                        .HasColumnType("integer")
                        .HasComment("分区标识");

                    b.Property<Guid>("RemoveBy")
                        .HasColumnType("UUID")
                        .HasComment("删除者标识");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
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

                    b.HasIndex("CreateBy")
                        .HasDatabaseName("IX_ArtemisSchool_CreateBy");

                    b.HasIndex("CreatedAt")
                        .HasDatabaseName("IX_ArtemisSchool_CreatedAt");

                    b.HasIndex("DeletedAt")
                        .HasDatabaseName("IX_ArtemisSchool_DeletedAt");

                    b.HasIndex("ModifyBy")
                        .HasDatabaseName("IX_ArtemisSchool_ModifyBy");

                    b.HasIndex("Partition")
                        .HasDatabaseName("IX_ArtemisSchool_Partition");

                    b.HasIndex("RemoveBy")
                        .HasDatabaseName("IX_ArtemisSchool_RemoveBy");

                    b.HasIndex("UpdatedAt")
                        .HasDatabaseName("IX_ArtemisSchool_UpdatedAt");

                    b.ToTable("ArtemisSchool", "School", t =>
                        {
                            t.HasComment("学校数据集");
                        });
                });

            modelBuilder.Entity("Artemis.Service.School.Context.ArtemisSchoolStudent", b =>
                {
                    b.Property<Guid>("SchoolId")
                        .HasColumnType("uuid")
                        .HasComment("学校标识");

                    b.Property<Guid>("StudentId")
                        .HasColumnType("uuid")
                        .HasComment("学生标识");

                    b.Property<DateTime>("MoveIn")
                        .HasColumnType("TIMESTAMP")
                        .HasComment("转入时间");

                    b.Property<DateTime?>("MoveOut")
                        .HasColumnType("TIMESTAMP")
                        .HasComment("转出时间");

                    b.HasKey("SchoolId", "StudentId", "MoveIn")
                        .HasName("PK_ArtemisSchoolStudent");

                    b.HasIndex("MoveIn")
                        .HasDatabaseName("IX_ArtemisSchoolStudent_MoveIn");

                    b.HasIndex("MoveOut")
                        .HasDatabaseName("IX_ArtemisSchoolStudent_MoveOut");

                    b.HasIndex("StudentId");

                    b.ToTable("ArtemisSchoolStudent", "School", t =>
                        {
                            t.HasComment("学校学生映射数据集");
                        });
                });

            modelBuilder.Entity("Artemis.Service.School.Context.ArtemisSchoolTeacher", b =>
                {
                    b.Property<Guid>("SchoolId")
                        .HasColumnType("uuid")
                        .HasComment("学校标识");

                    b.Property<Guid>("TeacherId")
                        .HasColumnType("uuid")
                        .HasComment("教师标识");

                    b.Property<DateTime>("MoveIn")
                        .HasColumnType("TIMESTAMP")
                        .HasComment("转入时间");

                    b.Property<DateTime?>("MoveOut")
                        .HasColumnType("TIMESTAMP")
                        .HasComment("转出时间");

                    b.HasKey("SchoolId", "TeacherId", "MoveIn")
                        .HasName("PK_ArtemisSchoolTeacher");

                    b.HasIndex("MoveIn")
                        .HasDatabaseName("IX_ArtemisSchoolTeacher_MoveIn");

                    b.HasIndex("MoveOut")
                        .HasDatabaseName("IX_ArtemisSchoolTeacher_MoveOut");

                    b.HasIndex("TeacherId");

                    b.ToTable("ArtemisSchoolTeacher", "School", t =>
                        {
                            t.HasComment("学校教师映射数据集");
                        });
                });

            modelBuilder.Entity("Artemis.Service.School.Context.ArtemisStudent", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasComment("标识");

                    b.Property<string>("Address")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasComment("住址");

                    b.Property<DateOnly>("Birthday")
                        .HasColumnType("date")
                        .HasComment("学生生日");

                    b.Property<string>("Cert")
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasComment("证件号码");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasComment("学生编码");

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

                    b.Property<string>("DivisionCode")
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasComment("住址区划代码");

                    b.Property<DateTime>("EnrollmentDate")
                        .HasColumnType("timestamp with time zone")
                        .HasComment("入学时间");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("character varying(8)")
                        .HasComment("学生性别");

                    b.Property<string>("Major")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasComment("专业");

                    b.Property<Guid>("ModifyBy")
                        .HasColumnType("UUID")
                        .HasComment("更新者标识");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasComment("学生名称");

                    b.Property<string>("Nation")
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasComment("民族");

                    b.Property<int>("Partition")
                        .HasColumnType("integer")
                        .HasComment("分区标识");

                    b.Property<Guid>("RemoveBy")
                        .HasColumnType("UUID")
                        .HasComment("删除者标识");

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

                    b.HasIndex("CreateBy")
                        .HasDatabaseName("IX_ArtemisStudent_CreateBy");

                    b.HasIndex("CreatedAt")
                        .HasDatabaseName("IX_ArtemisStudent_CreatedAt");

                    b.HasIndex("DeletedAt")
                        .HasDatabaseName("IX_ArtemisStudent_DeletedAt");

                    b.HasIndex("ModifyBy")
                        .HasDatabaseName("IX_ArtemisStudent_ModifyBy");

                    b.HasIndex("Partition")
                        .HasDatabaseName("IX_ArtemisStudent_Partition");

                    b.HasIndex("RemoveBy")
                        .HasDatabaseName("IX_ArtemisStudent_RemoveBy");

                    b.HasIndex("UpdatedAt")
                        .HasDatabaseName("IX_ArtemisStudent_UpdatedAt");

                    b.ToTable("ArtemisStudent", "School", t =>
                        {
                            t.HasComment("学生数据集");
                        });
                });

            modelBuilder.Entity("Artemis.Service.School.Context.ArtemisStudentCurrentAffiliation", b =>
                {
                    b.Property<Guid>("StudentId")
                        .HasColumnType("uuid")
                        .HasComment("学生标识");

                    b.Property<Guid>("ClassId")
                        .HasColumnType("uuid")
                        .HasComment("班级标识");

                    b.Property<Guid>("SchoolId")
                        .HasColumnType("uuid")
                        .HasComment("学校标识");

                    b.HasKey("StudentId")
                        .HasName("PK_ArtemisStudentCurrentAffiliation");

                    b.HasIndex("ClassId");

                    b.HasIndex("SchoolId");

                    b.ToTable("ArtemisStudentCurrentAffiliation", "School", t =>
                        {
                            t.HasComment("学生当前所属关系数据集");
                        });
                });

            modelBuilder.Entity("Artemis.Service.School.Context.ArtemisTeacher", b =>
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

                    b.Property<string>("TeacherCode")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasComment("教师编码");

                    b.Property<string>("TeacherName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasComment("教师名称");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TIMESTAMP")
                        .HasComment("更新时间");

                    b.HasKey("Id")
                        .HasName("PK_ArtemisTeacher");

                    b.HasIndex("CreateBy")
                        .HasDatabaseName("IX_ArtemisTeacher_CreateBy");

                    b.HasIndex("CreatedAt")
                        .HasDatabaseName("IX_ArtemisTeacher_CreatedAt");

                    b.HasIndex("DeletedAt")
                        .HasDatabaseName("IX_ArtemisTeacher_DeletedAt");

                    b.HasIndex("ModifyBy")
                        .HasDatabaseName("IX_ArtemisTeacher_ModifyBy");

                    b.HasIndex("Partition")
                        .HasDatabaseName("IX_ArtemisTeacher_Partition");

                    b.HasIndex("RemoveBy")
                        .HasDatabaseName("IX_ArtemisTeacher_RemoveBy");

                    b.HasIndex("UpdatedAt")
                        .HasDatabaseName("IX_ArtemisTeacher_UpdatedAt");

                    b.ToTable("ArtemisTeacher", "School", t =>
                        {
                            t.HasComment("教师数据集");
                        });
                });

            modelBuilder.Entity("Artemis.Service.School.Context.ArtemisTeacherCurrentAffiliation", b =>
                {
                    b.Property<Guid>("SchoolId")
                        .HasColumnType("uuid")
                        .HasComment("学校标识");

                    b.Property<Guid>("ClassId")
                        .HasColumnType("uuid")
                        .HasComment("班级标识");

                    b.Property<Guid>("TeacherId")
                        .HasColumnType("uuid")
                        .HasComment("教师标识");

                    b.HasKey("SchoolId", "ClassId", "TeacherId")
                        .HasName("PK_ArtemisTeacherCurrentAffiliation");

                    b.HasIndex("ClassId");

                    b.HasIndex("TeacherId");

                    b.ToTable("ArtemisTeacherCurrentAffiliation", "School", t =>
                        {
                            t.HasComment("教师当前所属关系数据集");
                        });
                });

            modelBuilder.Entity("Artemis.Service.School.Context.ArtemisClass", b =>
                {
                    b.HasOne("Artemis.Service.School.Context.ArtemisTeacher", "HeadTeacher")
                        .WithOne("HeadTeacherClass")
                        .HasForeignKey("Artemis.Service.School.Context.ArtemisClass", "HeadTeacherId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired()
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

            modelBuilder.Entity("Artemis.Service.School.Context.ArtemisClassStudent", b =>
                {
                    b.HasOne("Artemis.Service.School.Context.ArtemisClass", "Class")
                        .WithMany("ClassStudents")
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_ArtemisClassStudent_ArtemisClass");

                    b.HasOne("Artemis.Service.School.Context.ArtemisStudent", "Student")
                        .WithMany("ClassStudents")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_ArtemisClassStudent_ArtemisStudent");

                    b.Navigation("Class");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Artemis.Service.School.Context.ArtemisClassTeacher", b =>
                {
                    b.HasOne("Artemis.Service.School.Context.ArtemisClass", "Class")
                        .WithMany("ClassTeachers")
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_ArtemisClassTeacher_ArtemisClass");

                    b.HasOne("Artemis.Service.School.Context.ArtemisTeacher", "Teacher")
                        .WithMany("ClassTeachers")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_ArtemisClassTeacher_ArtemisTeacher");

                    b.Navigation("Class");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("Artemis.Service.School.Context.ArtemisSchoolStudent", b =>
                {
                    b.HasOne("Artemis.Service.School.Context.ArtemisSchool", "School")
                        .WithMany("SchoolStudents")
                        .HasForeignKey("SchoolId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_ArtemisSchoolStudent_ArtemisSchool");

                    b.HasOne("Artemis.Service.School.Context.ArtemisStudent", "Student")
                        .WithMany("SchoolStudents")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_ArtemisSchoolStudent_ArtemisStudent");

                    b.Navigation("School");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Artemis.Service.School.Context.ArtemisSchoolTeacher", b =>
                {
                    b.HasOne("Artemis.Service.School.Context.ArtemisSchool", "School")
                        .WithMany("SchoolTeachers")
                        .HasForeignKey("SchoolId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_ArtemisSchoolTeacher_ArtemisSchool");

                    b.HasOne("Artemis.Service.School.Context.ArtemisTeacher", "Teacher")
                        .WithMany("SchoolTeachers")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_ArtemisSchoolTeacher_ArtemisTeacher");

                    b.Navigation("School");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("Artemis.Service.School.Context.ArtemisStudentCurrentAffiliation", b =>
                {
                    b.HasOne("Artemis.Service.School.Context.ArtemisClass", "Class")
                        .WithMany("CurrentStudents")
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_ArtemisStudentCurrentAffiliation_ArtemisClass");

                    b.HasOne("Artemis.Service.School.Context.ArtemisSchool", "School")
                        .WithMany("CurrentStudents")
                        .HasForeignKey("SchoolId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_ArtemisStudentCurrentAffiliation_ArtemisSchool");

                    b.HasOne("Artemis.Service.School.Context.ArtemisStudent", "Student")
                        .WithOne("CurrentAffiliation")
                        .HasForeignKey("Artemis.Service.School.Context.ArtemisStudentCurrentAffiliation", "StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_ArtemisStudentCurrentAffiliation_ArtemisStudent");

                    b.Navigation("Class");

                    b.Navigation("School");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Artemis.Service.School.Context.ArtemisTeacherCurrentAffiliation", b =>
                {
                    b.HasOne("Artemis.Service.School.Context.ArtemisClass", "Class")
                        .WithMany("CurrentTeachers")
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_ArtemisTeacherCurrentAffiliation_ArtemisClass");

                    b.HasOne("Artemis.Service.School.Context.ArtemisSchool", "School")
                        .WithMany("CurrentTeachers")
                        .HasForeignKey("SchoolId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_ArtemisTeacherCurrentAffiliation_ArtemisSchool");

                    b.HasOne("Artemis.Service.School.Context.ArtemisTeacher", "Teacher")
                        .WithMany("CurrentAffiliations")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_ArtemisTeacherCurrentAffiliation_ArtemisTeacher");

                    b.Navigation("Class");

                    b.Navigation("School");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("Artemis.Service.School.Context.ArtemisClass", b =>
                {
                    b.Navigation("ClassStudents");

                    b.Navigation("ClassTeachers");

                    b.Navigation("CurrentStudents");

                    b.Navigation("CurrentTeachers");
                });

            modelBuilder.Entity("Artemis.Service.School.Context.ArtemisSchool", b =>
                {
                    b.Navigation("Classes");

                    b.Navigation("CurrentStudents");

                    b.Navigation("CurrentTeachers");

                    b.Navigation("SchoolStudents");

                    b.Navigation("SchoolTeachers");
                });

            modelBuilder.Entity("Artemis.Service.School.Context.ArtemisStudent", b =>
                {
                    b.Navigation("ClassStudents");

                    b.Navigation("CurrentAffiliation")
                        .IsRequired();

                    b.Navigation("SchoolStudents");
                });

            modelBuilder.Entity("Artemis.Service.School.Context.ArtemisTeacher", b =>
                {
                    b.Navigation("ClassTeachers");

                    b.Navigation("CurrentAffiliations");

                    b.Navigation("HeadTeacherClass");

                    b.Navigation("SchoolTeachers");
                });
#pragma warning restore 612, 618
        }
    }
}