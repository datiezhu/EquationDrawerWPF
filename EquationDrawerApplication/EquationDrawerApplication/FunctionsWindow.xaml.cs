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
    /// Interaction logic for PreferencesWindow.xaml
    /// </summary>
    public delegate void OnButtonPressedEventHandler(object sender, EventArgs args);
    public partial class PreferencesWindow : Window
    {
        public event OnButtonPressedEventHandler eventHandler;
        public PreferencesWindow()
        {
            InitializeComponent();
        }

        private void PreferenceButton_Click(object sender, RoutedEventArgs e)
        {
            onEventHandler(null);
        }

        protected virtual void onEventHandler(EventArgs args) {
            if (this.eventHandler != null) this.eventHandler(this, args);
        }
    }
}
