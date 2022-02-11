﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using database;

#nullable disable

namespace database.Migrations
{
    [DbContext(typeof(QuoteSourceDbContext))]
    [Migration("20220211190258_FixBugWithFK")]
    partial class FixBugWithFK
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("database.entity.Market", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Market");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Forex"
                        },
                        new
                        {
                            Id = 2,
                            Name = "UsaStock"
                        },
                        new
                        {
                            Id = 3,
                            Name = "MmvbStock"
                        },
                        new
                        {
                            Id = 4,
                            Name = "CryptoCurrency"
                        });
                });

            modelBuilder.Entity("database.entity.Quote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CandleStart")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("Close")
                        .HasColumnType("numeric(15,9)");

                    b.Property<decimal>("High")
                        .HasColumnType("numeric(15,9)");

                    b.Property<decimal>("Low")
                        .HasColumnType("numeric(15,9)");

                    b.Property<decimal>("Open")
                        .HasColumnType("numeric(15,9)");

                    b.Property<int>("StockTimeFrameId")
                        .HasColumnType("integer");

                    b.Property<decimal>("Volume")
                        .HasColumnType("numeric(19,9)");

                    b.HasKey("Id");

                    b.HasIndex("StockTimeFrameId", "CandleStart")
                        .IsUnique();

                    b.ToTable("Quote");
                });

            modelBuilder.Entity("database.entity.Stock", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("FinamId")
                        .HasColumnType("integer");

                    b.Property<int>("MarketId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("MarketId", "Code");

                    b.ToTable("Stock");
                });

            modelBuilder.Entity("database.entity.StockTimeFrame", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("LoadedFrom")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("LoadedTill")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("StockId")
                        .HasColumnType("integer");

                    b.Property<int>("TimeFrameId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("TimeFrameId");

                    b.HasIndex("StockId", "TimeFrameId")
                        .IsUnique();

                    b.ToTable("StockTimeFrame");
                });

            modelBuilder.Entity("database.entity.TimeFrame", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("TimeFrame");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Minute"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Minute5"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Minute10"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Minute15"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Minute30"
                        },
                        new
                        {
                            Id = 6,
                            Name = "Hour"
                        },
                        new
                        {
                            Id = 8,
                            Name = "Daily"
                        },
                        new
                        {
                            Id = 9,
                            Name = "Weekly"
                        },
                        new
                        {
                            Id = 10,
                            Name = "Monthly"
                        });
                });

            modelBuilder.Entity("database.entity.Quote", b =>
                {
                    b.HasOne("database.entity.StockTimeFrame", "StockTimeFrame")
                        .WithMany("Quotes")
                        .HasForeignKey("StockTimeFrameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("StockTimeFrame");
                });

            modelBuilder.Entity("database.entity.Stock", b =>
                {
                    b.HasOne("database.entity.Market", "Market")
                        .WithMany("Stocks")
                        .HasForeignKey("MarketId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Market");
                });

            modelBuilder.Entity("database.entity.StockTimeFrame", b =>
                {
                    b.HasOne("database.entity.Stock", "Stock")
                        .WithMany("StockTimeFrames")
                        .HasForeignKey("StockId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("database.entity.TimeFrame", "TimeFrame")
                        .WithMany("StockTimeFrames")
                        .HasForeignKey("TimeFrameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Stock");

                    b.Navigation("TimeFrame");
                });

            modelBuilder.Entity("database.entity.Market", b =>
                {
                    b.Navigation("Stocks");
                });

            modelBuilder.Entity("database.entity.Stock", b =>
                {
                    b.Navigation("StockTimeFrames");
                });

            modelBuilder.Entity("database.entity.StockTimeFrame", b =>
                {
                    b.Navigation("Quotes");
                });

            modelBuilder.Entity("database.entity.TimeFrame", b =>
                {
                    b.Navigation("StockTimeFrames");
                });
#pragma warning restore 612, 618
        }
    }
}
