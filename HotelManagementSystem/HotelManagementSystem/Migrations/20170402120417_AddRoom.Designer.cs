using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using HotelManagementSystem.Models.Infrastructure;

namespace HotelManagementSystem.Migrations
{
    [DbContext(typeof(IdentityContext))]
    [Migration("20170402120417_AddRoom")]
    partial class AddRoom
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

                    b.HasKey("RoomID");

                    b.ToTable("Rooms");
                });
        }
    }
}
