using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheDailyRoutine.Infrastructure.Data.Models;

namespace TheDailyRoutine.Infrastructure.Data.Configuration
{
    internal class HabitConfiguration : IEntityTypeConfiguration<Habit>
    {
        public void Configure(EntityTypeBuilder<Habit> builder)
        {
            builder
            .HasData(new Habit()
            {
                Id = 1,
                Title = "Хидратация",
                Description = "Пий 8 чаши вода на ден",
                Predefined = true
            },
            new Habit()
            {
                Id = 2,
                Title = "Физическа активност",
                Description = "Тренирай 30 минути",
                Predefined = true
            },
            new Habit()
            {
                Id = 3,
                Title = "Сън",
                Description = "Спи поне 7 часа",
                Predefined = true
            },
            new Habit()
            {
                Id = 4,
                Title = "Хигиена",
                Description = "Измий зъбите си два пъти дневно",
                Predefined = true
            },
            new Habit()
            {
                Id = 5,
                Title = "Балансирани хранения",
                Description = "Яж три пъти на ден",
                Predefined = true
            },
            new Habit()
            {
                Id = 6,
                Title = "Консумация на плодове и зеленчуци",
                Description = "Яж поне 5 порции плодове и зеленчуци",
                Predefined = true
            },
            new Habit()
            {
                Id = 7,
                Title = "Ограничаване на захарта",
                Description = "Избягвай сладки изделия",
                Predefined = true
            },
            new Habit()
            {
                Id = 8,
                Title = "Планиране на деня ",
                Description = "Направи дневен план сутринта",
                Predefined = true
            },
            new Habit()
            {
                Id = 9,
                Title = "Изпълнение на задачи",
                Description = "Завърши 3 важни задачи за деня",
                Predefined = true
            },
            new Habit()
            {
                Id = 10,
                Title = "Преглед на постигнатото",
                Description = "Обобщи успехите си вечерта",
                Predefined = true
            },
            new Habit()
            {
                Id = 11,
                Title = "Медитация",
                Description = "Медитирай 10 минути",
                Predefined = true
            },
            new Habit()
            {
                Id = 12,
                Title = "Дневник на благодарностите",
                Description = "Запиши 3 неща, за които си благодарен",
                Predefined = true
            },
            new Habit()
            {
                Id = 13,
                Title = "Почивка от технологии",
                Description = "30 минути без устройства",
                Predefined = true
            },
            new Habit()
            {
                Id = 14,
                Title = "Свързване с близки",
                Description = "Обади се на приятел или роднина",
                Predefined = true
            },
            new Habit()
            {
                Id = 15,
                Title = "Доброта",
                Description = "Направи нещо добро за друг човек",
                Predefined = true
            },
            new Habit()
            {
                Id = 16,
                Title = "Четене",
                Description = "Прочети поне 10 страници книга",
                Predefined = true
            },
            new Habit()
            {
                Id = 17,
                Title = "Ново умение",
                Description = "Научи нещо ново",
                Predefined = true
            },
            new Habit()
            {
                Id = 18,
                Title = "Онлайн курсове",
                Description = "Гледай урок или лекция",
                Predefined = true
            },
            new Habit()
            {
                Id = 19,
                Title = "Почистване",
                Description = "Подреди стаята си",
                Predefined = true
            },
            new Habit()
            {
                Id = 20,
                Title = "Готвене",
                Description = "Приготви домашна храна",
                Predefined = true
            });
        }
    }
}
