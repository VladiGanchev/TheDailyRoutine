using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheDailyRoutine.Infrastructure.Data.Constants
{
    public static class DataConstants
    {
        public const string RequireErrorMessage = "The field {0} is required.";

        public const string StringLengthErrorMessage = "The field {0} must be between {2} and {1} symbols.";

        public const string RequiredDateTimeFormat = "dd/MM/yyyy HH:mm";

        public const int UserFirstNameMaxLength = 12;
        public const int UserFirstNameMinLength = 1;

        public const int UserLastNameMaxLength = 15;
        public const int UserLastNameMinLength = 3;

        public const int HabitDescriptionMinLength = 10;
        public const int HabitDescriptionMaxLength = 150;

        public const int NotificationContentMinLength = 10;
        public const int NotificationContentMaxLength = 150;


        public const int CompletionNoteMinLength = 10;
        public const int CompletionNoteMaxLength = 150;

    }
}
