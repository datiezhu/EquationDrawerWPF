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
    /// Interaction logic for PersonalizeWindow.xaml
    /// </summary>
    public delegate void OnCheckBoxEventHandler(object sender, EventArgs args);
    public partial class PersonalizeWindow : Window
    {
        public event OnCheckBoxEventHandler OnCheckBoxEventHandler;
        public PersonalizeWindow()
        {
            InitializeComponent();
        }


        protected virtual void onCheckBoxEventHandler(EventArgs args) {
            if (this.OnCheckBoxEventHandler != null) this.OnCheckBoxEventHandler(this, args);
        }
        private void onCheckButtonListener(object sender, RoutedEventArgs args) {
            onCheckBoxEventHandler(null);
        }
    }
}
