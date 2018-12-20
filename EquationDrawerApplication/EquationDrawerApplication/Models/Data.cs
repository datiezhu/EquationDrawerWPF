using EquationDrawerApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace EquationDrawerApplication.Models
{
    public class Data : INotifyPropertyChanged
    {
        public bool axis;
        public bool Axis {
            get { return axis; }
            set { axis = value; OnPropertyChanged("Axis"); }
        }
        public bool tick;
        public bool Tick
        {
            get { return tick; }
            set { tick = value; OnPropertyChanged("Tick"); }
        }
        public bool numbers;
        public bool Numbers
        {
            get { return numbers; }
            set { numbers = value; OnPropertyChanged("Numbers"); }
        }
        public bool grid;
        public bool Grid
        {
            get { return grid; }
            set { grid = value; OnPropertyChanged("Grid"); }
        }

        public double minX, minY, maxX, maxY;

        public double MinX {
            get { return minX; }
            set { minX = value; OnPropertyChanged("MinX"); }
        }
        public double MinY
        {
            get { return minY; }
            set { minY = value; OnPropertyChanged("MinY"); }
        }
        public double MaxX
        {
            get { return maxX; }
            set { maxX = value; OnPropertyChanged("MaxX"); }
        }
        public double MaxY
        {
            get { return maxY; }
            set { maxY = value; OnPropertyChanged("MaxY"); }
        }

        public Color backgroundColor;
        public Color BackgroundColor
        {
            get { return backgroundColor; }
            set { backgroundColor = value; OnPropertyChanged("BackgroundColor"); }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public Data()
        {
            Axis = Tick = Numbers = Grid = true;
            minX = minY = -20;
            maxX = maxY = 20;
            backgroundColor = Color.FromRgb(55, 55, 55);
        }

        public bool wantAxis()
        {
            return Axis;
        }
        public bool wantGrid()
        {
            return Grid;
        }
        public bool wantTicks()
        {
            return Tick;
        }
        public bool wantNumbers()
        {
            return Numbers;
        }

        public void goHome() {
            minX = minY = -20;
            maxX = maxY = 20;    
        }
        public void zoomIn(float value) {
            minX /= value;
            minY /= value;
            maxX /= value;
            maxY /= value;
        }

        public void resize(Point start, Point end)
        {
                maxX = Math.Max(start.X, end.X);
                maxY = Math.Max(start.Y, end.Y);
                minX = Math.Min(start.X, end.X);
                minY = Math.Min(start.Y, end.Y);
        }
        public void drag(Point start, Point end) {
            Debug.WriteLine("Xmin: " + minX + "Xmax: " + maxX + "Ymin: " + minY + "Ymax: " + maxY);
            if (start.X < end.X && Math.Abs(end.X-start.X)> 15)
            { 
                maxX -= 0.60 * (maxX-minX)/100;
                minX -= 0.60 * (maxX - minX) / 100;
            }
            else if(start.X > end.X && Math.Abs(end.X - start.X) > 15)
            {
                maxX += 0.60 * (maxX - minX) / 100;
                minX += 0.60 * (maxX - minX) / 100;
            }

            if (start.Y < end.Y && Math.Abs(end.Y - start.Y) > 15)
            {
                maxY += 0.60 * (maxY - minY) / 100;
                minY += 0.60 * (maxY - minY) / 100;
            }
            else if (start.Y > end.Y && Math.Abs(end.Y - start.Y) > 15)
            {
                maxY -= 0.60 * (maxY - minY) / 100;
                minY -= 0.60 * (maxY - minY) / 100;
            }
            Debug.WriteLine("Xmin: " + minX + "Xmax: " + maxX + "Ymin: " + minY + "Ymax: " + maxY);

        }
        public void zoomOut(float value)
        {
            minX *= value;
            minY *= value;
            maxX *= value;
            maxY *= value;
        }
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
