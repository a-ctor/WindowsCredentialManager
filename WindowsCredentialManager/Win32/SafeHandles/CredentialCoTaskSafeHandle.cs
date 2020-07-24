namespace WindowsCredentialManager.Win32.SafeHandles
{
  using System;
  using System.ComponentModel;
  using System.Runtime.InteropServices;
  using System.Security;
  using Types;
  using static UnsafeNativeApi;

  public class CredentialCoTaskSafeHandle : SafeHandle
  {
    /// <inheritdoc />
    public override bool IsInvalid => handle == IntPtr.Zero;

    public CredentialCoTaskSafeHandle()
      : this (true)
    {
    }

    /// <inheritdoc />
    public CredentialCoTaskSafeHandle (bool ownsHandle)
      : base (IntPtr.Zero, ownsHandle)
    {
    }

    public byte[] ToArray (uint count)
    {
      if (IsInvalid)
        throw new InvalidOperationException ("Handle is null.");
      if (count == 0)
        return Array.Empty<byte>();

      var result = new byte[count];
      Marshal.Copy (handle, result, 0, result.Length);

      return result;
    }

    public unsafe void GetPromptDetails (uint size, out string domain, out string username, out SecureString password)
    {
      IntPtr domainPtr = IntPtr.Zero;
      IntPtr usernamePtr = IntPtr.Zero;
      IntPtr passwordPtr = IntPtr.Zero;
      var passwordCapacity = byte.MaxValue;
      try
      {
        int domainLength = byte.MaxValue;
        domainPtr = Marshal.AllocHGlobal (domainLength);

        int usernameLength = byte.MaxValue;
        usernamePtr = Marshal.AllocHGlobal (usernameLength);

        int passwordLength = passwordCapacity;
        passwordPtr = Marshal.AllocHGlobal (passwordLength);

        if (!CredUnPackAuthenticationBufferW (
          CRED_PACK.ProtectedCredentials,
          this,
          size,
          usernamePtr,
          ref usernameLength,
          domainPtr,
          ref domainLength,
          passwordPtr,
          ref passwordLength))
        {
          throw new Win32Exception();
        }

        domain = Marshal.PtrToStringUni (domainPtr, domainLength > 0 ? domainLength - 1 : 0);
        username = Marshal.PtrToStringUni (usernamePtr, usernameLength > 0 ? usernameLength - 1 : 0);
        password = new SecureString ((char*) passwordPtr, passwordLength > 0 ? passwordLength - 1 : 0);
      }
      finally
      {
        Marshal.FreeHGlobal (domainPtr);
        Marshal.FreeHGlobal (usernamePtr);

        var ptr = (byte*) passwordPtr;
        for (var i = 0; i < passwordCapacity; i++)
          ptr[i] = 0;

        Marshal.FreeHGlobal (passwordPtr);
      }
    }

    /// <inheritdoc />
    protected override bool ReleaseHandle()
    {
      Marshal.ZeroFreeCoTaskMemUnicode (handle);
      return true;
    }
  }
}
