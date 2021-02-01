
using System;
using Protocol;

namespace Application
{
    class FileModel:Notifier
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
        

        public string FilePath { get; set; }

        public CryptoNote CryptoNote { get; set; }

        public FileModel()
        {
            Text.ContentsChanged += ContentsChanged;
            Password.ContentsChanged += ContentsChanged;
        }

        private void ContentsChanged()
        {
            Saved = false;
        }
    }
}
