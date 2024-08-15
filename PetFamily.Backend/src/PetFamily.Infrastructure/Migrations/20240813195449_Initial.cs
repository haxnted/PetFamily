using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "volunteers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    surname = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    patronymic = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    general_description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    age_experience = table.Column<string>(type: "text", nullable: false),
                    phone_number = table.Column<string>(type: "text", nullable: false),
                    pets_adopted_count = table.Column<int>(type: "integer", nullable: false),
                    pets_found_home_quantity = table.Column<int>(type: "integer", nullable: false),
                    pets_under_treatment_count = table.Column<int>(type: "integer", nullable: false),
                    requisites = table.Column<string>(type: "jsonb", nullable: true),
                    social_links = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_volunteers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "pets",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    nick_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    type_animal = table.Column<int>(type: "integer", nullable: false),
                    general_description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    breed = table.Column<string>(type: "text", nullable: false),
                    color = table.Column<string>(type: "text", nullable: false),
                    pet_health_information = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    address = table.Column<string>(type: "text", nullable: false),
                    weight = table.Column<double>(type: "double precision", nullable: false),
                    height = table.Column<double>(type: "double precision", nullable: false),
                    phone_number = table.Column<string>(type: "text", nullable: false),
                    birth_date = table.Column<DateOnly>(type: "date", nullable: false),
                    is_castrated = table.Column<bool>(type: "boolean", nullable: false),
                    is_vaccinated = table.Column<bool>(type: "boolean", nullable: false),
                    date_created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    volunteer_id = table.Column<Guid>(type: "uuid", nullable: true),
                    requisites = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pets", x => x.id);
                    table.ForeignKey(
                        name: "fk_pets_volunteers_volunteer_id",
                        column: x => x.volunteer_id,
                        principalTable: "volunteers",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "pet_photo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    pet_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pet_photo", x => x.id);
                    table.ForeignKey(
                        name: "fk_pet_photo_pets_pet_id",
                        column: x => x.pet_id,
                        principalTable: "pets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_pet_photo_pet_id",
                table: "pet_photo",
                column: "pet_id");

            migrationBuilder.CreateIndex(
                name: "ix_pets_volunteer_id",
                table: "pets",
                column: "volunteer_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "pet_photo");

            migrationBuilder.DropTable(
                name: "pets");

            migrationBuilder.DropTable(
                name: "volunteers");
        }
    }
}
