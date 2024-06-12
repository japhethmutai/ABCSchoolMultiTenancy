using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Migrators.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class InitialTenancyMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Multitenancy");

            migrationBuilder.CreateTable(
                name: "Tenants",
                schema: "Multitenancy",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Identifier = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    ConnectionString = table.Column<string>(type: "text", nullable: true),
                    AdminEmail = table.Column<string>(type: "text", nullable: true),
                    ValidUpTo = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TenancySubscriptions",
                schema: "Multitenancy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SubscriptionDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    SubscriptionAmount = table.Column<int>(type: "integer", nullable: false),
                    PeriodInMonths = table.Column<int>(type: "integer", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: true),
                    ABCSchoolTenantInfoId = table.Column<string>(type: "character varying(64)", nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_Identifier",
                schema: "Multitenancy",
                table: "Tenants",
                column: "Identifier",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TenancySubscriptions",
                schema: "Multitenancy");

            migrationBuilder.DropTable(
                name: "Tenants",
                schema: "Multitenancy");
        }
    }
}
