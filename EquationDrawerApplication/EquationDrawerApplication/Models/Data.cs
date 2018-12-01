using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

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
            backgroundColor = Color.FromRgb(2, 255, 0);
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


        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
