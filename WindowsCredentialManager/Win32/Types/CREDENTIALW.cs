namespace WindowsCredentialManager.Win32.Types
{
  using System;
  using System.Runtime.InteropServices;

  [StructLayout (LayoutKind.Sequential)]
  internal struct CREDENTIALW
  {
    public CREDENTIAL_FLAGS Flags;
    public CREDENTIAL_TYPE Type;
    [MarshalAs (UnmanagedType.LPWStr)] public string TargetName;
    [MarshalAs (UnmanagedType.LPWStr)] public string? Comment;
    public DateTimeOffset LastWritten;
    public int BlobSize;
    public SecureBlob? Blob;
    public CREDENTIAL_PERSIST Persist;
    public int AttributeCount;
    public IntPtr Attributes;
    [MarshalAs (UnmanagedType.LPWStr)] public string? TargetAlias;
    [MarshalAs (UnmanagedType.LPWStr)] public string? UserName;
  }
}
