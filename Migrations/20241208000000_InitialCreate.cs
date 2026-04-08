using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomPcStoreApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Эта миграция предназначена для синхронизации с существующей базой данных
            // Все таблицы уже созданы через SQL скрипт, поэтому миграция пустая
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Откат не требуется для существующей базы данных
        }
    }
}