using EquationDrawerApplication.Model;
using org.mariuszgromada.math.mxparser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EquationDrawerApplication.ViewModels.Commands
{
    public class AddFunctionCommand : ICommand
    {
        public ViewModelBase ViewModel { get; set; }

        public AddFunctionCommand(ViewModelBase viewModel) {
            this.ViewModel = viewModel;
        }

        //public event EventHandler CanExecuteChanged;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            
            if (parameter != null) {
                Equation equation = parameter as Equation;
                Argument x = new Argument("x");
                Expression e = new Expression(equation.Expression,x);
                
                if (!e.checkSyntax() || String.IsNullOrEmpty(equation.Name))
                    return false;


                return true;
            }
            return false;
        }

        public void Execute(object parameter)
        {
            ViewModel.addFunction(parameter as Equation);
        }
    }
}
