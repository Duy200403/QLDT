using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppApi.DataAccess.Migrations.Auth
{
    /// <inheritdoc />
    public partial class AddParentIdRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderNumber",
                table: "Role",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "Role",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderNumber",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Role");
        }
    }
}
