namespace WindowsCredentialManager.Win32.Types
{
  using System;
  using System.Runtime.InteropServices;

  [StructLayout (LayoutKind.Sequential)]
  internal struct CREDUI_INFO
  {
    public int Size;
    public IntPtr ParentHandle;
    [MarshalAs (UnmanagedType.LPWStr)] public string MessageText;
    [MarshalAs (UnmanagedType.LPWStr)] public string CaptionText;
    private IntPtr Bitmap; // Ignored
  }
}
