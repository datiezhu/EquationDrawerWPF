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
using EquationDrawerApplication.ViewModels;
using org.mariuszgromada.math.mxparser;
using Expression = org.mariuszgromada.math.mxparser.Expression;

namespace EquationDrawerApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Attributes
        private Canvas canvas;
        private Transformation transformation;
        //private Model model;

        //Delegates
        
        public MainWindow()
        {
            //viewModel = new ViewModelBase();
            InitializeComponent();
            //DataContext = viewModel;
          //  model = new Model();
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

        

        private void drawChart() {
            this.canvas = myCanvas;
            canvas.Children.Clear();
            this.transformation = new Transformation(canvas);
            transformation.setInterval(-20, 20, -20, 20);
          //  if (model.wantAxis()) drawAxis();
          //  if (model.wantTicks()) drawTicks();
          //  if (model.wantNumbers()) drawNumbers();
        }

        //Functions Window Listeners
        void onAddFunctionListener(object sender, EventArgs args) {

        }

        //Draw Methods
        private void drawAxis() {
            Line ejeX = new Line();
            Line ejeY = new Line();

            ejeX.Stroke = ejeY.Stroke = Brushes.Black;
            ejeX.X1 = transformation.minScreenX;
            ejeX.Y1 = transformation.getScreenY(0);
            ejeX.X2 = transformation.maxScreenX;
            ejeX.Y2 = transformation.getScreenY(0);


            ejeY.X1 = transformation.getScreenX(0);
            ejeY.Y1 = transformation.minScreenY;
            ejeY.X2 = transformation.getScreenX(0);
            ejeY.Y2 = transformation.maxScreenY;

            canvas.Children.Add(ejeX);
            canvas.Children.Add(ejeY);
        }
        private void drawTicks() { }
        private void drawNumbers() { }

        //Button Listeners
        private void onLoadedCanvas(object sender, RoutedEventArgs e) { this.drawChart(); }
        private void onSizeChanged(object sender, RoutedEventArgs e) { this.drawChart(); }
        private void zoomInButtonListener(object sender, RoutedEventArgs e) {
        }
        private void zoomOutButtonListener(object sender, RoutedEventArgs e){
        }
        private void homeButtonListener(object sender, RoutedEventArgs e){
        }
        private void personalizeInButtonListener(object sender, RoutedEventArgs e){
            PersonalizeWindow personalizeWindow = new PersonalizeWindow();
            personalizeWindow.Show();
        }
        private void functionsButtonListener(object sender, RoutedEventArgs e){
            FunctionWindow functionWindow = new FunctionWindow();
            functionWindow.Show();
        }
        private void exportButtonListener(object sender, RoutedEventArgs e){
        }


       
    }
}
