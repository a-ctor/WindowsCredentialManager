namespace WindowsCredentialManager.Win32.Types
{
  using System;

  [Flags]
  internal enum CREDENTIAL_FLAGS
  {
    None = 0x0,
    PromptNow = 0x2,
    UsernameTarget = 0x4
  }
}
