using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using EquationDrawerApplication.Model;
using EquationDrawerApplication.Models;
using EquationDrawerApplication.ViewModels;
using Microsoft.Win32;
using org.mariuszgromada.math.mxparser;
using Xceed.Wpf.Toolkit;
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
        private ViewModelBase equations;
        private bool isPointer=true, isDrag, isRect; 
        //private Model model;

        //Delegates
        
        public MainWindow()
        {
            //viewModel = new ViewModelBase();
            InitializeComponent();
            model = Application.Current.Resources["model"] as Data;
            equations = Application.Current.Resources["ViewModelBase"] as ViewModelBase;
            equations.CollectionChanged += this.OnCollectionChanged;
            //DataContext = viewModel;
            //  model = new Model();
        }


        void OnCollectionChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("Holaaaa");
            canvas.Children.Clear();

            drawChart();
            drawEquations();
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
            
            if(model.wantGrid() || model.wantAxis())drawAxis(model.wantAxis());
            if (model.wantGrid()) drawGrid();
            if (model.wantTicks()) drawTicks();
            if (model.wantNumbers()) drawNumbers();
        }

        //Functions Window Listeners
        void onAddFunctionListener(object sender, EventArgs args) {

        }

        //Events
        void onCheckBoxChanged(object sender, EventArgs args) {
            canvas.Children.Clear();
            drawChart();
            drawEquations();
        }
        void onIntervalChanged(object sender, EventArgs args) {
            canvas.Children.Clear();
            drawChart();
            drawEquations();
        }

        void onSliderChanged(object sender, EventArgs args)
        {
            canvas.Children.Clear();
            drawChart();
            drawEquations();
        }
        void onSelectedColorChanged(object sender, EventArgs args)
        {
            canvas.Children.Clear();
            drawChart();
            drawEquations();

        }

        //Draw Methods
        private void drawAxis(bool axis) {
            Line ejeX = new Line();
            Line ejeY = new Line();
            if(axis)
                ejeX.Stroke = ejeY.Stroke = Brushes.White;
            else
                ejeX.Stroke = ejeY.Stroke = Brushes.Gray;
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
                    line.Stroke = Brushes.White;
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
                    line.Stroke = Brushes.White;
                    line.X1 = transformation.getScreenX(i);
                    line.Y1 = transformation.getScreenY(yValue - stepValueY / 5);
                    line.X2 = transformation.getScreenX(i);
                    line.Y2 = transformation.getScreenY(yValue+stepValueY / 5);
                    canvas.Children.Add(line);
                }
                for (double i = -stepValueX; i >= xMin; i -= stepValueX)
                { //Grid Vertical Positivo
                    Line line = new Line();
                    line.Stroke = Brushes.White;
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
                    line.Stroke = Brushes.White;
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
                    line.Stroke = Brushes.White;
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
                    line.Stroke = Brushes.White;
                    line.X1 = transformation.getScreenX(xValue - stepValueX / 5);
                    line.Y1 = transformation.getScreenY(i);
                    line.X2 = transformation.getScreenX(xValue+stepValueX / 5);
                    line.Y2 = transformation.getScreenY(i);
                    canvas.Children.Add(line);
                }
                for (double i = -stepValueY; i >= yMin; i -= stepValueY)
                { //Grid Vertical Positivo
                    Line line = new Line();
                    line.Stroke = Brushes.White;
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
                    line.Stroke = Brushes.White;
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
                    number.Foreground = new SolidColorBrush(Colors.White);
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
                    number.Foreground = new SolidColorBrush(Colors.White);
                    Canvas.SetLeft(number, transformation.getScreenX(i - stepValueX / 5));
                    Canvas.SetTop(number, transformation.getScreenY(yValue - stepValueY / 5));
                    canvas.Children.Add(number);
                }
                for (double i = -stepValueX; i >= xMin; i -= stepValueX)
                { //Grid Vertical Positivo
                    TextBlock number = new TextBlock();
                    number.Text = i.ToString("#.##");
                    number.Foreground = new SolidColorBrush(Colors.White);
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
                    number.Foreground = new SolidColorBrush(Colors.White);
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
                    number.Foreground = new SolidColorBrush(Colors.White);
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
                    number.Foreground = new SolidColorBrush(Colors.White);
                    Canvas.SetLeft(number, transformation.getScreenX(xValue + stepValueX / 4));
                    Canvas.SetTop(number, transformation.getScreenY(i + stepValueY / 5));
                    canvas.Children.Add(number);
                }
                for (double i = -stepValueY; i >= yMin; i -= stepValueY)
                { //Grid Vertical Positivo
                    TextBlock number = new TextBlock();
                    number.Text = i.ToString("#.##");
                    number.Foreground = new SolidColorBrush(Colors.White);
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
                    number.Foreground = new SolidColorBrush(Colors.White);
                    Canvas.SetLeft(number, transformation.getScreenX(xValue + stepValueX / 4));
                    Canvas.SetTop(number, transformation.getScreenY(i + stepValueY / 5));
                    canvas.Children.Add(number);
                }
            }



        }
        private void drawEquations() {
            Function function;
            Point previous= new Point(0,0);
            for (int i = 0; i < equations.Count; i++) {
                if (equations.ElementAt(i).Active){
                    Polyline polyline = new Polyline();
                    Equation equation = equations.ElementAt(i);
                    polyline.Stroke = new SolidColorBrush(equation.Color);
                    polyline.StrokeThickness = equation.Width;
                    double x, y, screenX, screenY;
                    PointCollection points = new PointCollection();
                    function = new Function("f(x)="+equation.Expression);
                    for (int j = 0; j < transformation.numPoints; j++)
                    {
                        x = transformation.getX(j);  
                        y = function.calculate(x);
                        if (!Double.IsNaN(y)) {
                            if (((previous.Y < 0 && y > 0) && (y- previous.Y) >1) || ((previous.Y > 0 && y < 0) && (previous.Y - y) > 1))
                            {
                 
                                polyline.Points = points;
                                canvas.Children.Add(polyline);
                                polyline = new Polyline();
                                polyline.Stroke = new SolidColorBrush(equation.Color);
                                polyline.StrokeThickness = equation.Width;
                                points = new PointCollection();
                                previous = new Point(x, y);
                            }
                            else
                            {
                                screenX = transformation.getScreenX(x);
                                screenY = transformation.getScreenY(y);
                                Point point = new Point(screenX, screenY);
                                previous = new Point(x, y);
                                points.Add(point);
                            }
                        }
                    }
                    polyline.Points = points;
                    canvas.Children.Add(polyline);
                }
            }

        }

        bool isLeftMouseButtonDownOnWindow,dragging=false;
        Point origMouseDownPoint, endMouseDownPoint;
        private Rectangle getRectangle()
        {
            Rectangle rect = new Rectangle()
            {
                Stroke = new SolidColorBrush(Color.FromArgb(150, 237, 59, 109)),
                Fill = new SolidColorBrush(Color.FromArgb(150, 237, 59, 109)),
                StrokeThickness = 2
            };
            
            rect.Width = Math.Abs(origMouseDownPoint.X - endMouseDownPoint.X);
            rect.Height = Math.Abs(origMouseDownPoint.Y - endMouseDownPoint.Y);
            Canvas.SetLeft(rect, Math.Min(origMouseDownPoint.X, endMouseDownPoint.X));
            Canvas.SetTop(rect, Math.Min(origMouseDownPoint.Y, endMouseDownPoint.Y));
            return rect;
        }

        
        private void onMouseDown(object sender, MouseButtonEventArgs e) {
            (sender as Canvas).CaptureMouse();
            if (e.ChangedButton == MouseButton.Left && isDrag) {
                isLeftMouseButtonDownOnWindow = true;
                origMouseDownPoint = e.GetPosition(myCanvas);
                this.Cursor = Cursors.Hand;
            }else if(e.ChangedButton == MouseButton.Left && isRect)
            {
                origMouseDownPoint = endMouseDownPoint= e.GetPosition(myCanvas);
                dragging = true;
            }
        }
        private void onMouseUp(object sender, MouseButtonEventArgs e)
        {
            (sender as Canvas).ReleaseMouseCapture();
            isLeftMouseButtonDownOnWindow = false;
            dragging = false;
            this.Cursor = Cursors.Arrow;
            //Debug.WriteLine("dsfdsfdsf: "+isRect);
            if (isRect)
            {
               
                endMouseDownPoint = e.GetPosition(myCanvas);
                Point startScreen, endScreen;
                startScreen = new Point();
                endScreen = new Point();

                startScreen.X = transformation.getX(origMouseDownPoint.X);
                startScreen.Y = transformation.getY(origMouseDownPoint.Y);
                endScreen.X = transformation.getX(endMouseDownPoint.X);
                endScreen.Y = transformation.getY(endMouseDownPoint.Y);

                /*Debug.WriteLine("START PANT--- X: " + origMouseDownPoint.X + " , Y: " + origMouseDownPoint.Y);
                Debug.WriteLine("END PANT--- X: " + endMouseDownPoint.X + " , Y: " + endMouseDownPoint.Y);
                Debug.WriteLine("START--- X: " + startScreen.X + " , Y: " + startScreen.Y);
                Debug.WriteLine("END--- X: " + endScreen.X + " , Y: " + endScreen.Y);*/

                model.resize(startScreen, endScreen);
                canvas.Children.Clear();
                drawChart();
                drawEquations();
            }

        }
        private void onMouseMoved(object sender, MouseEventArgs e)
        {
            if (isLeftMouseButtonDownOnWindow && isDrag){
                Point current = e.GetPosition(myCanvas);
                //Debug.WriteLine("Draggin\n");

                model.drag(origMouseDownPoint, current);
                canvas.Children.Clear();
                drawChart();
                drawEquations();

            }else if(isRect && dragging)
            {
                endMouseDownPoint = e.GetPosition(myCanvas);
                //Debug.WriteLine("Draggin\n");
                if (canvas != null) canvas.Children.Clear();
                drawChart();
                drawEquations();
                myCanvas.Children.Add(getRectangle());
            }
        }
        private void MyCanvas_MouseEnter(object sender, MouseEventArgs e)
        {
            buttonStackPanel.Visibility = Visibility.Visible;
            if(isRect) this.Cursor = Cursors.Cross;
            if (isDrag) this.Cursor = Cursors.Hand;
            if(isPointer) this.Cursor = Cursors.Arrow;


        }
        private void MyCanvas_MouseLeave(object sender, MouseEventArgs e)
        {
            buttonStackPanel.Visibility = Visibility.Collapsed;
            this.Cursor = Cursors.Arrow;

        }
        private void MyStack_MouseEnter(object sender, MouseEventArgs e)
        {
            buttonStackPanel.Visibility = Visibility.Visible;
        }
        private void MyStack_MouseLeave(object sender, MouseEventArgs e)
        {
            //buttonStackPanel.Visibility = Visibility.Hidden;
        }



        private void onTouchEllipseEvent(object sender, RoutedEventArgs e) {
        }
        private void onLoadedCanvas(object sender, RoutedEventArgs e) {
            this.drawChart(); }
        private void onSizeChanged(object sender, RoutedEventArgs e) {
            if(canvas!=null)canvas.Children.Clear();
            drawChart();
            drawEquations();
        }
        private void functionCheckBoxListener(object sender, RoutedEventArgs e) {
            canvas.Children.Clear();
            drawChart();
            this.drawEquations();
        }
        
        private void zoomInButtonListener(object sender, RoutedEventArgs e) {
            model.zoomIn();
            canvas.Children.Clear();
            drawChart();
            drawEquations();
        }
        private void zoomOutButtonListener(object sender, RoutedEventArgs e){
            model.zoomOut();
            canvas.Children.Clear();
            drawChart();
            drawEquations();
        }
        private void homeButtonListener(object sender, RoutedEventArgs e){
            model.goHome();
            canvas.Children.Clear();
            drawChart();
            drawEquations();
        }

        

        private void personalizeInButtonListener(object sender, RoutedEventArgs e){
            PersonalizeWindow personalizeWindow = new PersonalizeWindow();
            personalizeWindow.OnCheckBoxEventHandler += onCheckBoxChanged;
            personalizeWindow.OnIntervalEventHandler += onIntervalChanged;
            personalizeWindow.OnSliderChangedEventHandler += onSliderChanged;
            personalizeWindow.OnSelectedColorEventHandler += onSelectedColorChanged;
            personalizeWindow.Owner = this;
            personalizeWindow.Show();
        }
        private void functionsButtonListener(object sender, RoutedEventArgs e){
            FunctionWindow functionWindow = new FunctionWindow();
            functionWindow.Show();
        }
        private void exportButtonListener(object sender, RoutedEventArgs e){

            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Grafica_"+DateTime.Today.ToString("dd_MM_yyyy_") + DateTime.Now.ToString("HH:mm:ss"); // Default file name
            
            dlg.DefaultExt = ".png"; // Default file extension
            dlg.Filter = "PNG|*.png|GIF|*.gif|BMP|*.bmp|JPEG|*.jpg;*.jpeg"; // Filter files by extension
            

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string filename = dlg.FileName;
                int extension = dlg.FilterIndex;

                Rect bounds = VisualTreeHelper.GetDescendantBounds(myCanvas);
                double dpi = 96d;


                RenderTargetBitmap rtb = new RenderTargetBitmap((int)bounds.Width, (int)bounds.Height, dpi, dpi, System.Windows.Media.PixelFormats.Default);


                DrawingVisual dv = new DrawingVisual();
                using (DrawingContext dc = dv.RenderOpen())
                {
                    VisualBrush vb = new VisualBrush(myCanvas);
                    dc.DrawRectangle(vb, null, new Rect(new Point(), bounds.Size));
                }

                rtb.Render(dv);

                BitmapEncoder pngEncoder = new PngBitmapEncoder();
                pngEncoder.Frames.Add(BitmapFrame.Create(rtb));

                try
                {
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();

                    pngEncoder.Save(ms);
                    ms.Close();

                    System.IO.File.WriteAllBytes(filename, ms.ToArray());
                }
                catch (Exception err)
                {
                    System.Windows.MessageBox.Show(err.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                //SaveCanvasToFile(this, canvas, 96, filename);
            }

        }

        

        

       

        private void onDifferentCursorListener(object sender, RoutedEventArgs e)
        {
            if(isPointer)cursorImage.Source = new BitmapImage((new Uri("resources/cursor.png", UriKind.Relative)));
            if(isDrag)moveImage.Source = new BitmapImage((new Uri("resources/move.png", UriKind.Relative)));
            if(isRect)rectImage.Source = new BitmapImage((new Uri("resources/zoomRectt.png", UriKind.Relative)));
            isPointer = isRect = isDrag = false;

            switch ((sender as Button).Name)
            {
                case "pointerButton":
                    isPointer = true;
                    cursorImage.Source = new BitmapImage((new Uri("resources/cursorPressed.png", UriKind.Relative)));
                    this.Cursor = Cursors.Arrow;
                    break;
                case "dragButton":
                    isDrag = true;
                    this.Cursor = Cursors.Hand;
                    moveImage.Source = new BitmapImage((new Uri("resources/movePressed.png", UriKind.Relative)));
                    break;
                case "rectButton":
                    isRect = true;
                    this.Cursor = Cursors.Cross;
                    rectImage.Source = new BitmapImage((new Uri("resources/zoomRectPressed.png", UriKind.Relative)));
                    break;

            }
        }
    }
}

/*
 * 
 * 
 * WindowClosed----- Application.Current.Shutdowm
 * */
