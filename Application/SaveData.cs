
using System;
using System.IO;
using Protocol;

namespace Application
{
    class SaveData : Notifier
    {
        public string Name => string.IsNullOrEmpty(FilePath) ? "New Note" : Path.GetFileNameWithoutExtension(FilePath);

        

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






        private string _filePath;
        public string FilePath
        {
            get => _filePath;
            set
            {
                _filePath = value;
                OnChange(nameof(Name));
            }
        }


        public SaveData()
        {
            //Message.ContentsChanged += ContentsChanged;
            //Message.ContentsChanged += () => OnChange(nameof(Message));
            //Password.ContentsChanged += ContentsChanged;
        }

        private void ContentsChanged()
        {
            Saved = false;
        }

    }
}
