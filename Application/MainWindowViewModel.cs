
using System.Windows;
using System.Windows.Input;
using Protocol;

namespace Application
{
    class MainWindowViewModel : ViewModel
    {


        public string Text
        {
            get => _fileModel?.Text.PlainText.ToText();
            set
            {
                if (_fileModel?.Text == null) return;
                _fileModel.Text.PlainText = value.ToBytes();
            }
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

        private FileModel _fileModel;

        public FileModel FileModel
        {
            get => _fileModel;
            set
            {
                if (_fileModel != null) Unbind(_fileModel);
                _fileModel = value;
                BindFileModel(value);
            }
        }

        public ICommand ChangePassword { get; set; }

        public ICommand Save { get; set; }

        private void BindFileModel(FileModel model)
        {
            if (model == null) return;
            Bind(model, nameof(FileModel.Text),nameof(Text));
            Bind(model, nameof(FileModel.Name), nameof(FileName));
            Bind(model, nameof(FileModel.Saved), nameof(FileName));
        }

        public string FileName => _fileModel==null?string.Empty:_fileModel.Name+(_fileModel.Saved?string.Empty:" *");
    }
}
