using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPSEI.Data.Migrations
{
    /// <inheritdoc />
    public partial class modificacionesAsistencias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Faltas",
                table: "Asistencias");

            migrationBuilder.DropColumn(
                name: "HoraEntrada",
                table: "Asistencias");

            migrationBuilder.DropColumn(
                name: "Retardo",
                table: "Asistencias");

            migrationBuilder.RenameColumn(
                name: "Total",
                table: "Asistencias",
                newName: "NoTarjeta");

            migrationBuilder.RenameColumn(
                name: "Nombre",
                table: "Asistencias",
                newName: "SerialDispositivo");

            migrationBuilder.RenameColumn(
                name: "HoraSalida",
                table: "Asistencias",
                newName: "Hora");

            migrationBuilder.AddColumn<string>(
                name: "Direccion",
                table: "Asistencias",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaHora",
                table: "Asistencias",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NombreDispositivo",
                table: "Asistencias",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NombreEmpleado",
                table: "Asistencias",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Direccion",
                table: "Asistencias");

            migrationBuilder.DropColumn(
                name: "FechaHora",
                table: "Asistencias");

            migrationBuilder.DropColumn(
                name: "NombreDispositivo",
                table: "Asistencias");

            migrationBuilder.DropColumn(
                name: "NombreEmpleado",
                table: "Asistencias");

            migrationBuilder.RenameColumn(
                name: "SerialDispositivo",
                table: "Asistencias",
                newName: "Nombre");

            migrationBuilder.RenameColumn(
                name: "NoTarjeta",
                table: "Asistencias",
                newName: "Total");

            migrationBuilder.RenameColumn(
                name: "Hora",
                table: "Asistencias",
                newName: "HoraSalida");

            migrationBuilder.AddColumn<int>(
                name: "Faltas",
                table: "Asistencias",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "HoraEntrada",
                table: "Asistencias",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Retardo",
                table: "Asistencias",
                type: "int",
                nullable: true);
        }
    }
}
