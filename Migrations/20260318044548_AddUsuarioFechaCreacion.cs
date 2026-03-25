using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Migrations
{
    /// <inheritdoc />
    public partial class AddUsuarioFechaCreacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Titulo",
                table: "Videojuegos",
                newName: "titulo");

            migrationBuilder.RenameColumn(
                name: "Stock",
                table: "Videojuegos",
                newName: "stock");

            migrationBuilder.RenameColumn(
                name: "Precio",
                table: "Videojuegos",
                newName: "precio");

            migrationBuilder.RenameColumn(
                name: "Plataforma",
                table: "Videojuegos",
                newName: "plataforma");

            migrationBuilder.RenameColumn(
                name: "Genero",
                table: "Videojuegos",
                newName: "genero");

            migrationBuilder.RenameColumn(
                name: "Descripcion",
                table: "Videojuegos",
                newName: "descripcion");

            migrationBuilder.RenameColumn(
                name: "Desarrolladora",
                table: "Videojuegos",
                newName: "desarrolladora");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Videojuegos",
                newName: "id");

            migrationBuilder.AlterColumn<string>(
                name: "video_url",
                table: "Videojuegos",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<double>(
                name: "puntaje_promedio",
                table: "Videojuegos",
                type: "double",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double");

            migrationBuilder.AlterColumn<string>(
                name: "imagen_url",
                table: "Videojuegos",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<DateTime>(
                name: "fecha_lanzamiento",
                table: "Videojuegos",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "fecha_creacion",
                table: "Videojuegos",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<string>(
                name: "titulo",
                table: "Videojuegos",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "stock",
                table: "Videojuegos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "precio",
                table: "Videojuegos",
                type: "decimal(65,30)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<string>(
                name: "plataforma",
                table: "Videojuegos",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "genero",
                table: "Videojuegos",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "descripcion",
                table: "Videojuegos",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "desarrolladora",
                table: "Videojuegos",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "titulo",
                table: "Videojuegos",
                newName: "Titulo");

            migrationBuilder.RenameColumn(
                name: "stock",
                table: "Videojuegos",
                newName: "Stock");

            migrationBuilder.RenameColumn(
                name: "precio",
                table: "Videojuegos",
                newName: "Precio");

            migrationBuilder.RenameColumn(
                name: "plataforma",
                table: "Videojuegos",
                newName: "Plataforma");

            migrationBuilder.RenameColumn(
                name: "genero",
                table: "Videojuegos",
                newName: "Genero");

            migrationBuilder.RenameColumn(
                name: "descripcion",
                table: "Videojuegos",
                newName: "Descripcion");

            migrationBuilder.RenameColumn(
                name: "desarrolladora",
                table: "Videojuegos",
                newName: "Desarrolladora");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Videojuegos",
                newName: "Id");

            migrationBuilder.UpdateData(
                table: "Videojuegos",
                keyColumn: "video_url",
                keyValue: null,
                column: "video_url",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "video_url",
                table: "Videojuegos",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Videojuegos",
                keyColumn: "Titulo",
                keyValue: null,
                column: "Titulo",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Titulo",
                table: "Videojuegos",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "Stock",
                table: "Videojuegos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "puntaje_promedio",
                table: "Videojuegos",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Precio",
                table: "Videojuegos",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Videojuegos",
                keyColumn: "Plataforma",
                keyValue: null,
                column: "Plataforma",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Plataforma",
                table: "Videojuegos",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Videojuegos",
                keyColumn: "imagen_url",
                keyValue: null,
                column: "imagen_url",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "imagen_url",
                table: "Videojuegos",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Videojuegos",
                keyColumn: "Genero",
                keyValue: null,
                column: "Genero",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Genero",
                table: "Videojuegos",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<DateTime>(
                name: "fecha_lanzamiento",
                table: "Videojuegos",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "fecha_creacion",
                table: "Videojuegos",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Videojuegos",
                keyColumn: "Descripcion",
                keyValue: null,
                column: "Descripcion",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Videojuegos",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Videojuegos",
                keyColumn: "Desarrolladora",
                keyValue: null,
                column: "Desarrolladora",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Desarrolladora",
                table: "Videojuegos",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
