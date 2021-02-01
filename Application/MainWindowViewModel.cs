
using System.Windows.Input;
using Protocol;

namespace Application
{
    class MainWindowViewModel : ViewModel
    {


        public string Text
        {
            get => _fileModel?.Text.PlainText.ToText();
            set => _fileModel.Text.PlainText = value.ToBytes();
        }

        private FileModel _fileModel;

        public FileModel FileModel
        {
            get => _fileModel;
            set
            {
                if (_fileModel != null) Unbind(_fileModel);
                _fileModel = value;
                BindFileModel(value);
                OnChange();
            }
        }

        public ICommand ChangePassword { get; set; }

        private void BindFileModel(FileModel model)
        {
            //Bind(model, nameof(FileModel.Text),nameof(Text));
            Bind(model, nameof(FileModel.Name), nameof(FileName));
            Bind(model, nameof(FileModel.Saved), nameof(FileName));
        }

        public string FileName => _fileModel==null?string.Empty:_fileModel.Name+(_fileModel.Saved?string.Empty:" *");
    }
}
