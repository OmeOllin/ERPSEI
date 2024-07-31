using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPSEI.Data.Migrations
{
    /// <inheritdoc />
    public partial class modificacionHorariosAsistencia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Asistencias",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "HorarioId",
                table: "Asistencias",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Horarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    NombreHorario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Entrada = table.Column<TimeSpan>(type: "time", nullable: false),
                    ToleranciaEntrada = table.Column<TimeSpan>(type: "time", nullable: false),
                    ToleranciaFalta = table.Column<TimeSpan>(type: "time", nullable: false),
                    Salida = table.Column<TimeSpan>(type: "time", nullable: false),
                    ToleranciaSalida = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Horarios", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Horarios");

            migrationBuilder.DropColumn(
                name: "HorarioId",
                table: "Asistencias");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Asistencias",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
