using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
namespace EquationDrawerApplication
{
    internal class Transformation
    {
        public double minScreenX { get; set; }
        public double minScreenY { get; set; }
        public double minX { get; set; }
        public double minY { get; set; }
        public double maxScreenX { get; set; }
        public double maxScreenY { get; set; }
        public double maxX { get; set; }
        public double maxY { get; set; }

        public double numPoints { get;}
        public Transformation(Canvas canvas)
        {
            minScreenX = 0;
            minScreenY = 0;
            maxScreenX = canvas.ActualWidth;
            maxScreenY = canvas.ActualHeight;
            numPoints = 10*canvas.ActualWidth;
        }

        public void setInterval(double minX, double maxX, double minY, double maxY)
        {
            this.minX = minX;
            this.maxX = maxX;
            this.minY = minY;
            this.maxY = maxY;
        }

        public double getX(double x)
        {
            return minX + x * (maxX - minX) / numPoints;
        }

        public double getY(double y)
        {
            return minY + y * (maxY - minY) / maxScreenY;
        }

        public double getScreenX(double x)
        {
            return (maxScreenX - minScreenX) * (x - minX) / (maxX - minX) + minScreenX;
        }
        public double getScreenY(double y)
        {
            return (minScreenY - maxScreenY) * (y - minY) / (maxY - minY) + maxScreenY;
        }
    }
}
