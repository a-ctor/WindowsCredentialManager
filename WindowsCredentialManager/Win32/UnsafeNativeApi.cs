namespace WindowsCredentialManager.Win32
{
  using System;
  using System.Runtime.InteropServices;
  using SafeHandles;
  using Types;

  internal static unsafe class UnsafeNativeApi
  {
    [DllImport ("Advapi32.dll", SetLastError = true)]
    public static extern bool CredWriteW (ref CREDENTIALW credential, int flags);

    [DllImport ("Advapi32.dll", SetLastError = true)]
    public static extern bool CredReadW (
      [MarshalAs (UnmanagedType.LPWStr)] string targetName,
      CREDENTIAL_TYPE type,
      int flags, // use 0 only
      out CredentialSafeHandle credential);

    [DllImport ("Advapi32.dll")]
    public static extern bool CredDeleteW (
      [MarshalAs (UnmanagedType.LPWStr)] string targetName,
      CREDENTIAL_TYPE type,
      int flags // use 0 only
    );

    [DllImport ("Advapi32.dll")]
    public static extern void CredFree (CREDENTIALW_RAW* buffer);

    [DllImport ("Credui.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
    public static extern int CredUIPromptForWindowsCredentialsW (
      ref CREDUI_INFO uiInfo,
      int errorMessage,
      ref uint authPackage,
      byte[]? authInBuffer,
      uint authInBufferSize,
      out CredentialCoTaskSafeHandle authOutBuffer,
      out uint authOutBufferSize,
      ref bool save,
      CREDUI_WINDOW windowType);

    [DllImport ("Credui.dll", SetLastError = true)]
    public static extern bool CredUnPackAuthenticationBufferW (
      CRED_PACK flags,
      CredentialCoTaskSafeHandle authBuffer,
      uint authBufferSize,
      IntPtr userName,
      ref int userNameCapacity,
      IntPtr domain,
      ref int domainCapacity,
      IntPtr password,
      ref int passwordCapacity);
  }
}
