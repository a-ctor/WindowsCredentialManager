namespace WindowsCredentialManager.Win32
{
  using System;
  using System.Runtime.InteropServices;

  internal abstract class SecureBlob : SafeHandle
  {
    /// <inheritdoc />
    public override bool IsInvalid => handle == IntPtr.Zero;

    /// <inheritdoc />
    protected SecureBlob()
      : base (IntPtr.Zero, true)
    {
    }

    public abstract int Size { get; }

    /// <inheritdoc />
    protected abstract override bool ReleaseHandle();
  }
}
