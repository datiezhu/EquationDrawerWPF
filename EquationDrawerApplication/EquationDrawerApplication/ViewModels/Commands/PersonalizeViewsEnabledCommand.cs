using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EquationDrawerApplication.ViewModels.Commands
{
    public class PersonalizeViewsEnabledCommand : ICommand
    {
        public ViewModelBase viewmodel { get; set; }
        public PersonalizeViewsEnabledCommand(ViewModelBase viewModel)
        {
            this.viewmodel = viewmodel;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            if (parameter == null)
                return false;
            else
                return true;
        }

        public void Execute(object parameter)
        {
            
        }
    }
}
