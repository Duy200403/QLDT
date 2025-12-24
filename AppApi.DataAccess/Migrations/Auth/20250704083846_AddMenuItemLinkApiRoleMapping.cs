using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppApi.DataAccess.Migrations.Auth
{
    /// <inheritdoc />
    public partial class AddMenuItemLinkApiRoleMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "MenuItem",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ApiRoleMappingMenuItem",
                columns: table => new
                {
                    ApiRoleMappingsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MenuItemsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiRoleMappingMenuItem", x => new { x.ApiRoleMappingsId, x.MenuItemsId });
                    table.ForeignKey(
                        name: "FK_ApiRoleMappingMenuItem_ApiRoleMapping_ApiRoleMappingsId",
                        column: x => x.ApiRoleMappingsId,
                        principalTable: "ApiRoleMapping",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApiRoleMappingMenuItem_MenuItem_MenuItemsId",
                        column: x => x.MenuItemsId,
                        principalTable: "MenuItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApiRoleMappingMenuItem_MenuItemsId",
                table: "ApiRoleMappingMenuItem",
                column: "MenuItemsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiRoleMappingMenuItem");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "MenuItem");
        }
    }
}
