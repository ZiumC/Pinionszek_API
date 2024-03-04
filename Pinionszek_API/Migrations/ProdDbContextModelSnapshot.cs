﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Pinionszek_API.DbContexts;

#nullable disable

namespace Pinionszek_API.Migrations
{
    [DbContext(typeof(ProdDbContext))]
    partial class ProdDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Pinionszek_API.Models.DatabaseModel.Budget", b =>
                {
                    b.Property<int>("IdBudget")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdBudget"), 1L, 1);

                    b.Property<DateTime>("BudgetYear")
                        .HasColumnType("date");

                    b.Property<int>("IdBudgetStatus")
                        .HasColumnType("int");

                    b.Property<int>("IdUser")
                        .HasColumnType("int");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("OpendDate")
                        .HasColumnType("date");

                    b.Property<decimal>("Revenue")
                        .HasColumnType("money");

                    b.Property<decimal>("Surplus")
                        .HasColumnType("money");

                    b.HasKey("IdBudget");

                    b.HasIndex("IdBudgetStatus");

                    b.HasIndex("IdUser");

                    b.ToTable("Budgets");
                });

            modelBuilder.Entity("Pinionszek_API.Models.DatabaseModel.BudgetStatus", b =>
                {
                    b.Property<int>("IdBudgetStatus")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdBudgetStatus"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("IdBudgetStatus");

                    b.ToTable("BudgetStatuses");
                });

            modelBuilder.Entity("Pinionszek_API.Models.DatabaseModel.DetailedCategory", b =>
                {
                    b.Property<int>("IdDetailedCategory")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdDetailedCategory"), 1L, 1);

                    b.Property<int>("IdGeneralCategory")
                        .HasColumnType("int");

                    b.Property<int>("IdUser")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.HasKey("IdDetailedCategory");

                    b.HasIndex("IdGeneralCategory");

                    b.HasIndex("IdUser");

                    b.ToTable("DetailedCategories");
                });

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

            modelBuilder.Entity("Pinionszek_API.Models.DatabaseModel.GeneralCategory", b =>
                {
                    b.Property<int>("IdGeneralCategory")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdGeneralCategory"), 1L, 1);

                    b.Property<bool>("IsDefault")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.HasKey("IdGeneralCategory");

                    b.ToTable("GeneralCategories");
                });

            modelBuilder.Entity("Pinionszek_API.Models.DatabaseModel.Payment", b =>
                {
                    b.Property<int>("IdPayment")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdPayment"), 1L, 1);

                    b.Property<decimal>("Charge")
                        .HasColumnType("money");

                    b.Property<int>("DetailedCategoryIdDetailedCategory")
                        .HasColumnType("int");

                    b.Property<int>("IdBudget")
                        .HasColumnType("int");

                    b.Property<int>("IdDetailedCategory")
                        .HasColumnType("int");

                    b.Property<int>("IdPaymentStatus")
                        .HasColumnType("int");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasMaxLength(350)
                        .HasColumnType("nvarchar(350)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.Property<DateTime?>("PaymentDate")
                        .HasColumnType("date");

                    b.Property<decimal>("Refund")
                        .HasColumnType("money");

                    b.HasKey("IdPayment");

                    b.HasIndex("DetailedCategoryIdDetailedCategory");

                    b.HasIndex("IdBudget");

                    b.HasIndex("IdPaymentStatus");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("Pinionszek_API.Models.DatabaseModel.PaymentStatus", b =>
                {
                    b.Property<int>("IdPaymentStatus")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdPaymentStatus"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("IdPaymentStatus");

                    b.ToTable("PaymentStatuses");
                });

            modelBuilder.Entity("Pinionszek_API.Models.DatabaseModel.SharedPayment", b =>
                {
                    b.Property<int>("IdSharedPayment")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdSharedPayment"), 1L, 1);

                    b.Property<int>("IdFriend")
                        .HasColumnType("int");

                    b.Property<int>("IdPayment")
                        .HasColumnType("int");

                    b.HasKey("IdSharedPayment");

                    b.HasIndex("IdFriend");

                    b.HasIndex("IdPayment");

                    b.ToTable("SharedPayments");
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

                    b.HasData(
                        new
                        {
                            IdUser = 1,
                            Email = "test1@test.pl",
                            Login = "test1",
                            LoginAttempts = 0,
                            Password = "password1",
                            PasswordSalt = "passsalt",
                            RegisteredAt = new DateTime(2024, 3, 4, 15, 18, 58, 857, DateTimeKind.Local).AddTicks(6330),
                            UserTag = 1001
                        },
                        new
                        {
                            IdUser = 2,
                            Email = "test2@test.pl",
                            Login = "test2",
                            LoginAttempts = 0,
                            Password = "password2",
                            PasswordSalt = "passsalt",
                            RegisteredAt = new DateTime(2023, 2, 9, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            UserTag = 1002
                        },
                        new
                        {
                            IdUser = 3,
                            Email = "test3@test.pl",
                            Login = "test3",
                            LoginAttempts = 0,
                            Password = "password3",
                            PasswordSalt = "passsalt",
                            RegisteredAt = new DateTime(2023, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            UserTag = 1003
                        },
                        new
                        {
                            IdUser = 4,
                            Email = "test4@test.pl",
                            Login = "test4",
                            LoginAttempts = 0,
                            Password = "password4",
                            PasswordSalt = "passsalt",
                            RegisteredAt = new DateTime(2023, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            UserTag = 1004
                        });
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

            modelBuilder.Entity("Pinionszek_API.Models.DatabaseModel.Budget", b =>
                {
                    b.HasOne("Pinionszek_API.Models.DatabaseModel.BudgetStatus", "BudgetStatus")
                        .WithMany("Budget")
                        .HasForeignKey("IdBudgetStatus")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Pinionszek_API.Models.DatabaseModel.User", "User")
                        .WithMany("Budgets")
                        .HasForeignKey("IdUser")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BudgetStatus");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Pinionszek_API.Models.DatabaseModel.DetailedCategory", b =>
                {
                    b.HasOne("Pinionszek_API.Models.DatabaseModel.GeneralCategory", "GeneralCategory")
                        .WithMany()
                        .HasForeignKey("IdGeneralCategory")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Pinionszek_API.Models.DatabaseModel.User", "User")
                        .WithMany("UserCategories")
                        .HasForeignKey("IdUser")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GeneralCategory");

                    b.Navigation("User");
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

            modelBuilder.Entity("Pinionszek_API.Models.DatabaseModel.Payment", b =>
                {
                    b.HasOne("Pinionszek_API.Models.DatabaseModel.DetailedCategory", "DetailedCategory")
                        .WithMany("Payments")
                        .HasForeignKey("DetailedCategoryIdDetailedCategory")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Pinionszek_API.Models.DatabaseModel.Budget", "Budget")
                        .WithMany("Payments")
                        .HasForeignKey("IdBudget")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Pinionszek_API.Models.DatabaseModel.PaymentStatus", "PaymentStatus")
                        .WithMany("Payment")
                        .HasForeignKey("IdPaymentStatus")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Budget");

                    b.Navigation("DetailedCategory");

                    b.Navigation("PaymentStatus");
                });

            modelBuilder.Entity("Pinionszek_API.Models.DatabaseModel.SharedPayment", b =>
                {
                    b.HasOne("Pinionszek_API.Models.DatabaseModel.Friend", "Friend")
                        .WithMany("SharedPayments")
                        .HasForeignKey("IdFriend")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Pinionszek_API.Models.DatabaseModel.Payment", "Payment")
                        .WithMany("SharedPayments")
                        .HasForeignKey("IdPayment")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Friend");

                    b.Navigation("Payment");
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

            modelBuilder.Entity("Pinionszek_API.Models.DatabaseModel.Budget", b =>
                {
                    b.Navigation("Payments");
                });

            modelBuilder.Entity("Pinionszek_API.Models.DatabaseModel.BudgetStatus", b =>
                {
                    b.Navigation("Budget");
                });

            modelBuilder.Entity("Pinionszek_API.Models.DatabaseModel.DetailedCategory", b =>
                {
                    b.Navigation("Payments");
                });

            modelBuilder.Entity("Pinionszek_API.Models.DatabaseModel.Friend", b =>
                {
                    b.Navigation("SharedPayments");
                });

            modelBuilder.Entity("Pinionszek_API.Models.DatabaseModel.Payment", b =>
                {
                    b.Navigation("SharedPayments");
                });

            modelBuilder.Entity("Pinionszek_API.Models.DatabaseModel.PaymentStatus", b =>
                {
                    b.Navigation("Payment");
                });

            modelBuilder.Entity("Pinionszek_API.Models.DatabaseModel.User", b =>
                {
                    b.Navigation("Budgets");

                    b.Navigation("Friends");

                    b.Navigation("UserCategories");

                    b.Navigation("UserSettings")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
