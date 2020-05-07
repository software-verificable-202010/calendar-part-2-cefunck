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
    /// Lógica de interacción para WeekBody.xaml
    /// </summary>
    public partial class WeekBody : UserControl
    {
        const string WeekColumnNamePrefix = "weekColumn";
        private const string WeekColumnTitleResourceKeyPrefix = "WeekColumnTiResource";

        public WeekBody()
        {
            InitializeComponent();
            //AssingValuesToWeekColumnTitleResources();
            createAndInsertDayColumns(); 
            
        }
        public void createAndInsertDayColumns()
        {
            for (int weekColumnIndex = 1; weekColumnIndex <= 7; weekColumnIndex++)
            {
                WeekColumn weekColumnElement = new WeekColumn(weekColumnIndex);
                //weekColumnElement.Name = WeekColumnNamePrefix + weekColumnIndex.ToString();
                string weekColumnTitleResourceKey = WeekColumnTitleResourceKeyPrefix + weekColumnIndex.ToString();
                //weekColumnElement.SetResourceReference(WeekColumn.TitleProperty, weekColumnTitleResourceKey);
                weekColumnElement.SetValue(Grid.ColumnProperty, weekColumnIndex);
                WeekBodyGrid.Children.Add(weekColumnElement);
            }
        }
        /*


        
        private void AssingValuesToWeekColumnTitleResources()
        {
            for (int weekColumnIndex = 1; weekColumnIndex <= 7; weekColumnIndex++)
            {
                string weekColumnTitleResourceKey = WeekColumnTitleResourceKeyPrefix + weekColumnIndex.ToString();
                string weekColumnTitleResourceValue = "asdasd 2";
                App.Current.Resources[weekColumnTitleResourceKey] = weekColumnTitleResourceValue;
            }
        }
         */
    }
}
