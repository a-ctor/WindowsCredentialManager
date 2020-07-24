namespace WindowsCredentialManager.Win32.Types
{
  internal enum CREDENTIAL_TYPE
  {
    Generic = 1,
    DomainPassword = 2,
    DomainCertificate = 3,
    DomainVisiblePassword = 4,
    GenericCertificate = 5,
    DomainExtended = 6
  }
}
