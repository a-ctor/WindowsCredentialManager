namespace WindowsCredentialManager
{
  using System.Runtime.InteropServices;
  using System.Security;
  using Win32;
  using Win32.Blobs;
  using Win32.Types;

  public sealed class GenericCredentials : Credential
  {
    public string? UserName { get; set; }

    public SecureString? Password { get; set; }

    public GenericCredentials (string targetName)
      : base (targetName, CredentialType.Generic)
    {
    }

    /// <inheritdoc />
    internal override unsafe void Deserialize (CREDENTIALW_RAW* credentialW)
    {
      UserName = Marshal.PtrToStringUni (credentialW->UserName);
      Password = Win32Utility.UniStringToSecureString (credentialW->Blob, credentialW->BlobSize);
    }

    /// <inheritdoc />
    internal override void Serialize (ref CREDENTIALW credentialW)
    {
      var password = Password;

      credentialW.UserName = UserName;
      credentialW.Blob = password == null
        ? null
        : new RawStringSecureBlob (password);
    }
  }
}
