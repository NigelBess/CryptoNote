

using System;

namespace Application
{
    class VulnerableData
    {
        public readonly EncryptedString Message;

        public readonly EncryptedString Password;
        public bool IsWiped => !Message.IsDefined && !Password.IsDefined;

        public VulnerableData(EncryptedString message, EncryptedString password)
        {
            Message = message;
            Password = password;
        }


        public void Wipe()
        {
            Message.Wipe();
            Password.Wipe();
        }
    }
}
