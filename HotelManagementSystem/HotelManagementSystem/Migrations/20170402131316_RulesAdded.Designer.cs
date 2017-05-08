using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using HotelManagementSystem.Models.Infrastructure;

namespace HotelManagementSystem.Migrations
{
    [DbContext(typeof(IdentityContext))]
    [Migration("20170402131316_RulesAdded")]
    partial class RulesAdded
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("HotelManagementSystem.Models.Entities.Storage.Rule", b =>
                {
                    b.Property<Guid>("RuleID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("RuleID");

                    b.ToTable("Rules");
                });
        }
    }
}
