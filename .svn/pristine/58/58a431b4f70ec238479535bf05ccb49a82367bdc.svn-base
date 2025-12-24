using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppApi.DataAccess.Migrations.WebApi
{
    /// <inheritdoc />
    public partial class AddMenuItemOrderNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderNumber",
                table: "MenuItem",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderNumber",
                table: "MenuItem");
        }
    }
}
