namespace WindowsCredentialManager.Win32
{
  using System;
  using Types;

  internal static class UnsafeNativeApiExtensions
  {
    public static CREDENTIAL_PERSIST ConvertToApiEnum (this CredentialPersistence persistence)
    {
      return persistence switch
      {
        CredentialPersistence.Session => CREDENTIAL_PERSIST.Session,
        CredentialPersistence.LocalMachine => CREDENTIAL_PERSIST.LocalMachine,
        CredentialPersistence.Enterprise => CREDENTIAL_PERSIST.Enterprise,
        _ => throw new ArgumentOutOfRangeException (nameof(persistence), persistence, null)
      };
    }

    public static CredentialPersistence ConvertToConsumerEnum (this CREDENTIAL_PERSIST persistence)
    {
      return persistence switch
      {
        CREDENTIAL_PERSIST.Session => CredentialPersistence.Session,
        CREDENTIAL_PERSIST.LocalMachine => CredentialPersistence.LocalMachine,
        CREDENTIAL_PERSIST.Enterprise => CredentialPersistence.Enterprise,
        _ => throw new ArgumentOutOfRangeException (nameof(persistence), persistence, null)
      };
    }

    public static CREDENTIAL_TYPE ConvertToApiEnum (this CredentialType type)
    {
      return type switch
      {
        CredentialType.Generic => CREDENTIAL_TYPE.Generic,
        _ => throw new ArgumentOutOfRangeException (nameof(type), type, null)
      };
    }
  }
}
