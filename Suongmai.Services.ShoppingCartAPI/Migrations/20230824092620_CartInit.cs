using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Suongmai.Services.ShoppingCartAPI.Migrations
{
    /// <inheritdoc />
    public partial class CartInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarHeaders",
                columns: table => new
                {
                    CartHeaderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CouponCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarHeaders", x => x.CartHeaderId);
                });

            migrationBuilder.CreateTable(
                name: "CarDeatails",
                columns: table => new
                {
                    CartDetailsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CartHeaderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarDeatails", x => x.CartDetailsId);
                    table.ForeignKey(
                        name: "FK_CarDeatails_CarHeaders_CartHeaderId",
                        column: x => x.CartHeaderId,
                        principalTable: "CarHeaders",
                        principalColumn: "CartHeaderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarDeatails_CartHeaderId",
                table: "CarDeatails",
                column: "CartHeaderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarDeatails");

            migrationBuilder.DropTable(
                name: "CarHeaders");
        }
    }
}
