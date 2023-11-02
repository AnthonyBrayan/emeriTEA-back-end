using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Administrador",
                columns: table => new
                {
                    Id_Administrador = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name_administrador = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administrador", x => x.Id_Administrador);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id_Category = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name_category = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id_Category);
                });

            migrationBuilder.CreateTable(
                name: "Guest",
                columns: table => new
                {
                    Id_guest = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name_guest = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guest", x => x.Id_guest);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id_Product = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name_product = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    stock = table.Column<int>(type: "int", nullable: false),
                    Id_Category = table.Column<int>(type: "int", nullable: false),
                    Id_Administrador = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id_Product);
                    table.ForeignKey(
                        name: "FK_Product_Administrador_Id_Administrador",
                        column: x => x.Id_Administrador,
                        principalTable: "Administrador",
                        principalColumn: "Id_Administrador",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Product_Category_Id_Category",
                        column: x => x.Id_Category,
                        principalTable: "Category",
                        principalColumn: "Id_Category",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GuestCart",
                columns: table => new
                {
                    Id_guestCart = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price_product = table.Column<double>(type: "float", nullable: false),
                    Quantity_product = table.Column<int>(type: "int", nullable: false),
                    Total_price = table.Column<double>(type: "float", nullable: false),
                    Id_guest = table.Column<int>(type: "int", nullable: false),
                    Id_Product = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuestCart", x => x.Id_guestCart);
                    table.ForeignKey(
                        name: "FK_GuestCart_Guest_Id_guest",
                        column: x => x.Id_guest,
                        principalTable: "Guest",
                        principalColumn: "Id_guest",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GuestCart_Product_Id_Product",
                        column: x => x.Id_Product,
                        principalTable: "Product",
                        principalColumn: "Id_Product",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GuestCart_Id_guest",
                table: "GuestCart",
                column: "Id_guest");

            migrationBuilder.CreateIndex(
                name: "IX_GuestCart_Id_Product",
                table: "GuestCart",
                column: "Id_Product");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Id_Administrador",
                table: "Product",
                column: "Id_Administrador");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Id_Category",
                table: "Product",
                column: "Id_Category");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GuestCart");

            migrationBuilder.DropTable(
                name: "Guest");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Administrador");

            migrationBuilder.DropTable(
                name: "Category");
        }
    }
}
