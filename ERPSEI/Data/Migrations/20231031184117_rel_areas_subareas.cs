using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPSEI.Data.Migrations
{
    /// <inheritdoc />
    public partial class rel_areas_subareas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Oficinas",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.AddColumn<int>(
                name: "AreaId",
                table: "Subareas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdArea",
                table: "Subareas",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 9,
                column: "Nombre",
                value: "Legal 1");

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 10,
                column: "Nombre",
                value: "Legal 2");

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 11,
                column: "Nombre",
                value: "Nóminas");

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 12,
                column: "Nombre",
                value: "Operaciones");

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 13,
                column: "Nombre",
                value: "Recursos Humanos");

            migrationBuilder.InsertData(
                table: "Areas",
                columns: new[] { "Id", "Nombre" },
                values: new object[] { 14, "Tesorería" });

            migrationBuilder.UpdateData(
                table: "Oficinas",
                keyColumn: "Id",
                keyValue: 9,
                column: "Nombre",
                value: "Los Reyes La Paz");

            migrationBuilder.UpdateData(
                table: "Oficinas",
                keyColumn: "Id",
                keyValue: 10,
                column: "Nombre",
                value: "Pafnuncio");

            migrationBuilder.UpdateData(
                table: "Oficinas",
                keyColumn: "Id",
                keyValue: 11,
                column: "Nombre",
                value: "Pirules");

            migrationBuilder.UpdateData(
                table: "Oficinas",
                keyColumn: "Id",
                keyValue: 12,
                column: "Nombre",
                value: "Polanco");

            migrationBuilder.UpdateData(
                table: "Oficinas",
                keyColumn: "Id",
                keyValue: 13,
                column: "Nombre",
                value: "Santa Mónica");

            migrationBuilder.UpdateData(
                table: "Oficinas",
                keyColumn: "Id",
                keyValue: 14,
                column: "Nombre",
                value: "Torre Esmeralda");

            migrationBuilder.UpdateData(
                table: "Subareas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AreaId", "IdArea" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Subareas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AreaId", "IdArea" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Subareas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "AreaId", "IdArea" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Subareas",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "AreaId", "IdArea" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Subareas",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "AreaId", "IdArea" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Subareas",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "AreaId", "IdArea" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Subareas",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "AreaId", "IdArea" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Subareas",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "AreaId", "IdArea" },
                values: new object[] { null, null });

            migrationBuilder.CreateIndex(
                name: "IX_Subareas_AreaId",
                table: "Subareas",
                column: "AreaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subareas_Areas_AreaId",
                table: "Subareas",
                column: "AreaId",
                principalTable: "Areas",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subareas_Areas_AreaId",
                table: "Subareas");

            migrationBuilder.DropIndex(
                name: "IX_Subareas_AreaId",
                table: "Subareas");

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DropColumn(
                name: "AreaId",
                table: "Subareas");

            migrationBuilder.DropColumn(
                name: "IdArea",
                table: "Subareas");

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 9,
                column: "Nombre",
                value: "Legal");

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 10,
                column: "Nombre",
                value: "Nóminas");

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 11,
                column: "Nombre",
                value: "Operaciones");

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 12,
                column: "Nombre",
                value: "Recursos Humanos");

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 13,
                column: "Nombre",
                value: "Tesorería");

            migrationBuilder.UpdateData(
                table: "Oficinas",
                keyColumn: "Id",
                keyValue: 9,
                column: "Nombre",
                value: "León");

            migrationBuilder.UpdateData(
                table: "Oficinas",
                keyColumn: "Id",
                keyValue: 10,
                column: "Nombre",
                value: "Los Reyes la Paz");

            migrationBuilder.UpdateData(
                table: "Oficinas",
                keyColumn: "Id",
                keyValue: 11,
                column: "Nombre",
                value: "Pafnuncio");

            migrationBuilder.UpdateData(
                table: "Oficinas",
                keyColumn: "Id",
                keyValue: 12,
                column: "Nombre",
                value: "Pirules");

            migrationBuilder.UpdateData(
                table: "Oficinas",
                keyColumn: "Id",
                keyValue: 13,
                column: "Nombre",
                value: "Polanco");

            migrationBuilder.UpdateData(
                table: "Oficinas",
                keyColumn: "Id",
                keyValue: 14,
                column: "Nombre",
                value: "Santa Mónica");

            migrationBuilder.InsertData(
                table: "Oficinas",
                columns: new[] { "Id", "Nombre" },
                values: new object[] { 15, "Torre Esmeralda" });
        }
    }
}
