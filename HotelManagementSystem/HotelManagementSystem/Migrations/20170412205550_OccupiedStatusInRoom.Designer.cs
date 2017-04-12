using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using HotelManagementSystem.Models.Infrastructure;

namespace HotelManagementSystem.Migrations
{
    [DbContext(typeof(StorageContext))]
    [Migration("20170412205550_OccupiedStatusInRoom")]
    partial class OccupiedStatusInRoom
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("HotelManagementSystem.Models.Entities.Storage.Room", b =>
                {
                    b.Property<Guid>("RoomID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("GuestFirstName");

                    b.Property<string>("GuestLastName");

                    b.Property<string>("Number");

                    b.Property<bool>("Occupied");

                    b.HasKey("RoomID");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("HotelManagementSystem.Models.Entities.Storage.Rule", b =>
                {
                    b.Property<Guid>("RuleID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("RuleID");

                    b.ToTable("Rules");
                });

            modelBuilder.Entity("HotelManagementSystem.Models.Entities.Storage.Task", b =>
                {
                    b.Property<Guid>("TaskID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Describe");

                    b.Property<Guid>("RoomID");

                    b.HasKey("TaskID");

                    b.HasIndex("RoomID");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("HotelManagementSystem.Models.Entities.Storage.Task", b =>
                {
                    b.HasOne("HotelManagementSystem.Models.Entities.Storage.Room", "Room")
                        .WithMany()
                        .HasForeignKey("RoomID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
