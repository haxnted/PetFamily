using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SpeciesNewConfigurate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_breeds_species_species_fk_id",
                table: "breeds");

            migrationBuilder.AddForeignKey(
                name: "fk_breeds_species_species_fk_id",
                table: "breeds",
                column: "species_fk_id",
                principalTable: "species",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_breeds_species_species_fk_id",
                table: "breeds");

            migrationBuilder.AddForeignKey(
                name: "fk_breeds_species_species_fk_id",
                table: "breeds",
                column: "species_fk_id",
                principalTable: "species",
                principalColumn: "id");
        }
    }
}
