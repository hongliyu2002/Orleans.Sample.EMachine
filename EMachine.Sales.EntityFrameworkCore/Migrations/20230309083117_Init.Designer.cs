﻿// <auto-generated />
using System;
using EMachine.Sales.EntityFrameworkCore.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EMachine.Sales.EntityFrameworkCore.Migrations
{
    [DbContext(typeof(SalesDbContext))]
    [Migration("20230309083117_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.3");

            modelBuilder.Entity("EMachine.Sales.Domain.Slot", b =>
                {
                    b.Property<Guid>("MachineId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Position")
                        .HasColumnType("INTEGER");

                    b.HasKey("MachineId", "Position");

                    b.ToTable("Slots", (string)null);
                });

            modelBuilder.Entity("EMachine.Sales.Domain.Snack", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(100)
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
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<long>("Version")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Snacks", (string)null);
                });

            modelBuilder.Entity("EMachine.Sales.Domain.SnackMachine", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("AmountInTransaction")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(100)
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
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<long>("Version")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("SnackMachines", (string)null);
                });

            modelBuilder.Entity("EMachine.Sales.Domain.Slot", b =>
                {
                    b.HasOne("EMachine.Sales.Domain.SnackMachine", null)
                        .WithMany("Slots")
                        .HasForeignKey("MachineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("EMachine.Sales.Domain.SnackPile", "SnackPile", b1 =>
                        {
                            b1.Property<Guid>("SlotMachineId")
                                .HasColumnType("TEXT");

                            b1.Property<int>("SlotPosition")
                                .HasColumnType("INTEGER");

                            b1.Property<decimal>("Price")
                                .HasColumnType("TEXT");

                            b1.Property<int>("Quantity")
                                .HasColumnType("INTEGER");

                            b1.Property<Guid>("SnackId")
                                .HasColumnType("TEXT");

                            b1.HasKey("SlotMachineId", "SlotPosition");

                            b1.HasIndex("SnackId");

                            b1.ToTable("Slots");

                            b1.HasOne("EMachine.Sales.Domain.Snack", "Snack")
                                .WithMany()
                                .HasForeignKey("SnackId")
                                .OnDelete(DeleteBehavior.Cascade)
                                .IsRequired();

                            b1.WithOwner()
                                .HasForeignKey("SlotMachineId", "SlotPosition");

                            b1.Navigation("Snack");
                        });

                    b.Navigation("SnackPile");
                });

            modelBuilder.Entity("EMachine.Sales.Domain.SnackMachine", b =>
                {
                    b.OwnsOne("EMachine.Sales.Domain.Money", "MoneyInside", b1 =>
                        {
                            b1.Property<Guid>("SnackMachineId")
                                .HasColumnType("TEXT");

                            b1.Property<decimal>("Amount")
                                .HasColumnType("TEXT");

                            b1.Property<int>("Yuan1")
                                .HasColumnType("INTEGER");

                            b1.Property<int>("Yuan10")
                                .HasColumnType("INTEGER");

                            b1.Property<int>("Yuan100")
                                .HasColumnType("INTEGER");

                            b1.Property<int>("Yuan2")
                                .HasColumnType("INTEGER");

                            b1.Property<int>("Yuan20")
                                .HasColumnType("INTEGER");

                            b1.Property<int>("Yuan5")
                                .HasColumnType("INTEGER");

                            b1.Property<int>("Yuan50")
                                .HasColumnType("INTEGER");

                            b1.HasKey("SnackMachineId");

                            b1.ToTable("SnackMachines");

                            b1.WithOwner()
                                .HasForeignKey("SnackMachineId");
                        });

                    b.Navigation("MoneyInside")
                        .IsRequired();
                });

            modelBuilder.Entity("EMachine.Sales.Domain.SnackMachine", b =>
                {
                    b.Navigation("Slots");
                });
#pragma warning restore 612, 618
        }
    }
}