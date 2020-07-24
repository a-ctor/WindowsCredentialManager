# WindowsCredentialManager

A .NET Standard 2.0 library that provides basic access to the Windows Credential Manager. Supported operations:

- Reading/Writing generic credentials
- Show a prompt for generic credentials

## Examples

Writing generic credentials:

```c#
var credentials = new GenericCredentials ("UNIQUE_TARGET_ID");
genericCredentials.UserName = "admin";
genericCredentials.Password = password;

genericCredentials.Save();
```

Reading generic credentials:

```c#
var genericCredentials = new GenericCredentials ("UNIQUE_TARGET_ID");
genericCredentials.Load();
```

Prompting the user for credentials:

```c#
var promptResult = CredentialsPrompt.Show (
    "Please enter your password", 
    "Password required!");

if (!promptResult.Cancelled)
{
  var username = promptResult.Username;
  var password = promptResult.Password;   
}
```

