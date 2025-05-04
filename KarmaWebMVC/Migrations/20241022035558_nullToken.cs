using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KarmaWebMVC.Migrations
{
    /// <inheritdoc />
    public partial class nullToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "AspNetUsers",
                type: "nvarchar(700)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(700)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
