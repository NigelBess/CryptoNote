
using System;
using CryptoNote;

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

        public Func<string> GetText { get; set; }

        public string FilePath { get; set; }

        public EncryptedNote EncryptedNote { get; set; }
    }
}
