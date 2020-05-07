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
        private const string BlankSpaceToSeparate = " ";
        public int Index 
        {
            get;
            set;
        }
        private string DayName 
        { 
            get 
            {
                string dayName = "";
                switch (Index)
                {
                    case 1:
                        dayName = "Lunes";
                        break;
                    case 2:
                        dayName = "Martes";
                        break;
                    case 3:
                        dayName = "Miércoles";
                        break;
                    case 4:
                        dayName = "Jueves";
                        break;
                    case 5:
                        dayName = "Viernes";
                        break;
                    case 6:
                        dayName = "Sábado";
                        break;
                    default:
                        dayName = "Domingo";
                        break;
                }
                return dayName;
            } 
        }
        private string DayNumber 
        { 
            get 
            {
                DateTime now = GetDisplayedDateResourceValue();
                int dayOfWeek = (int)now.DayOfWeek;
                if (dayOfWeek == 0)
                {
                    dayOfWeek = 7;
                }
                now = now.AddDays(-1*dayOfWeek+Index);
                return now.Day.ToString();
            } 
        }
        private string TitleResourceKey 
        {
            get 
            {
                return "WeekColumnTitle" + Index;
            }
        }
        private string TitleResourceValue 
        { 
            get 
            { 
                return DayName + BlankSpaceToSeparate + DayNumber ; 
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
    }
}
