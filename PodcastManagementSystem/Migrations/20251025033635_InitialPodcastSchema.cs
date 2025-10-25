﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PodcastManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class InitialPodcastSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Podcasts",
                columns: table => new
                {
                    PodcastID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatorID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Podcasts", x => x.PodcastID);
                });

            migrationBuilder.CreateTable(
                name: "Episodes",
                columns: table => new
                {
                    EpisodeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PodcastID = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    PlayCount = table.Column<int>(type: "int", nullable: false),
                    AudioFileURL = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    VideoFileURL = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Topic = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Host = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Episodes", x => x.EpisodeID);
                    table.ForeignKey(
                        name: "FK_Episodes_Podcasts_PodcastID",
                        column: x => x.PodcastID,
                        principalTable: "Podcasts",
                        principalColumn: "PodcastID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    SubscriptionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PodcastID = table.Column<int>(type: "int", nullable: false),
                    SubscribedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.SubscriptionID);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Podcasts_PodcastID",
                        column: x => x.PodcastID,
                        principalTable: "Podcasts",
                        principalColumn: "PodcastID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Episodes_PodcastID",
                table: "Episodes",
                column: "PodcastID");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_PodcastID",
                table: "Subscriptions",
                column: "PodcastID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Episodes");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "Podcasts");
        }
    }
}
