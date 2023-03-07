﻿// <auto-generated />
using System;
using EMachine.Sales.EntityFrameworkCore.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EMachine.Sales.EntityFrameworkCore.Migrations
{
    [DbContext(typeof(SalesDbContext))]
    partial class SalesDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.3");

            modelBuilder.Entity("EMachine.Sales.Domain.Entities.SlotBase", b =>
                {
                    b.Property<Guid>("MachineId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Position")
                        .HasColumnType("INTEGER");

                    b.Property<long>("ID")
                        .HasColumnType("INTEGER");

                    b.Property<long>("SnackId")
                        .HasColumnType("INTEGER");

                    b.HasKey("MachineId", "Position");

                    b.HasIndex("SnackId");

                    b.ToTable("Slots", (string)null);
                });

            modelBuilder.Entity("EMachine.Sales.Domain.Entities.SnackBase", b =>
                {
                    b.Property<long>("ID")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("DeletedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LastModifiedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Snacks", (string)null);
                });

            modelBuilder.Entity("EMachine.Sales.Domain.Entities.SnackMachineBase", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("AmountInTransaction")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("DeletedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("TEXT");

                    b.Property<int>("InsideYuan1")
                        .HasColumnType("INTEGER");

                    b.Property<int>("InsideYuan10")
                        .HasColumnType("INTEGER");

                    b.Property<int>("InsideYuan100")
                        .HasColumnType("INTEGER");

                    b.Property<int>("InsideYuan2")
                        .HasColumnType("INTEGER");

                    b.Property<int>("InsideYuan20")
                        .HasColumnType("INTEGER");

                    b.Property<int>("InsideYuan5")
                        .HasColumnType("INTEGER");

                    b.Property<int>("InsideYuan50")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LastModifiedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("SnackMachines", (string)null);
                });

            modelBuilder.Entity("EMachine.Sales.Domain.Entities.SlotBase", b =>
                {
                    b.HasOne("EMachine.Sales.Domain.Entities.SnackMachineBase", null)
                        .WithMany("Slots")
                        .HasForeignKey("MachineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EMachine.Sales.Domain.Entities.SnackBase", null)
                        .WithMany()
                        .HasForeignKey("SnackId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EMachine.Sales.Domain.Entities.SnackMachineBase", b =>
                {
                    b.Navigation("Slots");
                });
#pragma warning restore 612, 618
        }
    }
}
