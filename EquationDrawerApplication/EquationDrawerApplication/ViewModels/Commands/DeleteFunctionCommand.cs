using EquationDrawerApplication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EquationDrawerApplication.ViewModels.Commands
{
    public class DeleteFunctionCommand : ICommand
    {
        public ViewModelBase ViewModel { get; set; }
        public DeleteFunctionCommand(ViewModelBase viewModel) {
            this.ViewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {

            ViewModel.removeFunction(parameter as Equation);
        }
    }
}
