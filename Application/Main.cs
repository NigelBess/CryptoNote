using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Protocol;
using Microsoft.Win32;

namespace Application
{
    class Main:HandlesExceptions
    {
        private readonly MainWindow _mainWindow = new ();

        private PasswordWindow _passwordWindow;
        private readonly PasswordWindowViewModel _passwordViewModel = new();

        private readonly SettingsViewModel _settingsViewModel = new ();

        private readonly SaveData _saveData = new();
        private readonly NoteLocker _note;

        public Main()
        {
            _settingsViewModel.Settings = UserSettings.Default;
            _settingsViewModel.Version = Assembly.GetEntryAssembly().GetName().Version.ToString();
            _settingsViewModel.Protocol = Constants.ProtocolVersion.ToString();

            var password = new EncryptedString() {ErrorCaught = HandleError};
            var message = new EncryptedString() { ErrorCaught = HandleError };
            var vulnerableData = new VulnerableData(message,password) ;
            _note = new NoteLocker(vulnerableData) { ErrorCaught = HandleError };
            var mainWindowViewModel = new MainWindowViewModel(message) {SaveData = _saveData};

            message.ContentsChanged += ()=>_saveData.Saved = false;
            password.ContentsChanged += () =>_saveData.Saved = false;

            _mainWindow.DataContext = mainWindowViewModel;
            _mainWindow.Closing += OnWindowClose;
            _mainWindow.Title = "CryptoNote";

            SetupPasswordViewModelCommands();
            SetupMainViewModelCommands(mainWindowViewModel);
        }

        



        public void Start(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                CreateNewFile();
            }
            else
            {
                OpenPath(path);
            }
            
            _mainWindow.Show();
        }

        


        private void SetupPasswordViewModelCommands()
        {
            _passwordViewModel.ConfirmPassword = new Command(UserConfirmedPassword, UserCanConfirmPassword);
        }

        private void SetupMainViewModelCommands(MainWindowViewModel viewModel)
        {
            viewModel.ChangePassword = new Command(()=>TryChangePassword(), IsUnlocked);
            viewModel.CreateNew = new Command(CreateNewFile);
            viewModel.Lock = new Command(Lock, IsUnlocked);
            viewModel.Save = new Command(Save);
            viewModel.SaveAs = new Command(() =>
            {
                _saveData.FilePath = null;
                Save();
            });
            viewModel.Open = new Command(Open);
            viewModel.Unlock = new Command(Unlock, o => IsLocked());
            viewModel.OpenSettingsWindow = new Command(OpenSettingsWindow);
        }

        private void OpenSettingsWindow()
        {
            var settingsWindow = new SettingsWindow {DataContext = _settingsViewModel};
            settingsWindow.Closing += SettingsWindowClosed;
            settingsWindow.Activate();
            settingsWindow.Show();
        }

        private void SettingsWindowClosed(object sender,CancelEventArgs e)
        {
            UserSettings.Default.Save();
        }

        private void Unlock(object passwordBoxObj)
        {
            var saved = _saveData.Saved;
            var passwordBox = (PasswordBox) passwordBoxObj;
            var passwordText = passwordBox.Password.ToBytes();
            passwordBox.Password = string.Empty;
            _note.UnLock(passwordText);
            _saveData.Saved = saved;
        }

        private void Open()
        {
            if (_saveData != null && !_saveData.Saved)
            {
                var userDecision = MessageBox.Show("Are you sure you want to open a different file? You will lose any unsaved progress.", "Warning!", MessageBoxButton.YesNo);
                if (userDecision != MessageBoxResult.Yes) return;
            }

            var dialog = new OpenFileDialog()
            {
                AddExtension = true,
                Filter = $"Crypto Note File | *{Constants.FileExtension}",
                DefaultExt = Constants.FileExtension,
            };
            if (Directory.Exists(UserSettings.Default.LoadFolder))
                dialog.InitialDirectory = UserSettings.Default.SaveFolder;
            if (!dialog.ShowDialog() ?? false) return;
            OpenPath(dialog.FileName);
        }

        private void OpenPath(string path)
        {
            if (!new Reader().TryRead(path, out var note))
            {
                OnError("Unable to open selected file!");
                return;
            }

            note.OnError = OnError;
            _note.Load(note);
            _saveData.FilePath = path;
            _saveData.Saved = true;
        }

        private bool IsLocked() => _note.IsLocked;
        private bool IsUnlocked() => !IsLocked();


