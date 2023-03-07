using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EMachine.Sales.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SnackMachines",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    InsideYuan1 = table.Column<int>(type: "INTEGER", nullable: false),
                    InsideYuan2 = table.Column<int>(type: "INTEGER", nullable: false),
                    InsideYuan5 = table.Column<int>(type: "INTEGER", nullable: false),
                    InsideYuan10 = table.Column<int>(type: "INTEGER", nullable: false),
                    InsideYuan20 = table.Column<int>(type: "INTEGER", nullable: false),
                    InsideYuan50 = table.Column<int>(type: "INTEGER", nullable: false),
                    InsideYuan100 = table.Column<int>(type: "INTEGER", nullable: false),
                    AmountInTransaction = table.Column<decimal>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    DeletedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "TEXT", nullable: true),
                    DeletedBy = table.Column<string>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SnackMachines", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Snacks",
                columns: table => new
                {
                    ID = table.Column<long>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    DeletedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "TEXT", nullable: true),
                    DeletedBy = table.Column<string>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Snacks", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Slots",
                columns: table => new
                {
                    MachineId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Position = table.Column<int>(type: "INTEGER", nullable: false),
                    SnackId = table.Column<long>(type: "INTEGER", nullable: false),
                    ID = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Slots", x => new { x.MachineId, x.Position });
                    table.ForeignKey(
                        name: "FK_Slots_SnackMachines_MachineId",
                        column: x => x.MachineId,
                        principalTable: "SnackMachines",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Slots_Snacks_SnackId",
                        column: x => x.SnackId,
                        principalTable: "Snacks",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Slots_SnackId",
                table: "Slots",
                column: "SnackId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Slots");

            migrationBuilder.DropTable(
                name: "SnackMachines");

            migrationBuilder.DropTable(
                name: "Snacks");
        }
    }
}
