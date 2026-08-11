using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CineBook.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MOVIE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TITLE = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    DESCRIPTION = table.Column<string>(type: "text", nullable: true),
                    DURATION = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MOVIE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "THEATER",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LOCATION = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_THEATER", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "USER",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EMAIL = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PASSWORD = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    NAMA = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SCHEDULE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MOVIE_ID = table.Column<long>(type: "bigint", nullable: false),
                    THEATER_ID = table.Column<long>(type: "bigint", nullable: false),
                    START_TIME = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PRICE = table.Column<decimal>(type: "numeric(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SCHEDULE", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SCHEDULE_MOVIE_MOVIE_ID",
                        column: x => x.MOVIE_ID,
                        principalTable: "MOVIE",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SCHEDULE_THEATER_THEATER_ID",
                        column: x => x.THEATER_ID,
                        principalTable: "THEATER",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BOOKING",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    USER_ID = table.Column<long>(type: "bigint", nullable: false),
                    SCHEDULE_ID = table.Column<long>(type: "bigint", nullable: false),
                    TOTAL_TICKETS = table.Column<int>(type: "integer", nullable: false),
                    TOTAL_PRICE = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    BOOKING_STATUS = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BOOKING", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BOOKING_SCHEDULE_SCHEDULE_ID",
                        column: x => x.SCHEDULE_ID,
                        principalTable: "SCHEDULE",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BOOKING_USER_USER_ID",
                        column: x => x.USER_ID,
                        principalTable: "USER",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SEAT",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SCHEDULE_ID = table.Column<long>(type: "bigint", nullable: false),
                    SEAT_NUMBER = table.Column<string>(type: "text", nullable: false),
                    IS_AVAILABLE = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEAT", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SEAT_SCHEDULE_SCHEDULE_ID",
                        column: x => x.SCHEDULE_ID,
                        principalTable: "SCHEDULE",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BOOKING_SEAT",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BOOKING_ID = table.Column<long>(type: "bigint", nullable: false),
                    SEAT_ID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BOOKING_SEAT", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BOOKING_SEAT_BOOKING_BOOKING_ID",
                        column: x => x.BOOKING_ID,
                        principalTable: "BOOKING",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BOOKING_SEAT_SEAT_SEAT_ID",
                        column: x => x.SEAT_ID,
                        principalTable: "SEAT",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BOOKING_SCHEDULE_ID",
                table: "BOOKING",
                column: "SCHEDULE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_BOOKING_USER_ID",
                table: "BOOKING",
                column: "USER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_BOOKING_SEAT_BOOKING_ID",
                table: "BOOKING_SEAT",
                column: "BOOKING_ID");

            migrationBuilder.CreateIndex(
                name: "IX_BOOKING_SEAT_SEAT_ID",
                table: "BOOKING_SEAT",
                column: "SEAT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_SCHEDULE_MOVIE_ID",
                table: "SCHEDULE",
                column: "MOVIE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_SCHEDULE_THEATER_ID",
                table: "SCHEDULE",
                column: "THEATER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_SEAT_SCHEDULE_ID",
                table: "SEAT",
                column: "SCHEDULE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_USER_EMAIL",
                table: "USER",
                column: "EMAIL",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BOOKING_SEAT");

            migrationBuilder.DropTable(
                name: "BOOKING");

            migrationBuilder.DropTable(
                name: "SEAT");

            migrationBuilder.DropTable(
                name: "USER");

            migrationBuilder.DropTable(
                name: "SCHEDULE");

            migrationBuilder.DropTable(
                name: "MOVIE");

            migrationBuilder.DropTable(
                name: "THEATER");
        }
    }
}
