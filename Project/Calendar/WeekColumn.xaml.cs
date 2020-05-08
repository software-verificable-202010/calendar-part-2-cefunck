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
        private const string ColumnTitleResourceKeyPrefix = "WeekColumnTitle";
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
            AssingValuesToTitleResource(GetDisplayedDateResourceValue());
            CreateAndInsertTitleElementToColumn();
        }
        private void GenerateTitleResource()
        {
            if (App.Current.Resources[TitleResourceKey] == null)
            {
                App.Current.Resources.Add(TitleResourceKey, TitleResourceValue);
            }
        }
        private void AssingValuesToTitleResource(DateTime displayedDate)
        {
            App.Current.Resources[TitleResourceKey] = TitleResourceValue;
        }

        private void AssingValuesToAppointmensResource() 
        {
            List<Appointment> appointResourceValue = new List<Appointment>();
            appointResourceValue.Add(new Appointment("Título", "Descripción", new DateTime(2020,5,7,19,00,00), new DateTime(2020, 5, 7, 19, 30, 00)));
            App.Current.Resources["AppointmentsResource"] = appointResourceValue;
        }
        private void CreateAndInsertAppointmentElements()
        {
            foreach (Appointment appointment in (List<Appointment>)App.Current.Resources["AppointmentsResource"])
            {
                if (appointment.End.Day == getDateOfColumn().Day)
                {
                    int row = GetRowIndex(appointment.Start);
                    int column = GetColumIndex();
                    int rowSpan = GetRowSpan(appointment.Start, appointment.End);
                    int columnSpan = GetColumnSpan();
                    Border appointmentBorder = new Border();
                    //appointmentBorder.Name = "AppointmentBorder"();
                    appointmentBorder.SetValue(Grid.ColumnProperty, column);
                    appointmentBorder.SetValue(Grid.RowProperty, row);
                    appointmentBorder.SetValue(Grid.ColumnSpanProperty, columnSpan);
                    appointmentBorder.SetValue(Grid.RowSpanProperty, rowSpan);
                    WeekColumnGrid.Children.Add(appointmentBorder);

                    TextBlock appointmentText = new TextBlock();
                    //appointmentText.Name = "AppointmentTitle" + Index.ToString();
                    //appointmentText.SetResourceReference(TextBlock.TextProperty, appointment.Title);
                    appointmentText.SetValue(TextBlock.TextProperty, appointment.Title);
                    appointmentText.SetValue(Grid.ColumnProperty, column);
                    appointmentText.SetValue(Grid.RowProperty, row);
                    appointmentText.SetValue(Grid.ColumnSpanProperty, columnSpan);
                    appointmentText.SetValue(Grid.RowSpanProperty, rowSpan);
                    WeekColumnGrid.Children.Add(appointmentText);
                }
            }
        }

        private void CreateAndInsertTitleElementToColumn()
        {
            TextBlock titleElement = new TextBlock();
            titleElement.Name = "Title"+Index.ToString();
            titleElement.SetResourceReference(TextBlock.TextProperty, TitleResourceKey);
            titleElement.SetValue(Grid.ColumnProperty,0);
            titleElement.SetValue(Grid.RowProperty,0);
            titleElement.SetValue(Grid.ColumnSpanProperty,2);
            titleElement.SetValue(Grid.RowSpanProperty,2);
            WeekColumnGrid.Children.Add(titleElement);
        }
        private DateTime GetDisplayedDateResourceValue()
        {
            return (DateTime)App.Current.Resources[DisplayedDateResourceName];
        }
        private DateTime getDateOfColumn() 
        {
            DateTime displayedDate = GetDisplayedDateResourceValue();
            int year = displayedDate.Year;
            int month = displayedDate.Month;
            int day = (int)DayNumber;
            return new DateTime(year,month,day);
        }
        private int GetRowIndex(DateTime start)
        {
            int row = 2;
            return row;
        }
        private int GetColumIndex()
        {
            int column = 0;
            return column;
        }
        private int GetRowSpan(DateTime start, DateTime end)
        {
            int rowSpan = 20;
            return rowSpan;
        }
        private int GetColumnSpan()
        {
            int columnSpan = 1;
            return columnSpan;
        }
        private void AddAppointment_Click(object sender, RoutedEventArgs e)
        {
            int row = 100;
            Button b = (Button)sender;
            row = (int)b.GetValue(Grid.RowProperty);
            MessageBox.Show("row is: "+row);
            Window addAppointment = new Window();
            AddEditAppointment windowContent = new AddEditAppointment();
            addAppointment.Content = windowContent;
            addAppointment.Height = 400;
            addAppointment.Width = 800;
            addAppointment.Show();
        }
        private int GetDayNumberInWeek(DateTime date) 
        {
            const int oldSundayNumber = 0;
            const int newSundayNumber = 7;
            int dayNumber = (int)date.DayOfWeek;
            if (dayNumber == oldSundayNumber)
            {
                dayNumber = newSundayNumber;
            }
            return dayNumber;
        }
    }
}
