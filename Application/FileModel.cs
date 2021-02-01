
using System;
using Protocol;

namespace Application
{
    class FileModel : Notifier
    {
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnChange();
            }
        }


        private bool _saved = true;
        public bool Saved
        {
            get => _saved;
            set
            {
                _saved = value;
                OnChange();
            }
        }



        public readonly EncryptedString Text = new();

        public readonly EncryptedString Password = new();
        public readonly EncryptedString TentativePassword = new();


        public string FilePath { get; set; }


        public FileModel()
        {
            Text.ContentsChanged += ContentsChanged;
            Text.ContentsChanged += () => OnChange(nameof(Text));
            Password.ContentsChanged += ContentsChanged;
        }

        private void ContentsChanged()
        {
            Saved = false;
        }

        public void SwapToTentativePassword()
        {
            if (!TentativePassword.IsDefined) return;
            Password.Wipe();
            Password.TakeValue(TentativePassword);
            TentativePassword.Wipe();
        }
    }
}
