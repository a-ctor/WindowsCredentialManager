namespace WindowsCredentialManager.Win32.Blobs
{
  using System;
  using System.Runtime.InteropServices;
  using System.Security;

  internal class RawStringSecureBlob : SecureBlob
  {
    public RawStringSecureBlob (SecureString password)
    {
      handle = Marshal.SecureStringToGlobalAllocUnicode (password);
    }

    /// <inheritdoc />
    public override unsafe int Size => Win32Utility.GetUniStringLengthWithoutTerminator (handle);

    /// <inheritdoc />
    protected override bool ReleaseHandle()
    {
      Marshal.ZeroFreeGlobalAllocUnicode (handle);
      handle = IntPtr.Zero;

      return true;
    }
  }
}
