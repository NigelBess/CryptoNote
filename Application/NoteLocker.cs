using System;
using Protocol;

namespace Application
{
    class NoteLocker:HandlesExceptions
    {
        public byte[] Message => _vulnerableData.Password.GetPlainText();
        private readonly VulnerableData _vulnerableData;
        private CryptoNote _encryptedData;
        public bool IsPasswordSet => IsLocked || _vulnerableData.Password.IsDefined;
        public bool IsLocked => GetState() == State.Locked;
        public bool IsEmpty => GetState() == State.Empty;
        public bool IsUnlocked => GetState() == State.Unlocked;

        public NoteLocker(VulnerableData vulnerableData)
        {
            _vulnerableData = vulnerableData;
        }

        public byte[] GetCopyOfPasswordCiper()
        {
            var cipher = _vulnerableData.Password.Cipher;
            if (cipher == null) return null;
            var copy = new byte[cipher.Length];
            Array.Copy(cipher, copy, cipher.Length);
            return copy;
        }

        private State GetState()
        {
            if (!_vulnerableData.IsWiped) return State.Unlocked;
            return _encryptedData == null ? State.Empty : State.Locked;
        }

        public void Reset()
        {
            _encryptedData?.Wipe();
            _encryptedData = null;
            ClearVulnerableData();
            _vulnerableData.Message.SetPlainText(new byte[0]);
        }

        public void Load(CryptoNote note)
        {
            _encryptedData = note;
            ClearVulnerableData();
        }

        public CryptoNote GenerateEncryptedOutput()
        {
            byte[] message = null;
            byte[] password = null;
            CryptoNote note = null;
            SafeExecute(() =>
            {
                if (IsLocked)
                {
                    note = _encryptedData;
                    return;
                }

                if(IsEmpty)
                {
                    OnError("This note is empty!");
                    return;
                }
                if (!IsUnlocked)
                {
                    OnError("Unknown instruction for this lock state. Please alert a developer.");
                }

                note ??= new CryptoNote(UserSettings.Default.Iterations){OnError = OnError};
                message = _vulnerableData.Message.GetPlainText();
                password = _vulnerableData.Password.GetPlainText();
                note.Encrypt(message, password);

            }, message, password);
            return note;
        }

        public void Lock()
        {
            byte[] message = null;
            byte[] password = null;
            SafeExecute(() =>
            {
                if (IsLocked)
                {
                    OnError("This note is already Locked!");
                    return;
                }

                if (IsEmpty)
                {
                    OnError("There is no data to unlock!");
                    return;
                }

                if (!IsUnlocked)
                {
                    OnError("Unknown instruction for this lock state. Please alert a developer.");
                }

                if (!IsPasswordSet)
                {
                    OnError("Password has not been set!");
                }

                _encryptedData ??= new CryptoNote(UserSettings.Default.Iterations);
                message = _vulnerableData.Message.GetPlainText();
                password = _vulnerableData.Password.GetPlainText();
                _encryptedData.Encrypt(message, password);
                ClearVulnerableData();
            }, message, password);
            
        }





        private void ClearVulnerableData()
        {
            _vulnerableData?.Wipe();
        }

        public void UnLock(byte[] password)
        {
            byte[] message = null;
            SafeExecute(() =>
            {
                if (IsUnlocked)
                {
                    OnError("This note is already unlocked!");
                    return;
                }

                if (IsEmpty)
                {
                    OnError("There is no data to unlock!");
                    return;
                }

                if (!IsLocked)
                {
                    OnError("Unknown instruction for this lock state. Please alert a developer.");
                }

                if (!_encryptedData.TryDecrypt(password, out message))
                {
                    OnError("Invalid password or corrupt file!");
                    return;
                }
                _vulnerableData.Message.SetPlainText(message);
                _vulnerableData.Password.SetPlainText(password);
            },password, message);
        }

        private enum State
        {
            Locked,
            Unlocked,
            Empty,
        }

        public bool DoesPasswordMatch(byte[] oldPassword) => _vulnerableData.Password.Matches(oldPassword);

        public void SetPassword(byte[] newPassword) => _vulnerableData.Password.SetPlainText(newPassword);

        public void Wipe()
        {
            _vulnerableData.Wipe();
        }
    }
}
