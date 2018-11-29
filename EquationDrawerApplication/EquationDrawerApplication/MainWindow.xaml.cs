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
using org.mariuszgromada.math.mxparser;
using Expression = org.mariuszgromada.math.mxparser.Expression;

namespace EquationDrawerApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }



       


        void eventHandler(object sender, EventArgs args) {
            
        }

        private void ToolBar_Loaded(object sender, RoutedEventArgs e)
        {
            ToolBar toolBar = sender as ToolBar;
            var overflowGrid = toolBar.Template.FindName("OverflowGrid", toolBar) as FrameworkElement;
            if (overflowGrid != null)
            {
                overflowGrid.Visibility = Visibility.Collapsed;
            }

            var mainPanelBorder = toolBar.Template.FindName("MainPanelBorder", toolBar) as FrameworkElement;
            if (mainPanelBorder != null)
            {
                mainPanelBorder.Margin = new Thickness(0);
            }
        }


        private void zoomInButtonListener(object sender, RoutedEventArgs e) {
        }
        private void zoomOutButtonListener(object sender, RoutedEventArgs e){
        }
        private void homeButtonListener(object sender, RoutedEventArgs e){
        }
        private void personalizeInButtonListener(object sender, RoutedEventArgs e){
        }
        private void functionsButtonListener(object sender, RoutedEventArgs e){
        }
        private void exportButtonListener(object sender, RoutedEventArgs e){
        }


       
    }
}
