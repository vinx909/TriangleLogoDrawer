using Microsoft.EntityFrameworkCore.Migrations;

namespace TriangleLogoDrawer.Infrastructure.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BackgroundImagePath = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Points",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageId = table.Column<int>(type: "int", nullable: false),
                    X = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Y = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Points", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Points_Images_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Shapes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shapes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shapes_Images_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Triangles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageId = table.Column<int>(type: "int", nullable: false),
                    PointOneId = table.Column<int>(type: "int", nullable: false),
                    PointTwoId = table.Column<int>(type: "int", nullable: false),
                    PointThreeId = table.Column<int>(type: "int", nullable: false),
                    PointId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Triangles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Triangles_Images_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Triangles_Points_PointId",
                        column: x => x.PointId,
                        principalTable: "Points",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Triangles_Points_PointOneId",
                        column: x => x.PointOneId,
                        principalTable: "Points",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Triangles_Points_PointThreeId",
                        column: x => x.PointThreeId,
                        principalTable: "Points",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Triangles_Points_PointTwoId",
                        column: x => x.PointTwoId,
                        principalTable: "Points",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    ShapeId = table.Column<int>(type: "int", nullable: false),
                    TriangleId = table.Column<int>(type: "int", nullable: false),
                    OrderNumber = table.Column<int>(type: "int", nullable: false),
                    TriangleId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => new { x.ShapeId, x.TriangleId });
                    table.ForeignKey(
                        name: "FK_Orders_Shapes_ShapeId",
                        column: x => x.ShapeId,
                        principalTable: "Shapes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Triangles_TriangleId",
                        column: x => x.TriangleId,
                        principalTable: "Triangles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Orders_Triangles_TriangleId1",
                        column: x => x.TriangleId1,
                        principalTable: "Triangles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_TriangleId",
                table: "Orders",
                column: "TriangleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_TriangleId1",
                table: "Orders",
                column: "TriangleId1",
                unique: true,
                filter: "[TriangleId1] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Points_ImageId",
                table: "Points",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Shapes_ImageId",
                table: "Shapes",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Triangles_ImageId",
                table: "Triangles",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Triangles_PointId",
                table: "Triangles",
                column: "PointId");

            migrationBuilder.CreateIndex(
                name: "IX_Triangles_PointOneId",
                table: "Triangles",
                column: "PointOneId");

            migrationBuilder.CreateIndex(
                name: "IX_Triangles_PointThreeId",
                table: "Triangles",
                column: "PointThreeId");

            migrationBuilder.CreateIndex(
                name: "IX_Triangles_PointTwoId",
                table: "Triangles",
                column: "PointTwoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Shapes");

            migrationBuilder.DropTable(
                name: "Triangles");

            migrationBuilder.DropTable(
                name: "Points");

            migrationBuilder.DropTable(
                name: "Images");
        }
    }
}
