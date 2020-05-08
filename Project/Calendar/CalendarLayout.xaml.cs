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
    /// Lógica de interacción para CalendarLayout.xaml
    /// </summary>
    public partial class CalendarLayout : UserControl
    {
        private const int BodyGridRow = 1;
        private const int BodyGridColumn = 0;
        private const string DayNumberResourceKey = "bodyContentResourceKey";

        public CalendarLayout()
        {
            InitializeComponent();
            ContentControl calendarBody = CreateBodyContentControl();
            InsertBodyContentControlToGrid(calendarBody);
        }

        public ContentControl CreateBodyContentControl()
        {

            ContentControl bodyContentControl = new ContentControl();
            bodyContentControl.SetValue(Grid.ColumnProperty, BodyGridColumn);
            bodyContentControl.SetValue(Grid.RowProperty, BodyGridRow);
            bodyContentControl.SetResourceReference(ContentControl.ContentProperty, DayNumberResourceKey);
            return bodyContentControl;
        }

        public void InsertBodyContentControlToGrid(ContentControl calendarBody)
        {
            LayoutGrid.Children.Add(calendarBody);
        }
    }
}
