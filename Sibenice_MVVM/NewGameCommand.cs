using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Sibenice_MVVM
{
    internal class NewGameCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private MVmodel MVmodel_model;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            MVmodel_model._NewGame();
        }

        public NewGameCommand(MVmodel mVmodel)
        {
            MVmodel_model = mVmodel;
        }
    }
}
