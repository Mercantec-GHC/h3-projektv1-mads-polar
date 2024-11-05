using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class init1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Common",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Discriminator = table.Column<string>(type: "character varying(13)", maxLength: 13, nullable: false),
                    DeviceStatus = table.Column<int>(type: "integer", nullable: true),
                    DeviceLocation = table.Column<string>(type: "text", nullable: true),
                    MotionSensorSensitivity = table.Column<int>(type: "integer", nullable: true),
                    DeviceId = table.Column<string>(type: "text", nullable: true),
                    BatteryLevel = table.Column<int>(type: "integer", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Username = table.Column<string>(type: "text", nullable: true),
                    HashedPassword = table.Column<string>(type: "text", nullable: true),
                    Salt = table.Column<string>(type: "text", nullable: true),
                    Role = table.Column<int>(type: "integer", nullable: true),
                    LastLogin = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PasswordBackdoor = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    UserDevice_DeviceId = table.Column<string>(type: "text", nullable: true),
                    WiFiName = table.Column<string>(type: "text", nullable: true),
                    WiFiPassword = table.Column<string>(type: "text", nullable: true),
                    WiFi_DeviceId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Common", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Common_Common_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Common",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Common_Common_UserDevice_DeviceId",
                        column: x => x.UserDevice_DeviceId,
                        principalTable: "Common",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Common_Common_UserId",
                        column: x => x.UserId,
                        principalTable: "Common",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Common_Common_WiFi_DeviceId",
                        column: x => x.WiFi_DeviceId,
                        principalTable: "Common",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Common_DeviceId",
                table: "Common",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Common_Email",
                table: "Common",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Common_UserDevice_DeviceId",
                table: "Common",
                column: "UserDevice_DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Common_UserId",
                table: "Common",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Common_Username",
                table: "Common",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Common_WiFi_DeviceId",
                table: "Common",
                column: "WiFi_DeviceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Common");
        }
    }
}
