﻿// <auto-generated />
using System;
using FinanceControl.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FinanceControl.Migrations
{
    [DbContext(typeof(DbRepositoryContext))]
    [Migration("20190322095027_AddInfo_Rev1")]
    partial class AddInfo_Rev1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.8-servicing-32085")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("FinanceControl.Models.Account", b =>
                {
                    b.Property<long>("AccountId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AccountName");

                    b.Property<bool>("ActiveAccount");

                    b.Property<decimal>("Balance");

                    b.Property<long>("CurrencyId");

                    b.Property<string>("Description");

                    b.Property<long>("ItemId");

                    b.Property<int>("Sequence");

                    b.Property<decimal>("StartAmount");

                    b.Property<long>("UserId");

                    b.HasKey("AccountId");

                    b.HasIndex("CurrencyId");

                    b.HasIndex("ItemId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("FinanceControl.Models.Comment", b =>
                {
                    b.Property<long>("CommentId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CommentText");

                    b.Property<long>("TransactionId");

                    b.Property<long>("UserId");

                    b.HasKey("CommentId");

                    b.HasIndex("TransactionId")
                        .IsUnique();

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("FinanceControl.Models.Currency", b =>
                {
                    b.Property<long>("CurrencyId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code");

                    b.Property<string>("Description");

                    b.Property<long>("UserId");

                    b.HasKey("CurrencyId");

                    b.ToTable("Currencies");
                });

            modelBuilder.Entity("FinanceControl.Models.Group", b =>
                {
                    b.Property<long>("GroupId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Comment");

                    b.Property<string>("Name");

                    b.Property<int>("Type");

                    b.Property<long>("UserId");

                    b.HasKey("GroupId");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("FinanceControl.Models.Helper", b =>
                {
                    b.Property<int>("HelperId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Answer");

                    b.Property<string>("Question");

                    b.Property<int>("Target");

                    b.HasKey("HelperId")
                        .HasName("PK_Helpers");

                    b.ToTable("Helpers","help");
                });

            modelBuilder.Entity("FinanceControl.Models.Info", b =>
                {
                    b.Property<long>("InfoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("infoId")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Header")
                        .HasColumnName("header");

                    b.Property<string>("Text")
                        .HasColumnName("text");

                    b.Property<string>("Title")
                        .HasColumnName("title");

                    b.HasKey("InfoId")
                        .HasName("PK_Infos");

                    b.ToTable("Infos","help");
                });

            modelBuilder.Entity("FinanceControl.Models.Item", b =>
                {
                    b.Property<long>("ItemId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("GroupId");

                    b.Property<string>("Name");

                    b.Property<long>("UserId");

                    b.HasKey("ItemId");

                    b.HasIndex("GroupId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("FinanceControl.Models.Transaction", b =>
                {
                    b.Property<long>("TransactionId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("AccountBalance");

                    b.Property<long>("AccountId");

                    b.Property<decimal>("CurrencyAmount");

                    b.Property<DateTime>("DateTime");

                    b.Property<long>("ItemId");

                    b.Property<decimal>("RateToAccCurr");

                    b.Property<long>("UserId");

                    b.HasKey("TransactionId");

                    b.HasIndex("AccountId");

                    b.HasIndex("ItemId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("FinanceControl.Models.Account", b =>
                {
                    b.HasOne("FinanceControl.Models.Currency", "Currency")
                        .WithMany("Accounts")
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FinanceControl.Models.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FinanceControl.Models.Comment", b =>
                {
                    b.HasOne("FinanceControl.Models.Transaction")
                        .WithOne("Comment")
                        .HasForeignKey("FinanceControl.Models.Comment", "TransactionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FinanceControl.Models.Item", b =>
                {
                    b.HasOne("FinanceControl.Models.Group", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FinanceControl.Models.Transaction", b =>
                {
                    b.HasOne("FinanceControl.Models.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FinanceControl.Models.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}