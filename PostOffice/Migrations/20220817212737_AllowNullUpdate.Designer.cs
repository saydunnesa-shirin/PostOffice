﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PostOffice.Repository.Helpers;

#nullable disable

namespace PostOffice.Api.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20220817212737_AllowNullUpdate")]
    partial class AllowNullUpdate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("PostOffice.Repository.Entities.Bag", b =>
                {
                    b.Property<int>("BagId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BagId"), 1L, 1);

                    b.Property<string>("BagNumber")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<int>("ContentType")
                        .HasColumnType("int");

                    b.Property<int>("ItemCount")
                        .HasColumnType("int");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("ShipmentId")
                        .HasColumnType("int");

                    b.Property<decimal>("Weight")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("BagId");

                    b.HasIndex("ShipmentId");

                    b.ToTable("Bags");
                });

            modelBuilder.Entity("PostOffice.Repository.Entities.Parcel", b =>
                {
                    b.Property<int>("ParcelId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ParcelId"), 1L, 1);

                    b.Property<int?>("BagId")
                        .HasColumnType("int");

                    b.Property<string>("DestinationCountry")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("nvarchar(2)");

                    b.Property<string>("ParcelNumber")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("RecipientName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<decimal>("Weight")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("ParcelId");

                    b.HasIndex("BagId");

                    b.ToTable("Parcels");
                });

            modelBuilder.Entity("PostOffice.Repository.Entities.Shipment", b =>
                {
                    b.Property<int>("ShipmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ShipmentId"), 1L, 1);

                    b.Property<int>("Airport")
                        .HasColumnType("int");

                    b.Property<DateTime>("FlightDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FlightNumber")
                        .IsRequired()
                        .HasMaxLength(6)
                        .HasColumnType("nvarchar(6)");

                    b.Property<string>("ShipmentNumber")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("ShipmentId");

                    b.HasAlternateKey("ShipmentNumber")
                        .HasName("AlternateKey_ShipmentNumber");

                    b.ToTable("Shipments");
                });

            modelBuilder.Entity("PostOffice.Repository.Entities.Bag", b =>
                {
                    b.HasOne("PostOffice.Repository.Entities.Shipment", "Shipment")
                        .WithMany("Bags")
                        .HasForeignKey("ShipmentId");

                    b.Navigation("Shipment");
                });

            modelBuilder.Entity("PostOffice.Repository.Entities.Parcel", b =>
                {
                    b.HasOne("PostOffice.Repository.Entities.Bag", "Bag")
                        .WithMany("Parcels")
                        .HasForeignKey("BagId");

                    b.Navigation("Bag");
                });

            modelBuilder.Entity("PostOffice.Repository.Entities.Bag", b =>
                {
                    b.Navigation("Parcels");
                });

            modelBuilder.Entity("PostOffice.Repository.Entities.Shipment", b =>
                {
                    b.Navigation("Bags");
                });
#pragma warning restore 612, 618
        }
    }
}
