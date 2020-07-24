namespace WindowsCredentialManager.Win32.Types
{
  using System;

  [Flags]
  internal enum CRED_PACK
  {
    ProtectedCredentials = 0x1,
    WowBuffer = 0x2,
    GenericCredentials = 0x4,
    IdProviderCredentials = 0x8
  }
}
