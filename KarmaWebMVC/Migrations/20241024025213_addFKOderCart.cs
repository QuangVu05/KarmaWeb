using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KarmaWebMVC.Migrations
{
    /// <inheritdoc />
    public partial class addFKOderCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Tạo bảng Cart
            migrationBuilder.CreateTable(
                name: "Cart",
                columns: table => new
                {
                    CartId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"), // Khóa chính
                    CartName = table.Column<string>(nullable: true),
                    CartImage = table.Column<string>(nullable: true),
                    CartPrice = table.Column<decimal>(nullable: false),
                    CartQuantity = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true) // Khóa ngoại đến AspNetUsers
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cart", x => x.CartId);

                    // Thiết lập khóa ngoại với AspNetUsers
                    table.ForeignKey(
                        name: "FK_Cart_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
            // Thêm cột UserId vào bảng Order
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Order",
                nullable: true);

            // Tạo khóa ngoại từ bảng Order đến bảng AspNetUsers
            migrationBuilder.AddForeignKey(
                name: "FK_Order_AspNetUsers_UserId",
                table: "Order",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            // Tạo chỉ mục (index) cho cột UserId để tối ưu hóa truy vấn
            migrationBuilder.CreateIndex(
                name: "IX_Order_UserId",
                table: "Order",
                column: "UserId");
            // Tạo index cho cột UserId trong bảng Cart để tối ưu hóa truy vấn
            migrationBuilder.CreateIndex(
                name: "IX_Cart_UserId",
                table: "Cart",
                column: "UserId");
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Xóa bảng Cart khi rollback
            migrationBuilder.DropTable(
                name: "Cart");
            // Xóa chỉ mục cho cột UserId
            migrationBuilder.DropIndex(
                name: "IX_Order_UserId",
                table: "Order");

            // Xóa khóa ngoại
            migrationBuilder.DropForeignKey(
                name: "FK_Order_AspNetUsers_UserId",
                table: "Order");

            // Xóa cột UserId
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Order");
        }
    }
}
