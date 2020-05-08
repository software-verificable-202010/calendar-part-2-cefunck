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
    /// Lógica de interacción para MonthBody.xaml
    /// </summary>
    public partial class MonthBody : UserControl
    {
        private Brush highlightColor = Brushes.Red;        
        private const string DayElementNamePrefix = "dayElement";
        private const string DayNumberResourceKeyPrefix = "dayResource";
        private const string DisplayedDateResourceName = "displayedDate";
        private const string DayNumberResourceBlankValue = "";
        private const int DaysInWeek = 7;
        private const int IterationIndexOffset = 1;
        private const int GridRowIndexOffset = 1;
        private const int GridColumnIndexOffset = 1;
        private const int FirstDayNumberInMonth = 1;
        private const int NumberOfCellsInGrid = 42;
        private const int SaturdayGridColumnIndex = 5;
        private const int SundayGridColumnIndex = 6;

        public MonthBody()
        {
            InitializeComponent();
            GenerateDayNumberResources();
            AssingValuesToDayNumberResources(GetDisplayedDateResourceValue());
            List<TextBlock> dayElements = CreateDayElements();
            InsertDayElementsToGrid(dayElements);
            HighLightWeekends();
        }
       
        private void GenerateDayNumberResources()
        {
            for (int i = 0; i < NumberOfCellsInGrid; i++)
            {
                string dayNumberResourceKey = DayNumberResourceKeyPrefix + i.ToString();
                string dayNumberResourceValue = DayNumberResourceBlankValue;
                if (App.Current.Resources[dayNumberResourceKey] == null) 
                {
                    App.Current.Resources.Add(dayNumberResourceKey, dayNumberResourceValue);
                }                
            }
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
        }

        private List<TextBlock> CreateDayElements()
        {
            List<TextBlock> dayElements = new List<TextBlock>();
            for (int i = 0; i < NumberOfCellsInGrid; i++)
            {
                TextBlock dayElement = new TextBlock();                
                dayElement.Name = DayElementNamePrefix + i.ToString();
                string dayNumberResourceKey = DayNumberResourceKeyPrefix + i.ToString();
                dayElement.SetResourceReference(TextBlock.TextProperty, dayNumberResourceKey);
                Point dayElementGridCoordinates = GetGridCoordinatesByIterationIndex(i);
                dayElement.SetValue(Grid.ColumnProperty, (int)dayElementGridCoordinates.X);
                dayElement.SetValue(Grid.RowProperty, (int)dayElementGridCoordinates.Y);
                dayElements.Add(dayElement);
            }
            return dayElements;
        }

        private void InsertDayElementsToGrid(List<TextBlock> dayElements)
        {
            foreach (TextBlock dayElement in dayElements)
            {
                BodyGrid.Children.Add(dayElement);
            }
        }

        private void HighLightWeekends()
        {
            foreach (TextBlock dayElement in BodyGrid.Children)
            {
                int dayElementGridColumnIndex = (int)dayElement.GetValue(Grid.ColumnProperty);
                if (dayElementGridColumnIndex == SaturdayGridColumnIndex || dayElementGridColumnIndex == SundayGridColumnIndex)
                {
                    dayElement.Foreground = highlightColor;
                }
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
            int firstDayGridColumnIndex = GetDayNumberInWeek(firstDayOfDisplayedMonth) - GridColumnIndexOffset;
            return firstDayGridColumnIndex;
        }

        private DateTime GetDisplayedDateResourceValue()
        {
            return (DateTime)App.Current.Resources[DisplayedDateResourceName];
        }

        private Point GetGridCoordinatesByIterationIndex(int iterationIndex)
        {
            int gridColumn = (iterationIndex) % DaysInWeek;
            int gridRow = (iterationIndex / DaysInWeek) + GridRowIndexOffset;
            return new Point(gridColumn, gridRow); ;
        }

        private int GetDayNumberInWeek(DateTime date)
        {
            const int SundayDayOfWeek = 7;
            const int SystemEnumSundayDayOfWeek = 0;
            int dayNumber = (int)date.DayOfWeek;
            if (dayNumber == SystemEnumSundayDayOfWeek)
            {
                dayNumber = SundayDayOfWeek;
            }
            return dayNumber;
        }
    }
}
