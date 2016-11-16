using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using GitGud.Models;

namespace GitGud.Migrations
{
    [DbContext(typeof(GitGudContext))]
    partial class GitGudContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("GitGud.Models.Song", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ArtistName");

                    b.Property<string>("Name");

                    b.Property<string>("UploaderName");

                    b.HasKey("Id");

                    b.ToTable("Songs");
                });

            modelBuilder.Entity("GitGud.Models.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<int?>("SongId");

                    b.HasKey("Id");

                    b.HasIndex("SongId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("GitGud.Models.Tag", b =>
                {
                    b.HasOne("GitGud.Models.Song")
                        .WithMany("Tags")
                        .HasForeignKey("SongId");
                });
        }
    }
}
