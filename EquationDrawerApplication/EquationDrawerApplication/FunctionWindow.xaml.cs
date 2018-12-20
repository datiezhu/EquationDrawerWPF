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
using System.Windows.Shapes;

namespace EquationDrawerApplication
{
    /// <summary>
    /// Interaction logic for FunctionWindow.xaml
    /// </summary>
    public delegate void OnClosingWindowEventHandler(object sender, EventArgs args);

    public partial class FunctionWindow : Window
    {
        public event OnClosingWindowEventHandler OnClosingWindowEventHandler;
        public FunctionWindow()
        {
            InitializeComponent();
        }

        private void onColorChanged(object sender, RoutedEventArgs args){
            

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            onClosingWindowEventHandler(null);
        }

        protected virtual void onClosingWindowEventHandler(EventArgs args)
        {
            if (this.OnClosingWindowEventHandler != null) this.OnClosingWindowEventHandler(this, args);
        }

    }
}
