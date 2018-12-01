using EquationDrawerApplication.Model;
using EquationDrawerApplication.ViewModels.Commands;
using org.mariuszgromada.math.mxparser;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

 

namespace EquationDrawerApplication.ViewModels
{
    public class ViewModelBase : ObservableCollection<Equation>
    {
        public AddFunctionCommand addFunctionCommand { get; set; }
        public ViewModelBase() {
            this.addFunctionCommand = new AddFunctionCommand(this);
            for (int i = 0; i < 5; i++)
            {
                Add(new Equation()
                {
                    Name = "Name " + i,
                    Expression = "Expression " + i
                });
            }

        }


        public void addFunction(Equation equation) {
            Equation eq = new Equation();
            eq.Name = equation.Name;
            eq.Expression = equation.Expression;
            eq.Color = equation.Color;
            eq.Active = equation.Active;
            eq.Width = equation.Width;
            Add(eq);
           
        }
     
    }
}
