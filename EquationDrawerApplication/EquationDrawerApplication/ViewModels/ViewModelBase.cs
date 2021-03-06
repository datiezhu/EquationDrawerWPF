﻿using EquationDrawerApplication.Model;
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
        public DeleteFunctionCommand deleteFunctionCommand { get; set; }

        public ViewModelBase() {
            this.addFunctionCommand = new AddFunctionCommand(this);
            this.deleteFunctionCommand = new DeleteFunctionCommand(this);
            

        }

        public void removeFunction(Equation e) {
            RemoveItem(IndexOf(e));
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
