﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Pinionszek_API.DbContexts;

#nullable disable

namespace Pinionszek_API.Migrations
{
    [DbContext(typeof(ProdDbContext))]
    [Migration("20240302154037_CreatedUserSettingsTable")]
    partial class CreatedUserSettingsTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Pinionszek_API.Models.DatabaseModel.Friend", b =>
                {
                    b.Property<int>("IdFriend")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdFriend"), 1L, 1);

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("date");

                    b.Property<int>("FriendTag")
                        .HasColumnType("int");

                    b.Property<int>("IdUser")
                        .HasColumnType("int");

                    b.HasKey("IdFriend");

                    b.HasIndex("IdUser");

                    b.ToTable("Friends");
                });

            modelBuilder.Entity("Pinionszek_API.Models.DatabaseModel.User", b =>
                {
                    b.Property<int>("IdUser")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdUser"), 1L, 1);

                    b.Property<DateTime?>("BlockedTo")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("LoginAttempts")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(75)
                        .HasColumnType("nvarchar(75)");

                    b.Property<string>("PasswordSalt")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("RefreshToken")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime>("RegisteredAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserTag")
                        .HasColumnType("int");

                    b.HasKey("IdUser");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Pinionszek_API.Models.DatabaseModel.UserSettings", b =>
                {
                    b.Property<int>("IdUserSetting")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdUserSetting"), 1L, 1);

                    b.Property<bool>("DisplayBudgetRules")
                        .HasColumnType("bit");

                    b.Property<int>("IdUser")
                        .HasColumnType("int");

                    b.Property<decimal>("Needs")
                        .HasColumnType("money");

                    b.Property<decimal>("Savings")
                        .HasColumnType("money");

                    b.Property<bool>("UseBudgetRules")
                        .HasColumnType("bit");

                    b.Property<decimal>("Wants")
                        .HasColumnType("money");

                    b.HasKey("IdUserSetting");

                    b.HasIndex("IdUser")
                        .IsUnique();

                    b.ToTable("UserSettings");
                });

            modelBuilder.Entity("Pinionszek_API.Models.DatabaseModel.Friend", b =>
                {
                    b.HasOne("Pinionszek_API.Models.DatabaseModel.User", "User")
                        .WithMany("Friends")
                        .HasForeignKey("IdUser")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Pinionszek_API.Models.DatabaseModel.UserSettings", b =>
                {
                    b.HasOne("Pinionszek_API.Models.DatabaseModel.User", "User")
                        .WithOne("UserSettings")
                        .HasForeignKey("Pinionszek_API.Models.DatabaseModel.UserSettings", "IdUser")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Pinionszek_API.Models.DatabaseModel.User", b =>
                {
                    b.Navigation("Friends");

                    b.Navigation("UserSettings")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
