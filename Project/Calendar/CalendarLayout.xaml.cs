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
        const string CurrentCalendarViewOptionResourceName = "CurrentCalendarViewOption";
        public CalendarLayout()
        {
            InitializeComponent();
            createAndInsertContentControlToGrid();
            
            //this.Resources.Add("bodyContent", bodyContentControl);
            //string BodyContentResourceName = "bodyContent";
            //BodyContent.SetResourceReference(ContentControl.ContentProperty, BodyContentResourceName);


        }

        public void createAndInsertContentControlToGrid() 
        {
            ContentControl bodyContentControl = new ContentControl();
            bodyContentControl.SetValue(Grid.ColumnProperty, 0);
            bodyContentControl.SetValue(Grid.RowProperty, 1);
            bodyContentControl.Name = "BodyContent";
            string dayNumberResourceKey = "bodyContent";
            bodyContentControl.SetResourceReference(ContentControl.ContentProperty, dayNumberResourceKey);
            LayoutGrid.Children.Add(bodyContentControl);       
        }
    }
}
