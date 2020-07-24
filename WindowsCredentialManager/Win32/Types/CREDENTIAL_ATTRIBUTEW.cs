namespace WindowsCredentialManager.Win32.Types
{
  using System;
  using System.Runtime.InteropServices;

  [StructLayout (LayoutKind.Sequential)]
  internal struct CREDENTIAL_ATTRIBUTEW
  {
    public IntPtr Keyword;
    private int Flags; // Unused
    public int ValueSize;
    public IntPtr Value;
  }
}
