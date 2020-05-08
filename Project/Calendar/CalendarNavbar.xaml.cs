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
        private const string ColumnTitleResourceKeyPrefix = "WeekColumnTitle";
        private const string MonthAndYearResourceName = "monthAndYear";
        private const string DisplayedDateResourceName = "displayedDate";
        private const string DayNumberResourceBlankValue = "";
        private const int IterationIndexOffset = 1;
        private const int GridRowIndexOffset = 1;
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
            string selectedCalendarViewOption = GetSelectedCalendarView();
            SetBodyContentResourceValue(selectedCalendarViewOption);
        }

        private void PreviousMonth_Click(object sender, RoutedEventArgs e)
        {
            string currentSelectedCalendarViewOption = GetSelectedCalendarView();
            DateTime dateToDisplay = GetDisplayedDateResourceValue();
            if (currentSelectedCalendarViewOption == MonthViewOption)
            {
                dateToDisplay = dateToDisplay.AddMonths(NumberOfMonthToGoBack);
            } 
            else if(currentSelectedCalendarViewOption == WeekViewOption)
            {
                dateToDisplay = dateToDisplay.AddDays(NumberOfMonthToGoBack * Utilities.DaysInWeek);
            }
            SetDisplayedDateAndAssingAllResources(dateToDisplay);
        }

        private void NextMonth_Click(object sender, RoutedEventArgs e)
        {
            string currentSelectedCalendarViewOption = GetSelectedCalendarView();
            DateTime dateToDisplay = GetDisplayedDateResourceValue();
            if (currentSelectedCalendarViewOption == MonthViewOption)
            {
                dateToDisplay = dateToDisplay.AddMonths(NumberOfMonthsToAdvance);
            }
            else if (currentSelectedCalendarViewOption == WeekViewOption)
            {
                dateToDisplay = dateToDisplay.AddDays(Utilities.DaysInWeek);
            }
            SetDisplayedDateAndAssingAllResources(dateToDisplay);
        }

        private void SetDisplayedDateAndAssingAllResources(DateTime date)
        {
            SetDisplayedDateResourceValue(date);
            AssignValueToMonthAndYearResource(GetDisplayedDateResourceValue());
            AssingValuesToDayNumberResources();
            AssingValuesToDayColumnTitleResources();
        }

        private void AssignValueToMonthAndYearResource(DateTime date)
        {
            App.Current.Resources[MonthAndYearResourceName] = date.ToString(NavBarMonthFormat);
        }

        private void AssingValuesToDayNumberResources()
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
        }

        private void AssingValuesToDayColumnTitleResources()
        {
            for (int i = 1; i <= Utilities.DaysInWeek; i++)
            {
                DateTime displayedDate = GetDisplayedDateResourceValue();
                int dayOfWeek = Utilities.GetDayNumberInWeek(displayedDate);
                displayedDate = displayedDate.AddDays(Utilities.NegativeMultiplier * dayOfWeek + i);
                string columnTitleResourceKey = ColumnTitleResourceKeyPrefix + i.ToString();
                string columnTitleResourceValue = Utilities.GetNameOfDayInSpanish(displayedDate) + Utilities.BlankSpace + displayedDate.Day.ToString();
                App.Current.Resources[columnTitleResourceKey] = columnTitleResourceValue;
            }
        }

        private bool IsDayNumberInDisplayedMonth(int candidateDayNumber, Point dayElementGridCoordinates)
        {
            const int firstDayRowIndex = 1;
            DateTime displayedDate = GetDisplayedDateResourceValue();
            bool isFirstDayRow = dayElementGridCoordinates.Y == firstDayRowIndex;
            bool isNotFirstDayRow = dayElementGridCoordinates.Y > firstDayRowIndex;
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
            DateTime displayedDate = GetDisplayedDateResourceValue();
            DateTime firstDayOfDisplayedMonth = new DateTime(displayedDate.Year, displayedDate.Month, FirstDayNumberInMonth);
            int firstDayGridColumnIndex = Utilities.GetDayNumberInWeek(firstDayOfDisplayedMonth) - GridColumnIndexOffset;
            return firstDayGridColumnIndex;
        }

        private Point GetGridCoordinatesByIterationIndex(int iterationIndex)
        {
            int gridColumn = (iterationIndex) % Utilities.DaysInWeek;
            int gridRow = (iterationIndex / Utilities.DaysInWeek) + GridRowIndexOffset;
            return new Point(gridColumn, gridRow);
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

        private string GetSelectedCalendarView() 
        {            
            const int initOfValueSubstring = 38;
            return CurrentCalendarViewOptions.SelectedValue.ToString().Substring(initOfValueSubstring);
        }
    }
}
