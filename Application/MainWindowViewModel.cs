using System.ComponentModel;
using System.Runtime.CompilerServices;
using Application.Annotations;

namespace Application
{
    class MainWindowViewModel : ViewModel
    {

        private string _text;

        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                OnChange();
            }
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

        private void BindFileModel(FileModel model)
        {
            model.GetText = () => Text;
            Bind(model, nameof(FileModel.Name), nameof(FileName));
            Bind(model, nameof(FileModel.Saved), nameof(FileName));
        }

        public string FileName => _fileModel==null?string.Empty:_fileModel.Name+(_fileModel.Saved?string.Empty:" *");
    }
}
