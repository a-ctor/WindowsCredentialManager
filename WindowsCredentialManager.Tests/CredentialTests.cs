using System.Linq;

namespace WindowsCredentialManager.Tests
{
  using System.Runtime.InteropServices;
  using System.Security;
  using NUnit.Framework;

  [TestFixture]
  public class CredentialTests
  {
    [Explicit]
    [Test]
    public void SaveCredentialsTest()
    {
      var genericCredentials = new GenericCredentials ("CRED_TEST");

      genericCredentials.UserName = "my user";
      genericCredentials.Password = new SecureString();
      genericCredentials.Password.AppendChar ('a');
      genericCredentials.Password.AppendChar ('a');
      genericCredentials.Password.AppendChar ('a');
      genericCredentials.Password.AppendChar ('a');
      genericCredentials.Attributes.Add (new CredentialAttribute ("a", "a1"));
      genericCredentials.Attributes.Add (new CredentialAttribute ("b", ""));

      Assert.That (() => genericCredentials.Save(), Throws.Nothing);
    }

    [Explicit]
    [Test]
    public void LoadCredentialsTest()
    {
      var genericCredentials = new GenericCredentials ("CRED_TEST");
      genericCredentials.Load();

      Assert.That (genericCredentials.UserName, Is.EqualTo ("my user"));
      Assert.That (SecureStringToString (genericCredentials.Password), Is.EqualTo ("aaaa"));

      Assert.That (genericCredentials.Attributes.Count, Is.EqualTo (2));
      Assert.That (genericCredentials.Attributes[0].Keyword, Is.EqualTo ("a"));
      Assert.That (genericCredentials.Attributes[0].Value, Is.EqualTo ("a1"));
      Assert.That (genericCredentials.Attributes[1].Keyword, Is.EqualTo ("b"));
      Assert.That (genericCredentials.Attributes[1].Value, Is.EqualTo (""));
    }

    [Explicit]
    [Test]
    public void RemoveCredentialsTest()
    {
      var genericCredentials = new GenericCredentials ("CRED_TEST");

      genericCredentials.UserName = "my user";
      genericCredentials.Password = new SecureString();
      genericCredentials.Password.AppendChar ('a');
      genericCredentials.Password.AppendChar ('a');
      genericCredentials.Password.AppendChar ('a');
      genericCredentials.Password.AppendChar ('a');
      genericCredentials.Attributes.Add (new CredentialAttribute ("a", "a1"));
      genericCredentials.Attributes.Add (new CredentialAttribute ("b", ""));

      Assert.That (() => genericCredentials.Save(), Throws.Nothing);
      Assert.That (genericCredentials.Delete(), Is.True);
    }

    [Explicit]
    [Test]
    public void PromptCredentialsTest()
    {
      var credentialsPromptResult = CredentialsPrompt.ShowWithSaveButton ("a", "b", true);

      Assert.That (credentialsPromptResult, Is.Not.Null);
    }

    [Test]
    public void ArgumentExceptionThrownWhenSecretIsTooBig()
    {
      var genericCredentials = new GenericCredentials ("CRED_TEST");

      genericCredentials.UserName = "my user";
      genericCredentials.Password = new SecureString();

      for (var i = 0; i < 2561; i++)
      {
        genericCredentials.Password.AppendChar('x');
      }

      Assert.That(() => genericCredentials.Save(), Throws.ArgumentException);
    }

    private static string SecureStringToString (SecureString value)
    {
      var ptr = Marshal.SecureStringToGlobalAllocUnicode (value);
      try
      {
        return Marshal.PtrToStringUni (ptr);
      }
      finally
      {
        Marshal.ZeroFreeGlobalAllocUnicode (ptr);
      }
    }
  }
}
