using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Application
{
    class PasswordWindowViewModel:ViewModel
    {
        private bool _currentPasswordVisibile;

        public bool CurrentPasswordVisible
        {
            get => _currentPasswordVisibile;
            set

            {
                _currentPasswordVisibile = value;
                OnChange(nameof(CurrentPasswordVisibility));
            }
        }

        private string _currentError;
        public string CurrentError
        {
            get => _currentError;
            set
            {
                _currentError = value;
                OnChange();
            }
        }

        public Visibility CurrentPasswordVisibility => CurrentPasswordVisible ? Visibility.Visible : Visibility.Collapsed;

        public ICommand ChangePassword { get; set; }

    }
}
