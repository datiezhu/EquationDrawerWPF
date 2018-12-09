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
        private bool isPointer, isDrag, isRect; 
        //private Model model;

        //Delegates
        
        public MainWindow()
        {
            //viewModel = new ViewModelBase();
            InitializeComponent();
            model = Application.Current.Resources["model"] as Data;
            equations = Application.Current.Resources["ViewModelBase"] as ViewModelBase;
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
                        screenX = transformation.getScreenX(x);
                        screenY = transformation.getScreenY(y);
                        Point point = new Point(screenX, screenY);
                        points.Add(point);
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
                Stroke = Brushes.PaleVioletRed,
                Fill = Brushes.PaleVioletRed,
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
            Debug.WriteLine("dsfdsfdsf: "+isRect);
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


                Debug.WriteLine("START--- X: " + startScreen.X + " , Y: " + startScreen.Y);
                Debug.WriteLine("END--- X: " + startScreen.X + " , Y: " + endScreen.Y);

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
            dlg.FileName = "Image"; // Default file name
            
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
                SaveCanvasToFile(this, canvas, 96, filename);
            }

        }
        public static void SaveCanvasToFile(Window window, Canvas canvas, int dpi, string filename)
        {
            Size size = new Size(canvas.ActualWidth, canvas.ActualHeight);
            canvas.Measure(size);
            //canvas.Arrange(new Rect(size));

            var rtb = new RenderTargetBitmap(
                (int)canvas.ActualWidth, //width
                (int)canvas.ActualHeight, //height
                dpi, //dpi x
                dpi, //dpi y
                PixelFormats.Pbgra32 // pixelformat
                );
            rtb.Render(canvas);

            
            SaveRTBAsPNGBMP(rtb, filename);
        }

        private static void SaveRTBAsPNGBMP(RenderTargetBitmap bmp, string filename)
        {
            var enc = new System.Windows.Media.Imaging.PngBitmapEncoder();
            enc.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(bmp));

            using (var stm = System.IO.File.Create(filename))
            {
                enc.Save(stm);
            }
        }
        private static void SaveRTBAsJPEG(RenderTargetBitmap bmp, string filename)
        {
            var enc = new System.Windows.Media.Imaging.JpegBitmapEncoder();
            enc.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(bmp));

            using (var stm = System.IO.File.Create(filename))
            {
                enc.Save(stm);
            }
        }

        private void onDifferentCursorListener(object sender, RoutedEventArgs e)
        {
            isPointer = isRect = isDrag = false;
            switch((sender as Button).Name)
            {
                case "pointerButton":
                    isPointer = true;
                    break;
                case "dragButton":
                    isDrag = true;
                    break;
                case "rectButton":
                    isRect = true;
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
