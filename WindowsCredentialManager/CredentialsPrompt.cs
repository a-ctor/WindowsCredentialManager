namespace WindowsCredentialManager
{
  using System;
  using System.ComponentModel;
  using Win32.SafeHandles;
  using Win32.Types;
  using static Win32.UnsafeNativeApi;

  public class CredentialsPrompt
  {
    private const int c_cancelledErrorCode = 1223;

    public static CredentialsPromptResult Show (string text, string caption)
    {
      return Show (text, caption, IntPtr.Zero);
    }

    public static CredentialsPromptResult Show (string text, string caption, IntPtr parentHandle)
    {
      return ShowInternal (text, caption, parentHandle, false, false);
    }

    public static CredentialsPromptResult ShowWithSaveButton (string text, string caption)
    {
      return ShowWithSaveButton (text, caption, false, IntPtr.Zero);
    }

    public static CredentialsPromptResult ShowWithSaveButton (string text, string caption, bool saveDefault)
    {
      return ShowWithSaveButton (text, caption, saveDefault, IntPtr.Zero);
    }

    public static CredentialsPromptResult ShowWithSaveButton (string text, string caption, bool saveDefault, IntPtr parentHandle)
    {
      return ShowInternal (text, caption, parentHandle, true, saveDefault);
    }

    private static CredentialsPromptResult ShowInternal (string text, string caption, IntPtr parentHandle, bool showSave, bool saveDefault)
    {
      CREDUI_INFO credUiInfo = default;
      credUiInfo.Size = 5 * IntPtr.Size;
      credUiInfo.ParentHandle = parentHandle;
      credUiInfo.MessageText = text;
      credUiInfo.CaptionText = caption;

      uint inAuth = 0;

      CredentialCoTaskSafeHandle? outputBuffer = null;
      try
      {
        var windowType = CREDUI_WINDOW.Generic;
        if (showSave)
          windowType |= CREDUI_WINDOW.Checkbox;

        var save = saveDefault;
        var errorCode = CredUIPromptForWindowsCredentialsW (
          ref credUiInfo,
          0,
          ref inAuth,
          null,
          0,
          out outputBuffer,
          out var outputBufferSize,
          ref save,
          windowType);

        if (errorCode == c_cancelledErrorCode)
          return CredentialsPromptResult.CancelledResult;

        if (errorCode != 0)
          throw new Win32Exception (errorCode);

        outputBuffer.GetPromptDetails (
          outputBufferSize,
          out var domain,
          out var username,
          out var password);

        return new CredentialsPromptResult (
          false,
          domain,
          username,
          password,
          save);
      }
      finally
      {
        outputBuffer?.Dispose();
      }
    }
  }
}
