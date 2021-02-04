
using System.Windows;
using System.Windows.Input;
using Protocol;

namespace Application
{
    class MainWindowViewModel : ViewModel
    {
        private readonly EncryptedString _messageModel;

        public MainWindowViewModel(EncryptedString message)
        {
            _messageModel = message;
            message.ContentsChanged += MessageContentsChanged;
        }

        private void MessageContentsChanged()
        {

            Locked = !_messageModel.IsDefined;
            OnChange(nameof(Message));
        }


        public string Message 
        {
            get => _messageModel.GetPlainText().ToText();
            set => _messageModel.SetPlainText(value.ToBytes());
        }

        private Visibility _validationVisibility;

        public Visibility ValidationVisibility
        {
            get => _validationVisibility;
            set
            {
                _validationVisibility = value;
                OnChange();
            }
        }

        private Visibility _textVisibility;

        public Visibility TextVisibility
        {
            get => _textVisibility;
            set
            {
                _textVisibility = value;
                OnChange();
            }
        }
        public bool Locked
        {
            set
            {
                if (value)
                {
                    ValidationVisibility = Visibility.Visible;
                    TextVisibility = Visibility.Hidden;
                }
                else
                {
                    TextVisibility = Visibility.Visible;
                    ValidationVisibility = Visibility.Hidden;
                }
            }
        }

        public ICommand CreateNew { get; set; }

        public ICommand Lock { get; set; }

        private SaveData _saveData;

        public SaveData SaveData
        {
            get => _saveData;
            set
            {
                if (_saveData != null) Unbind(_saveData);
                _saveData = value;
                BindFileModel(value);
            }
        }

        public ICommand ChangePassword { get; set; }

        public ICommand Save { get; set; }
        public ICommand SaveAs { get; set; }
        public ICommand Open { get; set; }
        public ICommand Unlock { get; set; }
        public ICommand OpenSettingsWindow { get; set; }

        private void BindFileModel(SaveData model)
        {
            if (model == null) return;
            Bind(model, nameof(SaveData.Name), nameof(FileName));
            Bind(model, nameof(SaveData.Saved), nameof(FileName));
        }

        public string FileName => _saveData==null?string.Empty:_saveData.Name+(_saveData.Saved?string.Empty:" *");


        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnChange();
            }
        }
    }
}
