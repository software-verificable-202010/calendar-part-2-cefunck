using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Policy;
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
    /// Lógica de interacción para WeekColumn.xaml
    /// </summary>
    public partial class WeekColumn : UserControl
    {
        private const string DisplayedDateResourceName = "displayedDate";
        private const string ColumnTitleResourceKeyPrefix = "WeekColumnTitle";
        private const string ColumnTitleNamePrefix = "WeekColumnTitleElement";        

        public int Index 
        {
            get;
            set;
        }
        private string DayName 
        { 
            get 
            {
                string dayName;
                switch (Index)
                {
                    case Utilities.MondayNumberInweek:
                        dayName = Utilities.MondayName;
                        break;
                    case Utilities.TuesdayNumberInweek:
                        dayName = Utilities.TuesdayName;
                        break;
                    case Utilities.WednesdayNumberInweek:
                        dayName = Utilities.WednesdayName;
                        break;
                    case Utilities.ThursdayNumberInweek:
                        dayName = Utilities.ThursdayName;
                        break;
                    case Utilities.FridayNumberInweek:
                        dayName = Utilities.FridayName;
                        break;
                    case Utilities.SaturdayNumberInweek:
                        dayName = Utilities.SaturdayName;
                        break;
                    default:
                        dayName = Utilities.SundayName;
                        break;
                }
                return dayName;
            } 
        }
        private int DayNumber 
        { 
            get 
            {
                DateTime selectedDate = GetDisplayedDateResourceValue();
                int dayOfWeek = Utilities.GetDayNumberInWeek(selectedDate);
                selectedDate = selectedDate.AddDays(Utilities.NegativeMultiplier * dayOfWeek + Index);
                return selectedDate.Day;
            } 
        }
        private string TitleResourceKey 
        {
            get 
            {
                return ColumnTitleResourceKeyPrefix + Index;
            }
        }
        private string TitleResourceValue 
        { 
            get 
            {
                return DayName + Utilities.BlankSpace + DayNumber.ToString(); 
            } 
        }
        public WeekColumn(int columnIndex)
        {
            InitializeComponent();
            Index = columnIndex;
            GenerateTitleResource();
            AssingValuesToTitleResource();
            TextBlock title = CreateTitleElement();
            InsertTitleElementToColumn(title);
        }
        private void GenerateTitleResource()
        {
            if (App.Current.Resources[TitleResourceKey] == null)
            {
                App.Current.Resources.Add(TitleResourceKey, TitleResourceValue);
            }
        }
        private void AssingValuesToTitleResource()
        {
            App.Current.Resources[TitleResourceKey] = TitleResourceValue;
        }
        private TextBlock CreateTitleElement()
        {
            TextBlock titleElement = new TextBlock();
            titleElement.Name = ColumnTitleNamePrefix + Index.ToString();
            titleElement.SetResourceReference(TextBlock.TextProperty, TitleResourceKey);
            titleElement.SetValue(Grid.ColumnProperty, 0);
            titleElement.SetValue(Grid.RowProperty, 0);
            titleElement.SetValue(Grid.ColumnSpanProperty, 2);
            titleElement.SetValue(Grid.RowSpanProperty, 2);
            return titleElement;
        }
        private void InsertTitleElementToColumn(TextBlock titleElement)
        {
            WeekColumnGrid.Children.Add(titleElement);
        }
        private DateTime GetDisplayedDateResourceValue()
        {
            return (DateTime)App.Current.Resources[DisplayedDateResourceName];
        }
        private DateTime GetDateOfColumn() 
        {
            DateTime displayedDate = GetDisplayedDateResourceValue();
            int year = displayedDate.Year;
            int month = displayedDate.Month;
            int day = (int)DayNumber;
            return new DateTime(year,month,day);
        }
        private int GetRowIndex()
        {
            // HACK: Temporary fix until able to Refactor.
            const int FixedRowNumber = 2;
            return FixedRowNumber;
        }
        private int GetColumIndex()
        {
            // HACK: Temporary fix until able to Refactor.
            const int FixedColumnNumber = 0;
            return FixedColumnNumber;
        }
        private int GetRowSpan()
        {
            // HACK: Temporary fix until able to Refactor.
            const int FixedRowSpan = 20;
            return FixedRowSpan;
        }
        private int GetColumnSpan()
        {
            // HACK: Temporary fix until able to Refactor.
            const int FixedColumnSpan = 1;
            return FixedColumnSpan;
        }
        private void AddAppointment_Click(object sender, RoutedEventArgs e)
        {
            // TODO: to complete
            Button pressedCellOfWeekColumn = (Button)sender;
            int pressedCellRow = (int)pressedCellOfWeekColumn.GetValue(Grid.RowProperty);
            Window addAppointment = new Window();
            AddEditAppointment windowContent = new AddEditAppointment();
            addAppointment.Content = windowContent;
            addAppointment.Show();
        }
    }
}
