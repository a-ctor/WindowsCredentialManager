namespace WindowsCredentialManager.Win32.Types
{
  using System;
  using System.Runtime.InteropServices;

  [StructLayout (LayoutKind.Sequential)]
  internal readonly struct CREDENTIALW_RAW
  {
    public readonly CREDENTIAL_FLAGS Flags;
    public readonly CREDENTIAL_TYPE Type;
    public readonly IntPtr TargetName;
    public readonly IntPtr Comment;
    public readonly long LastWritten;
    public readonly int BlobSize;
    public readonly IntPtr Blob;
    public readonly CREDENTIAL_PERSIST Persist;
    public readonly int AttributeCount;
    public readonly IntPtr Attributes;
    public readonly IntPtr TargetAlias;
    public readonly IntPtr UserName;
  }
}
