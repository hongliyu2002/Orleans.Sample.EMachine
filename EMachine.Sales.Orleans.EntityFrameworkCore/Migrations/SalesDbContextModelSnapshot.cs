﻿// <auto-generated />
using System;
using EMachine.Sales.Orleans.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EMachine.Sales.Orleans.EntityFrameworkCore.Migrations
{
    [DbContext(typeof(SalesDbContext))]
    partial class SalesDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EMachine.Sales.Orleans.States.Slot", b =>
                {
                    b.Property<Guid>("MachineId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Position")
                        .HasColumnType("int");

                    b.HasKey("MachineId", "Position");

                    b.ToTable("Slots", (string)null);
                });

            modelBuilder.Entity("EMachine.Sales.Orleans.States.Snack", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTimeOffset?>("DeletedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("DeletedBy")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LastModifiedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("LastModifiedBy")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("PictureUrl")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.HasKey("Id");

                    b.ToTable("Snacks", (string)null);
                });

            modelBuilder.Entity("EMachine.Sales.Orleans.States.SnackMachine", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("AmountInTransaction")
                        .HasPrecision(10, 2)
                        .HasColumnType("decimal(10,2)");

                    b.Property<DateTimeOffset?>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTimeOffset?>("DeletedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("DeletedBy")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LastModifiedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("LastModifiedBy")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("SnackMachines", (string)null);
                });

            modelBuilder.Entity("EMachine.Sales.Orleans.States.Slot", b =>
                {
                    b.HasOne("EMachine.Sales.Orleans.States.SnackMachine", null)
                        .WithMany("Slots")
                        .HasForeignKey("MachineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("EMachine.Sales.Orleans.States.SnackPile", "SnackPile", b1 =>
                        {
                            b1.Property<Guid>("SlotMachineId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int>("SlotPosition")
                                .HasColumnType("int");

                            b1.Property<decimal>("Price")
                                .HasPrecision(10, 2)
                                .HasColumnType("decimal(10,2)");

                            b1.Property<int>("Quantity")
                                .HasColumnType("int");

                            b1.Property<Guid>("SnackId")
                                .HasColumnType("uniqueidentifier");

                            b1.HasKey("SlotMachineId", "SlotPosition");

                            b1.HasIndex("SnackId");

                            b1.ToTable("Slots");

                            b1.HasOne("EMachine.Sales.Orleans.States.Snack", null)
                                .WithMany()
                                .HasForeignKey("SnackId")
                                .OnDelete(DeleteBehavior.Cascade)
                                .IsRequired();

                            b1.WithOwner()
                                .HasForeignKey("SlotMachineId", "SlotPosition");
                        });

                    b.Navigation("SnackPile");
                });

            modelBuilder.Entity("EMachine.Sales.Orleans.States.SnackMachine", b =>
                {
                    b.OwnsOne("EMachine.Sales.Orleans.States.Money", "MoneyInside", b1 =>
                        {
                            b1.Property<Guid>("SnackMachineId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int>("Yuan1")
                                .HasColumnType("int");

                            b1.Property<int>("Yuan10")
                                .HasColumnType("int");

                            b1.Property<int>("Yuan100")
                                .HasColumnType("int");

                            b1.Property<int>("Yuan2")
                                .HasColumnType("int");

                            b1.Property<int>("Yuan20")
                                .HasColumnType("int");

                            b1.Property<int>("Yuan5")
                                .HasColumnType("int");

                            b1.Property<int>("Yuan50")
                                .HasColumnType("int");

                            b1.HasKey("SnackMachineId");

                            b1.ToTable("SnackMachines");

                            b1.WithOwner()
                                .HasForeignKey("SnackMachineId");
                        });

                    b.Navigation("MoneyInside")
                        .IsRequired();
                });

            modelBuilder.Entity("EMachine.Sales.Orleans.States.SnackMachine", b =>
                {
                    b.Navigation("Slots");
                });
#pragma warning restore 612, 618
        }
    }
}
