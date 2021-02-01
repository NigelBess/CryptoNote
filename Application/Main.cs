

using System;
using System.Windows.Controls;
using Protocol;
using Microsoft.Win32;

namespace Application
{
    class Main
    {
        private readonly MainWindow _mainWindow = new ();
        private readonly MainWindowViewModel _viewModel = new ();

        private readonly PasswordWindow _passwordWindow = new();
        private readonly PasswordWindowViewModel _passwordViewModel = new();

        private FileModel _fileModel;
        public void Start()
        {
            _mainWindow.DataContext = _viewModel;
            _passwordWindow.DataContext = _passwordViewModel;

            SetupViewModels();
            CreateNewFile();
            _mainWindow.Show();
        }

        public void SetupViewModels()
        {
            _passwordViewModel.ChangePassword = new Command(UserConfirmedPassword, UserCanConfirmPassword);
            _viewModel.ChangePassword = new Command(CreatePassword, null);
        }

        private void UserConfirmedPassword(object passwordBoxObject)
        {
            var passwordBoxes = GetPasswordBoxes(passwordBoxObject);
            byte[] oldPassword = null;
            byte[] newPassword = null;
            try
            {

                oldPassword = passwordBoxes[0].Password.ToBytes();
                if (_fileModel.Password.IsDefined && !_fileModel.Password.Equals(oldPassword))
                {
                    OnError("Incorrect Password!");
                    return;
                }
                newPassword = passwordBoxes[1].Password.ToBytes();
                _fileModel.Password.PlainText = newPassword;
            }
            catch (Exception e)
            {
                OnError(e);
            }
            finally
            {
                oldPassword?.Wipe();
                newPassword?.Wipe();
            }

            _passwordWindow.Close();
        }

        private bool UserCanConfirmPassword(object passwordBoxObject)
        {
            var passwordBoxes = GetPasswordBoxes(passwordBoxObject);

            if (passwordBoxes[1]?.SecurePassword == null || passwordBoxes[2]?.SecurePassword == null) return false;
            return passwordBoxes[1].SecurePassword.IsEqualTo(passwordBoxes[2].SecurePassword);
        }

        private PasswordBox[] GetPasswordBoxes(object passwordBoxObject)
        {
            var passwordBoxes = (object[])passwordBoxObject;
            var passwordBoxArray = new PasswordBox[passwordBoxes.Length];
            for (int i = 0; i < passwordBoxes.Length; i++)
            {
                passwordBoxArray[i] = (PasswordBox)passwordBoxes[i];
            }

            return passwordBoxArray;
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
            Writer.SaveToFile(model.CryptoNote, model.FilePath);

        }

        private void CreatePassword()
        {
            _passwordViewModel.CurrentPasswordVisible = _fileModel.Password.IsDefined;
            _passwordWindow.Show();
        }
        private string Browse()
        {
            var dialog = new OpenFileDialog();
            return dialog.ShowDialog() == true ? dialog.FileName : null;
        }
        private void OnError(string message)
        {
            OnError(new Exception(message));
        }
        private void OnError(Exception e)
        {

        }

    }
}
