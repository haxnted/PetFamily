﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.Species.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Species_Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "species");

            migrationBuilder.CreateTable(
                name: "species",
                schema: "species",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    type_animal = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_species", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "breeds",
                schema: "species",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    breed = table.Column<string>(type: "text", nullable: false),
                    species_fk_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_breeds", x => x.id);
                    table.ForeignKey(
                        name: "fk_breeds_species_species_fk_id",
                        column: x => x.species_fk_id,
                        principalSchema: "species",
                        principalTable: "species",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_breeds_species_fk_id",
                schema: "species",
                table: "breeds",
                column: "species_fk_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "breeds",
                schema: "species");

            migrationBuilder.DropTable(
                name: "species",
                schema: "species");
        }
    }
}
