﻿// <auto-generated />
using BankApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace BankApi.Migrations
{
    [DbContext(typeof(GatawayDbContext))]
    partial class GatawayDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BankApi.Models.Card", b =>
                {
                    b.Property<int>("CardId");

                    b.Property<int>("Balance");

                    b.Property<string>("CVV")
                        .IsRequired();

                    b.Property<string>("CardNumber")
                        .IsRequired();

                    b.Property<string>("CardholderName");

                    b.Property<int>("ExpiryMonth");

                    b.Property<int>("ExpiryYear");

                    b.Property<bool>("IsLimited");

                    b.Property<int?>("LimitSum");

                    b.HasKey("CardId");

                    b.ToTable("Cards");
                });

            modelBuilder.Entity("BankApi.Models.Order", b =>
                {
                    b.Property<int>("OrderId");

                    b.Property<int?>("CardId");

                    b.Property<int>("OrderStatusId");

                    b.Property<int>("OrderSum");

                    b.HasKey("OrderId");

                    b.HasIndex("CardId");

                    b.HasIndex("OrderStatusId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("BankApi.Models.OrderStatus", b =>
                {
                    b.Property<int>("OrderStatusId");

                    b.Property<string>("StatusName")
                        .IsRequired();

                    b.HasKey("OrderStatusId");

                    b.ToTable("OrderStatuses");
                });

            modelBuilder.Entity("BankApi.Models.Order", b =>
                {
                    b.HasOne("BankApi.Models.Card", "Card")
                        .WithMany("Orders")
                        .HasForeignKey("CardId");

                    b.HasOne("BankApi.Models.OrderStatus", "OrderStatus")
                        .WithMany("Orders")
                        .HasForeignKey("OrderStatusId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
