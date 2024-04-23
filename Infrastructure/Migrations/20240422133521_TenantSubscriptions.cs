using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TenantSubscriptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TenancySubscriptions",
                schema: "Multitenancy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubscriptionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubscriptionAmount = table.Column<int>(type: "int", nullable: false),
                    PeriodInMonths = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ABCSchoolTenantInfoId = table.Column<string>(type: "nvarchar(64)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenancySubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TenancySubscriptions_Tenants_ABCSchoolTenantInfoId",
                        column: x => x.ABCSchoolTenantInfoId,
                        principalSchema: "Multitenancy",
                        principalTable: "Tenants",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TenancySubscriptions_ABCSchoolTenantInfoId",
                schema: "Multitenancy",
                table: "TenancySubscriptions",
                column: "ABCSchoolTenantInfoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TenancySubscriptions",
                schema: "Multitenancy");
        }
    }
}
