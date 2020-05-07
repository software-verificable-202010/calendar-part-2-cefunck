using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Calendar
{
    /// <summary>
    /// Lógica de interacción para CalendarNavbar.xaml
    /// </summary>
    public partial class CalendarNavbar : UserControl
    {
        const string MonthViewOption = "Vista Mensual";
        const string WeekViewOption = "Vista Semanal";
        const string CurrentBodyContentResourceName = "bodyContent";
        private const string NavBarMonthFormat = "MMMM yyyy";
        private const string DayNumberResourceKeyPrefix = "dayResource";
        private const string DisplayedDateResourceName = "displayedDate";
        private const string DayNumberResourceBlankValue = "";
        private const int IterationIndexOffset = 1;
        private const int GridRowIndexOffset = 1;
        private const int DaysInWeek = 7;
        private const int FirstDayNumberInMonth = 1;
        private const int GridColumnIndexOffset = 1;
        private const int NumberOfCellsInGrid = 42;
        private const int NumberOfMonthsToAdvance = 1;
        private const int NumberOfMonthToGoBack = -1;


        public CalendarNavbar()
        {
            InitializeComponent();
            SetDisplayedDateResourceValue(DateTime.Now);
            AssignValueToMonthAndYearResource(GetDisplayedDateResourceValue());
        }

        private void CurrentCalendarViewOption_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string currentSelectedCalendarViewOption = CurrentCalendarViewOptions.SelectedValue.ToString().Substring(38);
            SetBodyContentResourceValue(currentSelectedCalendarViewOption);

        }

        private void PreviousMonth_Click(object sender, RoutedEventArgs e)
        {
            string currentSelectedCalendarViewOption = CurrentCalendarViewOptions.SelectedValue.ToString().Substring(38);
            DateTime dateToDisplay = DateTime.Now;
            if (currentSelectedCalendarViewOption == MonthViewOption)
            {
                dateToDisplay = GetDisplayedDateResourceValue().AddMonths(NumberOfMonthToGoBack);
            } 
            else if(currentSelectedCalendarViewOption == WeekViewOption)
            {
                dateToDisplay = GetDisplayedDateResourceValue().AddDays(NumberOfMonthToGoBack * DaysInWeek);
            }
            SetDisplayedDateAndAssingAllResources(dateToDisplay);
        }

        private void NextMonth_Click(object sender, RoutedEventArgs e)
        {
            string currentSelectedCalendarViewOption = CurrentCalendarViewOptions.SelectedValue.ToString().Substring(38);
            DateTime dateToDisplay = DateTime.Now;
            if (currentSelectedCalendarViewOption == MonthViewOption)
            {
                dateToDisplay = GetDisplayedDateResourceValue().AddMonths(NumberOfMonthsToAdvance);
            }
            else if (currentSelectedCalendarViewOption == WeekViewOption)
            {
                dateToDisplay = GetDisplayedDateResourceValue().AddDays(DaysInWeek);
            }
            SetDisplayedDateAndAssingAllResources(dateToDisplay);
        }

        private void SetDisplayedDateAndAssingAllResources(DateTime dateToDisplay)
        {
            SetDisplayedDateResourceValue(dateToDisplay);
            AssignValueToMonthAndYearResource(GetDisplayedDateResourceValue());
            AssingValuesToDayNumberResources(GetDisplayedDateResourceValue());
        }

        private void AssignValueToMonthAndYearResource(DateTime date)
        {
            App.Current.Resources["monthAndYear"] = date.ToString(NavBarMonthFormat);
        }

        private void AssingValuesToDayNumberResources(DateTime displayedDate)
        {
            for (int i = 0; i < NumberOfCellsInGrid; i++)
            {
                string dayNumberResourceKey = DayNumberResourceKeyPrefix + i.ToString();
                string dayNumberResourceValue = DayNumberResourceBlankValue;
                int candidateDayNumber = i - GetfirstDayGridColumnIndex() + IterationIndexOffset;
                Point dayElementGridCoordinates = GetGridCoordinatesByIterationIndex(i);
                if (IsDayNumberInDisplayedMonth(candidateDayNumber, dayElementGridCoordinates))
                {
                    dayNumberResourceValue = candidateDayNumber.ToString();
                }
                App.Current.Resources[dayNumberResourceKey] = dayNumberResourceValue;
            }
            for (int i = 1; i <= 7; i++)
            {
                DateTime now = GetDisplayedDateResourceValue();
                int dayOfWeek = (int)now.DayOfWeek;
                if (dayOfWeek == 0)
                {
                    dayOfWeek = 7;
                }
                now = now.AddDays(-1 * dayOfWeek + i);
                App.Current.Resources["WeekColumnTitle" + i.ToString()] = getNameOfDayInSpanish(now)+ " "+now.Day.ToString();
            }
        }

        private bool IsDayNumberInDisplayedMonth(int candidateDayNumber, Point dayElementGridCoordinates)
        {
            DateTime displayedDate = GetDisplayedDateResourceValue();
            bool isFirstDayRow = dayElementGridCoordinates.Y == 1;
            bool isNotFirstDayRow = dayElementGridCoordinates.Y > 1;
            bool isFirstDayColumnOrLater = dayElementGridCoordinates.X >= GetfirstDayGridColumnIndex();
            bool isDisplayableDayElementOfFirstRow = isFirstDayRow && isFirstDayColumnOrLater;
            bool isCandidateDayNumberInDisplayedMonth = candidateDayNumber <= GetNumberOfDaysOfMonth(displayedDate);
            bool isDisplayableDayElementOfRemainsRows = isNotFirstDayRow && isCandidateDayNumberInDisplayedMonth;
            return (isDisplayableDayElementOfFirstRow || isDisplayableDayElementOfRemainsRows);
        }

        private int GetNumberOfDaysOfMonth(DateTime date)
        {
            DateTime displayedDate = date;
            int numberOfDaysOfDisplayedMonth = DateTime.DaysInMonth(displayedDate.Year, displayedDate.Month);
            return numberOfDaysOfDisplayedMonth;
        }

        private int GetfirstDayGridColumnIndex()
        {
            const int SundayDayOfWeek = 7;
            const int SystemEnumSundayDayOfWeek = 0;
            DateTime displayedDate = GetDisplayedDateResourceValue();
            DateTime firstDayOfDisplayedMonth = new DateTime(displayedDate.Year, displayedDate.Month, FirstDayNumberInMonth);
            int firstDayGridColumnIndex = (int)(firstDayOfDisplayedMonth.DayOfWeek) - GridColumnIndexOffset;
            if ((int)(firstDayOfDisplayedMonth.DayOfWeek) == SystemEnumSundayDayOfWeek)
            {
                firstDayGridColumnIndex = SundayDayOfWeek - GridColumnIndexOffset;
            }
            return firstDayGridColumnIndex;
        }

        private Point GetGridCoordinatesByIterationIndex(int iterationIndex)
        {
            int gridColumn = (iterationIndex) % DaysInWeek;
            int gridRow = (iterationIndex / DaysInWeek) + GridRowIndexOffset;
            return new Point(gridColumn, gridRow); ;
        }

        private DateTime GetDisplayedDateResourceValue()
        {
            return (DateTime)App.Current.Resources[DisplayedDateResourceName];
        }

        private void SetDisplayedDateResourceValue(DateTime dateToDisplay)
        {
            App.Current.Resources[DisplayedDateResourceName] = dateToDisplay;
        }
        private void SetBodyContentResourceValue(string selectedCalendarViewOption)
        {           
            switch (selectedCalendarViewOption)
            {
                case WeekViewOption:                    
                    WeekBody weekBody = new WeekBody();
                    App.Current.Resources[CurrentBodyContentResourceName] = weekBody;
                    break;
                default:
                    MonthBody monthBody = new MonthBody();
                    App.Current.Resources[CurrentBodyContentResourceName] = monthBody;
                    break;
            }
            
        }

        private string getNameOfDayInSpanish(DateTime date) 
        {
            string spanishDayName = "";
            switch ((int)date.DayOfWeek)
            {
                case 1:
                    spanishDayName = "Lunes";
                    break;
                case 2:
                    spanishDayName = "Martes";
                    break;
                case 3:
                    spanishDayName = "Miércoles";
                    break;
                case 4:
                    spanishDayName = "Jueves";
                    break;
                case 5:
                    spanishDayName = "Viernes";
                    break;
                case 6:
                    spanishDayName = "Sábado";
                    break;
                default:
                    spanishDayName = "Domingo";
                    break;
            }
            return spanishDayName;
        }
    }
}
