using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheDailyRoutine.Infrastructure.Migrations
{
    public partial class SecondMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Habits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Predefined = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Habits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Mailable = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsersHabits",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HabitId = table.Column<int>(type: "int", nullable: false),
                    Frequency = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersHabits", x => new { x.UserId, x.HabitId });
                    table.ForeignKey(
                        name: "FK_UsersHabits_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersHabits_Habits_HabitId",
                        column: x => x.HabitId,
                        principalTable: "Habits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UsersNotifications",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NotificationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersNotifications", x => new { x.UserId, x.NotificationId });
                    table.ForeignKey(
                        name: "FK_UsersNotifications_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersNotifications_Notifications_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Completions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserHabitId = table.Column<int>(type: "int", nullable: false),
                    UserHabitUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserHabitHabitId = table.Column<int>(type: "int", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Completed = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Completions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Completions_UsersHabits_UserHabitUserId_UserHabitHabitId",
                        columns: x => new { x.UserHabitUserId, x.UserHabitHabitId },
                        principalTable: "UsersHabits",
                        principalColumns: new[] { "UserId", "HabitId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Habits",
                columns: new[] { "Id", "Description", "Predefined", "Title" },
                values: new object[,]
                {
                    { 1, "Пий 8 чаши вода на ден", true, "Хидратация" },
                    { 2, "Тренирай 30 минути", true, "Физическа активност" },
                    { 3, "Спи поне 7 часа", true, "Сън" },
                    { 4, "Измий зъбите си два пъти дневно", true, "Хигиена" },
                    { 5, "Яж три пъти на ден", true, "Балансирани хранения" },
                    { 6, "Яж поне 5 порции плодове и зеленчуци", true, "Консумация на плодове и зеленчуци" },
                    { 7, "Избягвай сладки изделия", true, "Ограничаване на захарта" },
                    { 8, "Направи дневен план сутринта", true, "Планиране на деня " },
                    { 9, "Завърши 3 важни задачи за деня", true, "Изпълнение на задачи" },
                    { 10, "Обобщи успехите си вечерта", true, "Преглед на постигнатото" },
                    { 11, "Медитирай 10 минути", true, "Медитация" },
                    { 12, "Запиши 3 неща, за които си благодарен", true, "Дневник на благодарностите" },
                    { 13, "30 минути без устройства", true, "Почивка от технологии" },
                    { 14, "Обади се на приятел или роднина", true, "Свързване с близки" },
                    { 15, "Направи нещо добро за друг човек", true, "Доброта" },
                    { 16, "Прочети поне 10 страници книга", true, "Четене" },
                    { 17, "Научи нещо ново", true, "Ново умение" },
                    { 18, "Гледай урок или лекция", true, "Онлайн курсове" },
                    { 19, "Подреди стаята си", true, "Почистване" },
                    { 20, "Приготви домашна храна", true, "Готвене" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Completions_UserHabitUserId_UserHabitHabitId",
                table: "Completions",
                columns: new[] { "UserHabitUserId", "UserHabitHabitId" });

            migrationBuilder.CreateIndex(
                name: "IX_UsersHabits_HabitId",
                table: "UsersHabits",
                column: "HabitId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersNotifications_NotificationId",
                table: "UsersNotifications",
                column: "NotificationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Completions");

            migrationBuilder.DropTable(
                name: "UsersNotifications");

            migrationBuilder.DropTable(
                name: "UsersHabits");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Habits");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");
        }
    }
}
