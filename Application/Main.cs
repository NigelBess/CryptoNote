

using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Protocol;
using Microsoft.Win32;

namespace Application
{
    class Main
    {
        private readonly MainWindow _mainWindow = new ();
        private readonly MainWindowViewModel _viewModel = new ();

        private PasswordWindow _passwordWindow;
        private readonly PasswordWindowViewModel _passwordViewModel = new();

        private FileModel _fileModel;

        public void Start()
        {
            _mainWindow.DataContext = _viewModel;

            SetupViewModels();
            CreateNewFile();
            _mainWindow.Show();
        }

        public void SetupViewModels()
        {
            _passwordViewModel.ChangePassword = new Command(UserConfirmedPassword, UserCanConfirmPassword);
            _viewModel.ChangePassword = new Command(()=>CreatePassword(ChangePasswordOfExistingFile));
            _viewModel.Save = new Command(Save);
        }

        private void UserConfirmedPassword(object passwordBoxObject)
        {
            var passwordBoxes = GetPasswordBoxes(passwordBoxObject);
            byte[] oldPassword = null;
            byte[] newPassword = null;
            try
            {

                oldPassword = passwordBoxes[0].Password.ToBytes();
                if (_fileModel.Password.IsDefined && !_fileModel.Password.Matches(oldPassword))
                {
                    OnError("Incorrect Password!");
                    return;
                }
                newPassword = passwordBoxes[1].Password.ToBytes();
                _fileModel.TentativePassword.PlainText = newPassword;
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

        private void Save()
        {
            byte[] message = null;
            byte[] password = null;

            try
            {
                if (!_fileModel.Password.IsDefined) {
                    CreatePassword(() =>
                    {
                        _fileModel.SwapToTentativePassword();
                        Save();
                        
                    });
                    return;
                }
                if (_fileModel.FilePath == null)
                {
                    _fileModel.FilePath = Browse(_fileModel.Name, out var fileName);
                    _fileModel.Name = fileName;
                }

                if (_fileModel.FilePath == null) return;
                var note = new CryptoNote(UserSettings.Default.Iterations);
                message = _fileModel.Text.PlainText;
                password = _fileModel.Password.PlainText;
                note.Encrypt(message, password);
                Writer.SaveToFile(note, _fileModel.FilePath);
            }
            catch (Exception e)
            {
                OnError(e);
            }
            finally
            {
                message?.Wipe();
                password?.Wipe();
            }

        }

        private void ChangePasswordOfExistingFile()
        {
            byte[] message = null;
            byte[] password = null;

            try
            {
                CryptoNote currentFileContents = null;
                //If the file has not been saved yet, just change the password locally without asking
                if (string.IsNullOrWhiteSpace(_fileModel.FilePath) || !new Reader().TryRead(_fileModel.FilePath, out currentFileContents))
                {
                    _fileModel.SwapToTentativePassword();
                    return;
                }

                //if the file exists, change the password on that file and locally
                var userDecision =
                    MessageBox.Show("Are you sure you want to change the password? This can not be undone.", "Warning!",
                        MessageBoxButton.YesNo);
                if (userDecision != MessageBoxResult.Yes) return;

                password = _fileModel.Password.PlainText;
                if (!currentFileContents.TryDecrypt(_fileModel.Password.PlainText, out message))
                {
                    OnError("File decryption failed!");
                    return;
                }

                _fileModel.SwapToTentativePassword();

                currentFileContents.Encrypt(message, _fileModel.Password.PlainText);
                Writer.SaveToFile(currentFileContents, _fileModel.FilePath);
            }
            catch (Exception e)
            {
                OnError(e);
            }
            finally
            {
                message?.Wipe();
                password?.Wipe();
            }




        }

        private void CreatePassword(Action onChange)
        {
            _passwordViewModel.CurrentPasswordVisible = _fileModel.Password.IsDefined;
            _passwordViewModel.CurrentError = null;
            _passwordWindow = new (){DataContext = _passwordViewModel};
            _passwordWindow.Closing += (o, e) => onChange();
            _passwordWindow.ShowDialog();
        }
        private string Browse(string defaultFileName, out string fileName)
        {
            var dialog = new SaveFileDialog()
            {
                AddExtension = true,
                Filter = "Crypto Note File | *.cryptoNote",
                DefaultExt = ".cryptoNote",
                FileName = defaultFileName,
            };
            if (Directory.Exists(UserSettings.Default.SaveFolder))
                dialog.InitialDirectory = UserSettings.Default.SaveFolder;
            var path =dialog.ShowDialog() == true ? dialog.FileName : null;
            UserSettings.Default.SaveFolder = Path.GetDirectoryName(path);
            fileName = Path.GetFileNameWithoutExtension(path);
            UserSettings.Default.Save();
            return path;
        }
        private void OnError(string message)
        {
            OnError(new Exception(message));
        }
        private void OnError(Exception e)
        {
            MessageBox.Show(e.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
        }

    }
}
