using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    class SettingsViewModel:ViewModel
    {
        public UserSettings Settings { get; set; }

        private string _version;
        public string Version
        {
            get => _version;
            set
            {
                _version = value;
                OnChange();
            }
        }

        private string _protocol;
        public string Protocol
        {
            get => _protocol;
            set
            {
                _protocol = value;
                OnChange();
            }
        }

    }
}
