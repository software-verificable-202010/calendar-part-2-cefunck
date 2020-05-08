using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar
{
    public static class Utilities
    {
        public const string MondayName = "Lunes";
        public const string TuesdayName = "Martes";
        public const string WednesdayName = "Miércoles";
        public const string ThursdayName = "Jueves";
        public const string FridayName = "Viernes";
        public const string SaturdayName = "Sábado";
        public const string SundayName = "Domingo";
        public const int MondayNumberInweek = 1;
        public const int TuesdayNumberInweek = 2;
        public const int WednesdayNumberInweek = 3;
        public const int ThursdayNumberInweek = 4;
        public const int FridayNumberInweek = 5;
        public const int SaturdayNumberInweek = 6;
        public const int SundayNumberInweek = 7;
        public const string BlankSpace = " ";
        public const int NegativeMultiplier = -1;
        public const int DaysInWeek = 7;
        public const int SystemEnumSundayNumber = 0;
        public static int GetDayNumberInWeek(DateTime date)
        {            
            int dayNumber = (int)date.DayOfWeek;
            if (dayNumber == SystemEnumSundayNumber)
            {
                dayNumber = SundayNumberInweek;
            }
            return dayNumber;
        }

        public static string GetNameOfDayInSpanish(DateTime date)
        {
            string spanishDayName;
            switch ((int)date.DayOfWeek)
            {
                case MondayNumberInweek:
                    spanishDayName = MondayName;
                    break;
                case TuesdayNumberInweek:
                    spanishDayName = TuesdayName;
                    break;
                case WednesdayNumberInweek:
                    spanishDayName = WednesdayName;
                    break;
                case Utilities.ThursdayNumberInweek:
                    spanishDayName = ThursdayName;
                    break;
                case FridayNumberInweek:
                    spanishDayName = FridayName;
                    break;
                case SaturdayNumberInweek:
                    spanishDayName = SaturdayName;
                    break;
                default:
                    spanishDayName = SundayName;
                    break;
            }
            return spanishDayName;
        }
    }
}
