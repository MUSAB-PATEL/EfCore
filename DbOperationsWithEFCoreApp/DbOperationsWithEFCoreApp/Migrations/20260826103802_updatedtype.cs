using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DbOperationsWithEFCoreApp.Migrations
{
    /// <inheritdoc />
    public partial class updatedtype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookPrices_CurrencyTypes_CurrencyTypeId",
                table: "BookPrices");

            migrationBuilder.DropTable(
                name: "CurrencyTypes");

            migrationBuilder.RenameColumn(
                name: "CurrencyTypeId",
                table: "BookPrices",
                newName: "CurrencyId");

            migrationBuilder.RenameIndex(
                name: "IX_BookPrices_CurrencyTypeId",
                table: "BookPrices",
                newName: "IX_BookPrices_CurrencyId");

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Description", "Title" },
                values: new object[,]
                {
                    { 1, "Indian Rupees", "INR" },
                    { 2, "United States Dollar", "USD" },
                    { 3, "Euro", "EUR" },
                    { 4, "British Pound", "GBP" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_BookPrices_Currencies_CurrencyId",
                table: "BookPrices",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookPrices_Currencies_CurrencyId",
                table: "BookPrices");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.RenameColumn(
                name: "CurrencyId",
                table: "BookPrices",
                newName: "CurrencyTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_BookPrices_CurrencyId",
                table: "BookPrices",
                newName: "IX_BookPrices_CurrencyTypeId");

            migrationBuilder.CreateTable(
                name: "CurrencyTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyTypes", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_BookPrices_CurrencyTypes_CurrencyTypeId",
                table: "BookPrices",
                column: "CurrencyTypeId",
                principalTable: "CurrencyTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
