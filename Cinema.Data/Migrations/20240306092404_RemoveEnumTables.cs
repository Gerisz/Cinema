using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Cinema.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveEnumTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seats_Statuses_StatusId",
                table: "Seats");

            migrationBuilder.DropTable(
                name: "Statuses");

            migrationBuilder.DropIndex(
                name: "IX_Seats_StatusId",
                table: "Seats");

            migrationBuilder.RenameColumn(
                name: "StatusId",
                table: "Seats",
                newName: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Seats",
                newName: "StatusId");

            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Statuses",
                columns: new[] { "Id", "Value" },
                values: new object[,]
                {
                    { 1, "Free" },
                    { 2, "Reserved" },
                    { 3, "Sold" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Seats_StatusId",
                table: "Seats",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_Statuses_StatusId",
                table: "Seats",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
