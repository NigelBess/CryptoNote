# CryptoNote #
CryptoNote is an open-source encryption tool that allows users to securely read and write sensitive data.


Private information on storage devices (Hard drives, SSD's, flash drives, etc.) is vulnerable to anyone who gains physical access to those devices. The solution is to encrypt this information. There are lots of ways to encrypt sensitive data, but most of them have certain vulnerabilities that CryptoNote addresses.

# Problems With Existing Solutions #

## File Encryption tools ##
One simple approach to encrypting data is to save it in an unencrypted format, such as a .txt file, and encrypt that file using a tool like 7Zip. This approach encrypts the data as a new file, but even if the original file is deleted, a data recovery software can find the original file and read it.

## Encrypted Storage Devices ##
One can purchase an encrypted storage device, but this doesn't guarantee security either. Encrypted storage devices aren't inherently different from other storage devices, but use software to read and write in an encrypted format. Some programs dont't interface well with encryption software and therefore can't write directly to encrypted storage devices. For example a notepad application might not be able to save notes on an encrypted drive.

Encrypted drives also cost money, and don't provide value beyond that of an existing storage device and good encryption software. 

## Closed-Source Encryption Software ##

Lots of programs exist to solve the problems mentioned above, but most of them have one thing in common: they are not open-source. A user of one of those programs has no way of knowing what the program is actually doing with their data. The user must trust that the creator of the program is not a malicious actor and that the program actually works as intended. Encryption exists to eliminate the need for trust, so closed-source programs invalidate the security provided by encryption.

# How CryptoNote Works #
CryptoNote accepts user input (sensitive data) and stores it in memory. When a user saves that data, is is encrypted in memory and saved to the storage device in an encrypted form. Sensitive data is never written to the storage device, only to memory. CryptoNote also erases data from memory whenever possible, but this does not provide security from malicious programs already installed on a computer. CryptoNote's purpose is to allow users to save encrypted data without opening vulnerabilities to data recovery software.

# Encryption Algorithm #

CryptoNote uses AES-256-CBC to encrypt and decrypt data. The 32 byte AES-256 key is derived from a user-provided password using PBKDF2 with a pseudorandom function of HMACSHA1.

For more in technical details please refer to the complete documentation, found [here](https://github.com/NigelBess/CryptoNote/blob/master/Docs/CryptoNote_Protocol_V1.pdf).
