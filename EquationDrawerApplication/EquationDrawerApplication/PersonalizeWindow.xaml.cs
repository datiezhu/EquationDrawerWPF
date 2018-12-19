using EquationDrawerApplication.Model;
using EquationDrawerApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
    public delegate void OnIntervalEventHandler(object sender, EventArgs args);
    public delegate void OnSliderChangedEventHandler(object sender, EventArgs args);
    public delegate void OnSelectedColorEventHandler(object sender, EventArgs args);
    public delegate void OnClosingPersonalizeWindowEventHandler(object sender, EventArgs args);
    public partial class PersonalizeWindow : Window
    {


        private ViewModelBase equations;
        public event OnCheckBoxEventHandler OnCheckBoxEventHandler;
        public event OnIntervalEventHandler OnIntervalEventHandler;
        public event OnSliderChangedEventHandler OnSliderChangedEventHandler;
        public event OnSelectedColorEventHandler OnSelectedColorEventHandler;
        public event OnClosingPersonalizeWindowEventHandler OnClosingPersonalizeWindowEventHandler;
        public PersonalizeWindow()
        {
            InitializeComponent();
            equations = Application.Current.Resources["ViewModelBase"] as ViewModelBase;
        }

        private void onTableViewSelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            
            if ((sender as ListView).SelectedIndex != -1)
            {
                textBoxName.IsEnabled = true;
                colorPickerFunction.IsEnabled = true;
                sliderFunction.IsEnabled = true;
            }
            else
            {
                textBoxName.IsEnabled = false;
                colorPickerFunction.IsEnabled = false;
                sliderFunction.IsEnabled = false;
            }


        }
        protected virtual void onClosingPersonalizeWindowEventHandler(EventArgs args)
        {
            if (this.OnClosingPersonalizeWindowEventHandler != null) this.OnClosingPersonalizeWindowEventHandler(this, args);
        }

        protected virtual void onSelectedColorEventHandler(EventArgs args) {
            if (this.OnSelectedColorEventHandler != null) this.OnSelectedColorEventHandler(this, args);
        }
        private void selecterColorListener(object sender, RoutedEventArgs args) {
            onSelectedColorEventHandler(null);
        }


        protected virtual void onSliderChangedEventHandler(EventArgs args)
        {
            if (this.OnSliderChangedEventHandler != null) this.OnSliderChangedEventHandler(this, args);
        }
        private void onSliderChangedListener(object sender, RoutedEventArgs args)
        {
            onSliderChangedEventHandler(null);
        }

        protected virtual void onCheckBoxEventHandler(EventArgs args) {
            if (this.OnCheckBoxEventHandler != null) this.OnCheckBoxEventHandler(this, args);
        }
        private void onCheckButtonListener(object sender, RoutedEventArgs args) {
            onCheckBoxEventHandler(null);
        }

        protected virtual void onIntervalEventHandler(EventArgs args)
        {
            if (this.OnIntervalEventHandler != null) this.OnIntervalEventHandler(this, args);
        }
        private void onIntervalListener(object sender, RoutedEventArgs args)
        {
            onIntervalEventHandler(null);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            onClosingPersonalizeWindowEventHandler(null);
        }
    }
}