        private void Lock()
        {
            var saved = _saveData.Saved;
            if (!_note.IsPasswordSet)
            {
                if (!TryChangePassword()) return;
            }
            _note.Lock();
            _saveData.Saved = saved;
        }

        private void UserConfirmedPassword(object passwordBoxObject)
        {
            var passwordBoxes = GetPasswordBoxes(passwordBoxObject);
            byte[] oldPassword = null;
            byte[] newPassword = null;
            SafeExecute(() =>
            {
                
                if (_note.IsPasswordSet)
                {
                    oldPassword = passwordBoxes[0].Password.ToBytes();
                    if (!_note.DoesPasswordMatch(oldPassword))
                    {
                        OnError("Incorrect Password!");
                        return;
                    }
                }
                newPassword = passwordBoxes[1].Password.ToBytes();
                _note.SetPassword(newPassword);
            },oldPassword,newPassword);
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
            if (_saveData != null && !_saveData.Saved)
            {
                var userDecision = MessageBox.Show("Are you sure you want to create a new file? You will lose any unsaved progress.","Warning!",MessageBoxButton.YesNo);
                if (userDecision != MessageBoxResult.Yes) return;
            }
            _note.Reset();
            _saveData.FilePath = null;
            _saveData.Saved = true;

        }

        private void Save()
        {
            if (!_note.IsPasswordSet)
            {
                if (!TryChangePassword()) return;
            }

            if (_saveData.FilePath == null)
            {
                _saveData.FilePath = Browse(_saveData.Name);
                if (_saveData.FilePath == null) return;
            }

            var note = _note.GenerateEncryptedOutput();
            var path = _saveData.FilePath;
            if (File.Exists(path)) File.Delete(path);
            Writer.SaveToFile(note, path);
            if (!VerifySuccessfulSave(note, path))
            {
                OnError("Note did not save successfully!");
                File.Delete(path);
                return;
            }
            _saveData.Saved = true;
        }

        private bool VerifySuccessfulSave(CryptoNote note, string path)
        {
            var ableToRead = new Reader().TryRead(path, out var loadedNote);
            if (!ableToRead) return false;
            if (!ByteArrayFunctions.AreEqual(note.Cipher, loadedNote.Cipher)) return false;
            if (!ByteArrayFunctions.AreEqual(note.Salt, loadedNote.Salt)) return false;
            if (!ByteArrayFunctions.AreEqual(note.InitializationVector, loadedNote.InitializationVector)) return false;
            if (note.Iterations != loadedNote.Iterations) return false;
            return true;
        }


        private bool TryChangePassword()
        {
            byte[] oldCipher = null;
            byte[] newCipher = null;
            bool success = false;
            SafeExecute(() =>
            {
                Func<bool> didPasswordChange;
                
                if (!_note.IsPasswordSet)
                {
                    didPasswordChange = () => _note.IsPasswordSet;
                }
                else
                {
                    oldCipher = _note.GetCopyOfPasswordCiper();
                    didPasswordChange = () => ByteArrayFunctions.AreEqual(oldCipher, _note.GetCopyOfPasswordCiper());
                }

                _passwordViewModel.CurrentPasswordVisible = _note.IsPasswordSet;
                _passwordWindow = new() { DataContext = _passwordViewModel };
                _passwordWindow.ShowDialog();
                success = didPasswordChange();
            }, oldCipher,newCipher);
            return success;
            
        }

        private string Browse(string defaultFileName)
        {
            var dialog = new SaveFileDialog()
            {
                AddExtension = true,
                Filter = $"Crypto Note File | *{Constants.FileExtension}",
                DefaultExt = Constants.FileExtension,
                FileName = defaultFileName,
            };
            if (Directory.Exists(UserSettings.Default.SaveFolder))
                dialog.InitialDirectory = UserSettings.Default.SaveFolder;
            if (!dialog.ShowDialog()??false) return null;
            var path = dialog.FileName;
            UserSettings.Default.SaveFolder = Path.GetDirectoryName(path);
            UserSettings.Default.Save();
            return path;
        }
        public void HandleError(Exception e)
        {
            MessageBox.Show(e.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
        }

        private void OnWindowClose(object sender, CancelEventArgs e)
        {
            if (!_saveData.Saved)
            {
               var userDecision = MessageBox.Show("You will lose all saved progress!", "Warning!", MessageBoxButton.OKCancel);
               if (userDecision != MessageBoxResult.OK)
               {
                   e.Cancel = true;
                    return;
               }
            }

            _note.Wipe();
        }

    }
}
