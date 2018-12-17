using org.mariuszgromada.math.mxparser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace EquationDrawerApplication.Model
{
    public class Equation : INotifyPropertyChanged
    {
        private String name;

        public String Name {
            get { return name; }
            set { name = value; OnPropertyChanged("Name"); }
        }

        private String expression;

        public String Expression {
            get { return expression; }
            set { expression = value; OnPropertyChanged("Expression"); }
        }

        public Color color;
        public Color Color
        {
            get { return color; }
            set { color = value; OnPropertyChanged("Color"); }
        }

        public double width;
        public double Width
        {
            get { return width; }
            set { width = value; OnPropertyChanged("Width"); }
        }

        public bool active;
        public bool Active {
            get { return active; }
            set { active = value; OnPropertyChanged("Active"); }
        }



        public Equation() {
            Random random = new Random();
            Name="Funcion";
            Expression ="";
            Active = false;
            Width = 3;
            Color = Color.FromRgb((byte)random.Next(0,256), (byte)random.Next(0, 256), (byte)random.Next(0, 256));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
