

using Protocol;
using Microsoft.Win32;

namespace Application
{
    class Main
    {
        private readonly MainWindow _mainWindow = new ();
        private readonly MainWindowViewModel _viewModel = new ();
        private FileModel _fileModel;
        public void Start()
        {
            _mainWindow.DataContext = _viewModel;
            _mainWindow.Show();
            CreateNewFile();
        }

        private void CreateNewFile()
        {
            _fileModel = new FileModel {Name = "New Note"};
            _viewModel.FileModel = _fileModel;
        }

        private void Save(FileModel model)
        {
            model.FilePath ??= Browse();
            if (model.FilePath == null) return;


        }

        private string Browse()
        {
            var dialog = new OpenFileDialog();
            return dialog.ShowDialog() == true ? dialog.FileName : null;
        }

    }
}
