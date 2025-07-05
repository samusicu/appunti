﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiEfCodeFirst.Migrations
{
    /// <inheritdoc />
    public partial class updateuser2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Users",
                newName: "Username");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Users",
                newName: "Name");
        }
    }
}
