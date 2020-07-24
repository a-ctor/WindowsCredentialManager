namespace WindowsCredentialManager.Win32.Types
{
  using System;

  [Flags]
  internal enum CREDUI_WINDOW
  {
    Generic = 0x1,
    Checkbox = 0x2,
    AuthPackageOnly = 0x10,
    InCredOnly = 0x20,
    EnumerateAdmins = 0x100,
    EnumerateCurrentUser = 0x200,
    SecurePrompt = 0x1000,
    PrePrompting = 0x2000
  }
}
