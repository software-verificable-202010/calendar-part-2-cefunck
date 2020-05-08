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
        private const string Blank = " ";
        private const string MondayName = "Lunes";
        private const string TuesdayName = "Martes";
        private const string WednesdayName = "Miércoles";
        private const string ThursdayName = "Jueves";
        private const string FridayName = "Viernes";
        private const string SaturdayName = "Sábado";
        private const string SundayName = "Domingo";
        private const int MondayNumberInweek = 1;
        private const int TuesdayNumberInweek = 2;
        private const int WednesdayNumberInweek = 3;
        private const int ThursdayNumberInweek = 4;
        private const int FridayNumberInweek = 5;
        private const int SaturdayNumberInweek = 6;
        private const int SundayNumberInweek = 7;
        private const int negativeMultiplier = -1;
        const int oldSundayNumber = 0;
        const int newSundayNumber = 7;

        public int Index 
        {
            get;
            set;
        }
        private string DayName 
        { 
            get 
            {
                string dayName = Blank;
                switch (Index)
                {
                    case MondayNumberInweek:
                        dayName = MondayName;
                        break;
                    case TuesdayNumberInweek:
                        dayName = TuesdayName;
                        break;
                    case WednesdayNumberInweek:
                        dayName = WednesdayName;
                        break;
                    case ThursdayNumberInweek:
                        dayName = ThursdayName;
                        break;
                    case FridayNumberInweek:
                        dayName = FridayName;
                        break;
                    case SaturdayNumberInweek:
                        dayName = SaturdayName;
                        break;
                    default:
                        dayName = SundayName;
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
                int dayOfWeek = GetDayNumberInWeek(selectedDate);
                selectedDate = selectedDate.AddDays(negativeMultiplier * dayOfWeek + Index);
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
                return DayName + Blank + DayNumber.ToString(); 
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
            const int fixedRowNumber = 2;
            return fixedRowNumber;
        }
        private int GetColumIndex()
        {
            // HACK: Temporary fix until able to Refactor.
            const int fixedColumnNumber = 0;
            return fixedColumnNumber;
        }
        private int GetRowSpan()
        {
            // HACK: Temporary fix until able to Refactor.
            const int fixedRowSpan = 20;
            return fixedRowSpan;
        }
        private int GetColumnSpan()
        {
            // HACK: Temporary fix until able to Refactor.
            const int fixedColumnSpan = 1;
            return fixedColumnSpan;
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
