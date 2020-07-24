namespace WindowsCredentialManager.Win32.SafeHandles
{
  using System;
  using System.Runtime.InteropServices;
  using Types;
  using static UnsafeNativeApi;

  internal unsafe class CredentialSafeHandle : SafeHandle
  {
    /// <inheritdoc />
    public override bool IsInvalid => handle == IntPtr.Zero;

    public CredentialSafeHandle()
      : this (true)
    {
    }

    /// <inheritdoc />
    public CredentialSafeHandle (bool ownsHandle)
      : base (IntPtr.Zero, ownsHandle)
    {
    }

    public CREDENTIALW_RAW* AsCredentialW()
    {
      if (handle == IntPtr.Zero)
        throw new InvalidOperationException ("The handle is null.");

      return (CREDENTIALW_RAW*) handle;
    }

    /// <inheritdoc />
    protected override bool ReleaseHandle()
    {
      CredFree ((CREDENTIALW_RAW*) handle);
      return true;
    }
  }
}
