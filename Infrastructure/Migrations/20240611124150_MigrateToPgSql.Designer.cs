﻿// <auto-generated />
using System;
using Infrastructure.Tenancy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(TenantDbContext))]
    [Migration("20240611124150_MigrateToPgSql")]
    partial class MigrateToPgSql
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Infrastructure.Tenancy.ABCSchoolTenantInfo", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("AdminEmail")
                        .HasColumnType("text");

                    b.Property<string>("ConnectionString")
                        .HasColumnType("text");

                    b.Property<string>("Identifier")
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<DateTime>("ValidUpTo")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("Identifier")
                        .IsUnique();

                    b.ToTable("Tenants", "Multitenancy");
                });

            modelBuilder.Entity("Infrastructure.Tenancy.TenancySubscription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ABCSchoolTenantInfoId")
                        .HasColumnType("character varying(64)");

                    b.Property<int>("PeriodInMonths")
                        .HasColumnType("integer");

                    b.Property<int>("SubscriptionAmount")
                        .HasColumnType("integer");

                    b.Property<DateTime>("SubscriptionDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("TenantId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ABCSchoolTenantInfoId");

                    b.ToTable("TenancySubscriptions", "Multitenancy");
                });

            modelBuilder.Entity("Infrastructure.Tenancy.TenancySubscription", b =>
                {
                    b.HasOne("Infrastructure.Tenancy.ABCSchoolTenantInfo", "ABCSchoolTenantInfo")
                        .WithMany()
                        .HasForeignKey("ABCSchoolTenantInfoId");

                    b.Navigation("ABCSchoolTenantInfo");
                });
#pragma warning restore 612, 618
        }
    }
}