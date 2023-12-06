﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PPE.Data;

#nullable disable

namespace PPE.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20231205172456_addProjectIdAndFonctionId")]
    partial class addProjectIdAndFonctionId
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PPE.Models.Attribute", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Attribute");
                });

            modelBuilder.Entity("PPE.Models.AttributeValue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AttributeId")
                        .HasColumnType("int");

                    b.Property<int>("PpeId")
                        .HasColumnType("int");

                    b.Property<int>("ValueId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AttributeId");

                    b.HasIndex("PpeId");

                    b.HasIndex("ValueId");

                    b.ToTable("AttributeValue");
                });

            modelBuilder.Entity("PPE.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("PPE.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("FunctionId")
                        .HasColumnType("int");

                    b.Property<int?>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NNI")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<string>("ShoeSize")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Size")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Tel")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FunctionId");

                    b.HasIndex("ProjectId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("PPE.Models.EmployeeStock", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<int>("FunctionId")
                        .HasColumnType("int");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<string>("Remarks")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("StockIn")
                        .HasColumnType("int");

                    b.Property<int>("StockOut")
                        .HasColumnType("int");

                    b.Property<int>("VariantValueId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("FunctionId");

                    b.HasIndex("ProjectId");

                    b.HasIndex("VariantValueId");

                    b.ToTable("EmployeeStocks");
                });

            modelBuilder.Entity("PPE.Models.Function", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Functions");
                });

            modelBuilder.Entity("PPE.Models.Manager", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Fonction")
                        .HasColumnType("int");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("Managers");
                });

            modelBuilder.Entity("PPE.Models.Ppe", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Threshold")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Ppes");
                });

            modelBuilder.Entity("PPE.Models.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Prefix")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("PPE.Models.Stock", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<int>("StockIn")
                        .HasColumnType("int");

                    b.Property<int>("StockOut")
                        .HasColumnType("int");

                    b.Property<int>("VariantValueId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.HasIndex("VariantValueId");

                    b.ToTable("Stocks");
                });

            modelBuilder.Entity("PPE.Models.StockDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("StockId")
                        .HasColumnType("int");

                    b.Property<int>("Total")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StockId");

                    b.ToTable("StockDetails");
                });

            modelBuilder.Entity("PPE.Models.Value", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Values");
                });

            modelBuilder.Entity("PPE.Models.Variant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("PpeId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PpeId");

                    b.ToTable("Variants");
                });

            modelBuilder.Entity("PPE.Models.VariantValue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("VariantId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("VariantId");

                    b.ToTable("VariantValues");
                });

            modelBuilder.Entity("PPE.Models.Attribute", b =>
                {
                    b.HasOne("PPE.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("PPE.Models.AttributeValue", b =>
                {
                    b.HasOne("PPE.Models.Attribute", "Attribute")
                        .WithMany("AttributeValues")
                        .HasForeignKey("AttributeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PPE.Models.Ppe", "Ppe")
                        .WithMany()
                        .HasForeignKey("PpeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PPE.Models.Value", "Value")
                        .WithMany("AttributeValues")
                        .HasForeignKey("ValueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Attribute");

                    b.Navigation("Ppe");

                    b.Navigation("Value");
                });

            modelBuilder.Entity("PPE.Models.Employee", b =>
                {
                    b.HasOne("PPE.Models.Function", "Function")
                        .WithMany("Employees")
                        .HasForeignKey("FunctionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PPE.Models.Project", "Project")
                        .WithMany("Employees")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Function");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("PPE.Models.EmployeeStock", b =>
                {
                    b.HasOne("PPE.Models.Employee", "Employee")
                        .WithMany("EmployeeStocks")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PPE.Models.Function", "Function")
                        .WithMany()
                        .HasForeignKey("FunctionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PPE.Models.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PPE.Models.VariantValue", "VariantValue")
                        .WithMany()
                        .HasForeignKey("VariantValueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("Function");

                    b.Navigation("Project");

                    b.Navigation("VariantValue");
                });

            modelBuilder.Entity("PPE.Models.Manager", b =>
                {
                    b.HasOne("PPE.Models.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("PPE.Models.Ppe", b =>
                {
                    b.HasOne("PPE.Models.Category", "Category")
                        .WithMany("Ppes")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("PPE.Models.Stock", b =>
                {
                    b.HasOne("PPE.Models.Project", "Project")
                        .WithMany("Stocks")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PPE.Models.VariantValue", "VariantValue")
                        .WithMany("Stocks")
                        .HasForeignKey("VariantValueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");

                    b.Navigation("VariantValue");
                });

            modelBuilder.Entity("PPE.Models.StockDetail", b =>
                {
                    b.HasOne("PPE.Models.Stock", "Stock")
                        .WithMany()
                        .HasForeignKey("StockId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Stock");
                });

            modelBuilder.Entity("PPE.Models.Variant", b =>
                {
                    b.HasOne("PPE.Models.Ppe", "Ppe")
                        .WithMany("Variants")
                        .HasForeignKey("PpeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ppe");
                });

            modelBuilder.Entity("PPE.Models.VariantValue", b =>
                {
                    b.HasOne("PPE.Models.Variant", "Variant")
                        .WithMany("VariantValues")
                        .HasForeignKey("VariantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Variant");
                });

            modelBuilder.Entity("PPE.Models.Attribute", b =>
                {
                    b.Navigation("AttributeValues");
                });

            modelBuilder.Entity("PPE.Models.Category", b =>
                {
                    b.Navigation("Ppes");
                });

            modelBuilder.Entity("PPE.Models.Employee", b =>
                {
                    b.Navigation("EmployeeStocks");
                });

            modelBuilder.Entity("PPE.Models.Function", b =>
                {
                    b.Navigation("Employees");
                });

            modelBuilder.Entity("PPE.Models.Ppe", b =>
                {
                    b.Navigation("Variants");
                });

            modelBuilder.Entity("PPE.Models.Project", b =>
                {
                    b.Navigation("Employees");

                    b.Navigation("Stocks");
                });

            modelBuilder.Entity("PPE.Models.Value", b =>
                {
                    b.Navigation("AttributeValues");
                });

            modelBuilder.Entity("PPE.Models.Variant", b =>
                {
                    b.Navigation("VariantValues");
                });

            modelBuilder.Entity("PPE.Models.VariantValue", b =>
                {
                    b.Navigation("Stocks");
                });
#pragma warning restore 612, 618
        }
    }
}
