using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using EquationDrawerApplication.Models;
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
        private Data model;
        //private Model model;

        //Delegates
        
        public MainWindow()
        {
            //viewModel = new ViewModelBase();
            InitializeComponent();
            model = Application.Current.Resources["model"] as Data;
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
            transformation.setInterval(model.minX, model.MaxX, model.MinY, model.MaxY);
            
            if (model.wantAxis()) drawAxis();
            if (model.wantGrid()) drawGrid();
            if (model.wantTicks()) drawTicks();
            if (model.wantNumbers()) drawNumbers();
        }

        //Functions Window Listeners
        void onAddFunctionListener(object sender, EventArgs args) {

        }

        //Events
        void onCheckBoxChanged(object sender, EventArgs args) {
            drawChart();
        }
        void onIntervalChanged(object sender, EventArgs args) {
            drawChart();
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
        private void drawGrid()
        {
            double xMin, xMax, yMin, yMax, stepValueX,stepValueY, width, height;
            double widthScreen, heightScreen;
            xMin = model.MinX;
            xMax = model.MaxX;
            yMin = model.MinY;
            yMax = model.MaxY;
            widthScreen = transformation.maxScreenX;
            heightScreen = transformation.maxScreenY;
            width = xMax - xMin;
            height = yMax - yMin;
            stepValueX = width/14;
            stepValueY = height / 10;
            //Eje X
            if (xMin >= 0) {
                for (double i = xMin + stepValueX; i <= width+xMin; i += stepValueX) {
                    Line line = new Line();
                    line.Stroke = Brushes.Gray;
                    line.X1 =transformation.getScreenX(i);
                    line.Y1 = transformation.minScreenY;
                    line.X2 = transformation.getScreenX(i);
                    line.Y2 =heightScreen;
                    canvas.Children.Add(line);
                }
            }
            if (xMin < 0 && xMax > 0)
            {
                for (double i = stepValueX; i <= width; i += stepValueX){ //Grid Vertical Positivo
                    Line line = new Line();
                    line.Stroke = Brushes.Gray;
                    line.X1 = transformation.getScreenX(i);
                    line.Y1 = transformation.minScreenY;
                    line.X2 = transformation.getScreenX(i);
                    line.Y2 = heightScreen;
                    canvas.Children.Add(line);
                }
                for(double i=-stepValueX; i>=xMin;i-=stepValueX){ //Grid Vertical Positivo
                    Line line = new Line();
                    line.Stroke = Brushes.Gray;
                    line.X1 = transformation.getScreenX(i);
                    line.Y1 = transformation.minScreenY;
                    line.X2 = transformation.getScreenX(i);
                    line.Y2 = heightScreen;
                    canvas.Children.Add(line);
                }
            }if(xMax<=0){
                for(double i=xMax-stepValueX; i>=xMin;i-=stepValueX){ //Grid Vertical Positivo
                    Line line = new Line();
                    line.Stroke = Brushes.Gray;
                    line.X1 = transformation.getScreenX(i);
                    line.Y1 = transformation.minScreenY;
                    line.X2 = transformation.getScreenX(i);
                    line.Y2 = heightScreen;
                    canvas.Children.Add(line);
                }
            }

            //Eje Y
            if (yMin >= 0)
            {
                for (double i = yMin + stepValueY; i <= height + yMin; i += stepValueY)
                { //Grid Vertical Positivo
                    Line line = new Line();
                    line.Stroke = Brushes.Gray;
                    line.X1 = transformation.minScreenX;
                    line.Y1 = transformation.getScreenY(i);
                    line.X2 = widthScreen;
                    line.Y2 = transformation.getScreenY(i);
                    canvas.Children.Add(line);
                }
            }if(yMin<0 && yMax>0){
                for(double i =stepValueY; i<=height;i+=stepValueY){ //Grid Vertical Positivo
                    Line line = new Line();
                    line.Stroke = Brushes.Gray;
                    line.X1 = transformation.minScreenX;
                    line.Y1 = transformation.getScreenY(i);
                    line.X2 = widthScreen;
                    line.Y2 = transformation.getScreenY(i);
                    canvas.Children.Add(line);
                }
                for(double i =-stepValueY; i>=yMin;i-=stepValueY){ //Grid Vertical Positivo
                    Line line = new Line();
                    line.Stroke = Brushes.Gray;
                    line.X1 = transformation.minScreenX;
                    line.Y1 = transformation.getScreenY(i);
                    line.X2 = widthScreen;
                    line.Y2 = transformation.getScreenY(i);
                    canvas.Children.Add(line);
                }
            }if(yMax<=0){
                for(double i =yMax-stepValueY; i>=yMin;i-=stepValueY){ //Grid Vertical Positivo
                    Line line = new Line();
                    line.Stroke = Brushes.Gray;
                    line.X1 = transformation.minScreenX;
                    line.Y1 = transformation.getScreenY(i);
                    line.X2 = widthScreen;
                    line.Y2 = transformation.getScreenY(i);
                    canvas.Children.Add(line);
                }
            }
        }



        
        private void drawTicks() {
            double xMin, xMax, yMin, yMax, stepValueX, stepValueY, width, height;
            double widthScreen, heightScreen;
            xMin = model.MinX;
            xMax = model.MaxX;
            yMin = model.MinY;
            yMax = model.MaxY;
            widthScreen = transformation.maxScreenX;
            heightScreen = transformation.maxScreenY;
            width = xMax - xMin;
            height = yMax - yMin;
            stepValueX = width / 14;
            stepValueY = height / 10;
            //Eje X
            double yValue;
            if (yMin <= 0 && yMax > 0) yValue = 0;
            else if (yMin > 0) yValue = yMin;
            else yValue = yMax;
            if (xMin >= 0)
            {
                for (double i = xMin + stepValueX; i <= width + xMin; i += stepValueX)
                {
                    Line line = new Line();
                    line.Stroke = Brushes.Black;
                    line.X1 = transformation.getScreenX(i);
                    line.Y1 = transformation.getScreenY(yValue - stepValueY/5);
                    line.X2 = transformation.getScreenX(i);
                    line.Y2 = transformation.getScreenY(yValue+stepValueY / 5);
                    canvas.Children.Add(line);
                }
            }
            if (xMin < 0 && xMax > 0)
            {
                for (double i = stepValueX; i <= width; i += stepValueX)
                { //Grid Vertical Positivo
                    Line line = new Line();
                    line.Stroke = Brushes.Black;
                    line.X1 = transformation.getScreenX(i);
                    line.Y1 = transformation.getScreenY(yValue - stepValueY / 5);
                    line.X2 = transformation.getScreenX(i);
                    line.Y2 = transformation.getScreenY(yValue+stepValueY / 5);
                    canvas.Children.Add(line);
                }
                for (double i = -stepValueX; i >= xMin; i -= stepValueX)
                { //Grid Vertical Positivo
                    Line line = new Line();
                    line.Stroke = Brushes.Black;
                    line.X1 = transformation.getScreenX(i);
                    line.Y1 = transformation.getScreenY(yValue - stepValueY / 5);
                    line.X2 = transformation.getScreenX(i);
                    line.Y2 = transformation.getScreenY(yValue+stepValueY / 5);
                    canvas.Children.Add(line);
                }
            }
            if (xMax <= 0)
            {
                for (double i = xMax - stepValueX; i >= xMin; i -= stepValueX)
                { //Grid Vertical Positivo
                    Line line = new Line();
                    line.Stroke = Brushes.Black;
                    line.X1 = transformation.getScreenX(i);
                    line.Y1 = transformation.getScreenY(yValue - stepValueY / 5);
                    line.X2 = transformation.getScreenX(i);
                    line.Y2 = transformation.getScreenY(yValue+stepValueY / 5);
                    canvas.Children.Add(line);
                }
            }

            //Eje Y
            double xValue;
            if (xMin <= 0 && xMax > 0) xValue = 0;
            else if (xMin > 0) xValue = xMin;
            else xValue = xMax;
            if (yMin >= 0)
            {
                for (double i = yMin + stepValueY; i <= height + yMin; i += stepValueY)
                { //Grid Vertical Positivo
                    Line line = new Line();
                    line.Stroke = Brushes.Black;
                    line.X1 = transformation.getScreenX(xValue - stepValueX/5);
                    line.Y1 = transformation.getScreenY(i);
                    line.X2 = transformation.getScreenX(xValue + stepValueX / 5);
                    line.Y2 = transformation.getScreenY(i);
                    canvas.Children.Add(line);
                }
            }
            if (yMin < 0 && yMax > 0)
            {
                
                
                for (double i = stepValueY; i <= height; i += stepValueY)
                { //Grid Vertical Positivo
                    Line line = new Line();
                    line.Stroke = Brushes.Black;
                    line.X1 = transformation.getScreenX(xValue - stepValueX / 5);
                    line.Y1 = transformation.getScreenY(i);
                    line.X2 = transformation.getScreenX(xValue+stepValueX / 5);
                    line.Y2 = transformation.getScreenY(i);
                    canvas.Children.Add(line);
                }
                for (double i = -stepValueY; i >= yMin; i -= stepValueY)
                { //Grid Vertical Positivo
                    Line line = new Line();
                    line.Stroke = Brushes.Black;
                    line.X1 = transformation.getScreenX(xValue - stepValueX / 5);
                    line.Y1 = transformation.getScreenY(i);
                    line.X2 = transformation.getScreenX(xValue+stepValueX / 5);
                    line.Y2 = transformation.getScreenY(i);
                    canvas.Children.Add(line);
                }
            }
            if (yMax <= 0)
            {
                for (double i = yMax - stepValueY; i >= yMin; i -= stepValueY)
                { //Grid Vertical Positivo
                    Line line = new Line();
                    line.Stroke = Brushes.Black;
                    line.X1 = transformation.getScreenX(xValue - stepValueX / 5);
                    line.Y1 = transformation.getScreenY(i);
                    line.X2 = transformation.getScreenX(xValue+stepValueX / 5);
                    line.Y2 = transformation.getScreenY(i);
                    canvas.Children.Add(line);
                }
            }
        }
        private void drawNumbers() {
            double xMin, xMax, yMin, yMax, stepValueX, stepValueY, width, height;
            double widthScreen, heightScreen;
            xMin = model.MinX;
            xMax = model.MaxX;
            yMin = model.MinY;
            yMax = model.MaxY;
            widthScreen = transformation.maxScreenX;
            heightScreen = transformation.maxScreenY;
            width = xMax - xMin;
            height = yMax - yMin;
            stepValueX = width / 14;
            stepValueY = height / 10;
            //Eje X
            double yValue;
            if (yMin < 0 && yMax > 0) yValue = 0;
            else if (yMin >= 0) yValue = yMin+ 4*(stepValueY/5);
            else yValue = yMax;
            if (xMin >= 0)
            {
                for (double i = xMin + stepValueX; i <= width + xMin; i += stepValueX)
                {
                    TextBlock number = new TextBlock();
                    number.Text = i.ToString("#.##");
                    number.Foreground = new SolidColorBrush(Colors.Black);
                    Canvas.SetLeft(number, transformation.getScreenX(i - stepValueX / 5));
                    Canvas.SetTop(number, transformation.getScreenY(yValue - stepValueY / 5));
                    canvas.Children.Add(number);
                }
            }
            if (xMin < 0 && xMax > 0)
            {
                for (double i = stepValueX; i <= width; i += stepValueX)
                { //Grid Vertical Positivo
                    TextBlock number = new TextBlock();
                    number.Text = i.ToString("#.##");
                    number.Foreground = new SolidColorBrush(Colors.Black);
                    Canvas.SetLeft(number, transformation.getScreenX(i - stepValueX / 5));
                    Canvas.SetTop(number, transformation.getScreenY(yValue - stepValueY / 5));
                    canvas.Children.Add(number);
                }
                for (double i = -stepValueX; i >= xMin; i -= stepValueX)
                { //Grid Vertical Positivo
                    TextBlock number = new TextBlock();
                    number.Text = i.ToString("#.##");
                    number.Foreground = new SolidColorBrush(Colors.Black);
                    Canvas.SetLeft(number, transformation.getScreenX(i - stepValueX / 5));
                    Canvas.SetTop(number, transformation.getScreenY(yValue - stepValueY / 5));
                    canvas.Children.Add(number);
                }
            }
            if (xMax <= 0)
            {
                for (double i = xMax - stepValueX; i >= xMin; i -= stepValueX)
                { //Grid Vertical Positivo
                    TextBlock number = new TextBlock();
                    number.Text = i.ToString("#.##");
                    number.Foreground = new SolidColorBrush(Colors.Black);
                    Canvas.SetLeft(number, transformation.getScreenX(i - stepValueX / 5));
                    Canvas.SetTop(number, transformation.getScreenY(yValue - stepValueY / 5));
                    canvas.Children.Add(number);
                }
            }

            //Eje Y
            double xValue;
            if (xMin <= 0 && xMax > 0) xValue = 0;
            else if (xMin > 0) xValue = xMin;
            else xValue = xMax - 4*(stepValueX/5) ;
            if (yMin >= 0)
            {
                for (double i = yMin + stepValueY; i <= height + yMin; i += stepValueY)
                { //Grid Vertical Positivo
                    TextBlock number = new TextBlock();
                    number.Text = i.ToString("#.##");
                    number.Foreground = new SolidColorBrush(Colors.Black);
                    Canvas.SetLeft(number, transformation.getScreenX(xValue + stepValueX / 4));
                    Canvas.SetTop(number, transformation.getScreenY(i + stepValueY / 5));
                    canvas.Children.Add(number);
                }
            }
            if (yMin < 0 && yMax > 0)
            {


                for (double i = stepValueY; i <= height; i += stepValueY)
                { //Grid Vertical Positivo
                    TextBlock number = new TextBlock();
                    number.Text = i.ToString("#.##");
                    number.Foreground = new SolidColorBrush(Colors.Black);
                    Canvas.SetLeft(number, transformation.getScreenX(xValue + stepValueX / 4));
                    Canvas.SetTop(number, transformation.getScreenY(i + stepValueY / 5));
                    canvas.Children.Add(number);
                }
                for (double i = -stepValueY; i >= yMin; i -= stepValueY)
                { //Grid Vertical Positivo
                    TextBlock number = new TextBlock();
                    number.Text = i.ToString("#.##");
                    number.Foreground = new SolidColorBrush(Colors.Black);
                    Canvas.SetLeft(number, transformation.getScreenX(xValue + stepValueX / 4));
                    Canvas.SetTop(number, transformation.getScreenY(i + stepValueY / 5));
                    canvas.Children.Add(number);
                }
            }
            if (yMax <= 0)
            {
                for (double i = yMax - stepValueY; i >= yMin; i -= stepValueY)
                { //Grid Vertical Positivo
                    TextBlock number = new TextBlock();
                    number.Text = i.ToString("#.##");
                    number.Foreground = new SolidColorBrush(Colors.Black);
                    Canvas.SetLeft(number, transformation.getScreenX(xValue + stepValueX / 4));
                    Canvas.SetTop(number, transformation.getScreenY(i + stepValueY / 5));
                    canvas.Children.Add(number);
                }
            }



        }

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
            personalizeWindow.OnCheckBoxEventHandler += onCheckBoxChanged;
            personalizeWindow.OnIntervalEventHandler += onIntervalChanged;
            personalizeWindow.Owner = this;
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
