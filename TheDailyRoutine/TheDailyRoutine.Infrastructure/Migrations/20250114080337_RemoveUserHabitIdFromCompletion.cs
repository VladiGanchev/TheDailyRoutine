using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheDailyRoutine.Infrastructure.Migrations
{
    public partial class RemoveUserHabitIdFromCompletion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserHabitId",
                table: "Completions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserHabitId",
                table: "Completions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
